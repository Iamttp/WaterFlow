using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [Header("House属性")]
    public int value;                           // 城堡生命值
    public int perValue;
    public int maxValue;                        // 最大生命值
    public int lv;                              // 等级
    public int maxLv;
    public int owner;

    public float timeDisOfAdd;    // 城堡生命值增加间隔
    public float timeDisOfMove;   // 士兵移动时间间隔

    [Header("对应buff")]
    public float sizeRate;        // 最大生命值比例
    public float addRate;
    public float moveRate;
    public float attackRate;      // 暴击比例

    public GameObject player;                   // save player
    public int index;                           // houseArray index

    private float timeUse;

    public Vector2Int pos;
    public int fogSize;             // 士兵侦察半径

    public void initHouse()
    {
        value = 0;
        if (owner == Global.instance.owner)
        {
            sizeRate = Global.instance.buffOfSize;
            addRate = Global.instance.buffOfAddSpeed;
            moveRate = Global.instance.buffOfMoveSpeed;
            attackRate = Global.instance.buffOfAttack;
        }
        else
        {
            sizeRate = 1;
            addRate = 1;
            moveRate = 1;
            attackRate = 0;
        }
        maxValue = Mathf.RoundToInt(lv * perValue * sizeRate);
        timeDisOfAdd = 0.9f / addRate;
        timeDisOfMove = 0.1f / moveRate;
    }

    void Start()
    {
        fogSize = 5;
        lv = 1;
        initHouse();
    }

    void Update()
    {
        if (Global.instance.isStop) return;
        timeUse -= Time.deltaTime;
        if (timeUse <= 0)
        {
            timeUse = timeDisOfAdd;
            if (value < maxValue) value++;
            else if (value > maxValue) value--;

            if (owner == Global.instance.owner) // fog
            {
                int width = Scene.instance.width;
                int height = Scene.instance.height;
                for (int i = -fogSize; i <= fogSize; i++)
                    for (int j = -fogSize; j <= fogSize; j++)
                        if (i * i + j * j <= fogSize * fogSize)
                        {
                            int newX = pos.x + i;
                            int newY = pos.y + j;
                            if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                            {
                                Scene.instance.fogVis[newX, newY] = true;
                            }
                        }
            }
        }
    }

    void OnTouched()
    {
        Color temp;
        for (int k = 0; k < Scene.instance.sizeOfHouse; k++)
            if (Scene.instance.g[index, k] != 0)
            {
                GameObject obj = Scene.instance.posToHouse[Scene.instance.housePosArray[k]];
                temp = obj.GetComponent<MeshRenderer>().material.GetColor("_Color");
                obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/New Material 3");
                obj.GetComponent<MeshRenderer>().material.SetFloat("_Rate", 10);
                obj.GetComponent<MeshRenderer>().material.SetColor("_Color", temp);
            }
        temp = gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color");
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/New Material 3");
        gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Rate", 2);
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", temp);
    }

    void UnTouched()
    {
        for (int k = 0; k < Scene.instance.sizeOfHouse; k++)
        {
            GameObject obj = Scene.instance.posToHouse[Scene.instance.housePosArray[k]];
            obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/New Material 1");
        }
    }


    public IEnumerator DelayToInvokeDo(GameObject lastObj, int val)
    {
        var lastObjScript = lastObj.GetComponent<House>();
        if (lastObjScript.value <= val)
        {
            yield break;
        }

        Soldier script = player.GetComponent<Soldier>();
        script.dstHouse = gameObject;
        script.attackRate = lastObjScript.attackRate;
        script.timeDisOfMove = lastObjScript.timeDisOfMove;
        script.path = Scene.instance.houseRoadPath[lastObjScript.index, index];
        if (script.path == null) yield break;
        script.basePos = lastObj.transform.position;
        script.now = 0;
        script.owner = lastObjScript.owner;
        script.srcHouseScript = lastObjScript;
        Instantiate(player, lastObjScript.transform.position, new Quaternion());
        lastObjScript.value--;

        yield return new WaitForSeconds(timeDisOfMove);
        StartCoroutine(DelayToInvokeDo(lastObj, val));
    }


    public void JustAttack(GameObject lastObj)
    {
        int val = lastObj.GetComponent<House>().value / 2; // 每次移动一半
        if (lastObj.GetComponent<House>().owner == owner && value + val > maxValue) val = maxValue - value;
        StartCoroutine(DelayToInvokeDo(lastObj, lastObj.GetComponent<House>().value - val));
    }


    GUIStyle style = new GUIStyle();
    private void OnGUI()
    {
        if (!Scene.instance.fogVis[pos.x, pos.y]) return;

        style.fontSize = 45;

        Vector2 mScreen = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
        if (owner == Global.instance.owner || Global.instance.owner == -1)
        {
            if (lv < maxLv && value >= maxValue)
            {
                if (GUI.Button(new Rect(mPoint.x, mPoint.y + 50, 60, 50), "UP"))
                {
                    lv++;
                    initHouse();
                }
            }
        }
        GUI.Label(new Rect(mPoint.x, mPoint.y + 10, 60, 50), value.ToString() + "/" + maxValue.ToString(), style);
    }
}
