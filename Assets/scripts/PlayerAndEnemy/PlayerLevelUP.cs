using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using static AllControl;


public class PlayerLevelUP : MonoBehaviour
{
    [SerializeField] public Canvas Canvas;
    private Attributes Attributes;
    public Button[] buttons;
    public Button AbilityButton;
    private List<Button> AbilityButtonList = new List<Button>();
    public int WaitQueue = 0;//掌管数值
    public int WaitQueue2 = 0;//掌管能力
    private int IsAbility = 0;//表示当前已经有abilitybutton
    //private int IsAttributeEnhance = 0;//表示当前已经有属性升级按钮显示
    public TextMeshProUGUI MyLV;//显示等级的文字

    // Start is called before the first frame update
    void Start()
    {
        Attributes = GetComponent<Attributes>();
        UpdateLV();
    }
    public void UpdateLV()
    {
        //if (MyLV == null) Debug.Log("我的对象去哪了！？？？？");
        MyLV.text = "LV:" + Attributes.level;//更新显示等级ui
    }
    // Update is called once per frame
    void Update()
    {
        if (Attributes.xp >= Attributes.NextLevelXP - 0.001f)
        {
            Attributes.xp -= Attributes.NextLevelXP;
            Attributes.TotalXP += Attributes.NextLevelXP;
            Attributes.NextLevelXP *= AllControl.GameManager.Instance.GrowthRate;
            Attributes.level++;
            UpdateLV();//更新显示lv
            //Debug.Log("我升级了");
            WaitQueue++;//可以多次积累，防止因为长时间不选而跳过
            if (Attributes.level % AllControl.GameManager.Instance.LevelsPerAbility == 0) WaitQueue2++;//每隔几级有一个能力
        }
        if (Attributes.IsPlayer == 1 && WaitQueue > 0) AttributeEnhancement();
        if (Attributes.IsPlayer == 0 && WaitQueue > 0) RobotAttributeEnhancement();//让敌人也能成长
        if (Attributes.IsPlayer == 1 && WaitQueue2 > 0 && IsAbility == 0) GetAbility();
    }
    void RobotAttributeEnhancement()
    {
        int p;//也许这里是屎山的根源之一，但是我现在懒得重构了，所以就用ifelse了。。。
        while (true)
        {
            p = Random.Range(0, 4);
            if (p == 0 && Attributes.MyStyle == (int)Attributes.AbilityStyle.Summon &&Attributes.Defense>=Attributes.AbilitySummonMaxDefense) continue;
            if (p == 2 && Attributes.MyStyle == (int)Attributes.AbilityStyle.Assassin && Attributes.MyStyle == (int)Attributes.AbilityStyle.Summon) continue;
            if (p == 3 && Attributes.MyStyle == (int)Attributes.AbilityStyle.Thorns && Attributes.MyStyle == (int)Attributes.AbilityStyle.Sniper) continue;
            break;
        }
        if (p == 0 ) DefenseUp();
        else if (p == 1) AttackPowerUp();
        else if (p == 2) AttackRangeUp();
        else if (p == 3) MoveSpeedUp();

        Ability Ability = GetRandomAbilities(1)[0];//让机器人也能获得能力
        if (Ability != null)
        {
            Debug.LogFormat("机器人获得能力，当前能力{0}", Ability.abilityName);
            Ability.AbilityLevel++;//能力等级+1
            Ability.unlockAction(Ability.AbilityLevel);
        }
        WaitQueue--;
    }
    void AttributeEnhancement()
    {
        //IsAttributeEnhance = 1;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
        }
        //Debug.Log("我到底有没有被运行啊！");
    }
    public void DefenseUp()
    {
        if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Summon && Attributes.Defense >= Attributes.AbilitySummonMaxDefense && Attributes.IsPlayer == 1)
        {
            Attributes.gameObject.GetComponent<PlayerUI>().UpdateTips("防御力已经到达最大上限");
            Debug.Log("已经到达最大上限");//升级前检测是否还能升级 召唤流限制防御力
            return;
        }
        Attributes.DefenseLV += 1;
        Attributes.Defense = 1.2f * Mathf.Pow(Attributes.DefenseLV, 0.5f);
        if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Summon && Attributes.Defense >= Attributes.AbilitySummonMaxDefense)
        {
            Attributes.Defense = Attributes.AbilitySummonMaxDefense;
        }
        HideButton();
    }
    public void AttackPowerUp()
    {
        Attributes.AttackPowerLV += 1;
        Attributes.AttackPower = 1f + 1f * Mathf.Pow(Attributes.AttackPowerLV, 0.6f);
        Attributes.AttackPower *= Attributes.AbilitySniperAttackEnhance;//狙击流增伤
        HideButton();
    }
    public void AttackRangeUp()
    {
        if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Assassin || Attributes.MyStyle == (int)Attributes.AbilityStyle.Summon && Attributes.IsPlayer == 1)
        {
            Attributes.gameObject.GetComponent<PlayerUI>().UpdateTips("攻击范围对近战无效");
            Debug.Log("已经到达最大上限");//升级前检测是否还能升级 刺客流限制范围 召唤流同样无效
            return;
        }
        if (Attributes.MyStyle == (int)Attributes.AbilityStyle.FirePower && Attributes.AttackRange >= Attributes.AbilityFirePowerMaxAttackRange && Attributes.IsPlayer == 1)
        {
            Attributes.gameObject.GetComponent<PlayerUI>().UpdateTips("攻击范围已经到达最大上限");
            Debug.Log("已经到达最大上限");//升级前检测是否还能升级 火力流限制范围
            return;
        }
        Attributes.AttackRangeLV += 1;
        Attributes.AttackRange = 5f + 0.3f * Mathf.Pow(Attributes.AttackRangeLV, 0.3f);
        Attributes.AttackRange *= Attributes.AbilitySniperRangeEnhance;//狙击流增加范围
        if (Attributes.MyStyle == (int)Attributes.AbilityStyle.FirePower && Attributes.AttackRange >= Attributes.AbilityFirePowerMaxAttackRange)
        {
            Attributes.AttackRange = Attributes.AbilityFirePowerMaxAttackRange;
        }
        if (Attributes.AttackRange > Attributes.VisionRange)
        {
            Attributes.VisionRange = Attributes.AttackRange;//如果攻击范围更大，则视野范围也更大
            Attributes.RobotVision.ChangeVision();//更新视野大小
        }
        HideButton();
    }
    public void MoveSpeedUp()
    {
        if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Thorns && Attributes.MoveSpeed >= Attributes.AbilityThornsMaxMoveSpeed && Attributes.IsPlayer==1)
        {
            Attributes.gameObject.GetComponent<PlayerUI>().UpdateTips("移速已经到达最大上限");
            Debug.Log("已经到达最大上限");//升级前检测是否还能升级 反伤流限制移速
            return;
        }
        if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Sniper && Attributes.MoveSpeed >= Attributes.AbilitySniperMaxMoveSpeed && Attributes.IsPlayer == 1)
        {
            Attributes.gameObject.GetComponent<PlayerUI>().UpdateTips("移速已经到达最大上限");
            Debug.Log("已经到达最大上限");//升级前检测是否还能升级 狙击流限制移速
            return;
        }
        Attributes.MoveSpeedLV += 1;//每升一级可以获得的提升(对数函数)
        Attributes.MoveSpeed = 2f + 0.8f * Mathf.Log(Attributes.MoveSpeedLV + 1);
        Attributes.MoveSpeed *= Attributes.AbilityAssassinMoveSpeedEnhance;//刺客的移速增幅
        if (Attributes.MyStyle==(int)Attributes.AbilityStyle.Thorns && Attributes.MoveSpeed > Attributes.AbilityThornsMaxMoveSpeed)
        {
            Attributes.MoveSpeed = Attributes.AbilityThornsMaxMoveSpeed;//反伤流的限制
        }
        if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Sniper && Attributes.MoveSpeed > Attributes.AbilitySniperMaxMoveSpeed)
        {
            Attributes.MoveSpeed = Attributes.AbilitySniperMaxMoveSpeed;//狙击流的限制
        }
        HideButton();
    }
    public void HideButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }




        WaitQueue--;
        //IsAttributeEnhance=0;
    }
    List<Ability> GetRandomAbilities(int count)
    {
        //if (Attributes.allAbilities == null)
        //{
        //    Debug.Log("在获取能力时发现能力列表为空");
        //    Debug.LogFormat("出现问题的对象名字是{0}", Attributes.gameObject.name);
        //}
        // 获取所有未解锁的能力
        List<Ability> available = Attributes.allAbilities.FindAll(a => a.AbleCheck(a.AbilityLevel));

        // 随机选择
        List<Ability> selected = new List<Ability>();
        while (selected.Count < count && available.Count > 0)//防止越界
        {
            int randomIndex = Random.Range(0, available.Count);//每次选择一个
            selected.Add(available[randomIndex]);//放进结果栏
            available.RemoveAt(randomIndex);//从可选栏中剔除，不放回抽取
        }

        return selected;
    }
    void GetAbility()
    {
        List<Ability> selected = GetRandomAbilities(Attributes.AbilityPerLevel);
        for (int i = 0; i < selected.Count; i++)
        {
            Button NewButton;
            
            if(AbilityButtonList.Count<=i)NewButton= Instantiate(AbilityButton,Vector3.zero, Quaternion.identity);//如果能力按钮对象不够，就再建一个
            else NewButton = AbilityButtonList[i];//如果有了就不用反复创建了避免无效操作
            NewButton.gameObject.SetActive(true);//使其显示
            NewButton.gameObject.transform.SetParent(Canvas.transform,false);//放到canvas里
            NewButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-AllControl.GameManager.Instance.AbilityButtonR+AllControl.GameManager.Instance.AbilityButtonBias + (i - 1) * (2 * AllControl.GameManager.Instance.AbilityButtonR / (Attributes.AbilityPerLevel)), -100, 0);//位置居中等距显示
            
            AbilityButton AbilityButtonScript = NewButton.GetComponent<AbilityButton>();
            AbilityButtonScript.Initialize(selected[i],HideAbilityButton);//传入当前能力以及隐藏按钮的选项
            if(AbilityButtonList.Count <= i) AbilityButtonList.Add(NewButton);
        }
        IsAbility = 1;
    }
    public void HideAbilityButton()
    {
        for (int i = 0; i < AbilityButtonList.Count; i++)
        {
            AbilityButtonList[i].gameObject.SetActive(false);
        }
        IsAbility = 0;
        WaitQueue2--;

    }
}
