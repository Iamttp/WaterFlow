using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public static User instance;

    public int owner;   // -1 表示完全权限

    void Start()
    {
        instance = this;
        owner = Global.instance.owner;
    }

    void Update()
    {

    }
}
