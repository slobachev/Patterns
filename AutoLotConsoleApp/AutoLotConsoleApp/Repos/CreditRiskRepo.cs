using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoLotDAL.Models;
using System.Data.Entity;

namespace AutoLotDAL.Repos
{
    public class CreditRiskRepo : BaseRepo<CreditRisk>, IRepo<CreditRisk>
    {
        public CreditRiskRepo()
        {
            Table = Context.CreditRisks;
        }

        public new int Delete(int id, byte[] timeStamp)
        {
            Context.Entry(new CreditRisk()
            {
                CustomerId = id,
                Timestamp = timeStamp
            }).State = EntityState.Deleted;
            return SaveChanges();
        }
        public new Task<int> DeleteAsync(int id, byte[] timeStamp)
        {
            Context.Entry(new CreditRisk()
            {
                CustomerId = id,
                Timestamp = timeStamp
            }).State = EntityState.Deleted;
            return SaveChangesAsync();
        }
    }
}