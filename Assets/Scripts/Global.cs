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
    public int buffOfAttack; // 默认为1，表示进攻时一个士兵抵消一点防御
    public float buffOfMoveSpeed; // 默认为1，表示移动速度比率
    public float buffOfAddSpeed; // 默认为1，表示城堡人口增加速度比率
    public float buffOfSize; // 默认为1，表示城堡容量比率

    // 玩家Buff
    public class node
    {
        public string name;
        public string content;
        public float lv;

        public node(string name, string content, float lv = 1.0f)
        {
            this.name = name;
            this.content = content;
            this.lv = lv;
        }
    }
    public List<node> ls;
    public Dictionary<string, node> mp;

    public void DataInit()
    {
        lev = 1;
        flag = false;
        isStop = false;

        timeOfAttack = 1f;
        timeOfUP = 2f;
        
        buffOfAttack = 1;
        buffOfMoveSpeed = 1;
        buffOfAddSpeed = 1;
        buffOfSize = 1;

        ls = new List<node>();
        mp = new Dictionary<string, node>();
        ls.Add(new node("attack", "士兵攻击力增加100%"));
        ls.Add(new node("moveSpeed", "士兵移动速度增加100%"));
        ls.Add(new node("addSpeed", "城堡士兵增加速度100%"));
        ls.Add(new node("size", "城堡容量大小增加100%"));
        mp["attack"] = ls[0];
        mp["moveSpeed"] = ls[1];
        mp["addSpeed"] = ls[2];
        mp["size"] = ls[3];
    }

    public void swap(int a,int b)
    {
        node temp = ls[a];
        ls[a] = ls[b];
        ls[b] = temp;
    }

    void Awake()
    {
        if(instance == null)
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
}
