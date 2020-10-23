using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static Global instance;

    public int owner;
    public int lev;
    public float timeOfAttack;
    public float timeOfUP;

    public void DataInit()
    {
        lev = 1;
        timeOfAttack = 1f;
        timeOfUP = 2f;
    }

    void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            DataInit();
        }
    }

    void Update()
    {
    }
}
