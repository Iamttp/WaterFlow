using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static Global instance;

    public bool isRandMap;
    public string mapName;

    public int fps;

    public int lev;
    public bool flag; // 用于控制，防止重复StartCoroutine
    public bool isStop;

    // 电脑AI参数
    public float timeOfAttack;
    public float timeOfUP;

    // 玩家参数
    public int owner; // 记住 不可在DataInit重置
    public float buffOfAttack; // 默认为0，表示进攻时多大概率一个水滴抵消两点防御
    public float buffOfMoveSpeed; // 默认为1，表示移动速度比率
    public float buffOfAddSpeed; // 默认为1，表示水泡人口增加速度比率
    public float buffOfSize; // 默认为1，表示水泡容量比率

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
    public int upTime; // 升级次数

    // 特效开关
    public bool isMode;
    public bool isShakeOpen;
    public bool isDeepOpen;

    public int width;
    public int height;
    public int sizeOfHouse;
    public int diff;

    public Color[] colorTable;      // 不同势力颜色索引

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

        upTime = 0;

        ls = new List<node>();
        mp = new Dictionary<string, node>();

        ls.Add(new node("attack", "水滴双倍攻击力几率增加15%", buffOfAttack));
        ls.Add(new node("moveSpeed", "水滴移动速度增加50%", buffOfMoveSpeed));
        ls.Add(new node("addSpeed", "水泡中水滴增加速度15%", buffOfAddSpeed));
        ls.Add(new node("size", "水泡容量大小增加20%", buffOfSize));
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
        }
    }

    void Update()
    {
    }

    public int getScore()
    {
        int scoreNum = instance.killCas * 100 + instance.killS * 10 -
    instance.lostCas * 10 - instance.lostS * 1 + instance.upTime * 200;
        if (instance.diff == 0) // 中等
        {
        }
        if (instance.diff == 1) // 困难
        {
            scoreNum *= 5;
        }
        if (instance.diff == 2) // 入门
        {
            scoreNum /= 2;
        }
        if (instance.diff == 3) // 地狱
        {
            scoreNum *= 10;
        }
        scoreNum += (1000 - (int)instance.useTime);
        if (scoreNum < 0) scoreNum = 0;
        return scoreNum;
    }
}
