using SQLite;
using SQLiteManager.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SQLiteManager.DataAccess
{
    /// <summary>
    /// Base class for every DataAccess to SQLite. This will provide the data model with the default
    /// queries that can be overriden for a custom implementation. Other queries can also be added
    /// to the new DataAccess-class that derives from this Base.
    /// </summary>
    /// <typeparam name="TDataModel">The data model for the DataAccess to SQLite.</typeparam>
    public class BaseDataAccess<TDataModel>
        where TDataModel : BaseDataModel, new()
    {
        /// <summary>
        /// The asynchronous connection to the SQLite-database.
        /// </summary>
        protected SQLiteAsyncConnection AsyncConnection => Database.AsyncConnection;

        public BaseDataAccess()
        {
            CreateTable();
        }
        
        /// <summary>
        /// Try to execute a query. If an exception is raised during execution, the default of TResult will be returned.
        /// </summary>
        /// <typeparam name="TResult">The expected result.</typeparam>
        /// <param name="query">The query statement to be executed.</param>
        /// <returns>The result of the query.</returns>
        protected TResult PerformQuery<TResult>(Func<Task<TResult>> query)
        {
            try
            {
                return query.Invoke().Result;
            }
            catch (Exception ex)
            {
                return default(TResult);
            }
        }

        /// <summary>
        /// Try to execute a query. If an exception is raised during execution, the default of TResult will be returned.
        /// </summary>
        /// <typeparam name="TResult">The expected result.</typeparam>
        /// <param name="query">The query statement to be executed.</param>
        /// <returns>The result of the query.</returns>
        protected TResult PerformQuery<TResult>(Func<TResult> query)
        {
            try
            {
                return query.Invoke();
            }
            catch (Exception)
            {
                return default(TResult);
            }
        }
        
        /// <summary>
        /// Create the table for the data model in the SQLite-database. If the table already exists, nothing will happen.
        /// </summary>
        /// <returns>TRUE = table is created (or table already existed) | FALSE = error has occured</returns>
        public virtual Boolean CreateTable()
        {
            return PerformQuery(() =>
            {
                AsyncConnection.CreateTableAsync<TDataModel>();

                return true;
            });
        }

        /// <summary>
        /// Drop the table for the data model in the SQLite-database. If the table isn't known in SQLite, nothing will happen.
        /// </summary>
        /// <returns>TRUE = table is dropped (or tabled didn't exist) | FALSE = error has occured</returns>
        public virtual Boolean DropTable()
        {
            return PerformQuery(() =>
            {
                AsyncConnection.DropTableAsync<TDataModel>();

                return true;
            });
        }

        /// <summary>
        /// Select every record for the data model in SQLite. The deleted records will not be included.
        /// </summary>
        /// <returns>A list of every data model saved in the SQLite-database.</returns>
        public virtual List<TDataModel> SelectAll()
        {
            return PerformQuery(() =>
            {
                return AsyncConnection.Table<TDataModel>()
                    .Where(x => x.IsDeleted == false)
                    .ToListAsync();
            });
        }

        /// <summary>
        /// Select a record by its Primary Key. This will also search inside the deleted records.
        /// </summary>
        /// <param name="idPrimaryKey">The Primary Key of the data model.</param>
        /// <returns>The data model itself. This will be NULL if no record is found with the given Primary Key.</returns>
        public virtual TDataModel SelectById(Guid idPrimaryKey)
        {
            return PerformQuery(() =>
            {
                return AsyncConnection.Table<TDataModel>()
                    .Where(x => x.Id.Equals(idPrimaryKey))
                    .FirstOrDefaultAsync();
            });
        }

        /// <summary>
        /// Insert a new data model-object in the table of SQLite.
        /// Will also update the properties "InsertTimestamp" and "UpdateTimestamp" of the given data model-object.
        /// </summary>
        /// <param name="item">The data model that needs to be inserted into SQLite.</param>
        /// <returns>Amount of records that have been inserted (should be 1).</returns>
        public virtual int Insert(TDataModel item)
        {
            return PerformQuery(() =>
            {
                DateTime now = DateTime.Now.ToLocalTime();
                item.InsertTimestamp = now;
                item.UpdateTimestamp = now;

                return AsyncConnection.InsertAsync(item);
            });
        }

        /// <summary>
        /// Insert multiple data model-objects in the table of SQLite.
        /// Will also update the properties "InsertTimestamp" and "UpdateTimestamp" for every data model-object in the given collection.
        /// </summary>
        /// <param name="items">The data model-objects that needs to be inserted into SQLite.</param>
        /// <returns>Amount of records that have been inserted.</returns>
        public virtual int InsertBatch(IEnumerable<TDataModel> items)
        {
            return PerformQuery(() =>
            {
                DateTime now = DateTime.Now.ToLocalTime();
                foreach (TDataModel item in items)
                {
                    item.InsertTimestamp = now;
                    item.UpdateTimestamp = now;
                }

                return AsyncConnection.InsertAllAsync(items);
            });
        }

        /// <summary>
        /// Update an existing data model-object in the table of SQLite.
        /// Will also update the property "UpdateTimestamp" of the given data model-object.
        /// </summary>
        /// <param name="item">The data model that needs to be updated in SQLite.</param>
        /// <returns>Amount of records that have been updated (should be 1).</returns>
        public virtual int Update(TDataModel item)
        {
            return PerformQuery(() =>
            {
                DateTime now = DateTime.Now.ToLocalTime();
                item.UpdateTimestamp = now;

                return AsyncConnection.UpdateAsync(item);
            });
        }

        /// <summary>
        /// Update multiple data model-objects in the table of SQLite.
        /// Will also update the property "UpdateTimestamp" for every data model-object in the given collection.
        /// </summary>
        /// <param name="items">The data model-objects that needs to be updated in SQLite.</param>
        /// <returns>Amount of records that have been updated.</returns>
        public virtual int UpdateBatch(IEnumerable<TDataModel> items)
        {
            return PerformQuery(() =>
            {
                DateTime now = DateTime.Now.ToLocalTime();
                foreach (TDataModel item in items)
                {
                    item.UpdateTimestamp = now;
                }

                return AsyncConnection.UpdateAllAsync(items);
            });
        }

        /// <summary>
        /// Delete an existing data model-object in the table of SQLite. By default the record itself will not
        /// be deleted, but its flag "IsDeleted" will be set to true.
        /// Will also update the property "DeletedTimestamp" of the given data model-object.
        /// </summary>
        /// <param name="item">The data model that needs to be deleted in SQLite.</param>
        /// <returns>Amount of records that have been deleted (should be 1).</returns>
        public virtual int Delete(TDataModel item)
        {
            return PerformQuery(() =>
            {
                item.DeletedTimestamp = DateTime.Now.ToLocalTime();
                item.IsDeleted = true;

                return AsyncConnection.UpdateAsync(item);
            });
        }

        /// <summary>
        /// Delete multiple data model-objects in the table of SQLite. By default the record itself will not
        /// be deleted, but its flag "IsDeleted" will be set to true.
        /// Will also update the propety "DeletedTimestamp" of the given data model-object.
        /// </summary>
        /// <param name="items">The data model-objects that needs to be deleted in SQLite.</param>
        /// <returns>Amount of records that have been deleted.</returns>
        public virtual int DeleteBatch(IEnumerable<TDataModel> items)
        {
            return PerformQuery(() =>
            {
                DateTime now = DateTime.Now.ToLocalTime();
                foreach (TDataModel item in items)
                {
                    item.DeletedTimestamp = now;
                    item.IsDeleted = true;
                }

                return AsyncConnection.UpdateAllAsync(items);
            });
        }
    }
}
