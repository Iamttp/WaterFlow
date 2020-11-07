using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Creation : MonoBehaviour
{
    private int width;
    private int height;

    public int[,] tableOfType;
    public GameObject[,] tableOfObj;
    public Dictionary<string, List<Vector2Int>> dic;

    public static Creation instance;
    public GameObject stop;
    public GameObject go;
    public GameObject dragHouse;

    void Awake()
    {
        instance = this;

        width = Global.instance.width;
        height = Global.instance.height;

        var index = Load.index2;
        Camera.main.transform.position = new Vector3(Load.camX[index], Load.camY[index], 150);
        Camera.main.orthographicSize = Load.camSize[index];

        tableOfType = new int[width, height];
        tableOfObj = new GameObject[width, height];
        dic = new Dictionary<string, List<Vector2Int>>();

        tableOfType[width >> 1, height >> 1] = 2; // 初始一个house

        Vector3 rotationVector = new Vector3(0, 180, 0);
        Quaternion rotation = Quaternion.Euler(rotationVector);

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                tableOfObj[i, j] = Instantiate(stop, stop.transform.position + new Vector3(i, j), rotation, instance.transform);
            }
        StaticBatchingUtility.Combine(gameObject); // 静态批处理

        draw();
    }

    public void draw(bool isHouse = true)
    {
        Vector3 rotationVector = new Vector3(0, 180, 0);
        Quaternion rotation = Quaternion.Euler(rotationVector);

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                switch (tableOfType[i, j])
                {
                    case 0:
                        if (tableOfObj[i, j].name == "stop(Clone)") continue;
                        DestroyImmediate(tableOfObj[i, j]);
                        tableOfObj[i, j] = Instantiate(stop, stop.transform.position + new Vector3(i, j), rotation, instance.transform);
                        break;
                    case 1:
                        if (tableOfObj[i, j].name == "go(Clone)") continue;
                        DestroyImmediate(tableOfObj[i, j]);
                        tableOfObj[i, j] = Instantiate(go, go.transform.position + new Vector3(i, j), rotation, instance.transform);
                        break;
                    case 2:
                        if (isHouse)
                            if (tableOfObj[i, j].name == "DragHouse(Clone)") continue;
                        DestroyImmediate(tableOfObj[i, j]);
                        tableOfObj[i, j] = Instantiate(dragHouse, dragHouse.transform.position + new Vector3(i, j), rotation, transform);
                        break;
                }
            }
        if(!isHouse) StaticBatchingUtility.Combine(gameObject); // 释放鼠标时，静态批处理
    }

    void Start()
    {
    }

    void Update()
    {

    }

    public void backFun()
    {
        Global.instance.DataInit();
        SceneManager.LoadScene(0);
    }

    public void save()
    {

    }

    public List<Vector2Int> houseArray;
    public List<Vector2Int>[,] houseRoadPath;

    public void test()
    {
        houseArray = new List<Vector2Int>();
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                if (tableOfType[i, j] == 2)
                {
                    houseArray.Add(new Vector2Int(i, j));
                }

        houseRoadPath = new List<Vector2Int>[houseArray.Count, houseArray.Count];
        for (int i = 0; i < houseArray.Count; i++)
            for (int j = 0; j < houseArray.Count; j++)
            {
                int bi = Mathf.RoundToInt(houseArray[i].x);
                int bj = Mathf.RoundToInt(houseArray[i].y);
                int ei = Mathf.RoundToInt(houseArray[j].x);
                int ej = Mathf.RoundToInt(houseArray[j].y);
                string s1 = "[" + bi + "," + bj + "],[" + ei + "," + ej + "]";
                string s2 = "[" + ei + "," + ej + "],[" + bi + "," + bj + "]";
                if (dic.ContainsKey(s1)) houseRoadPath[i, j] = dic[s1];
                if (dic.ContainsKey(s2)) houseRoadPath[j, i] = dic[s2];
            }

        //// TODO ------------------------------------------------------------ 保存地图信息 TODO
        //Global.instance.isRandMap = false;

        //var housePosArray = new List<Vector2Int>();
        //var table = new Scene.node[width, height];
        ////var g = new int[houseArray.Count, houseArray.Count]; // 保存水泡间路径大小   g[k, i] = g[i, k] = houseRoadPath[i, k].Count;
        ////var posToHouse = new Dictionary<Vector2Int, GameObject>(); // Render中 不需要
        //var houseOfOwner = new int[houseArray.Count];
        ////var soldierSet = new HashSet<GameObject>(); // Soldier中 不需要
        ////var ligVis = new float[width, height]; // Render 不需要

        //for (int i = 0; i < width; i++)
        //    for (int j = 0; j < height; j++)
        //        table[i, j].type = tableOfType[i, j];

        //foreach (var item in houseArray)
        //{
        //    housePosArray.Add(item);
        //    table[item.x, item.y].type = 2;
        //    table[item.x, item.y].indexOfArray = housePosArray.Count - 1;
        //}

        //for (int i = 0; i < houseArray.Count; i++)
        //{
        //    houseOfOwner[i] = UnityEngine.Random.Range(0, 4);
        //}

        ////for (int i = 0; i < houseArray.Count; i++)
        ////    for (int j = 0; j < houseArray.Count; j++)
        ////        if (i != j && g[i, j] == 0)
        ////            g[i, j] = g[j, i] = houseRoadPath[i, j].Count;
    }
}
