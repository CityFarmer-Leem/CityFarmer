using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Ÿ�� ���
[System.Serializable]
public class MapTexture
{
    [SerializeField]
    List<GameObject> M_texture_prb = new List<GameObject>();

    public GameObject GetList(int index)
    {
        return M_texture_prb[index];
    }
    public int GetCountOfIndex()
    {
        return M_texture_prb.Count;
    }
}

public class Node : MonoBehaviour
{
    public Info N_info;
    public UI_MainManger N_uimain;
    public GameObject N_instower;
    public static int N_num = -1;
    public MapTexture[] N_maptexture;
    void Awake()
    {
        N_uimain = GameObject.Find("P_Main").GetComponent<UI_MainManger>();
        for (int i = 0; i < Info.Instatnce.I_node_mat.Length; i++)
        {
            if (gameObject.GetComponent<MeshRenderer>().material.color == Info.Instatnce.I_node_mat[i].color)
            {
                GameObject a = Instantiate(N_maptexture[GameSystem.Instatce.G_Stage].GetList(i), transform.position, Quaternion.identity);
                a.transform.parent = transform;
                break;
            }
        }
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    void Update()
    {
        if (N_num == -1)
        {
            if (transform.GetChild(0).GetComponent<MeshRenderer>().enabled == false)
            {
                transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }
            if (gameObject.GetComponent<MeshRenderer>().enabled == true)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            if (transform.GetChild(0).GetComponent<MeshRenderer>().enabled == true)
            {
                transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            }
            if (gameObject.GetComponent<MeshRenderer>().enabled == false)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    void OnMouseDown()// ���콺 �Է°��� �޾� Ÿ���� ��ġ�ϰ� Ÿ���� ���׸��� ����
    {
        GameObject[] T_Towers = GameObject.FindGameObjectsWithTag("Tower");
        if (T_Towers.Length != 0)
        {
            for (int j = 0; j < T_Towers.Length; j++)
            {
                T_Towers[j].GetComponent<Tower>().T_rangecheck = false;
            }
        }

        if (gameObject.GetComponent<MeshRenderer>().material.color == Info.Instatnce.I_node_mat[5].color ||
            gameObject.GetComponent<MeshRenderer>().material.color == Info.Instatnce.I_node_mat[0].color)
        {
            if (N_num != -1)
            {
                StartCoroutine(GameSystem.Instatce.ResultText(N_uimain.UI_fail_text, "�Ǽ� �Ұ����� ���� �Դϴ�."));

            }
        }
        if (gameObject.GetComponent<MeshRenderer>().material.color == Info.Instatnce.I_node_mat[2].color ||
            gameObject.GetComponent<MeshRenderer>().material.color == Info.Instatnce.I_node_mat[3].color)
        {

            if (N_num != -1)// Ÿ�� �Ǽ��� ���� �����ϸ� ����ϰ� ���� ������ ���̳����� Ÿ���� �Ǽ���
            {
                bool Notenough = false;
                //for (int i = 0; i < Info.Instatnce.I_tower_ob[N_num].transform.GetChild(0).GetComponent<Tower>().T_buygold.Length; i++)
                //{
                //    if (Info.Instatnce.I_tower_ob[N_num].transform.GetChild(0).GetComponent
                //        <Tower>().T_buygold[i] > GameSystem.Instatce.G_gold[i])
                //    {
                //        Notenough = true;
                //    }                         
                //}
                for (int i = 0; i < Info.Instatnce.I_tower_base[GameSystem.Instatce.G_choiceint].GetList(N_num).transform.GetChild(0).GetComponent<Tower>().T_buygold.Length; i++)
                {
                    if (Info.Instatnce.I_tower_base[GameSystem.Instatce.G_choiceint].GetList(N_num).transform.GetChild(0).GetComponent
                        <Tower>().T_buygold[i] > GameSystem.Instatce.G_gold[i])
                    {
                        Notenough = true;
                    }
                }
                if (!Notenough)
                {
                    N_instower = Instantiate(Info.Instatnce.I_tower_base[GameSystem.Instatce.G_choiceint].GetList(N_num), new Vector3(gameObject.transform.
                       position.x, gameObject.transform.position.y, gameObject.transform.position.z),
                       Info.Instatnce.I_tower_base[GameSystem.Instatce.G_choiceint].GetList(N_num).transform.rotation);
                    Transform tr = transform;
                    N_instower.transform.parent = tr;

                    for (int j = 0; j < Info.Instatnce.I_tower_base[GameSystem.Instatce.G_choiceint].GetList(N_num).transform.GetChild(0).GetComponent<Tower>().T_buygold.Length; j++)
                    {
                        GameSystem.Instatce.G_gold[j] -= Info.Instatnce.I_tower_base[GameSystem.Instatce.G_choiceint].GetList(N_num).transform.GetChild(0).GetComponent
                        <Tower>().T_buygold[j];
                    }
                    N_num = -1;
                }


            }
            if (N_instower != null)
            {
                gameObject.GetComponent<MeshRenderer>().material = Info.Instatnce.I_node_mat[5];
            }
            else
                return;
        }
        if (gameObject.GetComponent<MeshRenderer>().material.color == Info.Instatnce.I_node_mat[3].color)
        {
            if (N_num != -1)
            {
                bool Notenough = false;
                for (int i = 0; i < Info.Instatnce.I_tower_base[GameSystem.Instatce.G_choiceint].GetList(N_num).transform.GetChild(0).GetComponent<Tower>().T_buygold.Length; i++)
                {
                    if (Info.Instatnce.I_tower_base[GameSystem.Instatce.G_choiceint].GetList(N_num).transform.GetChild(0).GetComponent
                        <Tower>().T_buygold[i] > GameSystem.Instatce.G_gold[i])
                    {
                        Notenough = true;
                    }
                }
                if (!Notenough)
                {
                    N_instower = Instantiate(Info.Instatnce.I_tower_base[GameSystem.Instatce.G_choiceint].GetList(N_num), new Vector3(gameObject.transform.
                       position.x, gameObject.transform.position.y, gameObject.transform.position.z),
                       Info.Instatnce.I_tower_base[GameSystem.Instatce.G_choiceint].GetList(N_num).transform.rotation);
                    Transform tr = transform;
                    N_instower.transform.parent = tr;

                    for (int j = 0; j < Info.Instatnce.I_tower_base[GameSystem.Instatce.G_choiceint].GetList(N_num).transform.GetChild(0).GetComponent<Tower>().T_buygold.Length; j++)
                    {
                        GameSystem.Instatce.G_gold[j] -= Info.Instatnce.I_tower_base[GameSystem.Instatce.G_choiceint].GetList(N_num).transform.GetChild(0).GetComponent
                        <Tower>().T_buygold[j];
                    }
                    Info.Instatnce.I_tower_base[GameSystem.Instatce.G_choiceint].GetList(N_num).transform.GetChild(0).GetComponent<Tower>().T_Ats =
                        Info.Instatnce.I_tower_base[GameSystem.Instatce.G_choiceint].GetList(N_num).transform.GetChild(0).GetComponent<Tower>().T_Ats / 2;
                    N_num = -1;

                }
                if (N_instower != null)
                {
                    gameObject.GetComponent<MeshRenderer>().material = Info.Instatnce.I_node_mat[5];
                }
                else
                    return;
            }

        }

    }
}