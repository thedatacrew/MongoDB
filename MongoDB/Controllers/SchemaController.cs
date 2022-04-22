using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace MongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemaController : ControllerBase
    {
        private const string Database = "Demo1";
        private const string Collection = "Demo2";
        private readonly MongoClient dbClient;
        private readonly IMongoDatabase db;
        private readonly IMongoCollection<BsonDocument> collections;
        private readonly IConfiguration _config;

        public SchemaController(IConfiguration config)
        {
            _config = config;
            dbClient = new MongoClient(_config.GetConnectionString("MongoDbConnectionString"));
            db = dbClient.GetDatabase(Database);
            collections = db.GetCollection<BsonDocument>(Collection);
        }

        [HttpGet]
        public IActionResult Get(string callback)
        {
            var docs = collections.Find(new BsonDocument()).ToList();

            var records = new List<object>();
            var json = BsonTypeMapper.MapToDotNetValue(docs.FirstOrDefault());
            records.Add(json);
            var x = db.ListCollections().FirstOrDefault().AsBsonDocument;
            var rtn = string.IsNullOrEmpty(callback) ? "callback" : callback;
            return Ok(string.Concat(rtn, "(", JsonConvert.SerializeObject(records, Formatting.Indented), ")"));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Datatype data)
        {
            try
            {
                var filter = new FilterDefinitionBuilder<BsonDocument>().Eq("ProductID", 1);
                var update = new UpdateDefinitionBuilder<BsonDocument>().Set(data.Key, data.Value);
                var status = await collections.UpdateOneAsync(filter, update);
                if (status.MatchedCount > 0)
                {
                    return Ok();
                }
                return BadRequest("Unable to find document");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] FieldDatatype data)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Exists(data.Field);
                var updateDefinition = new BsonDocument("$set", new BsonDocument(data.Field, false));
                if (data.AllRecords)
                {
                    await collections.UpdateManyAsync(filter, updateDefinition);
                }
                else
                {
                    await collections.UpdateOneAsync(filter, updateDefinition);
                }

                AddUpdateToMessageQueue(data, "renameColumnQueue");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] FieldDatatype data)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Exists(data.Field);
                var updateDefinition = new BsonDocument("$unset", new BsonDocument(data.Field, false));
                if (data.AllRecords)
                {
                    await collections.UpdateManyAsync(filter, updateDefinition);
                }
                else
                {
                    await collections.UpdateOneAsync(filter, updateDefinition);
                }

                AddUpdateToMessageQueue(data, "deleteColumnQueue");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private void AddUpdateToMessageQueue(FieldDatatype data, string command)
        {
            
        }


        public struct Datatype
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public struct FieldDatatype
        {
            public string Field { get; set; }
            public bool AllRecords { get; set; }
        }

    }


}
