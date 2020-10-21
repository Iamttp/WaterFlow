using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    public Dropdown dropdown;

    void Start()
    {
    }

    void Update()
    {

    }

    public void loading()
    {
        SceneManager.LoadScene(1);
        Global.instance.owner = dropdown.value;
    }
}
