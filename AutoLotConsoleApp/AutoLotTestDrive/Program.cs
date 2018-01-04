using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AutoLotDAL.EF;
using AutoLotDAL.Repos;
using AutoLotDAL.Models;
using System.Data.Entity.Infrastructure;

namespace AutoLotTestDrive
{
    class Program
    {
        static void Main(string[] args)
        {
            //Database.SetInitializer(new DataInitializer());
            Console.WriteLine("***** Fun with ADO.NET EF Code First *****\n");
            var car1 = new Inventory() { Make = "Yugo", Color = "Brown", PetName = "Brownie" };
            var car2 = new Inventory() { Make = "SmartCar", Color = "Brown", PetName = "Shorty" };
            AddNewRecord(car1);
            AddNewRecord(car2);
            AddNewRecords(new List<Inventory> { car1, car2 });
            PrintAllInventory();
            ShowAllOrdersEagerlyFetched();
            PrintAllCustomersAndCreditRisks();
            var customerRepo = new CustomerRepo();
            var customer = customerRepo.GetOne(6);
            //customerRepo.Context.Entry(customer).State = EntityState.Detached;
            //var risk = MakeCustomerARisk(customer);
            //PrintAllCustomersAndCreditRisks();
            //UpdateRecordWithConcurrency();
        }

        private static void UpdateRecordWithConcurrency()
        {
            var car = new Inventory
            { Make = "Boshi", Color = "Green", PetName = "Cake" };
            AddNewRecord(car);

            var firstUser = new InventoryRepo();
            var firstCar = firstUser.GetOne(car.CarId);
            firstCar.PetName = "Dread";

            var secondUser = new InventoryRepo();
            var secondCar = secondUser.GetOne(car.CarId);
            secondCar.PetName = "Mad";

            firstUser.Save(firstCar);
            try
            {
                secondUser.Save(secondCar);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                firstUser.Dispose();
                secondUser.Dispose();
            }
        }

        private static void PrintAllInventory()
        {
            using (var repo = new InventoryRepo())
            {
                foreach (Inventory c in repo.GetAll())
                {
                    Console.WriteLine(c);
                }
            }
        }

        private static void AddNewRecord(Inventory car)
        {
            // Add record to the Inventory table of the AutoLot
            // database.
            using (var repo = new InventoryRepo())
            {
                repo.Add(car);
            }
        }
        private static void AddNewRecords(IList<Inventory> cars)
        {
            // Add record to the Inventory table of the AutoLot
            // database.
            using (var repo = new InventoryRepo())
            {
                repo.AddRange(cars);
            }
        }

        private static void UpdateRecord(int carId)
        {
            using (var repo = new InventoryRepo())
            {
                var carToUpdate = repo.GetOne(carId);
                if (carToUpdate != null)
                {
                    carToUpdate.Color = "Green";
                    repo.Save(carToUpdate);
                }
            }
        }

        //Lazy loading
        private static void ShowAllOrders()
        {
            using (var repo = new OrderRepo())
            {
                Console.WriteLine("*********** Pending Orders ***********");
                foreach (var itm in repo.GetAll())
                {
                    Console.WriteLine($"->{itm.Customer.FullName} is waiting on {itm.Car.PetName}");
                }
            }
        }

        //Eager loading
        private static void ShowAllOrdersEagerlyFetched()
        {
            using (var context = new AutoLotEntities())
            {
                var orders = context.Orders
                    .Include(x => x.Car)
                    .Include(y => y.Customer)
                    .ToList();
                foreach (var itm in orders)
                {
                    Console.WriteLine($"->{itm.Customer.FullName} is waiting on {itm.Car.PetName}");
                }
            }
        }

        private static CreditRisk MakeCustomerARisk(Customer customer)
        {
            using (var context = new AutoLotEntities())
            {
                context.Customers.Attach(customer);
                context.Customers.Remove(customer);
                var creditRisk = new CreditRisk()
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName
                };
                context.CreditRisks.Add(creditRisk);

                var creditRiskDupe = new CreditRisk()
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName
                };
                context.CreditRisks.Add(creditRiskDupe);

                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex);
                }
                
                return creditRisk;
            }
        }

        private static void PrintAllCustomersAndCreditRisks()
        {
            Console.WriteLine("*********** Customers ***********");
            using (var repo = new CustomerRepo())
            {
                foreach (var cust in repo.GetAll())
                {
                    Console.WriteLine($"->{cust.FirstName} {cust.LastName} is a Customer.");
                }
            }
            Console.WriteLine("*********** Credit Risks ***********");
            using (var repo = new CreditRiskRepo())
            {
                foreach (var risk in repo.GetAll())
                {
                    Console.WriteLine($"->{risk.FirstName} {risk.LastName} is a Credit Risk!");
                }
            }
        }
    }
}
