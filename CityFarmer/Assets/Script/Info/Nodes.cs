using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes : MonoBehaviour
{
    public object _id;
    public int LandSeq;
    public int UserSeq;
    //����Ʈ�� {foodseq, currenttime, state}�� �����Ǿ�����
    //List<int> = { 1,220,2} Ǫ�� 1, 220�� ����, �۹��� �⸣�� �� 
    public List<List<int>> Lands;
    List<Nodes> node = new List<Nodes>();
    

}