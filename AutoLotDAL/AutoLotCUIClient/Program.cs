using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoLotDAL.ConnectedLayer;
using AutoLotDAL.Models;
using System.Data;
using System.Configuration;
using static System.Console;

namespace AutoLotCUIClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WriteLine("***** The AutoLot Console UI *****\n");

            // Get connection string from App.config
            var connectionString = 
                ConfigurationManager.ConnectionStrings["AutoLotSqlProvider"].ConnectionString;
            var userDone = false;
            var userCommand = "";

            //Create out InventoryDAL object
            var inventoryDAL = new InventoryDAL();
            inventoryDAL.OpenConnection(connectionString);

            try
            {
                ShowInstructions();
                do
                {
                    WriteLine("\nPlease enter your command");
                    userCommand = ReadLine();
                    WriteLine();
                    switch (userCommand?.ToUpper() ?? "")
                    {
                        case "I":
                            InsertNewCar(inventoryDAL);
                            break;
                        case "U":
                            UpdateCarPetName(inventoryDAL);
                            break;
                        case "D":
                            DeleteCar(inventoryDAL);
                            break;
                        case "L":
                            ListInventory(inventoryDAL);
                            break;
                        case "S":
                            ShowInstructions();
                            break;
                        case "P":
                            LookUpPetName(inventoryDAL);
                            break;
                        case "Q":
                            userDone = true;
                            break;
                        default:
                            WriteLine("Bad data! Try again");
                            break;
                    }
                } while (!userDone);
            }
            catch (Exception e)
            {
                WriteLine(e);
            }
            finally
            {
                inventoryDAL.CloseConnection();
            }
        }

        private static void LookUpPetName(InventoryDAL inventoryDal)
        {
            Write("Enter car id: ");
            var carId = int.Parse(ReadLine() ?? "0");

            WriteLine($"Petname of {carId} is {inventoryDal.LookUpPetName(carId).TrimEnd()}.");

        }

        private static void ListInventory(InventoryDAL inventoryDal)
        {
            var inventoryDataTable = inventoryDal.GetAllInventoryAsDataTable();
            DisplayData(inventoryDataTable);
        }

        private static void ListInventoryViaList(InventoryDAL inventoryDal)
        {
            var carList = inventoryDal.GetAllInventoryAsList();

            WriteLine("CarId:\tMake:\tColor:\tPetName:");
            carList.ForEach((c => WriteLine($"{c.CarId}\t{c.Make}\t{c.Color}\t{c.PetName}")));
        }

        private static void DisplayData(DataTable dt)
        {
            for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
            {
                Write($"{dt.Columns[curCol].ColumnName}\t");
            }
            WriteLine("\n-----------------------");


            for (int curRow = 0; curRow < dt.Rows.Count; curRow++)
            {
                for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
                {
                    Write($"{dt.Rows[curRow][curCol]}\t");
                }
                WriteLine();
            }
        }

        private static void DeleteCar(InventoryDAL inventoryDal)
        {
            // Get ID of car to delete.
            Write("Enter ID of Car to delete: ");
            var id = int.Parse(ReadLine()??"0");

            try
            {
                inventoryDal.DeleteCar(id);
            }
            catch (Exception e)
            {
                WriteLine(e);
            }
        }

        private static void UpdateCarPetName(InventoryDAL inventoryDal)
        {
            // First get the user data.
            Write("Enter Car ID: ");
            var carId = int.Parse(ReadLine()??"0");
            Write("Enter New Pet Name: ");
            var newCarPetName = ReadLine();

            // Now pass to data access library.
            inventoryDal.UpdateCarPetName(carId, newCarPetName);
        }

        private static void InsertNewCar(InventoryDAL inventoryDal)
        {
            Write("Enter Car Color: ");
            var newCarColor = ReadLine();
            Write("Enter Car Make: ");
            var newCarMake = ReadLine();
            Write("Enter Pet Name: ");
            var newCarPetName = ReadLine();
            Console.WriteLine(newCarPetName);
            
            // Now pass to data access library.
            inventoryDal.InsertAuto(newCarColor, newCarMake, newCarPetName);

            //var c = new NewCar
            //{
            //    Color = newCarColor,
            //    Make = newCarMake,
            //    PetName = newCarPetN
            //};
            //inventoryDal.InsertAuto(c);
        }

        static void ShowInstructions()
        {
            WriteLine("I: Inserts a new car.");
            WriteLine("U: Updates an existing car.");
            WriteLine("D: Deletes an existing car.");
            WriteLine("L: Lists current inventory.");
            WriteLine("S: Shows these instructions.");
            WriteLine("P: Looks up pet name.");
            WriteLine("Q: Quits program.");
        }
    }
}
