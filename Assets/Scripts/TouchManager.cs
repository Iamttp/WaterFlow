using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public Camera myCamera;
    private float hitTime = 0f;
    public bool isSelect;
    public GameObject lastObj;

    void Start()
    {
        lastObj = new GameObject();
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
                    if (obj.name == "house(Clone)")
                    {
                        if (!isSelect)
                        {
                            isSelect = true;
                            obj.SendMessage("OnTouched", SendMessageOptions.DontRequireReceiver);
                        }
                        else
                        {
                            isSelect = false;
                            obj.SendMessage("JustAttack", lastObj, SendMessageOptions.DontRequireReceiver);
                            obj.SendMessage("UnTouched", SendMessageOptions.DontRequireReceiver);
                            lastObj.SendMessage("UnTouched", SendMessageOptions.DontRequireReceiver);
                        }
                    }
                    else
                    {
                        isSelect = false;
                        if (lastObj.name == "house(Clone)") 
                            lastObj.SendMessage("UnTouched", SendMessageOptions.DontRequireReceiver);
                    }
                    lastObj = obj;
                }
            }
        }
    }
}
