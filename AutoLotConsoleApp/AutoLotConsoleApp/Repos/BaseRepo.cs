using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoLotDAL.EF;
using AutoLotDAL.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace AutoLotDAL.Repos
{
    public abstract class BaseRepo<T>: IDisposable, IRepo<T> where T : class, new()
    {
        public AutoLotEntities Context { get; } = new AutoLotEntities();
        protected DbSet<T> Table;

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
                Context.Dispose();

            disposed = true;
        }

        internal int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //Thrown when there is a concurrency error
                //for now, just rethrow the exception
                throw;
            }
            catch (DbUpdateException ex)
            {
                //Thrown when database update fails
                //Examine the inner exception(s) for additional
                //details and affected objects
                //for now, just rethrow the exception
                throw;
            }
            catch (CommitFailedException ex)
            {
                //handle transaction failures here
                //for now, just rethrow the exception
                throw;
            }
            catch (Exception ex)
            {
                //some other exception happened and should be handled
                throw;
            }
        }

        internal async Task<int> SaveChangesAsync()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //Thrown when there is a concurrency error
                //for now, just rethrow the exception
                throw;
            }
            catch (DbUpdateException ex)
            {
                //Thrown when database update fails
                //Examine the inner exception(s) for additional
                //details and affected objects
                //for now, just rethrow the exception
                throw;
            }
            catch (CommitFailedException ex)
            {
                //handle transaction failures here
                //for now, just rethrow the exception
                throw;
            }
            catch (Exception ex)
            {
                //some other exception happened and should be handled
                throw;
            }
        }

        public int Add(T entity)
        {
            Table.Add(entity);
            return Context.SaveChanges();
        }

        public Task<int> AddAsync(T entity)
        {
            Table.Add(entity);
            return Context.SaveChangesAsync();
        }

        public int AddRange(IList<T> entities)
        {
            Table.AddRange(entities);
            return Context.SaveChanges();
        }

        public Task<int> AddRangeAsync(IList<T> entities)
        {
            Table.AddRange(entities);
            return Context.SaveChangesAsync();
        }

        public int Save(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return SaveChanges();
        }

        public Task<int> SaveAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return SaveChangesAsync();
        }

        public int Delete(int id, byte[] timeStamp)
        {
            Context.Entry(new Inventory()
            {
                CarId = id,
                Timestamp = timeStamp
            }).State = EntityState.Deleted;
            return SaveChanges();
        }

        public Task<int> DeleteAsync(int id, byte[] timeStamp)
        {
            Context.Entry(new Inventory()
            {
                CarId = id,
                Timestamp = timeStamp
            }).State = EntityState.Deleted;
            return SaveChangesAsync();
        }

        public T GetOne(int? id) => Table.Find(id);

        public Task<T> GetOneAsync(int? id) => Table.FindAsync(id);

        public List<T> GetAll() => Table.ToList();

        public Task<List<T>> GetAllAsync() => Table.ToListAsync();

        public List<T> ExecuteQuery(string sql) => Table.SqlQuery(sql).ToList();

        public Task<List<T>> ExecuteQueryAsync(string sql)
            => Table.SqlQuery(sql).ToListAsync();

        public List<T> ExecuteQuery(string sql, object[] sqlParametersObjects)
            => Table.SqlQuery(sql, sqlParametersObjects).ToList();

        public Task<List<T>> ExecuteQueryAsync(string sql, object[] sqlParametersObjects)
            => Table.SqlQuery(sql, sqlParametersObjects).ToListAsync();
    }
}
