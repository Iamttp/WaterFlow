using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public List<Vector2Int> path;   // 士兵要走的路程
    public int now;                 // 当前路程索引
    public Vector3 basePos;         // 起点位置
    public GameObject dstHouse;
    public int owner;
    private float timeUse;          // 行走间隔 House.timeDisOfMove

    void Start()
    {
        timeUse = House.timeDisOfMove;
        gameObject.GetComponent<MeshRenderer>().material.color = Scene.instance.colorTable[owner];
    }

    void Update()
    {
        timeUse -= Time.deltaTime;
        if (timeUse < 0)
        {
            timeUse = House.timeDisOfMove;
            transform.position = new Vector3(path[now].x * Scene.instance.allScale, path[now].y * Scene.instance.allScale, basePos.z + 1);
            now++;
            if (now >= path.Count)
            {
                var script = dstHouse.GetComponent<House>();
                if (script.owner != owner)
                {
                    script.value--;
                    if(script.value <= 0)
                    {
                        script.owner = owner;
                        script.lv = 1;
                        script.maxValue = script.lv * script.perValue;
                    }
                }
                else script.value++;
                DestroyImmediate(gameObject);
                return;
            }
        }
    }
}
