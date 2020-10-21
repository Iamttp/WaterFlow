using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        //Zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.fieldOfView <= 100)
                Camera.main.fieldOfView += 2;
            if (Camera.main.orthographicSize <= 50)
                Camera.main.orthographicSize += 0.5F;
        }
        //Zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.fieldOfView > 2)
                Camera.main.fieldOfView -= 2;
            if (Camera.main.orthographicSize >= 1)
                Camera.main.orthographicSize -= 0.5F;
        }

        //移动
        Vector3 dirVector3 = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift)) dirVector3.y = 3;
            else dirVector3.y = 1;
            Camera.main.transform.position += dirVector3 * 0.1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftShift)) dirVector3.y = -3;
            else dirVector3.y = -1;
            Camera.main.transform.position += dirVector3 * 0.1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift)) dirVector3.x = 3;
            else dirVector3.x = 1;
            Camera.main.transform.position += dirVector3 * 0.1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift)) dirVector3.x = -3;
            else dirVector3.x = -1;
            Camera.main.transform.position += dirVector3 * 0.1f;
        }
    }
}
