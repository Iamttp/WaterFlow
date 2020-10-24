using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Computer : MonoBehaviour
{
    public static Computer instance;

    public Text text;

    public float hitTimeOfAttack = 0f;
    public float hitTimeOfUP = 0f;

    void Start()
    {
        instance = this;
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
        var obj = Scene.instance;

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
        }

        hitTimeOfUP += Time.deltaTime;
        if (hitTimeOfUP > Global.instance.timeOfUP)
        {
            hitTimeOfUP = 0f;
            int i = Random.Range(0, obj.housePosArray.Count);
            if (obj.posToHouse[obj.housePosArray[i]].GetComponent<House>().owner == Global.instance.owner) return;
            var obj2 = obj.posToHouse[obj.housePosArray[i]].GetComponent<House>();
            if (obj2.lv < obj2.maxLv && obj2.value >= obj2.maxValue)
            {
                obj2.value -= obj2.maxValue;
                obj2.maxValue = ++obj2.lv * obj2.perValue;
            }
        }
    }
}
