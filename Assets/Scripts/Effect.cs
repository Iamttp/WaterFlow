using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public static Effect instance;

    private Vector3 lastPos;
    private int timeOfShake = 0;
    void Start()
    {
        instance = this;
    }

    void Update()
    {

    }

    private IEnumerator shakeEffect()
    {
        yield return new WaitForSeconds(0.1f);

        Vector3 dirVector3 = Vector3.zero;
        dirVector3.y = Mathf.Sin(Time.time) * Random.Range(2, 8);
        dirVector3.x = Mathf.Cos(Time.time) * Random.Range(2, 8);
        Camera.main.transform.position = lastPos + dirVector3;
        timeOfShake++;
        if (timeOfShake == 10)
        {
            timeOfShake = 0;
            Camera.main.transform.position = lastPos;
        }
        else
            StartCoroutine(shakeEffect());
    }

    public void shake()
    {
        if (timeOfShake != 0) timeOfShake = 0; // 当前正在抖动，重置抖动
        else
        {
            lastPos = Camera.main.transform.position;
            StartCoroutine(shakeEffect());
        }
    }
}
