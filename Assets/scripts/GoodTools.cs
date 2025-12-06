using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//这里的函数基本上都是千问写的
//这里的函数都是一些基础的函数，实现一些非常基础的功能
//这些功能这辈子也不需要我更改，所以我就可以放心让ai写了，毕竟我也不需要修改添加什么
public class GoodTools : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public static void RotateAroundBToLine(GameObject objectA,GameObject objectB,Vector3 targetPosition)
    {
        if (objectA == null || objectB == null)
        {
            Debug.LogError("Object A or B is null.");
            return;
        }

        Vector3 bPos = objectB.transform.position;
        Vector3 aPos = objectA.transform.position;

        // 1. 计算轨道半径
        float radius = Vector3.Distance(aPos, bPos);
        if (radius <= 0f)
        {
            Debug.LogWarning("A and B are at the same position.");
            return;
        }

        // 2. 获取从 B 指向目标的方向（归一化）
        Vector3 directionToTarget = (targetPosition - bPos).normalized;
        if (directionToTarget.sqrMagnitude == 0f) return;

        // 3. 新位置：A 在这条线上，距离为 radius
        Vector3 newPosition = bPos + directionToTarget * radius;
        objectA.transform.position = newPosition;

        // 4. 计算从 A 指向 B 的向量
        Vector3 toCenter = bPos - newPosition;  // A -> B

        // 5. 计算角度，使 A 的 Y 轴（up）指向 B
        float angle = Mathf.Atan2(toCenter.y, toCenter.x) * Mathf.Rad2Deg;

        // 关键：+90° 偏移，让局部 Y 轴对准中心（原本是 X 轴对准，+90 后变成 Y 轴对准）
        angle += 90f;

        objectA.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
