using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AllControl;

public class PlayerLife : MonoBehaviour
{
    public Attributes LastAttacker;
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
        for (int i = Attributes.DamageLines.Count - 1; i >= 0; i--)
        {
            Attributes.DamageLinePool.GetComponent<EverythingPool>().ReturnItem(Attributes.DamageLines[i]);//归还所有伤害线
        }
        if (LastAttacker != null)
        {
            LastAttacker.xp += Attributes.TotalXP * GameManager.Instance.LootXPRate + GameManager.Instance.BasicLootXP;
        }
        if (Attributes.IsPlayer == 0)
        { 
            //transform.parent.gameObject.GetComponent<FindPool>().MyPool.gameObject.GetComponent<EverythingPool>().ReturnItem(transform.parent.gameObject);
            transform.position = Vector3.one;//临时写的，可以一直爽
            Attributes.hp = Attributes.MaxHP;//这样怪物一直重生我可以一直杀杀杀
        } 
        else
        {
            Attributes.GameOver.gameObject.SetActive(true);//出现游戏结束的按钮
            Attributes.TempCamera.gameObject.SetActive(true);//临时设置一个摄像头
            Attributes.TempCamera.transform.position=new Vector3(transform.position.x,transform.position.y,-10);//移动到角色死亡位置
            Attributes.InGameCanvas.gameObject.SetActive(false);//防止升级按钮挡视线
            if(Attributes.IsPlayer==1)//去除ui
            {
                gameObject.GetComponent<PlayerLevelUP>().HideAbilityButton();
                gameObject.GetComponent<PlayerLevelUP>().HideButton();
                gameObject.GetComponent<PlayerLevelUP>().WaitQueue = 0;
                
            }

            gameObject.SetActive(false);//禁用物体
        }
    }
}
