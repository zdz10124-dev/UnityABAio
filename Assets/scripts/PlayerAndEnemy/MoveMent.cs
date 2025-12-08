using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static AllControl;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Attributes Attributes;
    int AttackCD = 0;

    private Vector3 position;
    public AttackFunction AttackFunction;


    //[SerializeField] private float MoveSpeed=7;
    void Start()
    {
        //Debug.Log("我被挂载了");
        rb = GetComponent<Rigidbody2D>();
        Attributes = rb.GetComponent<Attributes>();
        //引入类的属性
    }

    // Update is called once per frame
    void Update()
    {
        if (AttackCD > 0) AttackCD--;//攻击间隔的实现
        if (Attributes.AbilitySummonCreatureList.Count > 0) Attributes.SummonedCreatrueMove(position);//召唤物向鼠标方向移动
    }
    public void UpdatePos(Vector3 pos)
    {
        position = pos;//鼠标的位置
    }
    public void PressStayMouse0()//按下左键的操作
    {
        attack();
    }
    public void PressDownLeftShift()
    {
        if (Attributes.AbilitySummonHelpEat)//召唤物帮吃逻辑
        {
            if (Attributes.SummonHelpEat)
            {
                Attributes.SummonHelpEat = false;
                Attributes.PlayerUI.UpdateTips("帮吃已关闭");
            }
            else
            {
                Attributes.SummonHelpEat = true;
                Attributes.PlayerUI.UpdateTips("帮吃已打开");
            }
        }
    }
    public void PressStayMouse1()
    {
        if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Summon && Attributes.SummonCD == 0)
        {
            Attributes.SummonCD = Attributes.AbilitySummonCD;//重设cd
            if (Attributes.AbilitySummonCreatureList.Count >= Attributes.AbilitySummonMaxCreatureCount)
            {
                Attributes.RemoveACreature(0);//如果超了，就删了再重新生成一个
            }
            Attributes.Summon(position);
        }
    }
    public void PressStayLeftShift(Vector2 v)
    {
        if (Attributes.FlashCD == 0 && Attributes.FlashCount > 0 && v != Vector2.zero)//闪现逻辑
        {
            rb.transform.position += new Vector3((v * Attributes.AbilityAssassinFlashRange).x, (v * Attributes.AbilityAssassinFlashRange).y, 0);//闪现
            Attributes.FlashCount -= 1;//使用一次闪现
            Attributes.FlashCD = Attributes.AbilityAssassinFlashCD;//重置CD
            Attributes.PlayerUI.FlashPicture.gameObject.SetActive(false);//去除图标
        }
    }
    void attack()
    {
        if (AttackCD > 0) return;
        Vector3 PlayerPosition = rb.transform.position;//转换为3维，防止三维二维运算出错
        if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Assassin)
        {
            AttackFunction.SwingKnife(Attributes, position);//如果是刺客，攻击改为挥刀
        }
        else if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Summon)
        {
            for (int i = 0; i < Attributes.AbilitySummonCreatureList.Count; i++) AttackFunction.SwingKnife(Attributes.AbilitySummonCreatureList[i], position);//召唤物每一个都挥刀
        }
        else
        {
            Vector2 direction = (position - PlayerPosition).normalized;//获取方向向量
            //Debug.LogFormat("当前角色坐标={0}，鼠标坐标={1}，方向={2}",rb.transform.position,position,direction);
            AttackFunction.shoot(rb, direction, Attributes);
        }
        AttackCD = Attributes.AttackTime;//重置攻击间隔


    }
}


