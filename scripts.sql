--
-- Name: "client"; Type: TABLE;
--
create table "client" (
    "id" bigint NOT NULL IDENTITY(1,1), 
    "first_name" character varying(255) NOT NULL,
    "last_name" character varying(255) NOT NULL,
    "birth_date" datetime NOT NULL,
    "created_at" datetime NOT NULL,
    "modified_at" datetime,
    PRIMARY KEY ("id")
);

--
-- Name: "email"; Type: TABLE;
--
create table "email" (
    "id" bigint NOT NULL IDENTITY(1,1), 
	"address" character varying(255) NOT NULL,
	"domain" character varying(255),
    "created_at" datetime NOT NULL DEFAULT GETDATE(),
    "modified_at" datetime,
	"client_id" bigint NOT NULL,
    PRIMARY KEY ("id"),
	FOREIGN KEY ("client_id") REFERENCES client(id)
);

--
-- Name: "document"; Type: TABLE;
--
create table "document" (
    "id" bigint NOT NULL IDENTITY(1,1), 
	"number" character varying(255) NOT NULL,
	"given_by" character varying(255),
	"given_at" datetime,
    "created_at" datetime NOT NULL DEFAULT GETDATE(),
    "modified_at" datetime,
	"client_id" bigint NOT NULL,
    PRIMARY KEY ("id"),
	FOREIGN KEY ("client_id") REFERENCES client(id)
);

--
-- Name: "phone"; Type: TABLE;
--
create table "phone" (
    "id" bigint NOT NULL IDENTITY(1,1), 
	"number" character varying(255) NOT NULL,
	"operator" character varying(255),
    "created_at" datetime NOT NULL DEFAULT GETDATE(),
    "modified_at" datetime,
	"client_id" bigint NOT NULL,
    PRIMARY KEY ("id"),
	FOREIGN KEY ("client_id") REFERENCES client(id)
);

--
-- Name: "user"; Type: TABLE;
--
create table "user" (
    "id" bigint NOT NULL IDENTITY(1,1), 
	"username" character varying(50) NOT NULL,
	"password" character varying(50),
    "created_at" datetime NOT NULL DEFAULT GETDATE(),
    PRIMARY KEY ("id")
);



CREATE PROCEDURE spGetAllClients
    @pageNumber INT = 1,
    @pageSize INT = 5,
    @likeName VARCHAR (100) = '',
    @likeLastName VARCHAR (100) = '',
	@includePhone BIT = 0
AS
	BEGIN
		SELECT total_count,
			cl.id as client_id,
			cl.first_name as client_first_name,
			cl.last_name as client_last_name,
			cl.birth_date as client_birth_date,
			ph.id as phone_id,
			ph.number as phone_number, 
			ph.operator as phone_operator, 
			em.id as email_id, 
			em.address as email_address, 
			em.domain as email_domain, 
			doc.id as document_id, 
			doc.number as document_number,
			doc.given_by as document_given_by,
			doc.given_at as document_given_at 
		FROM (SELECT *, Count(*) Over () as total_count FROM client 
			WHERE first_name LIKE Concat(@likeName,'%')
			AND last_name Like CONCAT(@likeLastName, '%')
			ORDER BY id OFFSET (@pageSize*(@pageNumber-1))
			ROWS FETCH NEXT (@pageSize) ROWS ONLY) as cl
        LEFT JOIN document as doc on cl.Id = doc.client_id
        LEFT JOIN email as em on cl.Id = em.client_id
		LEFT JOIN phone as ph on cl.Id = ph.client_id
        Order By cl.id
	END


CREATE PROCEDURE spGetClient
	@id INT
AS
	BEGIN
		SELECT cl.id as client_id, 
		   cl.first_name as client_first_name, 
		   cl.last_name as client_last_name,
		   cl.birth_date as client_birth_date,  
		   ph.id as phone_id, 
		   ph.number as phone_number, 
		   ph.operator as phone_operator, 
		   em.id as email_id, 
		   em.address as email_address, 
		   em.domain as email_domain, 
		   doc.id as document_id, 
		   doc.number as document_number,
		   doc.given_by as document_given_by,
		   doc.given_at as document_given_at 
		from (SELECT * from client WHERE id = @id) as cl
        LEFT JOIN document as doc on cl.Id = doc.client_id
        LEFT JOIN email as em on cl.Id = em.client_id
        LEFT JOIN phone as ph on cl.Id = ph.client_id
	END

	SELECT cl.id as client_id, 
		   cl.first_name as client_first_name, 
		   cl.last_name as client_last_name,
		   cl.birth_date as client_birth_date,  
		   ph.id as phone_id, 
		   ph.number as phone_number, 
		   ph.operator as phone_operator, 
		   em.id as email_id, 
		   em.address as email_address, 
		   em.domain as email_domain, 
		   doc.id as document_id, 
		   doc.number as document_number,
		   doc.given_by as document_given_by,
		   doc.given_at as document_given_at 
		from (SELECT * from client WHERE id = 3) as cl
        LEFT JOIN document as doc on cl.Id = doc.client_id
        LEFT JOIN email as em on cl.Id = em.client_id
        LEFT JOIN phone as ph on cl.Id = ph.client_id

 CREATE PROCEDURE spDeleteClientPhones
	@id INT
AS
	BEGIN
		DELETE from phone
		where client_id = @id
	END

CREATE PROCEDURE spUpdateClient
	@id INT,
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50),
	@BirthDate DATETIME
AS
	BEGIN
		update client
		set 
			first_name = CASE WHEN LEN(@firstName) > 0 THEN @firstName ELSE first_name END,
			last_name = CASE WHEN LEN(@LastName) > 0 THEN @LastName ELSE last_name END,
			birth_date = CASE WHEN @BirthDate IS NOT NULL THEN @BirthDate ELSE birth_date END
		where id = @id
	END


	
CREATE PROCEDURE spGetAllClients
    @pageNumber INT = 1,
    @pageSize INT = 5,
    @likeName VARCHAR (100) = '',
    @likeLastName VARCHAR (100) = '',
	@includePhone BIT = 0,
	@includeEmail BIT = 0,
	@includeDocument BIT = 0
AS
	BEGIN
		DECLARE @q varchar(max) = 'SELECT total_count,
				cl.id as client_id,
				cl.first_name as client_first_name,
				cl.last_name as client_last_name,
				cl.birth_date as client_birth_date';
		DECLARE @offset INT;
		IF @includePhone = 1
		BEGIN
		    SET @q = @q + ',
				ph.id as phone_id,
				ph.number as phone_number, 
				ph.operator as phone_operator';
		END
		
		IF @includeEmail = 1
		BEGIN
		    SET @q = @q + ',
				em.address as email_address, 
				em.domain as email_domain' ;
		END
		
		IF @includeDocument = 1
		BEGIN
		    SET @q = @q + ', 
				doc.number as document_number,
				doc.given_by as document_given_by,
				doc.given_at as document_given_at';
		END
		
		SET @offset = (@pageSize * (@pageNumber-1));
		SET @q = CONCAT(@q, ' FROM (SELECT *, Count(*) Over () as total_count FROM client 
				WHERE first_name LIKE ''' + Concat(@likeName, '%') + '''
				AND last_name Like ''' + CONCAT(@likeLastName, '%') + '''
				ORDER BY id OFFSET (', @offset, ')
				ROWS FETCH NEXT (', @pageSize, ') ROWS ONLY) as cl');
		
		IF @includePhone = 1
		BEGIN
		    SET @q = @q + ' LEFT JOIN phone as ph on cl.Id = ph.client_id';
		END
		
		IF @includeEmail = 1
		BEGIN
		    SET @q = @q + ' LEFT JOIN email as em on cl.Id = em.client_id' ;
		END
		
		IF @includeDocument = 1
		BEGIN
		    SET @q = @q + ' LEFT JOIN document as doc on cl.Id = doc.client_id';
		END

		SET @q = @q + ' Order By cl.id'

		EXECUTE(@q)

	END



insert into document (number, given_by, given_at, client_id)
values 
('152634', 'state', GETDATE(), 2),
('526341', 'state', GETDATE(), 2),
('263415', 'state', GETDATE(), 3),
('634152', 'state', GETDATE(), 5),
('341526', 'state', GETDATE(), 5),
('415263', 'state', GETDATE(), 6),
('485967', 'state', GETDATE(), 6),
('859674', 'state', GETDATE(), 6),
('596748', 'state', GETDATE(), 3),
('967485', 'state', GETDATE(), 3),
('674859', 'state', GETDATE(), 4)

insert into email (address, domain, client_id)
values
('test@test.ts','@test.ts',2),
('best@test.ts','@test.ts',3),
('nest@test.ts','@test.ts',4),
('vest@test.ts','@test.ts',5),
('lest@test.ts','@test.ts',6)

insert into phone (number, operator, client_id)
values
('+123456789','testOp',2),
('+234567891','bestOp',3),
('+345678912','nestOp',4),
('+456789123','vestOp',5),
('+567892134','lestOp',6)