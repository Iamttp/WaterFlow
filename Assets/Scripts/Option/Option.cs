using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public Dropdown dropdownOfOwner;
    public Dropdown dropdownOfSize;
    public Dropdown dropdownOfCas;
    public Dropdown dropdownOfDiff;

    public Slider slider;
    public Slider sliderWater;

    public Toggle deep;
    public Toggle shake;
    public Toggle isMode;

    public Canvas canvas;

    void Start()
    {
        canvas.GetComponent<CanvasGroup>().alpha = 0;
        StartCoroutine(alphaC());

        dropdownOfOwner.value = Global.instance.owner;
        dropdownOfCas.value = Load.index1;
        dropdownOfSize.value = Load.index2;
        dropdownOfDiff.value = Global.instance.diff;

        slider.value = Music.instance.music.volume;
        sliderWater.value = Load.waterSpeed;

        shake.isOn = Global.instance.isShakeOpen;
        deep.isOn = Global.instance.isDeepOpen;
        isMode.isOn = Global.instance.isMode;
    }

    private IEnumerator alphaC()
    {
        if (canvas == null) yield break;
        if (canvas.GetComponent<CanvasGroup>().alpha >= 0.9f) yield break;
        canvas.GetComponent<CanvasGroup>().alpha += 0.1f;
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(alphaC());
    }

    void Update()
    {
    }


    public void sliderControl()
    {
        if (Music.instance.music.isPlaying)
        {
            //音乐播放中设置音乐音量 取值范围 0.0F到 1.0
            Music.instance.music.volume = slider.value;
        }
    }

    public void sliderWaterControl()
    {
        Load.waterSpeed = sliderWater.value;
        if (Back.instance != null) Back.instance.changeSpeed();
    }


    public void backFun()
    {
        Global.instance.owner = dropdownOfOwner.value;
        Load.index1 = dropdownOfCas.value;
        Load.index2 = dropdownOfSize.value;
        Global.instance.sizeOfHouse = Load.casArray[dropdownOfCas.value];
        Global.instance.width = Global.instance.height = Load.sizeArray[dropdownOfSize.value];
        Global.instance.diff = dropdownOfDiff.value;

        Global.instance.isShakeOpen = shake.isOn;
        Global.instance.isDeepOpen = deep.isOn;
        Global.instance.isMode = isMode.isOn;

        Global.instance.DataInit();
        SceneManager.LoadScene(0);
    }

    public void goTip()
    {
        Global.instance.owner = dropdownOfOwner.value;
        Load.index1 = dropdownOfCas.value;
        Load.index2 = dropdownOfSize.value;
        Global.instance.sizeOfHouse = Load.casArray[dropdownOfCas.value];
        Global.instance.width = Global.instance.height = Load.sizeArray[dropdownOfSize.value];
        Global.instance.diff = dropdownOfDiff.value;

        Global.instance.isShakeOpen = shake.isOn;
        Global.instance.isDeepOpen = deep.isOn;
        Global.instance.isMode = isMode.isOn;

        Global.instance.DataInit();
        SceneManager.LoadScene(10);
    }
}
