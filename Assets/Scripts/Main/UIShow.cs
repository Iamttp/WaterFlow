using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 渲染到渲染纹理。显示当前局势
/// </summary>
public class UIShow : MonoBehaviour
{
    public GameObject cube;

    GameObject[] gameObjects;
    Scene scene;
    int len;
    int[] table = new int[4];

    void Start()
    {
        scene = Scene.instance;
        len = scene.housePosArray.Count;
        gameObjects = new GameObject[len];
        for (int i = 0; i < len; i++)
        {
            gameObjects[i] = Instantiate(cube, transform);
            gameObjects[i].transform.position += new Vector3(i - len / 2, 0, 0);
        }
        StaticBatchingUtility.Combine(gameObject); // 静态批处理
        transform.localScale = new Vector3(Load.camShowScale[Load.index1], 1, 1);
    }

    void Update()
    {
        if (Global.instance.isStop) return;
        for (int j = 0; j < 4; j++)
            table[j] = 0;

        for (int i = 0; i < len; i++)
        {
            table[scene.houseOfOwner[i]]++;
        }

        int index = 0;
        for (int j = 0; j < 4; j++)
            while (table[j]-- > 0)
                gameObjects[index++].GetComponent<MeshRenderer>().material.SetColor("_Color", scene.colorTable[j]);
    }
}
