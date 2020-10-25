using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public List<Vector2Int> path;   // 士兵要走的路程
    public int now;                 // 当前路程索引
    public Vector3 basePos;         // 起点位置
    public House srcHouseScript;
    public GameObject dstHouse;
    public int owner;

    public float attackRate;
    public float timeDisOfMove;
    public int fogSize;             // 士兵侦察半径
    private float timeUse;          // 行走间隔 House.timeDisOfMove

    void Start()
    {
        fogSize = 2;
        timeUse = timeDisOfMove;
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Scene.instance.colorTable[owner]);
    }

    void Update()
    {
        if (Global.instance.isStop) return;
        timeUse -= Time.deltaTime;
        if (timeUse < 0)
        {
            timeUse = timeDisOfMove;
            transform.position = new Vector3(path[now].x * Scene.instance.allScale, path[now].y * Scene.instance.allScale, basePos.z + 1);

            if (owner == Global.instance.owner) // fog
            {
                int width = Scene.instance.width;
                int height = Scene.instance.height;
                for (int i = -fogSize; i <= fogSize; i++)
                    for (int j = -fogSize; j <= fogSize; j++)
                        if (i * i + j * j <= fogSize * fogSize)
                        {
                            int newX = path[now].x + i;
                            int newY = path[now].y + j;
                            if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                            {
                                Scene.instance.fogVis[newX, newY] = true;
                            }
                        }
            }

            now++;
            if (now >= path.Count)
            {
                var script = dstHouse.GetComponent<House>();
                if (script.owner != owner)
                {
                    if (attackRate >= Random.value)
                    {
                        script.value -= 2;
                        if (owner == Global.instance.owner) Global.instance.killS += 2;
                        if (script.owner == Global.instance.owner) Global.instance.lostS += 2;
                        Effect.instance.red(script);
                    }
                    else
                    {
                        script.value--;
                        if (owner == Global.instance.owner) Global.instance.killS++;
                        if (script.owner == Global.instance.owner) Global.instance.lostS++;
                    }
                    if (script.value <= 0)
                    {
                        if (script.owner == Global.instance.owner || owner == Global.instance.owner)
                        {
                            Effect.instance.shake();
                            if (owner == Global.instance.owner) Global.instance.killCas++;
                            if (script.owner == Global.instance.owner) Global.instance.lostCas++;
                        }
                        script.owner = owner;
                        script.lv = 1;
                        script.initHouse();
                    }
                }
                else script.value++;
                DestroyImmediate(gameObject);
                return;
            }
        }
    }
}
