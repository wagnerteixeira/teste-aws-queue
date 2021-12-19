using System;
using Amazon.SQS;
using Amazon.SQS.Model;
using Shared.Models;
using Data.Repositories.Interfaces;
using Data.Repositories.Models;
using Microsoft.Extensions.Logging;
using Amazon;

namespace Data.Repositories
{
    public class AwsDynamoRepository : IAwsDynamoRepository
    {
        public AwsDynamoRepository()
        {
        }

        public static async Task<bool> SaveDocumentAsync(Guid id)
        {
            var client = new AmazonDynamoDBClient();
            var result = false;
            try
            {
                var request1 = new PutItemRequest
                {
                    TableName = "DocumentTeste",
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "Id", new AttributeValue { N = id }},
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAILED to write the new document, because:\n       {0}.", ex.Message);
            }

            return result;
        }
    }
}
