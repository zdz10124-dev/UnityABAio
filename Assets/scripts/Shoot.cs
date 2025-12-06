using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    //int CD = 0;
    public GameObject Pool;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (CD > 0) CD--;
    }
    public void shoot(Rigidbody2D rb,Vector2 direction,Attributes Attributes)//rb是射出者
    {
        //Debug.Log("我可射了");
        //if (CD>0) return;
        if (Attributes.AbilityFirePowerBulletCount == 1)
        {
            MakeBullet(rb, direction, Attributes);
        }
        else//霰弹的实现
        {
            List<Vector3> dirs=GenerateSpreadDirections(direction,Attributes.AbilityFirePowerScatteringAngle,Attributes.AbilityFirePowerBulletCount);
            for (int i = 0; i < dirs.Count; i++)
            {
                MakeBullet(rb,(Vector3)dirs[i],Attributes);
            }
        }

    }
    private void MakeBullet(Rigidbody2D rb, Vector2 direction, Attributes Attributes)
    {
        GameObject newBullet = Pool.GetComponent<BulletPool>().GetBullet(rb.transform.position);//创建子弹
        newBullet.SetActive(true);
        Bullet Bullet = newBullet.GetComponent<Bullet>();//获取子弹挂载的脚本
        Bullet.Initialize(direction, Attributes);//传递参数
    }
    private List<Vector3> GenerateSpreadDirections(Vector3 direction, float x, int n)//功能：以direction为中心，左右x度范围内均分n个方向向量并返回
    {
        //这是千问写的，感觉还不错，而且封装的不错，就直接留着了
        List<Vector3> directions = new List<Vector3>();

        // 如果 n <= 0，直接返回空列表
        if (n <= 0)
            return directions;

        // 将输入方向转换为二维向量并获取其角度（欧拉角，相对于正右方向）
        Vector2 dir2D = new Vector2(direction.x, direction.y);
        float centerAngle = Mathf.Atan2(dir2D.y, dir2D.x) * Mathf.Rad2Deg;

        // 扇形范围：从 leftAngle 到 rightAngle（共 2x 度）
        float leftAngle = centerAngle - x;
        float rightAngle = centerAngle + x;

        // 总区间长度
        float totalAngle = 2f * x;

        // 均匀采样 n 个点（不包含边界，所以使用开区间）
        for (int i = 1; i <= n; i++)
        {
            // 当前比例位置（避免取到 0 和 1，因为是开区间）
            float t = i / (n + 1.0f); // 这样确保不会碰到左右边界

            // 插值角度
            float angle = leftAngle + t * totalAngle;

            // 转换为单位向量
            float radian = angle * Mathf.Deg2Rad;
            Vector3 resultDir = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f);
            resultDir = resultDir.normalized; // 确保是单位向量

            directions.Add(resultDir);
        }

        return directions;
    }
}
