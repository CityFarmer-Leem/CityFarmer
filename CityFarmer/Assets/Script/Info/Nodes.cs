using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Nodes
{
    public object _id;
    public int LandSeq;
    public int UserSeq;
    //����Ʈ�� {foodseq, currenttime, state}�� �����Ǿ�����
    //List<int> = { 1,220,2} Ǫ�� 1, 220�� ����, �۹��� �⸣�� �� 
    public List<List<int>> Lands;


}
