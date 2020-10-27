using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LIST : MonoBehaviour
{
    public Text score;
    public static string AssetCachesDir
    {
        get
        {
            string dir = "";
#if UNITY_EDITOR
            dir = Application.dataPath + "/Resources/";
#elif UNITY_IOS
            dir = Application.temporaryCachePath + "/";//路径：Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Library/Caches/
#elif UNITY_ANDROID
            dir = Application.persistentDataPath + "/";//路径：/data/data/xxx.xxx.xxx/files/
#else
            dir = Application.dataPath + "/Resources/";//路径：/xxx_Data/StreamingAssets/
#endif
            return dir;
        }
    }

    void Start()
    {
        FileInfo fi = new FileInfo(AssetCachesDir + "Json.txt");

        if (!fi.Exists)
        {
            score.text = "暂无记录";
            return;
        }

        List<string> strArray = read();
        strArray.Sort();
        strArray.Reverse();
        StringBuilder str = new StringBuilder();
        for (int i = 0; i < strArray.Count; i++)
        {
            str.Append((i + 1) + " ------------------------------------ " + strArray[i] + "\n");
        }
        score.text = str.ToString();
    }

    void Update()
    {

    }

    public static void write(string str)
    {
        FileInfo fi = new FileInfo(AssetCachesDir + "Json.txt");
        StreamWriter sw;
        if (fi.Exists)
        {
            sw = fi.AppendText();
        }
        else
        {
            sw = fi.CreateText();
        }
        sw.WriteLine(str);
        sw.Close();
    }

    List<string> read()
    {
        List<string> strArray = new List<string>();
        FileStream fi = new FileStream(AssetCachesDir + "Json.txt", FileMode.Open);
        if (fi.CanRead)
        {
            StreamReader sw = new StreamReader(fi);
            string jsonStr;
            while ((jsonStr = sw.ReadLine()) != null)
            {
                strArray.Add(jsonStr);
            }
        }
        return strArray;
    }

    public void backFun()
    {
        Global.instance.DataInit();
        SceneManager.LoadScene(0);
    }
}
