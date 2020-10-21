using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    private float hitTimeOfAttack = 0f;
    private float hitTimeOfUP = 0f;

    void Start()
    {

    }

    void Update()
    {
        var obj = Scene.instance;

        hitTimeOfAttack += Time.deltaTime;
        if (hitTimeOfAttack > 0.1f)
        {
            hitTimeOfAttack = 0f;
            int i = Random.Range(0, obj.housePosArray.Count);
            int k = Random.Range(0, obj.housePosArray.Count);
            if (i != k)
            {
                obj.posToHouse[obj.housePosArray[i]].GetComponent<House>().JustAttack(obj.posToHouse[obj.housePosArray[k]]);
            }

        }

        hitTimeOfUP += Time.deltaTime;
        if (hitTimeOfUP > 0.5f)
        {
            hitTimeOfUP = 0f;
            var obj2 = obj.posToHouse[obj.housePosArray[Random.Range(0, obj.housePosArray.Count)]].GetComponent<House>();
            if (obj2.lv < obj2.maxLv && obj2.value >= obj2.maxValue)
            {
                obj2.value -= obj2.maxValue;
                obj2.maxValue = ++obj2.lv * obj2.perValue;
            }
        }
    }
}
