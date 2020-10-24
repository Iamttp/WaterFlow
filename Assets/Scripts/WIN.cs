using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WIN : MonoBehaviour
{
    public Text text;
    void Start()
    {
        text.text = "YOU WIN !" +
            "\nUse Time : " + Global.instance.useTime + "s" +
            "\nSiege Cities ： " + Global.instance.killCas +
            "\nLost Cities ： " + Global.instance.lostCas +
            "\nKill Soldiers ： " + Global.instance.killS +
            "\nLost Soldiers ： " + Global.instance.lostS;
    }

    void Update()
    {

    }
}
