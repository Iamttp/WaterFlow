using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Creation : MonoBehaviour
{
    public int width;
    public int height;

    public int[,] tableOfType;
    private GameObject[,] tableOfObj;
    public Dictionary<string, List<Vector2Int>> dic;

    public static Creation instance;
    public GameObject stop;
    public GameObject go;
    public GameObject dragHouse;
    public Text bugText;

    void Awake()
    {
        instance = this;
        if (Global.instance == null) // 测试用
        {
            width = height = 40;
        }
        else
        {
            width = Global.instance.width;
            height = Global.instance.height;
        }

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
        if (!isHouse) StaticBatchingUtility.Combine(gameObject); // 释放鼠标时，静态批处理
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

    private List<Vector2Int> houseArray;
    private List<Vector2Int>[,] houseRoadPath;

    public void test()
    {
        houseArray = new List<Vector2Int>();
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                if (tableOfType[i, j] == 2)
                {
                    houseArray.Add(new Vector2Int(i, j));
                }
        int num = 0;
        int firstOwner = 0;
        for (int i = 0; i < houseArray.Count; i++)
        {
            int owner = tableOfObj[houseArray[i].x, houseArray[i].y].GetComponent<Drag>().owner;
            if (i == 0) firstOwner = owner;
            if (owner == firstOwner) num++;
        }
        if (num == houseArray.Count) return; // 若全为

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

        // TODO 保存地图信息
        // https://www.xuanyusong.com/archives/831

        Global.instance.mapName = System.DateTime.Now.ToString();

        try
        {
            string appDBPath = Application.persistentDataPath + "/iamttp.db";
            DbAccess db = new DbAccess("URI=file:" + appDBPath);

            Debug.Log("-1-");

            int index = 0;
            List<string[]> insertStr = new List<string[]>();
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    insertStr.Add(new string[] { "'" + Global.instance.mapName + "'", i.ToString(), j.ToString(),
                    tableOfType[i, j].ToString(), index.ToString() });
                    if (tableOfType[i, j] == 2) index++;
                }
            db.InsertIntoAll("mapTable", new string[] { "mapIndex", "i", "j", "type", "indexOfArray" }, insertStr);

            Debug.Log("-2-");

            insertStr = new List<string[]>();
            for (int i = 0; i < houseArray.Count; i++)
            {
                int owner = tableOfObj[houseArray[i].x, houseArray[i].y].GetComponent<Drag>().owner;
                insertStr.Add(new string[] { "'" + Global.instance.mapName + "'", i.ToString(), owner.ToString(), "'" + houseArray[i].ToString() + "'" });
            }
            db.InsertIntoAll("housePosArray", new string[] { "mapIndex", "i", "owner", "Vec2" }, insertStr);

            Debug.Log("-3-");

            insertStr = new List<string[]>();
            for (int i = 0; i < houseArray.Count; i++)
                for (int j = 0; j < houseArray.Count; j++)
                {
                    insertStr.Add(new string[] { "'" + Global.instance.mapName + "'", i.ToString(), j.ToString(), "'" + GetString(houseRoadPath[i, j]) + "'" });
                }
            db.InsertIntoAll("houseRoadPath", new string[] { "mapIndex", "i", "j", "ListVec2" }, insertStr);

            db.CloseSqlConnection();

            Global.instance.isRandMap = false;
            SceneManager.LoadScene(1);
        }
        catch (System.Exception ex)
        {
            bugText.text = ex.Message;
        }
    }

    private static string GetString(List<Vector2Int> lists)
    {
        StringBuilder res = new StringBuilder();
        if (lists != null)
            foreach (var item in lists)
                res.Append(item.ToString()).Append("&");
        return res.ToString();
    }

}
