using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class House : MonoBehaviour
{
    [Header("House属性")]
    public GameObject pointLig;
    public GameObject plane;
    public int value;                           // 水泡生命值
    public int perValue;
    public int maxValue;                        // 最大生命值
    public int lv;                              // 等级
    public int maxLv;
    public int owner;

    public float timeDisOfAdd;    // 水泡生命值增加间隔
    public float timeDisOfMove;   // 水滴移动时间间隔

    [Header("对应buff")]
    public float sizeRate;        // 最大生命值比例
    public float addRate;
    public float moveRate;
    public float attackRate;      // 暴击比例

    public GameObject player;                   // save player
    public int index;                           // houseArray index

    private float timeUse;

    public Vector2Int pos;

    private int sizeOfLig = 10;
    public void initHouse()
    {
        if (owner == Global.instance.owner)
        {
            sizeRate = Global.instance.buffOfSize;
            addRate = Global.instance.buffOfAddSpeed;
            moveRate = Global.instance.buffOfMoveSpeed;
            attackRate = Global.instance.buffOfAttack;
        }
        else
        {
            if (Global.instance.diff == 0) // 中等
            {
                sizeRate = 1.2f;
                addRate = 1.2f;
                moveRate = 1.2f;
                attackRate = 0.1f;
            }
            if (Global.instance.diff == 1) // 困难
            {
                sizeRate = 1.5f;
                addRate = 1.5f;
                moveRate = 1.5f;
                attackRate = 0.2f;
            }
            if (Global.instance.diff == 2) // 入门
            {
                sizeRate = 1;
                addRate = 1;
                moveRate = 1;
                attackRate = 0;
            }
            if (Global.instance.diff == 3) // 地狱
            {
                sizeRate = 2f;
                addRate = 2f;
                moveRate = 2f;
                attackRate = 0.3f;
            }
        }
        maxValue = Mathf.RoundToInt(lv * perValue * sizeRate);
        timeDisOfAdd = 1f / addRate;
        timeDisOfMove = 0.1f / moveRate;

        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Scene.instance.colorTable[owner]);
        pointLig.SetActive(owner == Global.instance.owner);
    }

    void Start()
    {
        lv = 1;
        value = 0;
        initHouse();
        guiMe = Resources.Load<GUISkin>("Textures/skin");
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

            if (owner == Global.instance.owner)
                for (int i = -sizeOfLig; i <= sizeOfLig; i++)
                    for (int j = -sizeOfLig; j <= sizeOfLig; j++)
                    {
                        int newX = pos.x + i;
                        int newY = pos.y + j;
                        if (newX >= 0 && newX < Scene.instance.width && newY >= 0 && newY < Scene.instance.height)
                            Scene.instance.ligVis[newX, newY] = 2; // 持续2秒
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

                var script = obj.GetComponent<House>();
                script.plane.SetActive(true);
                if (script.owner == owner)
                    script.plane.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0, 1, 0, 1));
                else
                    script.plane.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 0, 0, 1));
            }
        temp = gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color");
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/New Material 3");
        gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Rate", 2);
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", temp);

        plane.SetActive(true);
        plane.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0, 1, 1, 1));
    }

    void UnTouched()
    {
        Color temp;
        for (int k = 0; k < Scene.instance.sizeOfHouse; k++)
        {
            GameObject obj = Scene.instance.posToHouse[Scene.instance.housePosArray[k]];
            temp = obj.GetComponent<MeshRenderer>().material.GetColor("_Color");
            obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/New Material 1");
            obj.GetComponent<MeshRenderer>().material.SetColor("_Color", temp);

            obj.GetComponent<House>().plane.SetActive(false);
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

    GUISkin guiMe;
    GUIStyle style1 = new GUIStyle();
    GUIStyle style2 = new GUIStyle();

    private void OnGUI()
    {
        if (owner != Global.instance.owner && Scene.instance.ligVis[pos.x, pos.y] < 0.2f) return;

        style1 = guiMe.button;
        style2 = guiMe.label;

        Vector2 mScreen = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
        if (owner == Global.instance.owner || Global.instance.owner == -1)
        {
            if (lv < maxLv && value >= maxValue)
            {
                if (GUI.Button(new Rect(mPoint.x - 30, mPoint.y + 70, 100, 50), "U", style1))
                {
                    Music.instance.playDown2();
                    Global.instance.upTime++;
                    lv++;
                    value -= maxValue;
                    initHouse();
                }
            }
        }

        if(value >= 10) 
            GUI.Label(new Rect(mPoint.x - 40, mPoint.y + 10, 150, 70), value.ToString() + "/" + maxValue.ToString(), style2);
        else
            GUI.Label(new Rect(mPoint.x - 40, mPoint.y + 10, 120, 70), value.ToString() + "/" + maxValue.ToString(), style2);
    }
}
