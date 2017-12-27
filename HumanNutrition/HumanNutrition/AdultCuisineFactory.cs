using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanNutrition
{
    /// <summary>
    /// A concrete product
    /// </summary>
    class BLT : Sandwich { }

    /// <summary>
    /// A concrete product
    /// </summary>
    class CremeBrulee : Dessert { }


    /// <summary>
    /// A concrete factory which creates concrete objects by implementing the abstract factory's methods.
    /// </summary>
    class AdultCuisineFactory : RecipeFactory
    {
        public override Sandwich CreateSandwich()
        {
            return new BLT();
        }

        public override Dessert CreateDessert()
        {
            return new CremeBrulee();
        }
    }
}
