using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePosition : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject StartPoint;
    private GameObject EndPoint;
    private LineRenderer lr;
    void Start()
    {
        lr=gameObject.GetComponent<LineRenderer>();
        lr.positionCount = 2;//两个点确定一条线
    }
    public void Initialize(GameObject a,GameObject b)//开始物体和结束物体
    {
        StartPoint = a;
        EndPoint = b;
    }
    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, StartPoint.transform.position);//动态更新
        lr.SetPosition(1, EndPoint.transform.position);
        lr.startWidth = 0.1f / Vector2.Distance(StartPoint.transform.position, EndPoint.transform.position);//距离越远越细
        lr.endWidth = lr.startWidth;
    }
}
