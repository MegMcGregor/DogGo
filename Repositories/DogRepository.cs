using DogGo.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace DogGo.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public DogRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], OwnerId, Notes, ImageUrl
                        FROM Dog
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();
                    while (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                        };

                        dogs.Add(dog);
                    }

                    reader.Close();

                    return dogs;
                }
            }
        }



        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], OwnerId, Notes, ImageUrl
                        FROM Dog
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Dog dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                        };

                        reader.Close();
                        return dog;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }              
                }
            }
        }


        public void AddDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO (Id, [Name], Breed, OwnerId, Notes, ImageUrl)
                        OUTPUT INSERTED.ID
                        VAOLUES (@name, @breen
                    ";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@OwnerId", dog.Id);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    if (dog.Notes != null)
                    {
                        cmd.Parameters.AddWithValue("@notes", dog.Notes);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@notes", DBNull.Value);
                    }
                    if (dog.ImageUrl != null)
                    {
                        cmd.Parameters.AddWithValue("@ImageUrl", dog.ImageUrl);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
                    }

                    int id = (int)cmd.ExecuteScalar();

                    dog.Id = id;
                }
            }
        }

        //public void UpdateOwner(Owner owner)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();

        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"
        //                    UPDATE Owner
        //                    SET 
        //                        [Name] = @name, 
        //                        Email = @email, 
        //                        Address = @address, 
        //                        Phone = @phone, 
        //                        NeighborhoodId = @neighborhoodId
        //                    WHERE Id = @id";

        //            cmd.Parameters.AddWithValue("@name", owner.Name);
        //            cmd.Parameters.AddWithValue("@email", owner.Email);
        //            cmd.Parameters.AddWithValue("@address", owner.Address);
        //            cmd.Parameters.AddWithValue("@phone", owner.Phone);
        //            cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);
        //            cmd.Parameters.AddWithValue("@id", owner.Id);

        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}

        //public void DeleteOwner(int ownerId)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();

        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"
        //                    DELETE FROM Owner
        //                    WHERE Id = @id
        //                ";

        //            cmd.Parameters.AddWithValue("@id", ownerId);

        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}
    }

}
