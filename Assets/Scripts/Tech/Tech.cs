using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tech : MonoBehaviour
{
    public Button[] buttons;
    public Text[] texts;
    public Text story;
    private string[] strs = new string[]
    {
        "公元2011年，NASA开普勒计划确认首颗位于宜居带的系外行星Kepler-22b，其直径大约是地球的2.4倍，距离人类大约有600光年。表面温度约为70华氏度(相当于21摄氏度)。",
        "公元2080年，SpaceX确认Kepler-22b为一颗温暖的海洋行星，存在大面积水域，并极可能存在高等生物。",
        "公元2240年，墨子号量子科学实验卫星接收到Kepler-22b高智慧生命体的友好信号，它们生活在富含氧气的巨大海洋中，并拥有极强的精神能力，能够自由的控制水泡、水滴。",
        "公元2352年，神舟零号飞船帮助人类实现虫洞旅行，登陆到Kepler-22b星球上，在一望无际的海洋中，只发现了断壁残垣，透过时隐时现的光线，科学家发现了证明它们存在过的唯一证据，被注入精神力的水泡、水滴。",
        "在得知高智慧生命体的消亡在于微小如病毒、数目如宇宙繁星的海洋塑料碎片造成Kepler-22b生态失衡后，程序员pttmai设计了一款游戏，希望为它们在宇宙中留下点记忆。"
    };

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
        story.text = strs[Global.instance.lev - 1];
        canvas.GetComponent<CanvasGroup>().alpha = 0;
        StartCoroutine(alphaC());
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

    public Canvas canvas;

    private IEnumerator alphaC()
    {
        if (canvas.GetComponent<CanvasGroup>().alpha >= 0.8f) yield break;
        canvas.GetComponent<CanvasGroup>().alpha += 0.1f;
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(alphaC());
    }

    void Update()
    {

    }
}
