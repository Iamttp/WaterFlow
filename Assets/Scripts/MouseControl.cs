using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 } //定义一个枚举，移动xy，或者只是移动x，或者y
    public RotationAxes axes = RotationAxes.MouseXAndY;                 //声明一个枚举变量，方便在外面修改移动模式
    public float sensitivityX = 15f;                                    //定义一个移动速度
    public float sensitivityY = 15f;

    public float minimumY = -60f;       //定义俯视最低值，建议这个值，要不然会转过头
    public float maximumY = 60f;        //定义俯视最高值，建议这个值，要不然会转过头

    float rotationY = 0f;               //存储实际转动的Y值

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

        if (Input.GetMouseButton(0))
            switch (axes)               //判断用户是用那种旋转方式
            {
                case RotationAxes.MouseXAndY:
                    float rotationX = transform.localEulerAngles.y - Input.GetAxis("Mouse X") * sensitivityX;

                    rotationY -= Input.GetAxis("Mouse Y") * sensitivityY; //
                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                    transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
                    break;
                case RotationAxes.MouseX:
                    transform.Rotate(0, -Input.GetAxis("Mouse X") * sensitivityX, 0);
                    break;
                case RotationAxes.MouseY:
                    rotationY -= Input.GetAxis("Mouse Y") * sensitivityY;
                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                    transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
                    break;
                default:
                    break;
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
