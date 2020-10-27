﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 单例类，全局管理，包含地图生成绘制 
/// 
/// TODO 不同城堡间路径重合需解决 （一定概率，城堡在道路边，影响视角）
/// TODO 局域网 
/// TODO 粒子特效 
/// TODO 士兵间碰撞 （现在未考虑士兵碰撞）
/// TODO 兵力显示（现在为城堡数显示）
/// 
/// 选择颜色，开始游戏
/// 通过点击矩形城堡，派遣士兵
/// 攻占所有城堡，游戏胜利
/// 失去城堡，游戏失败
/// 
/// WASD / 单指拖动 -> 移动视角
/// 鼠标滚轮 / 双指 / 滑动条 -> 缩放视角
/// 鼠标左键 / 单指点击 -> 选择
/// </summary>
public class Scene : MonoBehaviour
{
    public static Scene instance;

    [Header("地图单位Object")]
    public GameObject house;
    public GameObject stop;
    public GameObject go;
    public GameObject fog;
    public Text level;
    public Text show;
    public Text stopButton;

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
    public bool[,] fogVis;
    public float[,] fogVisAlpha;
    private bool[,] visTable; // 防止太密集

    public List<Vector2Int> housePosArray;
    public Dictionary<Vector2Int, GameObject> posToHouse;
    public Dictionary<Vector2Int, GameObject> posToFog;
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

    bool initHouseData()
    {
        housePosArray = new List<Vector2Int>();
        table = new node[width, height];
        fogVis = new bool[width, height];
        fogVisAlpha = new float[width, height];
        visTable = new bool[width, height];
        g = new int[sizeOfHouse, sizeOfHouse];
        posToHouse = new Dictionary<Vector2Int, GameObject>();
        posToFog = new Dictionary<Vector2Int, GameObject>();
        houseOfOwner = new int[Scene.instance.sizeOfHouse];

        // 0 stop 1 go 2 house 四个角的house数相同
        int allowTry = 100;
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
                if (allowTry-- <= 0)
                {
                    Debug.Log("house error restart");
                    SceneManager.LoadScene(2);
                    return false;
                }
                continue;
            }
        }
        return true;
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

        // 两条道路间周围存在道路或者城堡，返回false
        for (int i = 4; i < path.Count - 4; i++)
            for (int offsetY = -sizeOfVis / 2; offsetY <= sizeOfVis / 2; offsetY++)
                for (int offsetX = -sizeOfVis / 2; offsetX <= sizeOfVis / 2; offsetX++)
                {
                    int newX = offsetX + path[i].x;
                    int newY = offsetY + path[i].y;
                    if (table[newX, newY].type == 1 || table[newX, newY].type == 2)
                        return false;
                }

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
                        //Debug.Log("warning map road");
                        houseRoadPath[i, k] = null;
                    }
                }

        // 排除出现存在城堡无道路连接的情况
        for (int i = 0; i < housePosArray.Count; i++)
        {
            bool isOK = false;
            for (int k = 0; k < housePosArray.Count; k++)
                if (i != k && houseRoadPath[i, k] != null)
                {
                    isOK = true;
                    break;
                }
            if (!isOK)
            {
                Debug.Log("map error restart");
                SceneManager.LoadScene(2);
                return;
            }
        }

        // 排除出现多张图，每张图城堡数大于等于2的情况。
        countDfs = 0;
        visDfs = new bool[sizeOfHouse];
        visDfs[0] = true;
        dfs(0);
        Debug.Log(countDfs);
        if (countDfs != sizeOfHouse)
        {
            Debug.Log("map error restart");
            SceneManager.LoadScene(2);
            return;
        }
    }

    int countDfs;
    bool[] visDfs;
    void dfs(int index)
    {
        countDfs++;
        for (int i = 0; i < sizeOfHouse; i++)
            if (i != index && houseRoadPath[i, index] != null && !visDfs[i])
            {
                visDfs[i] = true;
                dfs(i);
            }
    }

    void Awake()
    {
        instance = this;
        Screen.SetResolution(1920, 1080, true);
        width = Global.instance.width;
        height = Global.instance.height;
        sizeOfHouse = Global.instance.sizeOfHouse;
        var i = Load.index2;
        Camera.main.transform.position = new Vector3(Load.camX[i], Load.camY[i], 150);
        Camera.main.orthographicSize = Load.camSize[i];
        if (initHouseData())
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

                        fog.transform.localScale = new Vector3(allScale, allScale, 10);
                        posToFog[new Vector2Int(i, j)] = Instantiate(fog, fog.transform.position + new Vector3(i * allScale, j * allScale), new Quaternion(), instance.transform);
                        break;
                    case 1:
                        go.transform.localScale = new Vector3(allScale, allScale, 1);
                        Instantiate(go, go.transform.position + new Vector3(i * allScale, j * allScale), new Quaternion(), instance.transform);

                        fog.transform.localScale = new Vector3(allScale, allScale, 10);
                        posToFog[new Vector2Int(i, j)] = Instantiate(fog, fog.transform.position + new Vector3(i * allScale, j * allScale), new Quaternion(), instance.transform);
                        break;
                    case 2:
                        house.transform.localScale = new Vector3(allScale, allScale, 1);
                        house.GetComponent<House>().index = table[i, j].indexOfArray;
                        house.GetComponent<House>().owner = houseOfOwner[table[i, j].indexOfArray];
                        house.GetComponent<House>().pos = new Vector2Int(i, j);
                        posToHouse[new Vector2Int(i, j)] = Instantiate(house, house.transform.position + new Vector3(i * allScale, j * allScale), new Quaternion(), instance.transform);
                        break;
                }
            }
        StaticBatchingUtility.Combine(gameObject); // 静态批处理
    }

    void Start()
    {
        renderScene();
        level.text = "Level " + Global.instance.lev;
        show.enabled = false;

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                fogVisAlpha[i, j] = 1;
    }

    public void FogTest()
    {
        foreach (var temp in posToHouse) // 每隔一段时间，确认城堡视野
        {
            var script = temp.Value.GetComponent<House>();
            if (script.owner == Global.instance.owner) script.fogSet(true);
        }
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                if (fogVisAlpha[i, j] < 1)
                    fogVisAlpha[i, j] += 0.01f;
            }
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                if (posToFog.ContainsKey(new Vector2Int(i, j)))
                {
                    posToFog[new Vector2Int(i, j)].SetActive(!fogVis[i, j]); // TODO

                    if (Global.instance.diff == 0 || Global.instance.diff == 2) continue;
                    if (!fogVis[i, j] && fogVisAlpha[i, j] < 1)
                    {
                        posToFog[new Vector2Int(i, j)].GetComponent<MeshRenderer>().material.SetFloat("_Alpha", fogVisAlpha[i, j]);
                    }
                }
            }
    }


    private IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(3);
        show.enabled = false;
        SceneManager.LoadScene(4);
    }

    private IEnumerator GameNEXT()
    {
        yield return new WaitForSeconds(3);
        show.enabled = false;
        Global.instance.timeOfAttack /= 2.0f;
        Global.instance.timeOfUP /= 2.0f;
        if (++Global.instance.lev == 6)
            SceneManager.LoadScene(3);
        else
        {
            Global.instance.flag = false;
            SceneManager.LoadScene(1);
        }
    }

    void Update()
    {
        if (Global.instance.isStop) stopButton.text = "START";
        else stopButton.text = "STOP";
        if (Global.instance.isStop) return;
        if (Global.instance.flag) return;
        int num = 0;
        for (int i = 0; i < housePosArray.Count; i++)
        {
            houseOfOwner[i] = posToHouse[housePosArray[i]].GetComponent<House>().owner;
            if (houseOfOwner[i] == Global.instance.owner) num++;
            posToHouse[housePosArray[i]].GetComponent<MeshRenderer>().material.SetColor("_Color", colorTable[houseOfOwner[i]]);
        }
        if (num == housePosArray.Count)
        {
            // 游戏胜利 下一关
            Global.instance.flag = true;
            show.enabled = true;
            show.text = "NEXT LEVEL";
            StartCoroutine(GameNEXT());
        }
        else if (num == 0)
        {
            // 游戏结束 重新开始
            Global.instance.flag = true;
            show.enabled = true;
            show.text = "GAME OVER";
            StartCoroutine(GameEnd());
        }
    }


    public void restart()
    {
        Global.instance.DataInit();
        SceneManager.LoadScene(1);
    }

    public void stopFun()
    {
        Global.instance.isStop = !Global.instance.isStop;
        if (Global.instance.isStop) stopButton.text = "START";
        else stopButton.text = "STOP";
    }

    public void backFun()
    {
        Global.instance.DataInit();
        SceneManager.LoadScene(0);
    }
}
