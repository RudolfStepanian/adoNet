using ATestApplication.BuisenessEntities;
using ATestApplication.Models.Customer;
using ATestApplication.Models.Document;
using ATestApplication.Models.Email;
using ATestApplication.Models.Phone;
using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ATestApplication.Services.Customer
{
    public class ClientService
    {
        string connectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
        // "Server=localhost;Database=master;Trusted_Connection=True;"

        public async Task<ClientList> GetAllClients(ClientFilterModel filter)
        {
            try
            {
                ClientList clientsList = new ClientList();
                clientsList.Items = new List<ClientGetModel>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spGetAllClients", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pageNumber", SqlDbType.Int).Value = filter?.PageNumber ?? 1;
                    cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = filter?.PageSize ?? 20;
                    cmd.Parameters.Add("@likeName", SqlDbType.VarChar).Value = filter?.Name ?? "";
                    cmd.Parameters.Add("@likeLastName", SqlDbType.VarChar).Value = filter?.LastName ?? "";
                    cmd.Parameters.Add("@includePhone", SqlDbType.Bit).Value = filter?.IncludePhone ?? false ? 1 : 0;
                    cmd.Parameters.Add("@includeEmail", SqlDbType.Bit).Value = filter?.IncludeEmail ?? false ? 1 : 0;
                    cmd.Parameters.Add("@includeDocument", SqlDbType.Bit).Value = filter?.IncludeDocuments ?? false ? 1 : 0;


                    con.Open();
                    using (SqlDataReader rdr = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (rdr.Read())
                        {
                            if (clientsList.TotalCount == null)
                                clientsList.TotalCount = (int)rdr["total_count"];
                            var clientId = (long)rdr["client_id"];
                            var client = clientsList.Items.FirstOrDefault(_ => _.Id == clientId);
                            bool newClient = client == null;
                            if (newClient)
                            {
                                client = new ClientGetModel();
                                client.Id = clientId;
                                client.Name = rdr["client_first_name"]?.ToString();
                                client.LastName = rdr["client_last_name"]?.ToString();
                                client.BirthDate = rdr.GetFieldValue<DateTime>(rdr.GetOrdinal("client_birth_date"));

                                client.Emails = new List<EmailGetModel>();
                                client.Documents = new List<DocumentGetModel>();
                                client.Phones = new List<PhoneGetModel>();
                            }
                            if (rdr["email_id"] != DBNull.Value)
                            {
                                EmailGetModel email = new EmailGetModel()
                                {
                                    Id = (long)rdr["email_id"],
                                    Address = rdr["email_address"].ToString(),
                                    Domain = rdr["email_domain"] != DBNull.Value ? rdr["email_domain"].ToString() : null
                                };
                                client.Emails.Add(email);
                            }
                            if (rdr["document_id"] != DBNull.Value)
                            {
                                DocumentGetModel document = new DocumentGetModel() 
                                {
                                    Id = (long)rdr["document_id"],
                                    Number = rdr["document_number"].ToString(),
                                    GivenBy = rdr["document_given_by"] != DBNull.Value 
                                        ? rdr["document_given_by"].ToString() 
                                        : null,
                                    GivenAt = rdr["document_given_at"] != DBNull.Value 
                                        ? (DateTime?)rdr.GetFieldValue<DateTime>(rdr.GetOrdinal("document_given_at")) 
                                        : null
                                };
                                client.Documents.Add(document);
                            }
                            if (rdr["phone_id"] != DBNull.Value)
                            {
                                PhoneGetModel phone = new PhoneGetModel()
                                { 
                                    Id = (long)rdr["phone_id"],
                                    Number = rdr["phone_number"].ToString()
                                };
                                client.Phones.Add(phone);
                            }

                            if (newClient)
                            {
                                clientsList.Items.Add(client);
                            }
                        }
                    }
                    con.Close();

                }
                clientsList.PageNumber = filter.PageNumber;
                clientsList.PageSize = filter.PageSize;
                return clientsList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClientGetModel> GetClient(long id)
        {
            try
            {
                ClientGetModel client = null;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spGetClient", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    con.Open();
                    using (SqlDataReader rdr = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (rdr.Read())
                        {
                            if (client == null)
                            {
                                client = new ClientGetModel();
                                client.Id = id;
                                client.Name = rdr["client_first_name"]?.ToString();
                                client.LastName = rdr["client_last_name"]?.ToString();
                                client.BirthDate = rdr.GetFieldValue<DateTime>(rdr.GetOrdinal("client_birth_date"));

                                client.Emails = new List<EmailGetModel>();
                                client.Documents = new List<DocumentGetModel>();
                                client.Phones = new List<PhoneGetModel>();
                            }
                            if (rdr["email_id"] != DBNull.Value)
                            {
                                if (!client.Emails.Select(_ => _.Id).Contains((long)rdr["email_id"]))
                                {
                                    EmailGetModel email = new EmailGetModel()
                                    {
                                        Id = (long)rdr["email_id"],
                                        Address = rdr["email_address"].ToString()
                                    };
                                    client.Emails.Add(email);
                                }
                            }
                            if (rdr["document_id"] != DBNull.Value)
                            {
                                if (!client.Documents.Select(_ => _.Id).Contains((long)rdr["document_id"]))
                                {
                                    DocumentGetModel document = new DocumentGetModel()
                                    {
                                        Id = (long)rdr["document_id"],
                                        Number = rdr["document_number"].ToString()
                                    };
                                    client.Documents.Add(document);
                                }
                            }
                            if (rdr["phone_id"] != DBNull.Value)
                            {
                                if (!client.Phones.Select(_ => _.Id).Contains((long)rdr["phone_id"]))
                                {
                                    DocumentGetModel document = new DocumentGetModel()
                                    {
                                        Id = (long)rdr["phone_id"],
                                        Number = rdr["phone_number"].ToString()
                                    };
                                    client.Documents.Add(document);
                                }
                            }
                        }
                    }
                    con.Close();

                }
                return client;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //To Add new client record    
        public void AddClient(ClientGetModel client)
        {   
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddClient", con);
                cmd.CommandType = CommandType.StoredProcedure;

                //cmd.Parameters.AddWithValue("@Name", client.Name);
                //cmd.Parameters.AddWithValue("@Gender", client.Gender);
                //cmd.Parameters.AddWithValue("@Department", client.Department);
                //cmd.Parameters.AddWithValue("@City", client.City);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        //To Update the records of a particluar client  
        public void UpdateClient(long id, ClientUpdateModel client)
        {
            try
            {
                bool modelIsValid = true;
                string errorMessage = string.Empty;
                if (client.FirstName.All(c => c.Equals(' ')))
                {
                    errorMessage = "First name cant be empty.";
                    modelIsValid = false;
                }
                if (client.LastName.All(c => c.Equals(' ')))
                {
                    errorMessage = $"{errorMessage} Last name cant be empty.";
                    modelIsValid = false;
                }
                if (!modelIsValid)
                {
                    throw new Exception(errorMessage);
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spUpdateClient", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@FirstName", client.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", client.LastName);
                    cmd.Parameters.AddWithValue("@BirthDate", client.BirthDate);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        //Get the details of a particular client  
        public ClientGetModel GetClientData(int? id)
        {
            ClientGetModel client = new ClientGetModel();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT * FROM tblClient WHERE ClientID= " + id;
                SqlCommand cmd = new SqlCommand(sqlQuery, con);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    //client.ID = Convert.ToInt32(rdr["ClientID"]);
                    //client.Name = rdr["Name"].ToString();
                    //client.Gender = rdr["Gender"].ToString();
                    //client.Department = rdr["Department"].ToString();
                    //client.City = rdr["City"].ToString();
                }
            }
            return client;
        }

        public void DeleteClientPhones(long id)
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteClientPhones", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
