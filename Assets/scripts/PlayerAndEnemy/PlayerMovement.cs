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
    Vector3 position;//鼠标位置
    private Movement Movement;


    //[SerializeField] private float MoveSpeed=7;
    void Start()
    {
        //Debug.Log("我被挂载了");
        rb= GetComponent<Rigidbody2D>();
        Attributes = rb.GetComponent<Attributes>();
        Movement = rb.GetComponent<Movement>();
        //引入类的属性
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 v=Vector2.zero;
        UpdatePosition();//更新鼠标位置
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

        Movement.UpdatePos(position);//更新鼠标位置
        if (Input.GetMouseButton(0))//按下左键射击
        {
            Movement.PressStayMouse0();
        }
        if(Input.GetMouseButton(1))//按下右键召唤
        {
            Movement.PressStayMouse1();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))//帮吃
        { 
            Movement.PressDownLeftShift();
        }
        if(Input.GetKey(KeyCode.LeftShift))//闪现
        {
            Movement.PressStayLeftShift(v);
        }
    } 
    void UpdatePosition()
    {
        position = Input.mousePosition;//获取鼠标坐标
        position.z = -Camera.main.transform.position.z;//校准z坐标
        position = Camera.main.ScreenToWorldPoint(position);//转换为世界坐标（三维)
    }
}


