using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserInfo : MonoBehaviour
{
    public static int UserSeq;
    public string UserId { get;  set; }
    public int UserLevel { get;  set; }
    public string UserName { get;  set; }
    public string UserEncyclopedia { get;  set; }
    public string UserReg { get;  set; }

    public int UserLandLevel { get; set; }

    public Land[] Lands = new Land[9];

   

}
