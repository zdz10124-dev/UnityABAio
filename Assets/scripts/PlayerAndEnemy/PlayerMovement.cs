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
    int CD=0;
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
        if (CD > 0) CD--;//攻击间隔的实现
        //急停
        if (Input.GetKeyUp(KeyCode.A))
        {
           // Debug.Log("松开a了");
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
           // Debug.Log("松开d了");
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            // Debug.Log("松开w了");
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            // Debug.Log("松开w了");
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        //移动
        if (Input.GetKey(KeyCode.A))
        {
          //  Debug.Log("按下a了");
            rb.velocity = new Vector2(-Attributes.MoveSpeed, rb.velocity.y);
        }
       
        if (Input.GetKey(KeyCode.D))
        {
           // Debug.Log("按下d了");
            rb.velocity = new Vector2(Attributes.MoveSpeed, rb.velocity.y);
        }
        if (Input.GetKey(KeyCode.W))
        {
            // Debug.Log("按下w了");
            rb.velocity = new Vector2(rb.velocity.x, Attributes.MoveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            // Debug.Log("按下s了");
            rb.velocity = new Vector2(rb.velocity.x, -Attributes.MoveSpeed);
        }


        if (Input.GetMouseButton(0))//按下左键射击
        {
            attack();
        }
    } 
    void attack()
    {
        if (CD > 0) return;
        Vector3 position= Input.mousePosition;//获取鼠标坐标
        position.z = -Camera.main.transform.position.z;//校准z坐标
        position = Camera.main.ScreenToWorldPoint(position);//转换为世界坐标（三维)
        Vector3 PlayerPosition = rb.transform.position;//转换为3维，防止三维二维运算出错
        if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Assassin)
        {
            AttackFunction.SwingKnife(Attributes, position);//如果是刺客，攻击改为挥刀
        }
        else
        {
            Vector2 direction = (position - PlayerPosition).normalized;//获取方向向量
            //Debug.LogFormat("当前角色坐标={0}，鼠标坐标={1}，方向={2}",rb.transform.position,position,direction);
            GetComponent<Shoot>().shoot(rb, direction, Attributes);
        }
        CD = Attributes.AttackTime;//重置攻击间隔


    }
}


