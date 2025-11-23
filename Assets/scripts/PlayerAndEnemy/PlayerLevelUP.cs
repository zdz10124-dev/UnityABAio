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
    private List<Button> AbilityButtonList=new List<Button>();
    public int WaitQueue = 0;
    private int IsAbility = 0;//表示当前已经有abilitybutton
    //private int IsAttributeEnhance = 0;//表示当前已经有属性升级按钮显示
    public TextMeshProUGUI MyLV;//显示等级的文字
    // Start is called before the first frame update
    void Start()
    {
        Attributes= GetComponent<Attributes>();
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
        if(Attributes.xp>=Attributes.NextLevelXP-0.001f)
        {
            Attributes.xp-=Attributes.NextLevelXP;
            Attributes.NextLevelXP *= AllControl.GameManager.Instance.GrowthRate;
            Attributes.level++;
            UpdateLV();//更新显示lv
            //Debug.Log("我升级了");
            WaitQueue++;//可以多次积累，防止因为长时间不选而跳过
            
        }
        if (Attributes.IsPlayer != 0 && WaitQueue > 0) AttributeEnhancement();
        if (Attributes.IsPlayer == 0 && WaitQueue > 0) RobotAttributeEnhancement();//让敌人也能成长
        if (Attributes.IsPlayer!=0 && WaitQueue>0 && (Attributes.level-WaitQueue+1)%5==0 && IsAbility==0) GetAbility();
    }
    void RobotAttributeEnhancement()
    {
        int p = Random.Range(0, 4);//也许这里是屎山的根源之一，但是我现在懒得重构了，所以就用ifelse了。。。
        if(p==0)DefenseUp();
        else if(p==1)AttackPowerUp();
        else if(p==2)AttackRangeUp();
        else if(p==3)MoveSpeedUp();
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
        Attributes.DefenseLV += 1;
        Attributes.Defense = 1.2f * Mathf.Pow(Attributes.DefenseLV, 0.5f);
        HideButton();
    }
    public void AttackPowerUp()
    {
        Attributes.AttackPowerLV += 1;
        Attributes.AttackPower = 1f + 2.5f * Mathf.Pow(Attributes.AttackPowerLV, 0.6f);
        HideButton();
    }
    public void AttackRangeUp()
    {
        Attributes.AttackRangeLV += 1;
        Attributes.AttackRange = 5f + 0.3f * Mathf.Pow(Attributes.AttackRangeLV, 0.3f);
        HideButton();
    }
    public void MoveSpeedUp()
    {
        Attributes.MoveSpeedLV += 1;//每升一级可以获得的提升(对数函数)
        Attributes.MoveSpeed = 2f + 0.8f * Mathf.Log(Attributes.MoveSpeedLV + 1);
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
        WaitQueue--;
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

    }
}
