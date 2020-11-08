using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Select : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void loadRand() // 开始游戏
    {
        Global.instance.isRandMap = true;
        SceneManager.LoadScene(1);
    }

    public void loadUnRand()
    {
        Global.instance.isRandMap = false;
        SceneManager.LoadScene(9);
    }
}
