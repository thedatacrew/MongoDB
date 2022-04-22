using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Configuration;
using System.Text.Json.Nodes;

namespace MongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {

        // Mongo connections
        private readonly string Connectionstring;
        private const string Database = "Demo1";
        private const string Collection = "Demo2";
        private readonly MongoClient dbClient;
        private readonly IMongoDatabase db;
        private readonly IMongoCollection<BsonDocument> collections;



        // CosmosDb
        private Database? database;
        private Container? container;
        private string databaseId = "db_tc_test_01";//"SampleDB2";
        private string containerId = "cl_tc_test_01";//"ItemContainer";
        private readonly CosmosClient cosmosClient;
        private readonly string EndpointUri;
        private readonly string PrimaryKey;

        // Generic
        private readonly IConfiguration _config;


        public DataController(IConfiguration config)
        {
            _config = config;
            Connectionstring = _config.GetConnectionString("MongoDbConnectionString");
            dbClient = new MongoClient(Connectionstring);

            EndpointUri = _config.GetConnectionString("EndPointUri");
            PrimaryKey = _config.GetConnectionString("PrimaryKey");
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions{ ApplicationName = "CosmosDBDotnetQuickstart" });

            db = dbClient.GetDatabase(Database);
            collections = db.GetCollection<BsonDocument>(Collection);

        }

        [HttpGet]
        public async Task<IActionResult> Get(string callback)
        {
            var records = new List<object>();
            //this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            //this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/LastName", 400);
            //var sqlQueryText = "SELECT * FROM c WHERE c.LastName Like 'Andersen'";
            //QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            //FeedIterator<dynamic> queryResultSetIterator = this.container.GetItemQueryIterator<dynamic>(queryDefinition);
            //while (queryResultSetIterator.HasMoreResults)
            //{
            //    var currentResultSet = await queryResultSetIterator.ReadNextAsync();
            //    Console.WriteLine("\tRead\n");
            //    foreach (var family in currentResultSet)
            //    {
            //        //var json = BsonTypeMapper.MapToDotNetValue(family);
            //        records.Add(family);
            //        Console.WriteLine("\tRead {0}\n", family);
            //    }
            //}


            var docs = collections.Find(new BsonDocument()).ToList();
            //var records = new List<object>();
            foreach (var d in docs)
            {
                var json = BsonTypeMapper.MapToDotNetValue(d);
                records.Add(json);
            }

            var rtn = string.IsNullOrEmpty(callback) ? "callback" : callback;
            return Ok(string.Concat(rtn, "(", JsonConvert.SerializeObject(records, Formatting.Indented),
                ")"));
        }

        [HttpPost]
        public IActionResult Post([FromBody] IEnumerable<datatype> data)
        {
            var entity = new BsonDocument();
            foreach (var o in data)
            {
                entity.Add(new BsonElement(o.Key, o.Value));
            }
            collections.InsertOneAsync(entity);
            return Ok("ok");
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] IEnumerable<datatype> data)
        {
           
            var inserted = collections.Find(x => true).FirstOrDefault();
            try
            {
                foreach (var o in data)
                {
                    inserted.Add(new BsonElement(o.Key, o.Value));
                }
                collections.ReplaceOne(new BsonDocument("_id", inserted["_id"]), inserted);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class datatype
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }


}
