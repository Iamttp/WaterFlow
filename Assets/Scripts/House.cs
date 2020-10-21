using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [Header("House属性")]
    public int value;                           // 城堡生命值
    public int maxValue;                        // 最大生命值
    public int owner;
    public static float timeDisOfAdd = 0.9f;    // 城堡生命值增加间隔
    public static float timeDisOfMove = 0.1f;   // 士兵移动时间间隔

    public GameObject player;                   // save player
    public int index;                           // houseArray index

    private float timeUse;

    void Start()
    {
        timeUse = timeDisOfAdd;
    }

    void Update()
    {
        timeUse -= Time.deltaTime;
        if (timeUse <= 0)
        {
            timeUse = timeDisOfAdd;
            if(value < maxValue) value++;
        }
    }

    void OnTouched()
    {
        gameObject.GetComponent<MeshRenderer>().material =
            Resources.Load<Material>("Materials/New Material 3");
        for (int k = 0; k < Scene.instance.sizeOfHouse; k++)
            if (Scene.instance.g[index, k] != 0)
            {
                GameObject obj = Scene.instance.posToHouse[Scene.instance.housePosArray[k]];
                obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/New Material 3");
            }
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
        Instantiate(player, transform.position, new Quaternion());
        lastObjScript.value--;

        yield return new WaitForSeconds(timeDisOfMove);
        StartCoroutine(DelayToInvokeDo(lastObj, val));
    }


    void JustAttack(GameObject lastObj)
    {
        int val = lastObj.GetComponent<House>().value / 2; // 每次移动一半
        // TODO 移动不超过目的地满 城堡owner改变

        StartCoroutine(DelayToInvokeDo(lastObj, val));
    }

    private void OnGUI()
    {
        float ContentWidth = 100;
        float ContentHeight = 50;
        //获取屏幕坐标
        Vector2 mScreen = Camera.main.WorldToScreenPoint(transform.position);
        //将屏幕坐标转化为GUI坐标
        Vector2 mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
        GUI.Label(new Rect(mPoint.x, mPoint.y, ContentWidth, ContentHeight), value.ToString());
    }
}
