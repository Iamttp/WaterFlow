using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public static Effect instance;

    private Vector3 lastPos;
    private Color lastColor;

    private int timeOfShake = 0;
    private int timeOfRed = 0;

    void Start()
    {
        instance = this;
    }

    void Update()
    {

    }

    private IEnumerator shakeEffect()
    {
        //Global.instance.isStop = true;
        yield return new WaitForSeconds(0.03f);

        Vector3 dirVector3 = Vector3.zero;
        dirVector3.y = Mathf.Sin(Time.time) * Random.Range(0, 3);
        dirVector3.x = Mathf.Cos(Time.time) * Random.Range(0, 3);
        Camera.main.transform.position = lastPos + dirVector3;
        if (++timeOfShake == 2)
        {
            timeOfShake = 0;
            Camera.main.transform.position = lastPos;
            //Global.instance.isStop = false;
        }
        else
            StartCoroutine(shakeEffect());
    }

    public void shake()
    {
        if (!Global.instance.isShakeOpen) return;
        if (timeOfShake != 0) timeOfShake = 0; // 当前正在抖动，重置抖动
        else
        {
            lastPos = Camera.main.transform.position;
            StartCoroutine(shakeEffect());
        }
    }

    public IEnumerator redEffect(House h)
    {
        //Global.instance.isStop = true;
        yield return new WaitForSeconds(0.03f);

        h.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(Random.value, Random.value, Random.value));
        if (++timeOfRed == 2)
        {
            timeOfRed = 0;
            h.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", lastColor);
            //Global.instance.isStop = false;
        }
        else
            StartCoroutine(redEffect(h));
    }

    public void red(House h)
    {
        if (!Global.instance.isDeepOpen) return;
        if (timeOfRed != 0) timeOfRed = 0;
        else
        {
            lastColor = h.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color");
            StartCoroutine(redEffect(h));
        }
    }
}
