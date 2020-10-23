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

    public GameObject player;                   // save player
    public int index;                           // houseArray index

    private float timeUse;

    public void initHouse()
    {
        lv = 1;
        value = 0;
        if (owner == Global.instance.owner)
        {
            maxValue = Mathf.RoundToInt(lv * perValue * Global.instance.buffOfSize);
            timeDisOfAdd /= Global.instance.buffOfAddSpeed;
            timeDisOfMove /= Global.instance.buffOfMoveSpeed;
        }
        else
        {
            maxValue = lv * perValue;
            timeDisOfAdd = 0.9f;
            timeDisOfMove = 0.1f;
        }
        timeUse = timeDisOfAdd;
    }

    void Start()
    {
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
        }
    }

    void OnTouched()
    {
        for (int k = 0; k < Scene.instance.sizeOfHouse; k++)
            if (Scene.instance.g[index, k] != 0)
            {
                GameObject obj = Scene.instance.posToHouse[Scene.instance.housePosArray[k]];
                obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/New Material 3");
                obj.GetComponent<MeshRenderer>().material.SetFloat("_Rate", 10);
            }
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/New Material 3");
        gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Rate", 2);
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
        script.path = Scene.instance.houseRoadPath[lastObjScript.index, index];
        if (script.path == null) yield break;
        script.basePos = lastObj.transform.position;
        script.now = 0;
        script.owner = lastObjScript.owner;
        script.srcHouseScript = lastObjScript;
        Instantiate(player, transform.position, new Quaternion());
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
        style.fontSize = 45;

        Vector2 mScreen = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
        if (owner == Global.instance.owner || Global.instance.owner == -1)
        {
            if (lv < maxLv && value >= maxValue)
            {
                if (GUI.Button(new Rect(mPoint.x, mPoint.y + 50, 60, 50), "UP"))
                {
                    value -= maxValue;
                    maxValue = ++lv * perValue;
                }
            }
        }
        GUI.Label(new Rect(mPoint.x, mPoint.y + 10, 60, 50), value.ToString() + "/" + maxValue.ToString(), style);
    }
}
