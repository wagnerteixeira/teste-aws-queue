using Data.Repositories.Interfaces;
using Data.Repositories.Models;
using Microsoft.Extensions.Logging;
using Amazon.DynamoDBv2.DataModel;
using Shared.Models;

namespace Data.Repositories
{
    public class AwsDynamoRepository<T> : IAwsDynamoRepository<T> 
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger<AwsRepository> _logger;
        private readonly AppSettings _appSettings;
        public AwsDynamoRepository(ILogger<AwsRepository> logger, 
            IDynamoDBContext dynamoDbContext,
            AppSettings appSettings)
        {
            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
            _appSettings = appSettings;
        }

        public async Task SaveMessageAsync(string id, T message)
        {
            try
            {
                var operationConfig = new DynamoDBOperationConfig()
                {
                    OverrideTableName = _appSettings.DynamoDbTable
                };
                await _dynamoDbContext.SaveAsync(new MessageModel<T>(id, message), operationConfig);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with send dynamo message", message);
                throw;
            }
        }
        
        public async Task DeleteMessageAsync(string id)
        {
            try
            {
                var operationConfig = new DynamoDBOperationConfig()
                {
                    OverrideTableName = _appSettings.DynamoDbTable
                };
                await _dynamoDbContext.DeleteAsync<MessageModel<T>>(id, operationConfig);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with delete dynamo message", id);
                throw;
            }
        }
        
        public async Task BatchSaveMessageAsync(IEnumerable<(string id, T message)> messages)
        {
            try
            {
                var operationConfig = new DynamoDBOperationConfig()
                {
                    OverrideTableName = _appSettings.DynamoDbTable
                };
                var batch = _dynamoDbContext.CreateBatchWrite<MessageModel<T>>(operationConfig);

                var messagesModel = messages.Select(message => new MessageModel<T>(message.id, message.message));
                
                batch.AddPutItems(messagesModel);
                await batch.ExecuteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with send batch dynamo messages", messages);
                throw;
            }
        }
        
        public async Task BatchDeleteMessageAsync(List<string> ids)
        {
            try
            {
                var operationConfig = new DynamoDBOperationConfig()
                {
                    OverrideTableName = _appSettings.DynamoDbTable
                };
                var batch = _dynamoDbContext.CreateBatchWrite<MessageModel<T>>(operationConfig);
                
                ids.ForEach(id => batch.AddDeleteKey(id));

                await batch.ExecuteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with delete batch dynamo messages", ids);
                throw;
            }
        }
    }
}
