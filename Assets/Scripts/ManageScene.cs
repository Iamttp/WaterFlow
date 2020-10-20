using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageScene : MonoBehaviour
{
    public static ManageScene instance;

    [Header("地图单位Object")]
    public GameObject house;
    public GameObject stop;
    public GameObject go;
    public GameObject myGameObject;

    [Header("地图基本属性")]
    public int width;
    public int height;
    public int sizeOfHouse;
    public int sizeOfVis;
    public float allScale;

    struct node
    {
        public int type;
        public int indexOfArray;
    };

    private node[,] table;
    private bool[,] visTable; // 防止太密集
    private List<Vector2Int> houseArray;
    public List<Vector2Int>[,] houseRoadPath;

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

        houseArray.Add(pos);
        table[x, y].type = 2;
        table[x, y].indexOfArray = houseArray.Count - 1;
        return true;
    }

    void initHouseData()
    {
        houseArray = new List<Vector2Int>();
        table = new node[width, height];
        visTable = new bool[width, height];

        // 0 stop 1 go 2 house 四个角的house数相同
        int[] offsetX = new int[] { 0, width >> 1, width >> 1, 0 };
        int[] offsetY = new int[] { 0, height >> 1, 0, height >> 1 };
        for (int i = 0; i < sizeOfHouse; i++)
        {
            int index = i % 4;
            int x = Random.Range(1, (width >> 1) - 1) + offsetX[index];
            int y = Random.Range(1, (height >> 1) - 1) + offsetY[index];

            if (!placeHouse(new Vector2Int(x, y)))
            {
                i--;
                continue;
            }
        }
    }

    void placeRoad(Vector2Int start, Vector2Int end, out List<Vector2Int> path)
    {
        bool isReverse = false;
        path = new List<Vector2Int>();
        // TODO k为无穷
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
                    if (table[(int)line.x, lastY].type == 0)
                    {
                        table[(int)line.x, lastY].type = 1;
                        path.Add(new Vector2Int((int)line.x, lastY));
                    }
                lastY = (int)line.y;
                if (table[(int)line.x, (int)line.y].type == 0)
                {
                    table[(int)line.x, (int)line.y].type = 1;
                    path.Add(new Vector2Int((int)line.x, (int)line.y));
                }
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
                    if (table[lastX, (int)line.y].type == 0)
                    {
                        table[lastX, (int)line.y].type = 1;
                        path.Add(new Vector2Int(lastX, (int)line.y));
                    }
                lastX = (int)line.x;
                if (table[(int)line.x, (int)line.y].type == 0)
                {
                    table[(int)line.x, (int)line.y].type = 1;
                    path.Add(new Vector2Int((int)line.x, (int)line.y));
                }
            }
        }
        if (isReverse) path.Reverse();
    }

    void initRoadData()
    {
        houseRoadPath = new List<Vector2Int>[houseArray.Count, houseArray.Count];
        for (int i = 0; i < houseArray.Count; i++)
            for (int k = i + 1; k < houseArray.Count; k++)
            {
                placeRoad(houseArray[i], houseArray[k], out houseRoadPath[i, k]);
                houseRoadPath[k, i] = new List<Vector2Int>(houseRoadPath[i, k]);
                houseRoadPath[k, i].Reverse();
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
                        house.GetComponent<HouseSelect>().index = table[i, j].indexOfArray;
                        Instantiate(house, house.transform.position + new Vector3(i * allScale, j * allScale), new Quaternion(), instance.transform);
                        break;
                }
            }
        StaticBatchingUtility.Combine(myGameObject); // 静态批处理
    }

    void Start()
    {
        renderScene();
    }

    void Update()
    {

    }
}
