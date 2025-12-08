using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AllControl;

public class PlayerLife : MonoBehaviour
{
    public Attributes LastAttacker;
    private Attributes Attributes;
    // Start is called before the first frame update
    public Attributes Owner;
    public EverythingPool SummonedCreaturePool;
    //脚本引用
    public GenerateSpawnPoint GenerateSpawnPoint;
    public void Start()
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
        if (LastAttacker != null && Attributes.IsPlayer!=2)//如果是召唤物就不给经验了
        {
            float dxp = Attributes.TotalXP * GameManager.Instance.LootXPRate + GameManager.Instance.BasicLootXP;
            if (LastAttacker.IsPlayer == 1 || LastAttacker.IsPlayer == 0) LastAttacker.xp += dxp;
            else if (LastAttacker.IsPlayer == 2) LastAttacker.gameObject.GetComponent<PlayerLife>().Owner.xp += dxp;//如果是召唤物，就给它的主人加经验
        }
        if (Attributes.IsPlayer == 0)
        {
            //transform.parent.gameObject.GetComponent<FindPool>().MyPool.gameObject.GetComponent<EverythingPool>().ReturnItem(transform.parent.gameObject);
            Attributes.hp = Attributes.MaxHP;//这样怪物一直重生我可以一直杀杀杀
            ReSpawn();
        }
        else if (Attributes.IsPlayer == 2)//召唤物死亡直接扔进对象池没什么好说的
        {
            Owner.AbilitySummonCreatureList.Remove(Attributes);//在主人的列表里删除
            SummonedCreaturePool.ReturnItem(gameObject);
        }
        else
        {
            Attributes.GameOver.gameObject.SetActive(true);//出现游戏结束的按钮
            Attributes.TempCamera.gameObject.SetActive(true);//临时设置一个摄像头
            Attributes.TempCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);//移动到角色死亡位置
            Attributes.InGameCanvas.gameObject.SetActive(false);//防止升级按钮挡视线
            Attributes.PlayerUI.EnhancedAttackPicture.gameObject.SetActive(false);//去除强普ui
            Attributes.PlayerUI.FlashPicture.gameObject.SetActive(false);//去除闪现ui

            gameObject.GetComponent<PlayerLevelUP>().HideAbilityButton();
            gameObject.GetComponent<PlayerLevelUP>().HideButton();
            gameObject.GetComponent<PlayerLevelUP>().WaitQueue = 0;
            gameObject.GetComponent<PlayerLevelUP>().WaitQueue2 = 0;
            //去除召唤物
            for (int i = 0; i < Attributes.AbilitySummonCreatureList.Count; i++) { Attributes.AbilitySummonCreatureList[i].hp = 0; }//如果召唤的人死了，它的召唤物也都血量归零

            gameObject.SetActive(false);//禁用物体
        }
    }
    public void ReSpawn()
    {
        //Debug.LogFormat("正在为{0}设置出生点",Attributes.name);
        if (GenerateSpawnPoint != null && Attributes != null) Attributes.transform.position = GenerateSpawnPoint.FindAPlaceToSpawn();
        else if (GenerateSpawnPoint == null) Debug.Log("没有找到GenerateSpawnPoint");
        else if (Attributes == null) Debug.Log("没有找到Attributes");
    }
}
