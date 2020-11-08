using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    public Canvas canvas;

    public static int index1;
    public static int index2;

    public static int[] casArray = new int[] { 8, 12, 4, 16 };
    public static int[] sizeArray = new int[] { 40, 60, 20, 80 };

    public static int[] camSize = new int[] { 20, 30, 10, 40 };
    public static int[] camX = new int[] { 20, 30, 10, 40 };
    public static int[] camY = new int[] { 20, 30, 10, 40 };

    //public static float[] camShowScale = new float[] { 0.9f, 0.6f, 1.8f, 0.45f };

    public static float waterSpeed;

    private static bool flag = false;

    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // android 息屏

        if (!flag)
        {
            flag = true;
            Global.instance.owner = 0;
            index1 = 0;
            index2 = 0;
            Global.instance.sizeOfHouse = casArray[0];
            Global.instance.width = Global.instance.height = sizeArray[0];
            Global.instance.diff = 0;

            waterSpeed = 0.01f;

            Global.instance.isShakeOpen = true;
            Global.instance.isDeepOpen = true;
        }

        canvas.GetComponent<CanvasGroup>().alpha = 0;
        StartCoroutine(alphaC());
    }

    private IEnumerator alphaC()
    {
        if (canvas == null) yield break;
        if (canvas.GetComponent<CanvasGroup>().alpha >= 0.9f) yield break;
        canvas.GetComponent<CanvasGroup>().alpha += 0.1f;
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(alphaC());
    }

    public void loading() // 开始游戏
    {
        SceneManager.LoadScene(8);
    }

    public void loadOption()
    {
        SceneManager.LoadScene(6);
    }

    public void loading2() // 排行榜
    {
        SceneManager.LoadScene(5);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
