using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tip : MonoBehaviour
{
    public Text text;
    string[] str;
    int index = 0;

    void Start()
    {
        Music.instance.playBack2();
        str = new string[]
        {
            "",
            "1. 游戏可以在选项界面选择不同难度，地图大小，水泡数目等。\n",
            "2. 游戏共5关，每次过关后可以提升一项能力，越后面的关卡，电脑操作速度越快。\n",
            "3. 难度越大越容易获得高分。\n",
            "4. 戴上耳机可以感受水滴声，放松身心。\n",
            "5. 派遣水滴数为起点水泡的水滴数的一半，同时会考虑到终点水泡的水滴数目。\n",
            "6. 水泡升级消耗一定的水滴，升级后部分属性会提升。\n",
            "7. 拖动模式适用于手机操作，起点和终点水泡判定取决于拖动手势的起点和终点。非拖动模式可以查看属性，派遣水滴，适用于电脑操作\n",
            "8. 困难和噩梦难度可以通过暂停和非拖动模式查看水泡属性\n",
        };
        str[0] = text.text;
    }

    void Update()
    {

    }

    public void backFun()
    {
        Music.instance.playBack();
        Global.instance.DataInit();
        SceneManager.LoadScene(0);
    }

    public void nextText()
    {
        if (++index < str.Length)
            text.text = str[index];
        else
            index = str.Length - 1;
    }

    public void preText()
    {
        if (--index >= 0)
            text.text = str[index];
        else
            index = 0;
    }
}
