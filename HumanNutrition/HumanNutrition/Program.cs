using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanNutrition
{
    class Program
    {
        /// <summary>
        /// The program to receive the anticipated food regarding the age type
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Who are you? (A)dult or (C)hild?");
            char input = Console.ReadKey().KeyChar;
            RecipeFactory factory;
            switch (Char.ToUpper(input))
            {
                case 'A':
                    factory = new AdultCuisineFactory();
                    break;

                case 'C':
                    factory = new KidCuisineFactory();
                    break;

                default:
                    throw new NotImplementedException();

            }

            var sandwich = factory.CreateSandwich();
            var dessert = factory.CreateDessert();

            Console.WriteLine("\nSandwich: " + sandwich.GetType().Name);
            Console.WriteLine("Dessert: " + dessert.GetType().Name);
        }
    }
}