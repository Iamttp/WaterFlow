using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour
{
    public Button ButtonPrefab;
    public GameObject panel;
    public string mapName;
    public Text info;

    void Start()
    {
        string appDBPath = Application.persistentDataPath + "/iamttp.db";
        if (System.IO.File.Exists(appDBPath))
        {
            DbAccess db = new DbAccess("URI=file:" + appDBPath);

            using (SqliteDataReader sqReader = db.ExecuteQuery("select mapIndex from mapTable group by mapIndex"))
            {
                while (sqReader.Read())
                {
                    string str = sqReader.GetString(sqReader.GetOrdinal("mapIndex"));
                    Button NewButton = Instantiate(ButtonPrefab);
                    NewButton.transform.SetParent(panel.transform);
                    NewButton.GetComponentInChildren<Text>().text = str;
                    NewButton.onClick.AddListener(delegate ()
                    {
                        OnClickAddButton(NewButton);
                    });
                }
                sqReader.Close();
            }
            db.CloseSqlConnection();
        }
    }

    void OnClickAddButton(Button NewButton)
    {
        string appDBPath = Application.persistentDataPath + "/iamttp.db";
        DbAccess db = new DbAccess("URI=file:" + appDBPath);

        mapName = NewButton.GetComponentInChildren<Text>().text;
        int size = 0;
        int sizeOfHouse = 0;
        using (SqliteDataReader sqReader = db.SelectWhere("mapTable", new string[] { "i", "j", "type", "indexOfArray" }, new string[] { "mapIndex" }, new string[] { "=" }, new string[] { mapName }))
        {
            while (sqReader.Read())
            {
                size++;
                if (sqReader.GetInt32(sqReader.GetOrdinal("type")) == 2) sizeOfHouse++;
            }
            sqReader.Close();
        }

        info.text = mapName
            + "\n地图大小 : " + (int)Mathf.Sqrt(size) + "*" + (int)Mathf.Sqrt(size)
            + "\n水泡数目 : " + sizeOfHouse;
    }

    void Update()
    {

    }

    public void startGame()
    {
        if (mapName == null || mapName == "") return;
        Global.instance.mapName = mapName;
        Global.instance.isRandMap = false;
        SceneManager.LoadScene(1);
    }

    public void startCreation()
    {
        Global.instance.isRandMap = false;
        SceneManager.LoadScene(7);
    }

    public void deleteCreation()
    {
        if (mapName == null || mapName == "") return;

        string appDBPath = Application.persistentDataPath + "/iamttp.db";
        DbAccess db = new DbAccess("URI=file:" + appDBPath);

        using (SqliteDataReader sqReader = db.ExecuteQuery("delete from mapTable where mapIndex = '" + mapName + "'"))
        {
            sqReader.Close();
        }
        using (SqliteDataReader sqReader = db.ExecuteQuery("delete from houseRoadPath where mapIndex = '" + mapName + "'"))
        {
            sqReader.Close();
        }
        using (SqliteDataReader sqReader = db.ExecuteQuery("delete from housePosArray where mapIndex = '" + mapName + "'"))
        {
            sqReader.Close();
        }
        db.CloseSqlConnection();

        Global.instance.isRandMap = false;
        SceneManager.LoadScene(9);
    }
}
