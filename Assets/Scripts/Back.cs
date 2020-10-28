using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour
{
    private float hitTime = 0f;
    private float x, z, a;
    void Start()
    {
        x = z = a = 0;
    }

    private IEnumerator setDrop()
    {
        if (a <= 0) yield break;
        var temp = gameObject.GetComponent<MeshRenderer>().material;
        temp.SetFloat("_CenterU", x);
        temp.SetFloat("_CenterV", z);
        temp.SetFloat("_Amount", a);
        a -= 0.005f;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(setDrop());
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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 pos = transform.worldToLocalMatrix.MultiplyPoint(hit.point);//把世界坐标转换成图片像素坐标
                    pos /= -10;
                    pos.x += 0.5f;
                    pos.z += 0.5f;
                    Debug.Log(pos);
                    x = pos.x;
                    z = pos.z;
                    a = 0.2f;
                    StartCoroutine(setDrop());
                }
            }
        }
    }
}
