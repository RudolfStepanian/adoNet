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
    "created_at" datetime NOT NULL,
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
    "created_at" datetime NOT NULL,
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
    "created_at" datetime NOT NULL,
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
    PRIMARY KEY ("id")
);



CREATE PROCEDURE spGetAllClients
    @pageNumber INT = 1,
    @pageSize INT = 5,
    @likeName VARCHAR (100) = '',
    @likeLastName VARCHAR (100) = ''
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



update client
set first_name = 'test'
where id = 3