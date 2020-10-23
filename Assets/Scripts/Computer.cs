using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public static Computer instance;
    public float hitTimeOfAttack = 0f;
    public float hitTimeOfUP = 0f;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (Scene.instance.isStop) return;
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
                if (obj.posToHouse[obj.housePosArray[k]].GetComponent<House>().owner != User.instance.owner && i != k)
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
            if (obj.posToHouse[obj.housePosArray[i]].GetComponent<House>().owner == User.instance.owner) return;
            var obj2 = obj.posToHouse[obj.housePosArray[i]].GetComponent<House>();
            if (obj2.lv < obj2.maxLv && obj2.value >= obj2.maxValue)
            {
                obj2.value -= obj2.maxValue;
                obj2.maxValue = ++obj2.lv * obj2.perValue;
            }
        }
    }
}
