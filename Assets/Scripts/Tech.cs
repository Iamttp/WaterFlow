using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tech : MonoBehaviour
{
    public Text[] texts;

    public delegate void mDelegate(int lv);
    Dictionary<string, mDelegate> mp;

    void hFunc(int lv)
    {

    }

    void Start()
    {
        mp = new Dictionary<string, mDelegate>();
        mp["hen"] = new mDelegate(hFunc);
        mp["hen"](1);
    }

    void Update()
    {

    }
}
