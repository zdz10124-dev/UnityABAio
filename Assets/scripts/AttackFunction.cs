using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GoodTools;
using static AllControl;

public class AttackFunction : MonoBehaviour
{
    // Start is called before the first frame update
    public EverythingPool KnifePool;
    public GameObject BulletPool;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void SwingKnife(Attributes Attacker,Vector3 pos)
    {
        GameObject Knife=KnifePool.GetItem(new Vector3(Attacker.transform.position.x,Attacker.transform.position.y+GameManager.Instance.SwingKnifeDertaY,Attacker.transform.position.z));
        RotateAroundBToLine(Knife,Attacker.gameObject,pos);
        if(Attacker.EnhancedAttackCount>0) 
        {
            Knife.GetComponent<KnifeDoDamage>().Initialize(Attacker,Attacker.AbilityAssassinEnhancedAttackHigherDamage);//如果是强化攻击，伤害变高
            Knife.transform.localScale = Knife.transform.localScale * Attacker.AbilityAssassinBiggerKnife;//如果是强化攻击，刀光变大
            if (Attacker.IsPlayer == 1) Attacker.PlayerUI.EnhancedAttackPicture.gameObject.SetActive(false);//强普标识去掉
            Attacker.EnhancedAttackCount = 0;
        }       
        else Knife.GetComponent<KnifeDoDamage>().Initialize(Attacker);//不是强化攻击，正常创建
        Attacker.EnhancedAttackCD = Attacker.AbilityAssassinEnhancedAttackCD;//只要你攻击了，就重置强化普攻cd
        Knife.transform.localScale = Knife.transform.localScale * Attacker.AbilityAssassinBiggerKnife;//能力：大刀光

    }
    public List<GameObject> GetExplodeRange(Attributes Attributes,Vector3 pos,float range)//获得被爆炸波及的游戏对象
    {
        List<GameObject> players = new List<GameObject>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(pos,range);

        foreach (Collider2D col in hits)
        {
            if (col.CompareTag("Player"))
            {
                if (col.gameObject.GetComponent<Attributes>().Team != Attributes.Team) players.Add(col.gameObject);
            }
        }

        return players;
    }
    public void Explode(Attributes Attributes,float ExplodeDamage,float range,Vector3 pos)//攻击人Attributes，爆炸伤害，爆炸半径,爆炸中心点
    {
        List<GameObject> a = GetExplodeRange(Attributes,pos,range);
        for (int i = 0; i < a.Count; i++)
        {
            a[i].GetComponent<Attributes>().GetDamage(Attributes, ExplodeDamage);//造成爆炸伤害
        }
    }
    public void shoot(Rigidbody2D rb, Vector2 direction, Attributes Attributes)//rb是射出者
    {
        //Debug.Log("我可射了");
        //if (CD>0) return;
        if (Attributes.AbilityFirePowerBulletCount == 1)
        {
            MakeBullet(rb, direction, Attributes);
        }
        else//霰弹的实现
        {
            List<Vector3> dirs = GenerateSpreadDirections(direction, Attributes.AbilityFirePowerScatteringAngle, Attributes.AbilityFirePowerBulletCount);
            for (int i = 0; i < dirs.Count; i++)
            {
                MakeBullet(rb, (Vector3)dirs[i], Attributes);
            }
        }

    }
    private void MakeBullet(Rigidbody2D rb, Vector2 direction, Attributes Attributes)
    {
        GameObject newBullet = BulletPool.GetComponent<BulletPool>().GetBullet(rb.transform.position);//创建子弹
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
    void Update()
    {
        
    }
}
