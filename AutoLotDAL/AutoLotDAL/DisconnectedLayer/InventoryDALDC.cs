using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace AutoLotDAL.DisconnectedLayer
{
    public class InventoryDALDC
    {
        private string _connectionString;
        private SqlDataAdapter _adapter;

        public InventoryDALDC(string connectionString)
        {
            _connectionString = connectionString;

            ConfigureAdapter(out _adapter);
        }

        private void ConfigureAdapter(out SqlDataAdapter adapter)
        {
            adapter = new SqlDataAdapter("Select * from Inventory", _connectionString);

            var builder = new SqlCommandBuilder(adapter);
        }

        public DataTable GetAllInventory()
        {
            var inventoryTable = new DataTable("Inventory");
            _adapter.Fill(inventoryTable);
            return inventoryTable;
        }

        public void UpdateInventory(DataTable inventroryTable)
        {
            _adapter.Update(inventroryTable);
        }


    }
}
