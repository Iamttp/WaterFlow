using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    //音源AudioSource相当于播放器，而音效AudioClip相当于磁带
    public AudioSource music;
    public AudioClip back;
    public AudioClip down;
    public AudioClip down2;
    public List<AudioClip> la; // 水滴到达水泡音效
    public List<AudioClip> lb; // 水滴碰撞水滴音效
    public static Music instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //给对象添加一个AudioSource组件
            music = gameObject.AddComponent<AudioSource>();
            //设置不一开始就播放音效
            music.playOnAwake = false;

            la = new List<AudioClip>();
            for (int i = 1; i <= 6; i++) // TODO 大小
                la.Add(Resources.Load<AudioClip>("Music/" + i));

            lb = new List<AudioClip>();
            for (int i = 1; i <= 3; i++)
                lb.Add(Resources.Load<AudioClip>("Music/b" + i));

            back = Resources.Load<AudioClip>("Music/back");
            down = Resources.Load<AudioClip>("Music/down");
            down2 = Resources.Load<AudioClip>("Music/down2");
            playBack();
        }
    }
    void Update()
    {
    }

    public void playShot()
    {
        music.PlayOneShot(la[Random.Range(0, la.Count)]);
    }

    public void playShotB()
    {
        music.PlayOneShot(lb[Random.Range(0, lb.Count)]);
    }

    public void playBack()
    {
        music.loop = true;
        music.clip = back;
        music.Play();
    }

    public void playDown()
    {
        music.PlayOneShot(down);
    }

    public void playDown2()
    {
        music.PlayOneShot(down2);
    }
}
