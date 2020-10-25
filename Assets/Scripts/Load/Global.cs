using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static Global instance;

    public int fps;

    public int lev;
    public bool flag; // 用于控制，防止重复StartCoroutine
    public bool isStop;

    // 电脑AI参数
    public float timeOfAttack;
    public float timeOfUP;

    // 玩家参数
    public int owner; // 记住 不可在DataInit重置
    public float buffOfAttack; // 默认为0，表示进攻时多大概率一个士兵抵消两点防御
    public float buffOfMoveSpeed; // 默认为1，表示移动速度比率
    public float buffOfAddSpeed; // 默认为1，表示城堡人口增加速度比率
    public float buffOfSize; // 默认为1，表示城堡容量比率

    // 玩家Buff
    public class node
    {
        public string name;
        public string content;
        public float value;
        public int lv;

        public node(string name, string content, float value, int lv = 1)
        {
            this.name = name;
            this.content = content;
            this.value = value;
            this.lv = lv;
        }
    }
    public List<node> ls;
    public Dictionary<string, node> mp;

    // 游戏记录
    public float useTime;
    public int killCas;
    public int lostCas;
    public int killS;
    public int lostS;

    // 特效开关
    public bool isShakeOpen;
    public bool isDeepOpen;

    public void DataInit()
    {
        useTime = 0;
        killCas = 0;
        lostCas = 0;
        killS = 0;
        lostS = 0;

        lev = 1;
        flag = false;
        isStop = false;

        timeOfAttack = 1f;
        timeOfUP = 2f;

        buffOfAttack = 0;
        buffOfMoveSpeed = 1;
        buffOfAddSpeed = 1;
        buffOfSize = 1;

        ls = new List<node>();
        mp = new Dictionary<string, node>();
        ls.Add(new node("attack", "士兵双倍攻击力几率增加15%", buffOfAttack));
        ls.Add(new node("moveSpeed", "士兵移动速度增加50%", buffOfMoveSpeed));
        ls.Add(new node("addSpeed", "城堡士兵增加速度15%", buffOfAddSpeed));
        ls.Add(new node("size", "城堡容量大小增加20%", buffOfSize));
        mp["attack"] = ls[0];
        mp["moveSpeed"] = ls[1];
        mp["addSpeed"] = ls[2];
        mp["size"] = ls[3];
    }

    public void swap(int a, int b)
    {
        node temp = ls[a];
        ls[a] = ls[b];
        ls[b] = temp;
    }

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            DataInit();
            Application.targetFrameRate = fps;
            isShakeOpen = true;
            isDeepOpen = true;
        }
    }

    void Update()
    {
    }

    public void clickIsDeep()
    {
        isShakeOpen = !isShakeOpen;
    }

    public void clickIsShake()
    {
        isDeepOpen = !isDeepOpen;
    }
}
