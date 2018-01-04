using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoLotDAL.Models;

namespace AutoLotDAL.Models
{
    public partial class Inventory
    {
        public override string ToString()
        {
            return $"{this.PetName ?? "**No Name**"} is a {this.Color} {this.Make} with ID " +
                $"{this.CarId}.";
        }
    }
}
