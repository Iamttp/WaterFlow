using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Computer : MonoBehaviour
{
    public static Computer instance;

    public Color[] colors;
    public Text text;
    public Text score;

    public float hitTimeOfAttack = 0f;
    public float hitTimeOfUP = 0f;
    public float fogTime;
    void Start()
    {
        instance = this;
        fogTime = 0.1f;
    }

    //将秒数转化为时分秒
    private string sec3Hms(long duration)
    {
        System.TimeSpan ts = new System.TimeSpan(0, 0, System.Convert.ToInt32(duration));
        string str = "";
        if (ts.Hours > 0)
            str = string.Format("{0:00}", ts.Hours) + ":" + string.Format("{0:00}", ts.Minutes) + ":" + string.Format("{0:00}", ts.Seconds);
        if (ts.Hours == 0 && ts.Minutes > 0)
            str = "00:" + string.Format("{0:00}", ts.Minutes) + ":" + string.Format("{0:00}", ts.Seconds);
        if (ts.Hours == 0 && ts.Minutes == 0)
            str = "00:00:" + string.Format("{0:00}", ts.Seconds);
        return str;
    }

    void Update()
    {
        if (Global.instance.isStop) return;
        Global.instance.useTime += Time.deltaTime;
        int timeInt = (int)(Global.instance.useTime);
        text.text = sec3Hms(timeInt);
        Scene obj = Scene.instance;

        hitTimeOfAttack += Time.deltaTime;
        if (hitTimeOfAttack > Global.instance.timeOfAttack)
        {
            hitTimeOfAttack = 0f;
            int haveTest = 3; // 尝试3次
            while (haveTest-- > 0)
            {
                int i = Random.Range(0, obj.housePosArray.Count);
                int k = Random.Range(0, obj.housePosArray.Count);
                if (obj.posToHouse[obj.housePosArray[k]].GetComponent<House>().owner != Global.instance.owner && i != k)
                {
                    obj.posToHouse[obj.housePosArray[i]].GetComponent<House>().JustAttack(obj.posToHouse[obj.housePosArray[k]]);
                    break;
                }
            }
            Scene.instance.FogTest();
        }

        hitTimeOfUP += Time.deltaTime;
        if (hitTimeOfUP > Global.instance.timeOfUP)
        {
            hitTimeOfUP = 0f;
            int i = Random.Range(0, obj.housePosArray.Count);
            House obj2 = obj.posToHouse[obj.housePosArray[i]].GetComponent<House>();
            if (obj2.owner == Global.instance.owner) return;
            if (obj2.lv < obj2.maxLv && obj2.value >= obj2.maxValue)
            {
                obj2.lv++;
                obj2.value -= obj2.maxValue;
                obj2.initHouse();
            }
        }


        fogTime -= Time.deltaTime;
        if (fogTime < 0)
        {
            fogTime = 0.1f;

            // 分数计算算法
            int scoreNum = Global.instance.getScore();
            score.text = "Score : " + scoreNum;
            int index = scoreNum / 10000;
            score.fontSize = 50 + 10 * (index / colors.Length);
            score.color = colors[index % colors.Length];

            Scene.instance.FogTest();
        }
    }
}
