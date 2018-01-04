using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoLotDAL.Models;
using System.Data.Entity;

namespace AutoLotDAL.Repos
{
    public class CustomerRepo : BaseRepo<Customer>, IRepo<Customer>
    {
        public CustomerRepo()
        {
            Table = Context.Customers;
        }

        public new int Delete(int id, byte[] timeStamp)
        {
            Context.Entry(new Customer()
            {
                CustomerId = id,
                Timestamp = timeStamp
            }).State = EntityState.Deleted;
            return SaveChanges();
        }
        public new Task<int> DeleteAsync(int id, byte[] timeStamp)
        {
            Context.Entry(new Customer()
            {
                CustomerId = id,
                Timestamp = timeStamp
            }).State = EntityState.Deleted;
            return SaveChangesAsync();
        }
    }
}