using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control2 : MonoBehaviour
{
    public static Control2 instance;

    public Slider slider;
    public static int maxOrt = 30;
    public static int minOrt = 3;

    float lastDis;

    public Text bugText;

    private void Awake()
    {
        instance = this;
        Input.multiTouchEnabled = true;//开启多点触碰
    }

    void Start()
    {
        slider.value = (Camera.main.orthographicSize - minOrt) / (maxOrt - minOrt);
    }

    GameObject GetGame(Vector2 touch, float disTest, bool isEnd = false)
    {
        GameObject minObj = null;
        float min_ = 999;
        foreach (var temp in Scene.instance.posToHouse)
        {
            if (isEnd)
            {
                if (Scene.instance.houseRoadPath[temp.Value.GetComponent<House>().index, lastObj.GetComponent<House>().index] == null) continue;
            }
            float dis = Vector2.Distance(temp.Key, new Vector2(touch.x, touch.y));
            if (dis < min_)
            {
                min_ = dis;
                minObj = temp.Value;
            }
        }
        if (min_ < disTest) return minObj;
        else return null;
    }

    bool isMoved = false;
    GameObject lastObj;
    void Update()
    {
        if (Input.touchCount <= 0)
        {
            isMoved = false;
            lastObj = null;
            return;
        }
        else if (Input.touchCount == 1) //单点触碰移动摄像机
        {
            Vector3 touch = Input.GetTouch(0).position;
            touch = Camera.main.ScreenToWorldPoint(touch + new Vector3(0, 0, 150));

            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                if (!Global.instance.isMode)
                {
                    Camera.main.transform.Translate(new Vector3(-Input.touches[0].deltaPosition.x * Time.deltaTime, -Input.touches[0].deltaPosition.y * Time.deltaTime, 0));  //手指在屏幕上移动，移动摄像机 TODO 是否保留
                }
                else
                {
                    if (!isMoved)
                    {
                        lastObj = GetGame(touch, 15); // 起点判定小于15
                    }
                    isMoved = true;
                }
            }
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                if (Global.instance.isMode)
                    if (isMoved)
                    {
                        GameObject minObj = GetGame(touch, 30, true); // 终点判定小于30
                        if (minObj != null && lastObj != null && lastObj.GetComponent<House>().owner == Global.instance.owner)
                        {
                            minObj.GetComponent<House>().JustAttack(lastObj);
                        }
                    }
            }
        }
        else if (Input.touchCount > 1)//多点触碰
        {
            Vector2 touch0, touch1;
            float distance;
            touch0 = Input.GetTouch(0).position;
            touch1 = Input.GetTouch(1).position;
            distance = Vector2.Distance(touch0, touch1);
            if (distance > lastDis)
            {
                if (Camera.main.fieldOfView > 2)
                    Camera.main.fieldOfView -= 2;
                if (Camera.main.orthographicSize >= 1)
                    Camera.main.orthographicSize -= 0.5F;
                slider.value = (Camera.main.orthographicSize - minOrt) / (maxOrt - minOrt);
            }
            else if (distance < lastDis)
            {
                if (Camera.main.fieldOfView <= 100)
                    Camera.main.fieldOfView += 2;
                if (Camera.main.orthographicSize <= 50)
                    Camera.main.orthographicSize += 0.5F;
                slider.value = (Camera.main.orthographicSize - minOrt) / (maxOrt - minOrt);
            }
            lastDis = distance;
        }
    }

    public void sliderControl()
    {
        Camera.main.orthographicSize = slider.value * (maxOrt - minOrt) + minOrt;
    }
}