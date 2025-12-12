using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using static AllControl;

public class EnemyAI : MonoBehaviour
{
    int CD;//因为敌人和玩家攻击的形式不一样（一个用鼠标触发，一个指令）所以CD也只能在这里多做一份
    public List<Collider2D> CollList;
    public Collider2D coll;
    private Rigidbody2D rb;
    private Attributes Attributes;
    private Collider2D Fruit;
    [SerializeField] private Collider2D Player;//视野中其他的player
    private Collider2D Covers;//灌木
    private Collider2D Bullets;//子弹
    private GameObject Parent;
    //脚本引用
    public AttackFunction AttackFunction;
    public Movement Movement;

    private int WalkTime = 0;
    private int RotateTime = 0;
    private int RotateDir = 0;
    [SerializeField] private int state = 0;
    [SerializeField] private Vector3 UnrealPos;

    private float KeepDistance=2f;//敌人追击保持距离等于攻击距离-这个
    private float GoodDistance = 1f;//保持游走的合适区间
    private float DangerDistance = 3f;//躲起来的敌人如果被接近到这个距离就会跑
    private float AvoidBulletPercent = 0.7f;//在这个比例以下都会尝试躲子弹
    private int HideToWalkPercent = 3;//躲起来转随机漫步的概率(千分之)
    private float DisCoverDistance = 5f;//当看到子弹能意识到草丛藏人的范围
    private float BeDetectedDistance = 1.5f;//当子弹接近自己到什么地步认为自己被发现

    private bool Cged=true;
    
    // Start is called before the first frame update
    void Start()
    {
        Parent = transform.parent.gameObject;
        rb=Parent.GetComponent<Rigidbody2D>();
        Attributes = GetComponent<Attributes>();
        
    }
    Vector3 RangedPos(Vector3 pos,float d=0.2f)
    {
        Vector3 Pos = new Vector3(pos.x + Random.Range(-d, d), pos.y + Random.Range(-d, d), pos.z);
        return Pos;
    }
    void MoveTo(Vector3 pos,float x=1f)
    {
        Vector2 direction = (pos - rb.transform.position).normalized;
        rb.velocity = x*direction * Attributes.MoveSpeed;//如果是-1就是反向移动

    }
    public void Reset()
    {
        Fruit=null;
        Player=null;//视野中其他的player
        Covers=null;//灌木
        Bullets=null;//子弹
        state = 0;

    }
    // Update is called once per frame
    void Update()
    {
        //if (transform.position != transform.parent.transform.position) Debug.LogErrorFormat("警告，敌人偏移,player坐标{0} 敌人坐标{1}", transform.position, transform.parent.transform.position);
        transform.position = rb.transform.position;
        if(RotateTime>0)RotateTime--;
        UnrealPos = new Vector3(rb.transform.position.x, rb.transform.position.y + 2, rb.transform.position.z);
        Movement.UpdatePos(UnrealPos);//默认情况下鼠标直接放在自己身上上方一点点
        for (int i=CollList.Count-1;i>=0;i--)
        {
            if (Vector2.Distance(CollList[i].transform.position, rb.transform.position)>Attributes.VisionRange || !CollList[i].gameObject.activeSelf)
            {
                CollList.RemoveAt(i);//在视野外的物体去除
                continue;
            }
            //else if (CollList[i].CompareTag("rock") && Vector2.Distance(rb.transform.position, CollList[i].transform.position)<1.5f)
            //{
            //    Attributes.transform.position = Attributes.transform.position + 1.5f*(Attributes.transform.position - CollList[i].transform.position).normalized;//遇到岩石弹开防止卡墙
            //}
            coll = CollList[i];
            if (coll == null)
            {
                CollList.RemoveAt(i);
                continue;
            }
            if (!coll.gameObject.activeSelf)
            {
                CollList.RemoveAt(i);
                continue;
            }
        }

        //Debug.LogFormat("当前状态是{0}", state);
        Cged = true;
        while (Cged)
        {
            Cged= false;
            switch (state)
            {
                case 0://随机漫步
                    {
                        RandomWalk();
                        break;
                    }
                case 1:
                    {
                        GetFood();
                        break;
                    }
                case 2:
                    {
                        ChasePlayer(); break;
                    }
                case 3:
                    {
                        FleeFromPlayer(); break;
                    }
                case 4:
                    {
                        GoHide(); break;
                    }
                case 5:
                    {
                        CheckBullet(); break;
                    }

            }
        }
        if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Summon) Movement.PressStayMouse1();//如果是召唤那就一直尝试召唤   
    }
    void RandomWalk()
    {
        for (int i=CollList.Count-1;i>=0;i--)
        {
            coll = CollList[i];
            if (coll != null)
            {
                //Debug.LogFormat("碰撞箱类型:{0}", coll.tag);
                if (state==0 && coll.CompareTag("cover"))
                {
                    if (Covers != null)
                    {
                        if (Vector2.Distance(Covers.transform.position, rb.transform.position) > Vector2.Distance(coll.transform.position, rb.transform.position))
                        {
                            Covers = coll;
                            //找更近的掩体
                            //Debug.Log("分支1");
                        }
                        //else Debug.Log("分支2");
                    }
                    else Covers = coll;
                    state = 4;
                    //if (coll == null) { Debug.Log("难道我coll也没了？"); }
                    //if (Covers == null) { Debug.Log("当前就没有covers了"); }
                }
                else if ((state==0 || state==4) &&coll.CompareTag("fruit"))//从漫步到干饭
                {
                    Fruit = coll;
                    state = 1;
                } 
                else if((state == 0 || state == 4 || state==1) && coll.CompareTag("bullet") && Bullets==null)//一次只侦查一颗子弹
                {
                    Bullets = coll;
                    state = 5;
                }
                else if (coll.CompareTag("Player") && !coll.gameObject.GetComponent<Attributes>().Hide)//从漫步到追人 前提是人没有躲草
                {
                    if (Player != null && Vector2.Distance(rb.transform.position, Player.transform.position) < Vector2.Distance(rb.transform.position, coll.transform.position)) continue;//优先选取近的敌人
                    Player = coll;
                    state = 2;
                }

            }
        }

        if (state != 0)
        {
            rb.velocity = new Vector2(0, 0);
            WalkTime = 0;
            return;
        }
        //Debug.LogFormat("walktime={0}", WalkTime);
        if(WalkTime==0)//时间间隔归零，改变随机漫步方向
        {
            WalkTime=Random.Range(GameManager.Instance.RandomMinWalkTime,GameManager.Instance.RandomMaxWalkTime);
            int randomValue1 = Random.Range(-1, 2);
            int randomValue2 = Random.Range(-1, 2);
            rb.velocity = new Vector2(randomValue1 * Attributes.MoveSpeed, randomValue2 * Attributes.MoveSpeed);
        }
        WalkTime--;



    }
    void GetFood()
    {
        if (Fruit!=null &&!Fruit.gameObject.activeSelf) Fruit = null;
        for (int i=CollList.Count-1;i>=0;i--)
        {
            coll = CollList[i];
            
            if (Fruit != null) if (Vector2.Distance(rb.transform.position, Fruit.transform.position) < GameManager.Instance.MinStep)
                {
                    state = 0;
                }
            if (coll != null)
            {
                if (state==1 && coll.CompareTag("fruit"))
                {
                    if (Fruit!=null && Vector2.Distance(Fruit.transform.position, transform.position) > Vector2.Distance(coll.transform.position, transform.position))
                    {
                        Fruit = coll;
                        //吃更近的水果
                    }
                }
                else if ((state == 1 ) && coll.CompareTag("bullet") && Bullets == null)//一次只侦查一颗子弹
                {
                    //Debug.Log("我发现子弹了");
                    Bullets = coll;
                    state = 5;
                }
                else if (coll.CompareTag("Player") && !coll.gameObject.GetComponent<Attributes>().Hide)//从干饭到追人
                {
                    if (Player != null && Vector2.Distance(rb.transform.position, Player.transform.position) < Vector2.Distance(rb.transform.position, coll.transform.position))continue;//优先选取近的敌人
                    Player = coll;
                    state = 2;
                }
                

            }
        }
        if (state==1 && (Fruit == null || !Fruit.gameObject.activeSelf)) state = 0;


        if (state != 1)
        {
            Fruit = null;
            Cged = true;
            return;
        }
        //Debug.Log("当前是吃水果状态");
        MoveTo(Fruit.transform.position);
    }
    void ChasePlayer()
    {
        if (Player!=null && !Player.gameObject.activeSelf) Player = null;
        for (int i=CollList.Count-1;i>=0;i--)
        {
            coll = CollList[i];
            
            if (coll.CompareTag("Player") && !coll.gameObject.GetComponent<Attributes>().Hide)//检查是否需要更换对象
            {
                if (Player != null && Vector2.Distance(rb.transform.position, Player.transform.position) < Vector2.Distance(rb.transform.position, coll.transform.position)) continue;//优先选取近的敌人
                Player = coll;
            }
        }
        if (Player != null)
        {
            //退出：脱离视野 或 血量太低
            if (Vector2.Distance(Player.transform.position, rb.transform.position) >= Attributes.VisionRange + Attributes.ExtraChaseRange || !Player.gameObject.activeSelf)//脱离范围则回到漫步状态
            {
                Player = null;
                state = 0;
            }
            if(Attributes.hp/Attributes.MaxHP <= Attributes.FleeHPPersent)
            {
                state = 3;
            }
        }
        if(Player==null)
        {
            state = 0;
        }
        if (state != 2)
        {
            if (state != 3) Player = null;
            Cged = true;
            return;
        }
        if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Assassin || Attributes.MyStyle == (int)Attributes.AbilityStyle.Summon) Movement.UpdatePos(Player.transform.position);//鼠标指向玩家
        else Movement.UpdatePos(RangedPos(Player.transform.position, 0.7f));
        UnrealPos = Player.transform.position;
        Movement.PressStayMouse0();            
        float dis = Vector2.Distance(rb.transform.position, Player.transform.position);
        if((Attributes.MyStyle==(int)Attributes.AbilityStyle.Sniper) && Attributes.Hide)
        {
            if (dis > Attributes.AttackRange) MoveTo(Player.transform.position);
            //如果太远，需要接近，如果小近但自己在躲，就不用管
            else if(dis<DangerDistance) MoveTo(Player.transform.position,-1);//如果太近了就要跑了
        }
        else if ((Attributes.MyStyle == (int)Attributes.AbilityStyle.Summon) && Attributes.Hide)
        {
            if (dis > Attributes.VisionRange) MoveTo(Player.transform.position);
            //如果太远，需要接近，如果小近但自己在躲，就不用管
            else if (dis < DangerDistance) MoveTo(Player.transform.position, -1);//如果太近了就要跑了
        }
        else if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Summon)
        {
            if(dis > Attributes.VisionRange) MoveTo(Player.transform.position);
            else MoveTo(Player.transform.position,-1);
        }
        else if(Attributes.MyStyle==(int)Attributes.AbilityStyle.Assassin)
        {
            if (dis > 1.5) MoveTo(Player.transform.position);
            else RotatePlayer();
        }
        else
        {

            if (dis > Attributes.AttackRange - KeepDistance) MoveTo(Player.transform.position);
            else if (dis > Attributes.AttackRange - KeepDistance - GoodDistance)
            {
                RotatePlayer();
            }
            else MoveTo(Player.transform.position, -1);//太近就反向逃跑
            if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Assassin && dis > Attributes.AbilityAssassinFlashRange) Movement.PressStayMouse1();//刺客要接闪现
        }

    }
    void RotatePlayer()
    {
        Vector3 dir = (Player.transform.position - rb.transform.position).normalized;
        if(RotateTime==0)//换向
        {
            //Debug.Log("尝试换向");
            RotateDir = Random.Range(-1, 2);
            while (RotateDir==0) RotateDir = Random.Range(-1, 2);
            RotateTime = Random.Range(30, 80);//左右摇摆
        }
        
        rb.velocity = new Vector3(RotateDir * dir.y, -RotateDir * dir.x, 0);//绕着垂直的方向走
    }
    void FleeFromPlayer()
    {
        if (Player != null && !Player.gameObject.activeSelf) Player = null;
        for (int i=CollList.Count-1;i>=0;i--)
        {
            coll = CollList[i];
            if (coll.CompareTag("Player") && !coll.gameObject.GetComponent<Attributes>().Hide)//检查是否需要更换对象
            {
                if (Player != null && Vector2.Distance(rb.transform.position, Player.transform.position) < Vector2.Distance(rb.transform.position, coll.transform.position)) continue;//优先选取近的敌人
                Player = coll;
            }
        }
        if (Player!=null) 
        {
            if(Vector2.Distance(Player.transform.position, rb.transform.position) >= Attributes.ExtraFleeRange)
            {
                state=0;

            }
        }
        if (Player == null) state = 0;

        if (state != 3)
        {
            Cged = true;
            Player = null;
            return;
        }
        if (Attributes.MyStyle == (int)Attributes.AbilityStyle.Assassin || Attributes.MyStyle == (int)Attributes.AbilityStyle.Summon) Movement.UpdatePos(Player.transform.position);//鼠标指向玩家
        else Movement.UpdatePos(RangedPos(Player.transform.position,0.7f));
        UnrealPos = Player.transform.position;
        Movement.PressStayMouse0();
        MoveTo(Player.transform.position,-1);
    }
    void GoHide()
    {

        int p = (int)Random.Range(0, 1000);
        //Debug.LogFormat("随机数是{0}", p);
        if(p<HideToWalkPercent)state=0;
        for (int i=CollList.Count-1;i>=0;i--)
        {
            coll = CollList[i];
            
            if (coll != null)
            {
                if ((state == 4) && coll.CompareTag("bullet") && Bullets == null && Vector2.Distance(rb.transform.position,coll.transform.position)<BeDetectedDistance )
                {
                    //在隐藏状态下，只有离得足够近才会进行子弹侦查
                    Debug.Log("我发现子弹了");
                    Bullets = coll;
                    state = 5;
                }
                else if (coll.CompareTag("Player") && !coll.gameObject.GetComponent<Attributes>().Hide)//从漫步到追人 前提是人没有躲草
                {
                    if (Player != null && Vector2.Distance(rb.transform.position, Player.transform.position) < Vector2.Distance(rb.transform.position, coll.transform.position)) continue;//优先选取近的敌人
                    Player = coll;
                    state = 2;
                }
            }
        }
        //Debug.LogFormat("躲草函数中发现state={0}", state);
        //if (Covers == null) Debug.Log("在检查之前就没了");
        if(Covers==null &&state==4)state=0;
        //if (Covers == null) Debug.Log("在检查之后没了");
        //Debug.LogFormat("躲草函数中发现state={0}", state);
        if (state != 4)
        {
            Cged = true;
            return;
        }
        //Debug.Log("尝试躲草ing");
        if (Vector2.Distance(rb.transform.position, Covers.transform.position) > GameManager.Instance.MinStep) MoveTo(Covers.transform.position);//如果距离掩体远，继续向掩体走去
        MoveTo(Covers.transform.position);//如果距离掩体远，继续向掩体走去
    }
    void CheckBullet()//赋予反侦察能力
    {
        if (Bullets != null && !Bullets.gameObject.activeSelf) Bullets = null;
        for (int i=CollList.Count-1;i>=0;i--)
        {
            coll = CollList[i];
            
            if (coll != null)
            {
                if (coll.CompareTag("Player") && !coll.gameObject.GetComponent<Attributes>().Hide)//追人
                {
                    if (Player != null && Vector2.Distance(rb.transform.position, Player.transform.position) < Vector2.Distance(rb.transform.position, coll.transform.position)) continue;//优先选取近的敌人
                    Player = coll;
                    state = 2;
                }
                else if (coll.CompareTag("Player") && !coll.gameObject.GetComponent<Attributes>().Hide)//追人
                {
                    if (Player != null && Vector2.Distance(rb.transform.position, Player.transform.position) < Vector2.Distance(rb.transform.position, coll.transform.position)) continue;//优先选取近的敌人
                    Player = coll;
                    state = 2;
                }
            }
        }
        if(Bullets!=null)
        {
            Attributes MaybeEnemy = Bullets.GetComponent<Bullet>().Attributes;
            if (Vector2.Distance(rb.transform.position, MaybeEnemy.transform.position)<DisCoverDistance)//发现在草丛里的人
            {
                if (!(Player != null && Vector2.Distance(rb.transform.position, Player.transform.position) < Vector2.Distance(rb.transform.position, MaybeEnemy.transform.position)))
                {
                Player = MaybeEnemy.GetComponent<Collider2D>();
                state = 2;
                }

            }
        }



        if (state==5 &&(Bullets == null || !Bullets.gameObject.activeSelf || Vector2.Distance(rb.transform.position, Bullets.transform.position) > Attributes.VisionRange))//检查子弹时，子弹没了或者走远了
        {
            state=0;
            if ((Attributes.MyStyle == (int)Attributes.AbilityStyle.Summon || Attributes.MyStyle == (int)Attributes.AbilityStyle.Sniper)) state = 4;
        }

        if (state != 5)
        {
            Bullets=null;
            Cged = true;
            return;
        }
        //两种：要么侦查，要么远离
        if(Attributes.hp / Attributes.MaxHP > AvoidBulletPercent)rb.velocity=-Bullets.GetComponent<Rigidbody2D>().velocity.normalized*Attributes.MoveSpeed;//前往侦查
        else rb.velocity = (rb.transform.position-Bullets.transform.position).normalized * Attributes.MoveSpeed;//远离这个是非之地
    }

}
