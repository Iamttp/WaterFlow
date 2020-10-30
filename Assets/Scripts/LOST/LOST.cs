using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LOST : MonoBehaviour
{
    public Text text;
    static string[] diffs = new string[] { "一般", "困难", "简单", "地狱" };
    static string[] size2s = new string[] { "中等", "偏大", "偏小", "特大" };
    static string[] size1s = new string[] { "中等", "偏多", "偏小", "特多" };

    void Start()
    {
        int num = Global.instance.getScore();
        text.text = "失败 ! 分数 : " + num +
            "\n难度等级: " + diffs[Global.instance.diff] +
            "\n地图大小: " + size2s[Load.index2] +  // 水泡数
            "\n水泡数目: " + size1s[Load.index1] +
            "\n\n用时 : " + (int)(Global.instance.useTime) + "s" +
            "\n攻占水泡 ： " + Global.instance.killCas +
            "\n失去水泡 ： " + Global.instance.lostCas +
            "\n消灭水滴 ： " + Global.instance.killS +
            "\n失去水滴 ： " + Global.instance.lostS;
        LIST.write(num.ToString());
    }

    void Update()
    {

    }

    public void backFun()
    {
        Global.instance.DataInit();
        SceneManager.LoadScene(0);
    }
}
