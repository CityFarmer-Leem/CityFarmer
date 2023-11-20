using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using UnityEngine;
using System.Linq;
using MongoDB.Bson;



public class InfoManager : MonoBehaviour
{
    public UserInfo UserInfo;
    public Money Money;
    public Land Land;
    public List<Item> Items = new List<Item>();
    public List<Food> Foods = new List<Food>();
    private static InfoManager _instance;
    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static InfoManager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(InfoManager)) as InfoManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // �Ʒ��� �Լ��� ����Ͽ� ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.
        DontDestroyOnLoad(gameObject);
    }
    public void InsertMoney()
    {
        StartSQL();
        string query = "INSERT INTO MONEY ( USER_SEQ, MONEY_GOLD,MONEY_RUBY )VALUES('"+UserInfo.UserSeq+"',0,0)";

        Maria.OnInsertOrUpdateRequest(query);
        Maria.SqlConnection.Close();
    }
    public void LoadMoney()
    {
        StartSQL();
        string query = "SELECT * FROM MONEY WHERE USER_SEQ = '"+UserInfo.UserSeq+"'" ;
        DataSet dataSet = Maria.OnSelectRequest(query,"MONEY");
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dataSet.GetXml());
        if (xmlDocument != null)
        {
            XmlNodeList data = xmlDocument.SelectNodes("NewDataSet/MONEY");
         

            foreach (XmlNode node in data)
            {
                Money.moneyGold = System.Convert.ToInt32(node.SelectSingleNode("MONEY_GOLD").InnerText);
                Money.moneyRuby = System.Convert.ToInt32(node.SelectSingleNode("MONEY_RUBY").InnerText);
            }
        }
        Maria.SqlConnection.Close();    
    }
    public T FindBySeq<T>(List<T> TList, int Seq)
    {
        // LINQ�� ����Ͽ� itemSeq�� ��ġ�ϴ� Item ã��
        T found = TList.FirstOrDefault(t => TypeSeq(t) == Seq);
        return found;
    }
    public int TypeSeq<T>(T t)
    {

        string type = t.ToJson().Split("Name")[0];
        string typeSeq = "";

        for (int i = 0; i < type.Length; i++)
        {
            char ch = type[i];
            if ('0' <= ch && ch <= '9')
            {
                typeSeq += ch;
            }
        }
        return int.Parse(typeSeq);
    }


    private XmlDocument OnLoadData(string DB)
    {
        StartSQL();
        string query = "SELECT * FROM " + DB;
        DataSet dataSet = Maria.OnSelectRequest(query, DB);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dataSet.GetXml());
        return xmlDocument;
    }
    private void StartSQL()
    {
        try
        {
            Maria.SqlConnection = new MySqlConnection(Maria.strConnection);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    public void LoadFood()
    {
        Foods.Clear();
        XmlDocument xmlDocument = OnLoadData("FOOD");

        if (xmlDocument != null)
        {
            XmlNodeList data = xmlDocument.SelectNodes("NewDataSet/FOOD");
            int foodCount = 0;

            foreach (XmlNode node in data)
            {
                foodCount++;
                Food food = new Food();
                food.FoodSeq = System.Convert.ToInt32(node.SelectSingleNode("FOOD_SEQ").InnerText);
                food.FoodName = node.SelectSingleNode("FOOD_NAME").InnerText;
                food.FoodText = node.SelectSingleNode("FOOD_TEXT").InnerText;
                food.FoodPrice = System.Convert.ToInt32(node.SelectSingleNode("FOOD_PRICE").InnerText);

                string time = node.SelectSingleNode("FOOD_TIME").InnerText;
                food.FoodTime = time.Split("T")[1];
                food.FoodLevel = System.Convert.ToInt32(node.SelectSingleNode("FOOD_LEVEL").InnerText);
                food.FoodSpriteString = node.SelectSingleNode("FOOD_SPRITE").InnerText;
                food.FoodSprite = food.foodSprite();

                string type = node.SelectSingleNode("FOOD_TYPE").InnerText;
                switch (type)
                {
                    case "Plant": food.foodtype = Food.Foodtype.Plant; break;
                    case "Meat": food.foodtype = Food.Foodtype.Meat; break;
                }
                Foods.Add(food);
            }
        }
        Maria.SqlConnection.Close();
    }

    public void LoadItem()
    {
        Items.Clear();
        XmlDocument xmlDocument = OnLoadData("ITEM");

        if (xmlDocument != null)
        {
            XmlNodeList data = xmlDocument.SelectNodes("NewDataSet/ITEM");
            int itemCount = 0;

            foreach (XmlNode node in data)
            {
                itemCount++;
                Item item = new Item();
                item.ItemSeq = System.Convert.ToInt32(node.SelectSingleNode("ITEM_SEQ").InnerText);
                item.ItemName = node.SelectSingleNode("ITEM_NAME").InnerText;
                item.ItemText = node.SelectSingleNode("ITEM_TEXT").InnerText;
                item.ItemPrice = System.Convert.ToInt32(node.SelectSingleNode("ITEM_PRICE").InnerText);
                item.ItemSpriteString = node.SelectSingleNode("ITEM_SPRITE").InnerText;
                item.ItemSprite = item.itemSprite();

                string type = node.SelectSingleNode("ITEM_TYPE").InnerText;

                switch (type)
                {
                    case "disposable": item.itemtype = Item.Itemtype.Disposable; break;
                    case "costume": item.itemtype = Item.Itemtype.Costume; break;

                }

                item.ItemMoney = System.Convert.ToBoolean(node.SelectSingleNode("ITEM_MONEY").InnerText);
                Items.Add(item);
            }
        }
        Maria.SqlConnection.Close();
    }

    public bool Login(string InfoUserId, string InfoUserPassword)
    {
        StartSQL();
        UserInfo.UserSeq = 0;
        //�α��� ����
        string query = "SELECT * FROM USER WHERE USER_ID = '" + InfoUserId + "' AND USER_PASSWORD = '" + InfoUserPassword + "'";
        DataSet dataSet = Maria.OnSelectRequest(query, "USER");

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dataSet.GetXml());

        if (xmlDocument != null)
        {
            XmlNodeList user = xmlDocument.SelectNodes("NewDataSet/USER");
          
            foreach (XmlNode node in user)
            {
                
                UserInfo.UserSeq = System.Convert.ToInt32(node.SelectSingleNode("USER_SEQ").InnerText);
                UserInfo.UserLevel = System.Convert.ToInt32(node.SelectSingleNode("USER_LEVEL").InnerText);
                UserInfo.UserId = node.SelectSingleNode("USER_ID").InnerText;
                UserInfo.UserName = node.SelectSingleNode("USER_NAME").InnerText;
                UserInfo.UserReg = node.SelectSingleNode("USER_REG").InnerText;
            }
        }
        if (UserInfo.UserSeq == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
        Maria.SqlConnection.Close();
    }

    public void SignUp(string InfoUserId, string InfoUserPassword, string InfoUserName)
    {
        StartSQL();
        string query = "SELECT * FROM USER WHERE USER_ID = '" + InfoUserId + "'";
        DataSet dataSet = Maria.OnSelectRequest(query, "USER");

        if (dataSet.GetXml().IndexOf("SEQ") >= 0)
        {
            Debug.Log("�̹� ���Ե� ���̵��Դϴ�.");
        }
        else
        {
            //ȸ������ ����
            query = "INSERT INTO USER ( USER_ID, USER_LEVEL, USER_NAME, USER_PASSWORD )VALUES('" + InfoUserId + "', '1', '" + InfoUserName + "','" + InfoUserPassword + "')";
            if (Maria.OnInsertOrUpdateRequest(query))
            {
                Debug.Log("ȸ�����Կ� �����Ͽ����ϴ�.");
            }
            else
            {
                Debug.Log("ȸ�����Կ� �����Ͽ����ϴ�.");
            }
        }
    }
}
