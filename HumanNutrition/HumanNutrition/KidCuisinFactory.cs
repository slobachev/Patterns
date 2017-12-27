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
    class PBandJ : Sandwich { }

    /// <summary>
    /// A concrete product
    /// </summary>
    class IceCreamSundae : Dessert { }

    /// <summary>
    /// A concrete factory which creates concrete objects by implementing the abstract factory's methods.
    /// </summary>
    class KidCuisineFactory : RecipeFactory
    {
        public override Sandwich CreateSandwich()
        {
            return new PBandJ();
        }

        public override Dessert CreateDessert()
        {
            return new IceCreamSundae();
        }
    }
}
