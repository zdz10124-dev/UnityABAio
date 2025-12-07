using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static AllControl;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Attributes Attributes;
    private float MoveSpeed;
    int AttackCD=0;
    Vector3 position;//鼠标位置

    public AttackFunction AttackFunction;


    //[SerializeField] private float MoveSpeed=7;
    void Start()
    {
        //Debug.Log("我被挂载了");
        rb= GetComponent<Rigidbody2D>();
        Attributes = rb.GetComponent<Attributes>();
        rb.transform.position=GameManager.Instance.PlayerSpawnPosition;
        //引入类的属性
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 v=Vector2.zero;
        UpdatePosition();//更新鼠标位置
        if (AttackCD > 0) AttackCD--;//攻击间隔的实现

        //移动
        if (Input.GetKey(KeyCode.A))
        {
            //  Debug.Log("按下a了");
            v += new Vector2(-Attributes.MoveSpeed, 0);
        }
       
        if (Input.GetKey(KeyCode.D))
        {
            // Debug.Log("按下d了");
            v += new Vector2(Attributes.MoveSpeed, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            // Debug.Log("按下w了");
            v += new Vector2(0, Attributes.MoveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            // Debug.Log("按下s了");
            v += new Vector2(0, -Attributes.MoveSpeed);
        }
        v=v.normalized;
        rb.velocity = v*Attributes.MoveSpeed;//有效防止斜着走走得快

        if (Input.GetMouseButton(0))//按下左键射击
        {
            attack();
        }
        if(Input.GetMouseButton(1))//按下右键召唤
        {
            if(Attributes.MyStyle==(int)Attributes.AbilityStyle.Summon && Attributes.SummonCD==0)
            {
                Attributes.SummonCD = Attributes.AbilitySummonCD;//重设cd
                if(Attributes.AbilitySummonCreatureList.Count >=Attributes.AbilitySummonMaxCreatureCount)
                {
                    Attributes.RemoveACreature(0);//如果超了，就删了再重新生成一个
                }
                Attributes.Summon(position);
            }
        }

        if(Input.GetKey(KeyCode.LeftShift) && Attributes.FlashCD==0 && Attributes.FlashCount>0 && v!=Vector2.zero)
        {
            rb.transform.position += new Vector3((v * Attributes.AbilityAssassinFlashRange).x, (v * Attributes.AbilityAssassinFlashRange).y,0);//闪现
            Attributes.FlashCount -= 1;//使用一次闪现
            Attributes.FlashCD = Attributes.AbilityAssassinFlashCD;//重置CD
            Attributes.PlayerUI.FlashPicture.gameObject.SetActive(false);//去除图标
        }




        if (Attributes.AbilitySummonCreatureList.Count > 0) Attributes.SummonedCreatrueMove(position);//召唤物向鼠标方向移动
         
    } 
    void UpdatePosition()
    {
        position = Input.mousePosition;//获取鼠标坐标
        position.z = -Camera.main.transform.position.z;//校准z坐标
        position = Camera.main.ScreenToWorldPoint(position);//转换为世界坐标（三维)
    }
    void attack()
    {
        if (AttackCD > 0) return;
        Vector3 PlayerPosition = rb.transform.position;//转换为3维，防止三维二维运算出错
        if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Assassin)
        {
            AttackFunction.SwingKnife(Attributes, position);//如果是刺客，攻击改为挥刀
        }
        else if(Attributes.MyStyle==(int)Attributes.AbilityStyle.Summon)
        {
            for(int i = 0;i<Attributes.AbilitySummonCreatureList.Count;i++) AttackFunction.SwingKnife(Attributes.AbilitySummonCreatureList[i], position);//召唤物每一个都挥刀
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


