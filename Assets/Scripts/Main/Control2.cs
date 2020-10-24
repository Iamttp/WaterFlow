using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control2 : MonoBehaviour
{
    float lastDis;
    void Start()
    {
        Input.multiTouchEnabled = true;//开启多点触碰
    }

    void Update()
    {
        if (Input.touchCount <= 0)
            return;
        if (Input.touchCount == 1) //单点触碰移动摄像机
        {
            if (Input.touches[0].phase == TouchPhase.Moved) //手指在屏幕上移动，移动摄像机
            {
                Camera.main.transform.Translate(new Vector3(-Input.touches[0].deltaPosition.x * Time.deltaTime, -Input.touches[0].deltaPosition.y * Time.deltaTime, 0));
            }
        }
        else if (Input.touchCount > 1)//多点触碰
        {
            Vector2 touch0, touch1;
            float distance;
            touch0 = Input.GetTouch(0).position;
            touch1 = Input.GetTouch(1).position;
            distance = Vector2.Distance(touch0, touch1);
            if(distance > lastDis)
            {
                if (Camera.main.fieldOfView > 2)
                    Camera.main.fieldOfView -= 2;
                if (Camera.main.orthographicSize >= 1)
                    Camera.main.orthographicSize -= 0.5F;
            }
            else
            {
                if (Camera.main.fieldOfView <= 100)
                    Camera.main.fieldOfView += 2;
                if (Camera.main.orthographicSize <= 50)
                    Camera.main.orthographicSize += 0.5F;
            }
            lastDis = distance;
        }
    }
}