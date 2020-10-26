using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    public Dropdown dropdownOfOwner;
    public Dropdown dropdownOfSize;
    public Dropdown dropdownOfCas;
    public Dropdown dropdownOfDiff;

    public static int index1;
    public static int index2;

    public static int[] casArray = new int[] { 8, 12, 4, 16 };
    public static int[] sizeArray = new int[] { 40, 60, 20, 80 };

    public static int[] camSize = new int[] { 20, 30, 10, 40 };
    public static int[] camX = new int[] { 20, 30, 10, 40 };
    public static int[] camY = new int[] { 20, 30, 10, 40 };

    public static float[] camShowScale = new float[] { 0.9f, 0.6f, 1.8f, 0.45f};

    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // android 息屏
    }

    void Update()
    {

    }

    public void loading()
    {
        SceneManager.LoadScene(1);
        Global.instance.owner = dropdownOfOwner.value;
        index1 = dropdownOfCas.value;
        index2 = dropdownOfSize.value;
        Global.instance.sizeOfHouse = casArray[dropdownOfCas.value];
        Global.instance.width = Global.instance.height = sizeArray[dropdownOfSize.value];
        Global.instance.diff = dropdownOfDiff.value;
    }

    public void loading2()
    {
        SceneManager.LoadScene(5);
    }
}
