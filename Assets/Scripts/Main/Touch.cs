using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 管理鼠标点击选中
/// </summary>
public class Touch : MonoBehaviour
{
    [Header("House Panel")]
    public GameObject panel;
    public Text lvText;
    public Text sizeText;
    public Text addText;
    public Text moveText;
    public Text attackText;

    public Camera myCamera;
    private float hitTime = 0f;
    public bool isSelect;       // 是否选择了我方城堡
    public GameObject lastObj;

    void Start()
    {
        lastObj = new GameObject();
        panel.SetActive(false);
    }

    void Update()
    {
        hitTime += Time.deltaTime;
        if (hitTime > 0.2f)
        {
            if (Input.GetMouseButton(0))
            {
                hitTime = 0f;
                RaycastHit hit;
                if (Physics.Raycast(myCamera.ScreenPointToRay(Input.mousePosition), out hit, 500f))
                {
                    GameObject obj = hit.collider.gameObject;
                    var script = obj.GetComponent<House>();
                    if (obj.name == "house(Clone)")
                    {
                        if (!isSelect && Scene.instance.fogVis[script.pos.x, script.pos.y])
                        {
                            if (Global.instance.diff == 0 || Global.instance.diff == 2) Global.instance.isStop = true;
                            if (Global.instance.isStop)
                            {
                                panel.SetActive(true);
                                houseInfo(obj);
                            }
                            if (obj.GetComponent<House>().owner == Global.instance.owner || Global.instance.owner == -1)
                            {
                                isSelect = true;
                                obj.SendMessage("OnTouched", SendMessageOptions.DontRequireReceiver);
                            }
                        }
                        else
                        {
                            if (Global.instance.diff == 0 || Global.instance.diff == 2) Global.instance.isStop = false;
                            if (!Global.instance.isStop)
                            {
                                isSelect = false;
                                panel.SetActive(false);
                            }
                            if (!Global.instance.isStop && lastObj.GetComponent<House>() != null)
                                if (lastObj.GetComponent<House>().owner == Global.instance.owner || Global.instance.owner == -1)
                                {
                                    obj.SendMessage("JustAttack", lastObj, SendMessageOptions.DontRequireReceiver);
                                    obj.SendMessage("UnTouched", SendMessageOptions.DontRequireReceiver);
                                    lastObj.SendMessage("UnTouched", SendMessageOptions.DontRequireReceiver);
                                }
                        }
                    }
                    else
                    {
                        if (Global.instance.diff == 0 || Global.instance.diff == 2) Global.instance.isStop = false;
                        isSelect = false;
                        panel.SetActive(false);
                        if (lastObj.name == "house(Clone)")
                            if (lastObj.GetComponent<House>().owner == Global.instance.owner || Global.instance.owner == -1)
                                lastObj.SendMessage("UnTouched", SendMessageOptions.DontRequireReceiver);
                    }
                    lastObj = obj;
                }
            }
        }
    }

    void houseInfo(GameObject obj)
    {
        var script = obj.GetComponent<House>();
        lvText.text = "LV : " + script.lv;
        addText.text = "士兵增加间隔 : " + script.timeDisOfAdd.ToString("f2") + "s";
        moveText.text = "士兵移动间隔 : " + script.timeDisOfMove.ToString("f2") + "s";
        sizeText.text = "士兵数 : " + script.value + " / " + script.maxValue;
        attackText.text = "士兵暴击率 : " + Mathf.RoundToInt(script.attackRate * 100) + "%";

        var tempColor = Scene.instance.colorTable[script.owner];
        tempColor.a /= 3;
        panel.GetComponent<Image>().color = tempColor;
    }
}
