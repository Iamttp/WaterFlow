using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LOST : MonoBehaviour
{
    public Text text;
    static string[] diffs = new string[] { "NORMAL", "HARD", "EASY", "CRAZY" };
    static string[] size2s = new string[] { "MIDDLE", "LARGE", "SMALL", "EXTRA LARGE" };
    static string[] size1s = new string[] { "MIDDLE", "MANY", "LITTLE", "GREAT MANY" };
    
    void Start()
    {
        text.text = "YOU LOST !" +
            "\nDiff LEVEL: " + diffs[Global.instance.diff] +
            "\nMap Size: " + size2s[Load.index2] +  // 城堡数
            "\nHouse Size: " + size1s[Load.index1] +
            "\n\nUse Time : " + (int)(Global.instance.useTime) + "s" +
            "\nSiege Cities ： " + Global.instance.killCas +
            "\nLost Cities ： " + Global.instance.lostCas +
            "\nKill Soldiers ： " + Global.instance.killS +
            "\nLost Soldiers ： " + Global.instance.lostS;
    }

    void Update()
    {

    }
}
