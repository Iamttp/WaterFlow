using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSelect : MonoBehaviour
{
    [Header("House属性")]
    public int value;

    [Header("单位属性")]
    public static float timeDis = 0.1f;

    public GameObject player; // save player
    public int index; // houseArray index


    void Start()
    {
        value = 3;
    }

    void Update()
    {

    }

    void OnTouched()
    {
        this.gameObject.GetComponent<MeshRenderer>().material =
            Resources.Load<Material>("Materials/New Material 3");
    }

    void UnTouched()
    {
        this.gameObject.GetComponent<MeshRenderer>().material =
            Resources.Load<Material>("Materials/New Material 1");
    }


    public IEnumerator DelayToInvokeDo(GameObject lastObj)
    {
        var lastObjScript = lastObj.GetComponent<HouseSelect>();
        if (lastObjScript.value <= 0)
        {
            yield break;
        }

        PlayerMove script = player.GetComponent<PlayerMove>();
        script.dstHouse = gameObject;
        script.path = ManageScene.instance.houseRoadPath[lastObjScript.index, index];
        script.basePos = lastObj.transform.position;
        script.now = 0;
        Instantiate(player, transform.position, new Quaternion());
        lastObjScript.value--;

        yield return new WaitForSeconds(timeDis);
        StartCoroutine(DelayToInvokeDo(lastObj));
    }


    void JustAttack(GameObject lastObj)
    {
        Debug.Log("派遣");
        StartCoroutine(DelayToInvokeDo(lastObj));
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
