using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public string abilityName;
    public string description;
    public bool isUnlocked=false;
    public System.Action unlockAction;
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

    //部分引用
    public GameObject GameOver;
    public Camera TempCamera;
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
        //一些ui需要更新
        gameObject.GetComponent<PlayerLevelUP>().UpdateLV();
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
    void InitializeAbilities()
    {
        allAbilities = new List<Ability>
        {
            new Ability {
                abilityName = "二段跳",
                description = "可以在空中再次跳跃",
                unlockAction = () => { 
                    // 具体的二段跳解锁逻辑
                    Debug.Log("二段跳已解锁！");
                    // 比如：GetComponent<PlayerMovement>().canDoubleJump = true;
                }
            },
            new Ability {
                abilityName = "冲刺",
                description = "短时间内大幅提升移动速度",
                unlockAction = () => { 
                    // 具体的冲刺解锁逻辑
                    Debug.Log("冲刺已解锁！");
                }
            },
            new Ability {
                abilityName = "冲刺",
                description = "短时间内大幅提升移动速度",
                unlockAction = () => { 
                    // 具体的冲刺解锁逻辑
                    Debug.Log("冲刺已解锁！");
                }
            }
            // 其他能力...
        };
    }
}

