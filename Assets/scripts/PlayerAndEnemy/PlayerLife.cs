using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class PlayerLife : MonoBehaviour
{
    private Attributes Attributes;
    // Start is called before the first frame update
    void Start()
    {
        Attributes = GetComponent<Attributes>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Attributes.hp <= 0)
        {
            die();
        }
    }
    void die()
    {
        if (Attributes.IsPlayer==0)transform.parent.gameObject.GetComponent<FindPool>().MyPool.gameObject.GetComponent<EverythingPool>().ReturnItem(transform.parent.gameObject);
        else
        {
            Attributes.GameOver.gameObject.SetActive(true);//出现游戏结束的按钮
            Attributes.TempCamera.gameObject.SetActive(true);//临时设置一个摄像头
            gameObject.SetActive(false);
        }
    }
}
