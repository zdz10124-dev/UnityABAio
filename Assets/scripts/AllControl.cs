using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllControl : MonoBehaviour
{
    public class GameManager
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null) _instance = new GameManager();
                return _instance;
            }
        }
        public float MinStep = 0.001f;//判断浮点数相等的微小值
        //难度
        public int Mode=1;//1简单，2困难
        //相机
        public float CameraMinSize = 2f;
        public float CameraMaxSize = 7f;
        public float CameraZoomSpeed = 2f;
        //机器人
        public int RandomMinWalkTime = 120;//单位是update触发一次。
        public int RandomMaxWalkTime = 180;//这两个控制随机漫步改变方向的时间间隔
        public float ExtraEXP = 2f;//人机多来点经验不然没啥挑战性了
        //升级按钮
        public int AbilityButtonR = 300;//abilitybutton中心点所在范围
        public int AbilityButtonBias = 300;//设置一下偏移方便居中
        //刷新率
        public int MaxFPS = 60;//目标帧率
        //重生
        public List<float> SpawnRanges = new List<float>{20,15,10,6,4};//复活时周围没有人的半径
        //UI
        public float DamageDisplayYUP = 0.5f;//伤害数值显示在角色上方多少
        public int DamaeDisplayCD = 10;//伤害显示的cd(也就是多长时间内的伤害会被累计显示)单位是帧
        //水果
        public float XPperCherry = 1f;//一个樱桃多少经验
        public float CherrySpawnMaxTime = 30f;//生成樱桃需要的时间 最大值
        public float CherrySpawnMinTime = 7f;//最小值
        public float CherryHPUp = 1f;//一个樱桃回的血
        public float GrowthRate = 1.1f;//下一级比上一级所需经验的比例
        //升级
        public int LevelsPerAbility = 1;//升多少级能获得一个能力
        //击杀所得
        public float BasicLootXP = 7f;//杀死敌人的基础获得经验值
        public float LootXPRate = 0.1f;//杀死敌人时掠夺的经验值
        //场景池相关
        public int BulletPoolSize = 300;
        public int FruitPoolSize = 100;
        //突然意识到在inspoector里设置poolsize更好，能更好的对应每个不同的脚本
        //鉴于前面这两个已经写了就保留吧，其他的poolsize请在特定的脚本里设置

        //地图生成相关
        //具体的大小限制，敌人，物品等等在脚本处配置，这里只设置全局性的内容
        public int GridUnitLength = 2;//地图生成时一个图格的大小
        public int FightRoomMinWidth = 20;//一个战斗地图的宽最短是几个格子
        //具体的升级后属性提升的公式请去levelup的具体函数里改，里面用加dy的方式来求下一项是多少
        //挥刀
        public float SwingKnifeDertaY=1;//刀光出现在人物中心点上方多少
        //队伍
        public int TeamCount=0;//队伍计数器
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}