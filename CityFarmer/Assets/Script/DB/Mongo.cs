using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

public class Mongo : MonoBehaviour
{
    public static MongoClient client;
   public static IMongoDatabase database;
   
    void Start()
    {   
       
      
        
        
      
       
   
    }
    public  void MongoDBConnection()
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
  
    public  void InitMongo<T>(string DB,T type)
    {
        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(DB);

        

      
        string json = JsonUtility.ToJson(type);
        BsonDocument bson = JsonToBson(json);
       
        // ������ ����
        collection.InsertOne(bson);

        Debug.Log("Data inserted successfully!");
    }
    public void UpdateInventory<T>(Inventory inventory,T type)
    {
        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Inventory");



        Debug.Log(UserInfo.UserSeq);
       
        var filter = Builders<BsonDocument>.Filter.Eq("UserSeq", UserInfo.UserSeq);
        if()
        {

        }
        var update = Builders<BsonDocument>.Update.Set("ItemSeqs", inventory.ItemSeqs);
        var update2 = Builders<BsonDocument>.Update.Set("FoodSeqs", inventory.FoodSeqs);
        // ������ ����
        var result = collection.UpdateOne(filter, update);
        var result2 = collection.UpdateOne(filter, update2);
   
        Debug.Log("Data inserted successfully!");
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
