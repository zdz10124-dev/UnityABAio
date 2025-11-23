using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public string abilityName;
    public string description;
    public int AbilityLevel;
    public System.Action<int> unlockAction;
    public System.Func<int,bool> AbleCheck;//参数int返回值bool
}
public class Attributes : MonoBehaviour
{
    public int IsPlayer = 0;

    public float MoveSpeed = 1f;
    //关于角色相关的东西在inspector里改是没有用的


    public float hp = 10f;
    public float MaxHP = 10f;

    public float Defense = 0f;
    //攻击相关
    public float BulletSpeed = 5f;//子弹速度
    public float AttackRange = 5f;//攻击距离
    public float AttackPower = 1f;//攻击力
    public int AttackTime = 20;//攻击间隔
    //升级相关
    public float xp = 0f;
    public float NextLevelXP = 5;
    public float level = 0;
    public int AbilityPerLevel = 3;

    public int MoveSpeedLV = 0;
    public int DefenseLV = 0;
    public int AttackPowerLV = 0;
    public int AttackRangeLV = 0;
    //机器人
    public float VisionRange = 5f;//类似半径，这里设置的是一半的，方便与射程对比
    public float ExtraChaseRange = 2;//在视野外额外追击的距离
    public float ExtraFleeRange = 8;//在超过这个距离后停止逃跑
    public float FleeHPPersent = 0.2f;//血量比例为这个时逃跑
    //能力
    public List<Ability> allAbilities;
    public int MyStyle = -1;//选择的流派
    public enum AbilityStyle
    {
        FirePower,//火力流
        Sniper,//狙击流
        Thorns,//反伤流
        EatFruit,//吃水果流
        Assassin,//刺客
        Summon//召唤师
    };
    public enum AbilityLevels
    {
        Hedgehog
    }
    public List<int> AbilityLevel =new List<int> {0};
    public float AbilityHedgehog=0;

    //部分引用
    public GameObject GameOver;
    public Camera TempCamera;
    public Canvas InGameCanvas;
    // Start is called before the first frame update
    public void Reset()//用于死后重置
    {    
        MoveSpeed = 1f;



        hp = 10f;
        MaxHP = 10f;

         Defense = 0f;
        //攻击相关
        BulletSpeed = 5f;//子弹速度
        AttackRange = 5f;//攻击距离
        AttackPower = 1f;//攻击力
        AttackTime = 20;//攻击间隔
        //升级相关
        xp = 0f;
        NextLevelXP = 5;
        level = 0;
        AbilityPerLevel = 3;

        MoveSpeedLV = 0;
        DefenseLV = 0;
        AttackPowerLV = 0;
        AttackRangeLV = 0;
        //按钮啊，人物啊需要做的隐身现身
        GameOver.gameObject.SetActive(false);
        gameObject.SetActive(true);
        TempCamera.gameObject.SetActive(false);
        InGameCanvas.gameObject.SetActive(true);
        //一些ui需要更新
        gameObject.GetComponent<PlayerLevelUP>().UpdateLV();
        //重置特殊能力
        for (int i = 0; i < allAbilities.Count; i++) allAbilities[i].AbilityLevel = 0;
        AbilityHedgehog = 0;
    }
    void Start()
    {
        //Reset();
        InitializeAbilities(); // 初始化能力列表
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetDamage(Attributes EnemyAttributes,float damage,bool ReflectDamage=false)
    {
        hp -= (damage * 2 / (Defense + 2));//扣血公式：乘以（2/防御力+2)
        Debug.LogFormat("当前反伤比例是{0},isplayer是{1}\n", AbilityHedgehog, IsPlayer);
        if(AbilityHedgehog>0 && !ReflectDamage)EnemyAttributes.GetDamage(this, damage*AbilityHedgehog,true);//反弹伤害 且不反弹反伤的反伤
    }
    void InitializeAbilities()
    {
        allAbilities = new List<Ability>
        {
            new Ability {
                abilityName = "刺猬",
                description = "可以在收到伤害时反弹部分伤害",
  
                unlockAction = (int L) => {
                    MyStyle=(int)AbilityStyle.Thorns;
                    if(L==1)AbilityHedgehog=0.1f;//反弹的伤害比例
                    if(L==2)AbilityHedgehog=0.3f;
                    if(L==3)AbilityHedgehog=0.5f;
                    if(L==4)AbilityHedgehog=0.6f;
                    if(L==5)AbilityHedgehog=0.7f;

                },
                AbleCheck = (int L)=>
                {
                    if(L==5)return false;//到达最大等级
                    if(MyStyle==-1 || MyStyle==(int)AbilityStyle.Thorns)return true;
                    else return false;//已经有了别的流派
                }
            },
            new Ability {
                abilityName = "冲刺",
                description = "短时间内大幅提升移动速度",
                unlockAction = (int L) => { 
                    // 具体的冲刺解锁逻辑
                    Debug.Log("冲刺已解锁！");
                },
                AbleCheck = (int L)=>
                {
                   return true;
                }
            },
            new Ability {
                abilityName = "冲刺",
                description = "短时间内大幅提升移动速度",
                unlockAction = (int L) => { 
                    // 具体的冲刺解锁逻辑
                    Debug.Log("冲刺已解锁！");
                },
                AbleCheck = (int L)=>
                {
                    return true;
                }
            }
            // 其他能力...
        };
    }
}

