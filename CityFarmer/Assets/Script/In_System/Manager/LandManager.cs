using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LandManager : MonoBehaviour
{
    public Tilemap Tilemap;
    public Grid grid;
    public List<Vector3Int> Vector3MinPostion;
    public List<Vector3Int> Vector3MaxPostion;
    public NodeManager NodeManager;
    public List<Node> Nodes;
    private void Awake()
    {
        // ��� ���� ���� �ҷ�����
        for (int i = 0; i < 8; i++)
        {
            Vector3Int minPosition = new Vector3Int(-8 + (i * 4), -3, 0);
            Vector3Int maxPosition = new Vector3Int(-6 + (i * 4), -1, 0);
            Vector3MinPostion.Add(minPosition);
            Vector3MaxPostion.Add(maxPosition);
        }
   
    }
    private void Start()
    {
        LoadLand();

    }

    public void LoadLand()
    {
      
        for (int currentland = 0; currentland < NodeManager.Nodes.Count; currentland++)
        {
            coordinate(currentland);
        }


    }
    private void coordinate(int land)
    {
        Vector3Int minPosition = Vector3MinPostion[land]; // Ư�� ������ �ּ� ��ǥ
        Vector3Int maxPosition = Vector3MaxPostion[land]; // Ư�� ������ �ִ� ��ǥ
        List<Vector3Int> vector3s = new List<Vector3Int>();
        for (int x = minPosition.x; x <= maxPosition.x; x++)
        {

            for (int y = minPosition.y; y <= maxPosition.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                vector3s.Add(tilePosition);
              

            }
        }
        for (int tileCount = 0; tileCount< vector3s.Count; tileCount++)
        {
            Node node = new Node(vector3s[tileCount], NodeManager.Nodes[land].Lands[tileCount][0], NodeManager.Nodes[land].Lands[tileCount][1]);
            node.State = (Node.NodeState)NodeManager.Nodes[land].Lands[tileCount][2];
            node.SetNodeTile();
            Nodes.Add(node);
            Tilemap.SetTile(node.GetPosition(), node.GetStateNodeTile()); // Ÿ�� ����
        }
    }
    public int ConvertTimer(string inputTime)
    {
        // "PT3H40M20S" ������ ���ڿ��� TimeSpan���� �Ľ�
        TimeSpan timeSpan = XmlConvert.ToTimeSpan(inputTime);

        // �ð� ���� "3:40:20"���� ��ȯ
        int timer = (int)timeSpan.TotalHours * 3600 + timeSpan.Minutes * 60 + timeSpan.Seconds;

        return timer;
    }
    public string ConvertString(int inputTime)
    {
        string timer = "";
        int h, m, s;
        h = inputTime / 3600;
        m = inputTime / 60 %60;
        s = inputTime % 60;
        timer += h.ToString()+":";
        timer += m.ToString()+ ":";
        timer += s.ToString();
        return timer;
    }
    public string ConvertTimeString(string inputTime)
    {
       
        TimeSpan timeSpan = XmlConvert.ToTimeSpan(inputTime);

    
        string formattedTime = $"{(int)timeSpan.TotalHours}:{timeSpan.Minutes}:{timeSpan.Seconds}";

        return formattedTime;
    }
    public void LeveUPChangeLandTile(int LandLevel)
    {
        coordinate(LandLevel - 1);
        string query = "INSERT INTO LAND ( USER_SEQ )VALUES('" + UserInfo.UserSeq + "')";
        Maria.OnInsertOrUpdateRequest(query);
    }

}
