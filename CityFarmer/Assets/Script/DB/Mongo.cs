using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using System.Reflection;
using System.Threading.Tasks;

public class Mongo : MonoBehaviour
{
    public static MongoClient client;
   public static IMongoDatabase database;
 
    public void MongoDBConnection()
    {
        string connectionString = "mongodb+srv://myUser:dnflskfk1@mydb.qgumh09.mongodb.net/MyDB?retryWrites=true&appName=AtlasApp";
        // MongoClient ����
        client = new MongoClient(connectionString);
        // �����ͺ��̽� ����
        database = client.GetDatabase("MyDB");
    }

    // ���⿡�� ���� ���� �� MongoDB �۾� ����
    public  List<BsonDocument> LoadMongo(string DB)
    {
        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(DB);
        var filter = Builders<BsonDocument>.Filter.Eq("UserSeq", UserInfo.UserSeq);
        List<BsonDocument> result = collection.Find(filter).ToList();
        return result;
    }

    public static void InitMongoInventory(Inventory inventory)
    {
        Debug.Log("Start InitMongoInventory");

        var collection = database.GetCollection<BsonDocument>(inventory.GetType().Name);

        var document = new BsonDocument { { "UserSeq", inventory.UserSeq }, { "ItemSeqs", new BsonArray( inventory.ItemSeqs) },{"FoodSeqs",new BsonArray(inventory.FoodSeqs )},
            {"ItemValues",new BsonArray(inventory.ItemValues) },{"FoodValues",new BsonArray(inventory.FoodValues) }  };
        collection.InsertOneAsync(document);
    }
    public static void InitMongoEncyclopedia(Encyclopedia encyclopedia)
    {
        Debug.Log("Start InitMongoEncyclopedia");

        var collection = database.GetCollection<BsonDocument>(encyclopedia.GetType().Name);

        var document = new BsonDocument { { "UserSeq", encyclopedia.UserSeq }, {"FoodSeqs",new BsonArray(encyclopedia.FoodSeqs ) }};
        collection.InsertOneAsync(document);
    }
    public void UpdateMongo<T>(Inventory inventory,Encyclopedia encyclopedia,T type)
    {
        string typetext = type.GetType().ToString();
        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(typetext);
        var filter = Builders<BsonDocument>.Filter.Eq("UserSeq", UserInfo.UserSeq);

        switch (typetext)
        {
            case "Inventory":
                var InventoryItemupdate = Builders<BsonDocument>.Update.Set("ItemSeqs", inventory.ItemSeqs);
                var InventoryFoodupdate = Builders<BsonDocument>.Update.Set("FoodSeqs", inventory.FoodSeqs);
                var InventoryItemValueupdate = Builders<BsonDocument>.Update.Set("ItemValues", inventory.ItemValues);
                var InventoryFoodValueupdate = Builders<BsonDocument>.Update.Set("FoodValues", inventory.FoodValues);
                // ������ ����
                var inventoryItemResult = collection.UpdateOne(filter, InventoryItemupdate);
                var inventoryFoodResult = collection.UpdateOne(filter, InventoryFoodupdate);
                var inventoryItemValueResult = collection.UpdateOne(filter, InventoryItemValueupdate);
                var inventoryFoodValueResult = collection.UpdateOne(filter, InventoryFoodValueupdate); break;
            case "Encyclopedia":
                var EncyclopediaFoodupdate = Builders<BsonDocument>.Update.Set("FoodSeqs", encyclopedia.FoodSeqs);            
                var EncyclopediaFoodresult = collection.UpdateOne(filter, EncyclopediaFoodupdate);
                break;
        }
          
        Debug.Log("Data Update successfully!");
    }
    public  BsonDocument JsonToBson(string jsonString)
    {
        // JSON ���ڿ��� BsonDocument�� ��ȯ
        using (JsonReader reader = new JsonReader(jsonString))
        {
            BsonDocument bsonDocument = BsonSerializer.Deserialize<BsonDocument>(reader);
            return bsonDocument;
        }
    }
}
