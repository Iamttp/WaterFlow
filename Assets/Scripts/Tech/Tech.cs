using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tech : MonoBehaviour
{
    public Button[] buttons;
    public Text[] texts;

    public void attackFunc()
    {
        Global.instance.mp["attack"].lv++;
        Global.instance.mp["attack"].value += 0.15f;
        Global.instance.buffOfAttack = Global.instance.mp["attack"].value;
        SceneManager.LoadScene(2);
    }
    public void moveSpeedFunc()
    {
        Global.instance.mp["moveSpeed"].lv++;
        Global.instance.mp["moveSpeed"].value += 0.5f;
        Global.instance.buffOfMoveSpeed = Global.instance.mp["moveSpeed"].value;
        SceneManager.LoadScene(2);
    }
    public void addSpeedFunc()
    {
        Global.instance.mp["addSpeed"].lv++;
        Global.instance.mp["addSpeed"].value += 0.15f;
        Global.instance.buffOfAddSpeed = Global.instance.mp["addSpeed"].value;
        SceneManager.LoadScene(2);
    }
    public void sizeFunc()
    {
        Global.instance.mp["size"].lv++;
        Global.instance.mp["size"].value += 0.2f;
        Global.instance.buffOfSize = Global.instance.mp["size"].value;
        SceneManager.LoadScene(2);
    }

    void Start()
    {
        initText();
    }

    void initText()
    {
        // 洗牌算法
        for (int i = Global.instance.ls.Count - 1; i >= 0; i--)
        {
            int ran = Random.Range(0, i + 1);
            Global.instance.swap(i, ran);
        }

        for (int i = 0; i < 3; i++)
        {
            texts[i].text = Global.instance.ls[i].content + "\n当前等级：" + Global.instance.ls[i].lv;
            if (Global.instance.ls[i].name == "attack") buttons[i].onClick.AddListener(attackFunc);
            else if (Global.instance.ls[i].name == "moveSpeed") buttons[i].onClick.AddListener(moveSpeedFunc);
            else if (Global.instance.ls[i].name == "addSpeed") buttons[i].onClick.AddListener(addSpeedFunc);
            else if (Global.instance.ls[i].name == "size") buttons[i].onClick.AddListener(sizeFunc);
        }
    }

    void Update()
    {

    }
}
