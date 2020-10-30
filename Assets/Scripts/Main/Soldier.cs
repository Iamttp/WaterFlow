using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public List<Vector2Int> path;   // 水滴要走的路程
    public int now;                 // 当前路程索引
    public Vector3 basePos;         // 起点位置
    public House srcHouseScript;
    public GameObject dstHouse;
    public int owner;

    public float attackRate;
    public float timeDisOfMove;
    public int fogSize;             // 水滴侦察半径
    private float timeUse;          // 行走间隔 House.timeDisOfMove

    public GameObject pointLig;
    public GameObject sold;

    private int sizeOfLig = 5;

    void Start()
    {
        Scene.instance.soldierSet.Add(gameObject);
        now = 0;
        fogSize = 2;
        timeUse = timeDisOfMove;
        sold.GetComponent<MeshRenderer>().material.SetColor("_Color", Scene.instance.colorTable[owner]);
        pointLig.SetActive(owner == Global.instance.owner);
    }

    void Update()
    {
        if (Global.instance.isStop) return;
        timeUse -= Time.deltaTime;
        if (timeUse < 0)
        {
            timeUse = timeDisOfMove;

            if (now >= 0 && now < path.Count)
            {
                transform.position = new Vector3(path[now].x * Scene.instance.allScale, path[now].y * Scene.instance.allScale, basePos.z + 1);

                if (owner == Global.instance.owner) // 探测
                    for (int i = -sizeOfLig; i <= sizeOfLig; i++)
                        for (int j = -sizeOfLig; j <= sizeOfLig; j++)
                        {
                            int newX = path[now].x + i;
                            int newY = path[now].y + j;
                            if (newX >= 0 && newX < Scene.instance.width && newY >= 0 && newY < Scene.instance.height)
                                Scene.instance.ligVis[newX, newY] = 2; // 持续2秒
                        }

                foreach (var obj in Scene.instance.soldierSet)
                {
                    var script = obj.GetComponent<Soldier>();
                    if (owner != script.owner && script.path[script.now] == path[now]) // 先路径位置
                    {
                        //Debug.Log("col");
                        Music.instance.playShotB();
                        Scene.instance.soldierSet.Remove(obj);
                        Scene.instance.soldierSet.Remove(gameObject);
                        DestroyImmediate(obj);
                        DestroyImmediate(gameObject);
                        return;
                    }
                }
            }
            now++;

            if (now >= path.Count)
            {
                Music.instance.playShot(); // 水滴到达目的地
                var script = dstHouse.GetComponent<House>();
                if (script.owner != owner)
                {
                    if (attackRate >= Random.value)
                    {
                        script.value -= 2;
                        if (owner == Global.instance.owner) Global.instance.killS += 2;
                        if (script.owner == Global.instance.owner) Global.instance.lostS += 2;
                        if (script.owner == Global.instance.owner || owner == Global.instance.owner)
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
                        Music.instance.playDown();
                        if (script.owner == Global.instance.owner || owner == Global.instance.owner)
                        {
                            Effect.instance.shake();
                            if (owner == Global.instance.owner) Global.instance.killCas++;
                            if (script.owner == Global.instance.owner) Global.instance.lostCas++;
                        }
                        if (script.owner == Global.instance.owner && owner != Global.instance.owner)
                            script.SendMessage("UnTouched", SendMessageOptions.DontRequireReceiver);
                        script.owner = owner;
                        script.lv = 1;
                        script.value = 0;
                        script.initHouse();
                    }
                }
                else script.value++;

                Scene.instance.soldierSet.Remove(gameObject);
                DestroyImmediate(gameObject);
                return;
            }
        }
    }
}
