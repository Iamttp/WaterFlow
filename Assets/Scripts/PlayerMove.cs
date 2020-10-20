using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public List<Vector2Int> path;
    public int now;
    public Vector3 basePos;
    public GameObject dstHouse;
    private float timeUse;

    void Start()
    {
        timeUse = HouseSelect.timeDis;
    }

    void Update()
    {
        timeUse -= Time.deltaTime;
        if (timeUse < 0)
        {
            timeUse = HouseSelect.timeDis;
            transform.position = new Vector3(path[now].x * ManageScene.instance.allScale, path[now].y * ManageScene.instance.allScale, basePos.z + 1);
            now++;
            if (now >= path.Count)
            {
                dstHouse.GetComponent<HouseSelect>().value++;
                DestroyImmediate(gameObject);
                return;
            }
        }
    }
}
