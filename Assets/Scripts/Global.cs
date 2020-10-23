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
    public int owner;
    public int buffOfAttack; // 默认为1，表示进攻时一个士兵抵消一点防御
    public float buffOfMoveSpeed; // 默认为1，表示移动速度比率
    public float buffOfAddSpeed; // 默认为1，表示城堡人口增加速度比率
    public float buffOfSize; // 默认为1，表示城堡容量比率

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
