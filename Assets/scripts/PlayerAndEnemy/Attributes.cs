using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AllControl;

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
    public float TotalXP= 0f;
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
    public List<int> AbilityLevel =new List<int> {0};
    //具体能力的数据
    public float AbilityThornsMaxMoveSpeed = 2f;//反伤流限制的最大速度
    public float AbilityThornsHedgehog=0;//刺猬的反伤比例
    public float AbilityThornsSheild = 1;//能力：盾 的防御力倍率
    public float AbilityThornsHoldGround = 1;//能力：坚守阵地的防御力增幅比例
    public int AbilityThornsHurtEachOther = 0;//能力：互相伤害，指示是否启用
    public float AbilityThornsHurtEachOtherRange = 0;//线的最大距离
    public float AbilityThornsHurtEachOtherDamage = 0;//线的帧伤
    public List<GameObject> DamageLines;
    public List<Attributes> LiningPlayers;


    //部分引用
    //死亡后界面相关
    public GameObject GameOver;
    public Camera TempCamera;
    public Canvas InGameCanvas;
    //其他
    private Rigidbody2D rb;
    //对象池
    public GameObject DamageLinePool;
    // Start is called before the first frame update
    public void Reset()//用于死后重置
    {
        //return;//临时测试不重置
        MoveSpeed = 2f;



        hp = 10f;
        MaxHP = 10f;

         Defense = 0f;
        //攻击相关
        BulletSpeed = 10f;//子弹速度
        AttackRange = 5f;//攻击距离
        AttackPower = 1f;//攻击力
        AttackTime = 20;//攻击间隔
        //升级相关
        xp = 0f;
        TotalXP = 0f;
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
        AbilityThornsHedgehog = 0;
        AbilityThornsSheild = 1;
        AbilityThornsHoldGround = 1;
        AbilityThornsHurtEachOther = 0;//能力：互相伤害，指示是否启用
        AbilityThornsHurtEachOtherRange = 0;//线的最大距离
        AbilityThornsHurtEachOtherDamage = 0f;//线的帧伤
        DamageLines.Clear();
        LiningPlayers.Clear();
}
    void Start()
    {
        //Reset();
        InitializeAbilities(); // 初始化能力列表
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        BeDamaedByLine();
    }
    void BeDamaedByLine()
    {
        for (int i = LiningPlayers.Count - 1; i >= 0; i--)
        {
            Attributes enemy = LiningPlayers[i];
            if(Vector2.Distance(rb.transform.position,enemy.rb.transform.position)>enemy.AbilityThornsHurtEachOtherRange || enemy==null)//超出链接或者攻击者死了
            {
                DamageLinePool.GetComponent<EverythingPool>().ReturnItem(DamageLines[i]);
                DamageLines.RemoveAt(i);
                LiningPlayers.RemoveAt(i);//超出范围，去除链接
                continue;
            }
            GetDamage(enemy, enemy.AbilityThornsHurtEachOtherDamage, false);//敌我双方受到伤害，且不触发反伤
            enemy.GetDamage(enemy, enemy.AbilityThornsHurtEachOtherDamage, false);
        }
    }
    bool CheckStatic()
    {
        if ((rb.velocity.x < GameManager.Instance.MinStep || rb.velocity.x > -GameManager.Instance.MinStep) && (rb.velocity.y < GameManager.Instance.MinStep || rb.velocity.y > -GameManager.Instance.MinStep)) return true;//x,y速度小于某一值判定为静止
        return false;
     }
    public void GetDamage(Attributes EnemyAttributes,float damage,bool ReflectDamage=true)
    {
        gameObject.GetComponent<PlayerLife>().LastAttacker = EnemyAttributes;
        if (!CheckStatic()) hp -= (damage * 2 / (Defense + 2));//扣血公式：乘以（2/防御力+2)
        else hp -= (damage * 2 / (Defense*AbilityThornsHoldGround + 2));//如果静止，乘以系数
        Debug.LogFormat("当前反伤比例是{0},isplayer是{1}\n", AbilityThornsHedgehog, IsPlayer);
        if(AbilityThornsHedgehog>0 && ReflectDamage)EnemyAttributes.GetDamage(this, damage*AbilityThornsHedgehog,false);//反弹伤害 且不反弹反伤的反伤
    }
    void InitializeAbilities()
    {
        allAbilities = new List<Ability>
        {
            new Ability {
                abilityName = "刺猬",
                description = "可以在受到伤害时反弹部分伤害（用格挡前的伤害计算）",
  
                unlockAction = (int L) => {
                    if(L==1)//初次调用进行初始化
                    {
                        MyStyle=(int)AbilityStyle.Thorns;
                        BulletSpeed/=3;
                    }
                    if(L==1)AbilityThornsHedgehog=0.1f;//反弹的伤害比例
                    else if(L==2)AbilityThornsHedgehog=0.3f;
                    else if(L==3)AbilityThornsHedgehog=0.5f;
                    else if(L==4)AbilityThornsHedgehog=0.6f;
                    else if(L==5)AbilityThornsHedgehog=0.7f;

                },
                AbleCheck = (int L)=>
                {
                    if(L==5)return false;//到达最大等级
                    if(MyStyle==-1 || MyStyle==(int)AbilityStyle.Thorns)return true;
                    else return false;//已经有了别的流派
                }
            },
            new Ability {
                abilityName = "血牛",
                description = "立即回满血量，获得更多的最大血量",
                unlockAction = (int L) => {
                    if(L==1){hp=15; MaxHP=15; }
                    else if(L==2){hp=19;MaxHP=29; }
                    else if(L==3){hp=23;MaxHP=23; }
                    else if(L==4){hp=27;MaxHP=27; }
                    else if(L==5){hp=30;MaxHP=30; }
                },
                AbleCheck = (int L)=>
                {
                    if(L==5)return false;//到达最大等级
                    if(MyStyle==(int)AbilityStyle.Thorns)return true;
                    else return false;//别的流派
                }
            },
            new Ability {
                abilityName = "盾",
                description = "防御力乘以某一倍率",
                unlockAction = (int L) => {
                    if(L==1)AbilityThornsSheild=1.5f;
                    else if(L==2)AbilityThornsSheild=1.8f;
                    else if(L==3)AbilityThornsSheild=2.1f;
                    else if(L==4)AbilityThornsSheild=2.3f;
                    else if(L==5)AbilityThornsSheild=2.5f;
                },
                AbleCheck = (int L)=>
                {
                    if(L==5)return false;//到达最大等级
                    if(MyStyle==(int)AbilityStyle.Thorns)return true;
                    else return false;//别的流派
                }
            },
            new Ability {
                abilityName = "坚守阵地",
                description = "静止时防御力翻倍",
                unlockAction = (int L) => {
                    AbilityThornsHoldGround=2;
                },
                AbleCheck = (int L)=>
                {
                    if(L==1)return false;//到达最大等级
                    if(MyStyle==(int)AbilityStyle.Thorns)return true;
                    else return false;//别的流派
                }
            },
            new Ability {
                abilityName = "互相伤害",
                description = "子弹命中敌人时会创建你与敌人之间的链接（每人最多一条），每帧使敌我同时受到一定伤害(不计算额外反伤)",
                unlockAction = (int L) => {
                    if(L==1)
                    {
                        AbilityThornsHurtEachOther=1;
                        AbilityThornsHurtEachOtherRange=3f;
                        AbilityThornsHurtEachOtherDamage=0.02f;//一秒六十帧，则一秒1.2伤害
                    }
                    else if(L==2)
                    {
                        AbilityThornsHurtEachOtherRange=4f;
                        AbilityThornsHurtEachOtherDamage=0.03f;
                    }
                    else if(L==3)
                    {
                        AbilityThornsHurtEachOtherRange=5f;
                        AbilityThornsHurtEachOtherDamage=0.04f;
                    }
                    else if(L==4)
                    {
                        AbilityThornsHurtEachOtherRange=6f;
                        AbilityThornsHurtEachOtherDamage=0.05f;
                    }

                },
                AbleCheck = (int L)=>
                {
                    if(L==4)return false;//到达最大等级
                    if(MyStyle==(int)AbilityStyle.Thorns)return true;
                    else return false;//别的流派
                }
            }
            // 其他能力...
        };
    }
}

