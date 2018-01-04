using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AutoLotDAL.Models;

namespace AutoLotDAL.ConnectedLayer
{
    public class InventoryDAL
    {
        private SqlConnection _sqlConnection;

        public void OpenConnection(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
            _sqlConnection.Open();
        }

        public void CloseConnection()
        {
            _sqlConnection.Close();
        }

        public void InsertAuto(string color, string make, string petName)
        {
            var sqlQuery = "Insert into Inventory " +
                           "(Make, Color, PetName) Values " +
                           "(@Make, @Color, @PetName)";
            using (var command = new SqlCommand(sqlQuery, _sqlConnection))
            {
                var parameter = new SqlParameter
                {
                    ParameterName = "@Make",
                    Value = make,
                    SqlDbType = SqlDbType.Char,
                };
                command.Parameters.Add(parameter);


                parameter = new SqlParameter
                {
                    ParameterName = "@Color",
                    Value = color,
                    SqlDbType = SqlDbType.Char,
                };
                command.Parameters.Add(parameter);

                parameter = new SqlParameter
                {
                    ParameterName = "@PetName",
                    Value = petName,
                    SqlDbType = SqlDbType.Char
                };
                command.Parameters.Add(parameter);

                command.ExecuteNonQuery();
            }
        }

        public void InsertAuto(NewCar car)
        {
            var sqlQuery = "Insert into Inventory " +
                              "(Make, Color, PetName) Values " +
                              $"('{car.Make}', '{car.Color}', '{car.PetName}')";
            using (var command = new SqlCommand(sqlQuery, _sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }

        public void DeleteCar(int id)
        {
            var sqlQuery = $"Delete from Inventory where CarId = {id}";
            using (var command = new SqlCommand(sqlQuery, _sqlConnection))
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    var error = new Exception("Sorry! That car is on order", e);
                    throw error;
                }
            }
        }

        public void UpdateCarPetName(int id, string newPetName)
        {
            var sqlQuery = $"Update Inventory Set PetName = '{newPetName}' where CarId = {id}";
            using (var command = new SqlCommand(sqlQuery, _sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }

        public List<NewCar> GetAllInventoryAsList()
        {
            List<NewCar> inventory = new List<NewCar>();
            var sqlQuery = $"Select * from Inventory";

            using (var command = new SqlCommand(sqlQuery, _sqlConnection))
            {
                var inventoryReader = command.ExecuteReader();
                while (inventoryReader.Read())
                {
                    inventory.Add(new NewCar
                    {
                        CarId = (int)inventoryReader["CarId"],
                        Color = (string)inventoryReader["Color"],
                        Make = (string)inventoryReader["Make"],
                        PetName = (string)inventoryReader["PetName"]
                    });
                }
                inventoryReader.Close();
            }
            return inventory;
        }

        public DataTable GetAllInventoryAsDataTable()
        {
            var inventory = new DataTable();
            var sqlQuery = $"Select * from Inventory";

            using (var command = new SqlCommand(sqlQuery, _sqlConnection))
            {
                var inventoryReader = command.ExecuteReader();
                inventory.Load(inventoryReader);
                inventoryReader.Close();
            }
            return inventory;
        }

        public string LookUpPetName(int carId)
        {
            string carPetName;

            using (var command = new SqlCommand("GetPetName", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                var parameter = new SqlParameter
                {
                    ParameterName = "@carId",
                    SqlDbType = SqlDbType.Int,
                    SqlValue = carId,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);

                parameter = new SqlParameter
                {
                    ParameterName = "@petName",
                    SqlDbType = SqlDbType.Char,
                    Size = 10,
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameter);

                command.ExecuteNonQuery();
                carPetName = (string)command.Parameters["@petName"].Value;
            }
            return carPetName;
        }

        public void ProcessCreditRisk(bool throwEx, int custId)
        {
            string firstName, lastName;

            var cmdSelect = new SqlCommand($"Select * from Customers where CustomerId = {custId}",
                _sqlConnection);
            using (var dataReader = cmdSelect.ExecuteReader())
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    firstName = (string) dataReader["FirstName"];
                    lastName = (string) dataReader["LastName"];
                }
                else
                {
                    return;
                }
            }

            // Create command objects that represent each step of the operation.
            var cmdRemove =
                new SqlCommand($"Delete from Customers where CustomerId = {custId}",
                    _sqlConnection);

            var cmdInsert =
                new SqlCommand("Insert Into CreditRisks" +
                               $"(FirstName, LastName) Values('{firstName}', '{lastName}')",
                    _sqlConnection);

            // We will get this from the connection object
            SqlTransaction tx = null;
            try
            {
                tx = _sqlConnection.BeginTransaction();

                cmdInsert.Transaction = tx;
                cmdRemove.Transaction = tx;

                cmdInsert.ExecuteNonQuery();
                cmdRemove.ExecuteNonQuery();

                if (throwEx)
                {
                    throw new Exception("Sorry! Database error! Tx failed.");
                }

                tx.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Any error will roll back transaction.
                // Using the new conditional access operator to check for null.
                tx?.Rollback();
            }
        }

    }
}
