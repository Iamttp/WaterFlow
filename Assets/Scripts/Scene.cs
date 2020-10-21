using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例类，全局管理，包含地图生成绘制 TODO 不同城堡间路径重合需解决
/// </summary>
public class Scene : MonoBehaviour
{
    public static Scene instance;

    [Header("地图单位Object")]
    public GameObject house;
    public GameObject stop;
    public GameObject go;

    [Header("地图基本属性")]
    public int width;
    public int height;
    public int sizeOfHouse;
    public int sizeOfVis;           // 用于控制House分散放置
    public float allScale;

    public int[] houseOfOwner;
    public Color[] colorTable;      // 不同势力颜色索引
    struct node
    {
        public int type;
        public int indexOfArray;
    };

    private node[,] table;
    private bool[,] visTable; // 防止太密集

    public List<Vector2Int> housePosArray;
    public Dictionary<Vector2Int, GameObject> posToHouse;
    public List<Vector2Int>[,] houseRoadPath;
    public int[,] g; // 二维数组存图

    /// <summary>
    /// 在x,y处放置house 周围不出现
    /// </summary>
    bool placeHouse(Vector2Int pos)
    {
        int x = pos.x, y = pos.y;
        if (visTable[x, y]) return false;
        for (int offsetY = -sizeOfVis; offsetY <= sizeOfVis; offsetY++)
            for (int offsetX = -sizeOfVis; offsetX <= sizeOfVis; offsetX++)
            {
                int newX = offsetX + x;
                int newY = offsetY + y;
                if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                {
                    visTable[newX, newY] = true;
                }
            }

        housePosArray.Add(pos);
        table[x, y].type = 2;
        table[x, y].indexOfArray = housePosArray.Count - 1;
        return true;
    }

    void initHouseData()
    {
        housePosArray = new List<Vector2Int>();
        table = new node[width, height];
        visTable = new bool[width, height];
        g = new int[sizeOfHouse, sizeOfHouse];
        posToHouse = new Dictionary<Vector2Int, GameObject>();
        houseOfOwner = new int[Scene.instance.sizeOfHouse];

        // 0 stop 1 go 2 house 四个角的house数相同
        int[] offsetX = new int[] { 0, width >> 1, width >> 1, 0 };
        int[] offsetY = new int[] { 0, height >> 1, 0, height >> 1 };
        for (int i = 0; i < sizeOfHouse; i++)
        {
            int index = i % 4;
            houseOfOwner[i] = index;
            int x = Random.Range(1, (width >> 1) - 1) + offsetX[index];
            int y = Random.Range(1, (height >> 1) - 1) + offsetY[index];

            if (!placeHouse(new Vector2Int(x, y)))
            {
                i--;
                continue;
            }
        }
    }

    /// <summary>
    /// start end间绘制直线（路径），path保存结果
    /// </summary>
    /// <returns>是否出现路径交叉</returns>
    bool placeRoad(Vector2Int start, Vector2Int end, out List<Vector2Int> path)
    {
        bool isReverse = false;
        path = new List<Vector2Int>();
        Vector2Int dir = end - start;
        Vector2 line = start;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (start.x > end.x)
            {
                Vector2Int temp = start;
                start = end;
                end = temp;
                dir = end - start;
                line = start;
                isReverse = true;
            }
            float k = ((float)dir.y) / dir.x;
            int lastY = -1;
            while (line.x != end.x)
            {
                line.x++;
                line.y += k;
                if (lastY != -1 && lastY != (int)line.y)
                    path.Add(new Vector2Int((int)line.x, lastY));
                lastY = (int)line.y;
                path.Add(new Vector2Int((int)line.x, (int)line.y));
            }
        }
        else
        {
            if (start.y > end.y)
            {
                Vector2Int temp = start;
                start = end;
                end = temp;
                dir = end - start;
                line = start;
                isReverse = true;
            }
            float k = dir.x / ((float)dir.y);
            int lastX = -1;
            while (line.y != end.y)
            {
                line.y++;
                line.x += k;
                if (lastX != -1 && lastX != (int)line.x)
                    path.Add(new Vector2Int(lastX, (int)line.y));
                lastX = (int)line.x;
                path.Add(new Vector2Int((int)line.x, (int)line.y));
            }
        }
        if (isReverse) path.Reverse();
        for (int i = 3; i < path.Count - 3; i++)
            if (table[path[i].x, path[i].y].type == 1)
                return false;
        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int pos = path[i];
            if (table[pos.x, pos.y].type == 0)
                table[pos.x, pos.y].type = 1;
        }
        return true;
    }

    void initRoadData()
    {
        houseRoadPath = new List<Vector2Int>[housePosArray.Count, housePosArray.Count];
        for (int i = 0; i < housePosArray.Count; i++)
            for (int k = 0; k < housePosArray.Count; k++)
                if (i != k && g[i, k] == 0)
                {
                    if (placeRoad(housePosArray[i], housePosArray[k], out houseRoadPath[i, k]))
                    {
                        houseRoadPath[k, i] = new List<Vector2Int>(houseRoadPath[i, k]);
                        houseRoadPath[k, i].Reverse();
                        g[k, i] = g[i, k] = houseRoadPath[i, k].Count;
                    }
                    else
                    {
                        houseRoadPath[i, k] = null;
                    }
                }
    }

    void Awake()
    {
        instance = this;
        initHouseData();
        initRoadData();
    }

    void renderScene()
    {
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                switch (table[i, j].type)
                {
                    case 0:
                        stop.transform.localScale = new Vector3(allScale, allScale, 1);
                        Instantiate(stop, stop.transform.position + new Vector3(i * allScale, j * allScale), new Quaternion(), instance.transform);
                        break;
                    case 1:
                        go.transform.localScale = new Vector3(allScale, allScale, 1);
                        Instantiate(go, go.transform.position + new Vector3(i * allScale, j * allScale), new Quaternion(), instance.transform);
                        break;
                    case 2:
                        house.transform.localScale = new Vector3(allScale, allScale, 1);
                        house.GetComponent<House>().index = table[i, j].indexOfArray;
                        house.GetComponent<House>().owner = houseOfOwner[table[i, j].indexOfArray];
                        posToHouse[new Vector2Int(i, j)] = Instantiate(house, house.transform.position + new Vector3(i * allScale, j * allScale), new Quaternion(), instance.transform);
                        break;
                }
            }
        StaticBatchingUtility.Combine(gameObject); // 静态批处理
    }

    void Start()
    {
        renderScene();
    }

    void Update()
    {
        for (int i = 0; i < housePosArray.Count; i++)
        {
            houseOfOwner[i] = posToHouse[housePosArray[i]].GetComponent<House>().owner;
            posToHouse[housePosArray[i]].GetComponent<MeshRenderer>().material.color = colorTable[houseOfOwner[i]];
        }
    }
}
