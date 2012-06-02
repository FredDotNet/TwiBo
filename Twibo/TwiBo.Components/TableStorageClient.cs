using System;
using System.Linq;
using System.Data.Services.Client;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace TwiBo.Components
{
    public class TableStorageClient<TEntity>
        where TEntity : TableServiceEntity
    {
        private string tableName;
        private TableServiceContext _context;

        public TableStorageClient(string table)
        {
            tableName = table;
            CloudStorageAccount storageAccount = CloudStorageAccount.FromConfigurationSetting("twibostorage");
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the table if it doesn't exist
            tableClient.CreateTableIfNotExist(table);

            // Set context
            _context = tableClient.GetDataServiceContext();
            _context.IgnoreResourceNotFoundException = true;
        }

        public IQueryable<TEntity> CreateQuery()
        {
            DataServiceQuery<TEntity> query = _context.CreateQuery<TEntity>(tableName);
            return query.AsTableServiceQuery();
        }
        
        public void Insert(TEntity entity)
        {
            _context.AddObject(tableName, entity);
        }

        public void Update(TEntity entity)
        {
            _context.UpdateObject(entity);
        }

        public void Upsert(object entity)
        {
            _context.AttachTo(tableName, entity);
            _context.UpdateObject(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.DeleteObject(entity);
        }

        public DataServiceResponse SaveChanges()
        {
            return _context.SaveChanges(SaveChangesOptions.ReplaceOnUpdate);
        }
    }
}
