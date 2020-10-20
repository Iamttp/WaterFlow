using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSelect : MonoBehaviour
{
    [Header("House属性")]
    public int value;

    [Header("单位属性")]
    public static float timeDis = 0.1f;

    public GameObject player; // save player
    public int index; // houseArray index


    void Start()
    {
        value = 3;
    }

    void Update()
    {

    }

    void OnTouched()
    {
        this.gameObject.GetComponent<MeshRenderer>().material =
            Resources.Load<Material>("Materials/New Material 3");
    }

    void UnTouched()
    {
        this.gameObject.GetComponent<MeshRenderer>().material =
            Resources.Load<Material>("Materials/New Material 1");
    }


    public IEnumerator DelayToInvokeDo(GameObject lastObj)
    {
        var lastObjScript = lastObj.GetComponent<HouseSelect>();
        if (lastObjScript.value <= 0)
        {
            yield break;
        }

        PlayerMove script = player.GetComponent<PlayerMove>();
        script.dstHouse = gameObject;
        script.path = ManageScene.instance.houseRoadPath[lastObjScript.index, index];
        script.basePos = lastObj.transform.position;
        script.now = 0;
        Instantiate(player, transform.position, new Quaternion());
        lastObjScript.value--;
        
        yield return new WaitForSeconds(timeDis);
        StartCoroutine(DelayToInvokeDo(lastObj));
    }


    void JustAttack(GameObject lastObj)
    {
        Debug.Log("派遣");
        StartCoroutine(DelayToInvokeDo(lastObj));
    }
}
