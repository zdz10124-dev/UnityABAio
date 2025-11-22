using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static AllControl;

public class FightMapGenerator : MonoBehaviour
{
    //预先处理的范围
    [SerializeField] public List<int> MaxSize;//取不到
    [SerializeField] public List<int> MinSize;//可以取到
    [SerializeField] public List<int> MaxEnemyCount;//取不到
    [SerializeField] public List<int> MinEnemyCount;//取不到
    //最终确定的结果
    private int length;
    private int width;
    private int RoomSize;

    private int EnemyCount;
    //生成的东西相关
    public int GenerateSpecialBuilding;//有千分之它的概率尝试生成特殊建筑
    public int GenerateCommonBuilding;//同上

    private int SpecialBuildingTotalWeight;
    private int CommonBuildingTotalWeight;
    private int EnemyGenerateTotalWeight;
    public List<GameObject> SpecialBuilding;//可能被生成的特殊建筑
    public List<GameObject> CommonBuilding;//可能被生成的一般建筑
    public List<GameObject> Enemies;//可能被生成的敌人
    //备注：后来发现，其实Ibuilding接口能应用于所有需要生成东西的东西，所以敌人啥的也用这个接口了，望周知
    public GameObject rockpool;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i<SpecialBuilding.Count;i++)
        {
            SpecialBuildingTotalWeight += SpecialBuilding[i].GetComponent<IBuildings>().GenerateWeight;//计算总权重，方便加权随机
        }
        for (int i = 0; i < CommonBuilding.Count; i++)
        {
            CommonBuildingTotalWeight += CommonBuilding[i].GetComponent<IBuildings>().GenerateWeight;//同理，只不过计算的是普通物体的总权重
        }
        for(int i=0;i< Enemies.Count;i++)
        {
            EnemyGenerateTotalWeight += Enemies[i].GetComponent<IBuildings>().GenerateWeight;//同理
        }

        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Generate()
    {
        Debug.Log("地图生成中");
        //先确定房间大小
        int r = Random.Range(0, MaxSize.Count);//随机选择一种房间大小：小中大
        RoomSize = Random.Range(MinSize[r],MaxSize[r]);//确定房间具体大小
        width = GameManager.Instance.GridUnitLength * Random.Range(GameManager.Instance.FightRoomMinWidth, (int)Mathf.Sqrt(RoomSize / (GameManager.Instance.GridUnitLength * GameManager.Instance.GridUnitLength)));//此处确定的是实际宽度，并非格子数
        length = ((RoomSize / width) / GameManager.Instance.GridUnitLength) * GameManager.Instance.GridUnitLength;//将length确定，并且强制可以被整分为各个格子
        RoomSize = width * length;//更新一下实际的面积
        //划分地图格子
        int Xunits = width / GameManager.Instance.GridUnitLength;
        int Yunits = length / GameManager.Instance.GridUnitLength;
        int[,] Map=new int[Xunits,Yunits];//横边长竖边短
        //按格子开始生成
        for(int i=0; i<Xunits; i++)
        {
            for(int j=0; j<Yunits; j++)
            {
                if(Random.Range(0,1000)<GenerateSpecialBuilding)for (int k = 0; k < SpecialBuilding.Count; k++)
                {
                    if (Random.Range(0, SpecialBuildingTotalWeight) < SpecialBuilding[k].GetComponent<IBuildings>().GenerateWeight)
                    {
                        Map[i,j] = k+1;//毕竟0代表没有特殊建筑所以这里加个1到时候再-1就行了，别忘了
                        break;//已经生成建筑，所以跳过
                    }
                }

            }
        }
        //调用具体的生成逻辑
        for (int i = 0; i < Xunits; i++)
        {
            for (int j = 0; j < Yunits; j++)
            {
                //生成特殊建筑
                if (Map[i, j] > 0) SpecialBuilding[Map[i, j] - 1].GetComponent<IBuildings>().Generate(i * GameManager.Instance.GridUnitLength + GameManager.Instance.GridUnitLength / 2, j * GameManager.Instance.GridUnitLength + GameManager.Instance.GridUnitLength / 2);//调用具体的生成逻辑                                                                                                                                                                                                                                                           
                //生成普通建筑
                else if(Random.Range(0,1000)<GenerateCommonBuilding)//有千分之~的概率尝试生成普通建筑
                {
                    for (int k = 0; k < CommonBuilding.Count; k++)
                    {
                        if (Random.Range(0, CommonBuildingTotalWeight) < CommonBuilding[k].GetComponent<IBuildings>().GenerateWeight)
                        {
                            CommonBuilding[k].GetComponent<IBuildings>().Generate(i * GameManager.Instance.GridUnitLength + GameManager.Instance.GridUnitLength / 2, j * GameManager.Instance.GridUnitLength + GameManager.Instance.GridUnitLength / 2);//调用具体的生成逻辑                                                                                                                                                                                                                                                           

                            break;//已经生成建筑，所以跳过
                        }
                    }
                }
                

            }
        }
        //确定敌人数量
        //r= Random.Range(0, MaxEnemyCount.Count);
        EnemyCount = Random.Range(MinEnemyCount[r], MaxEnemyCount[r]);
        //生成敌人
        for(int i=0;i<EnemyCount;i++)
        {
            for (int k = 0; k < Enemies.Count; k++)//遍历敌人
            {
                if (Random.Range(0, EnemyGenerateTotalWeight) < Enemies[k].GetComponent<IBuildings>().GenerateWeight)
                {
                    //敌人位置随机
                    Enemies[k].GetComponent<IBuildings>().Generate(Random.Range(0,Xunits)*GameManager.Instance.GridUnitLength + GameManager.Instance.GridUnitLength / 2, Random.Range(0,Yunits) * GameManager.Instance.GridUnitLength + GameManager.Instance.GridUnitLength / 2);//调用具体的生成逻辑                                                                                                                                                                                                                                                           

                    break;//已经生成敌人，所以跳过
                }
            }
        }
        //生成边界
        for (int i = 0; i <= width+1; i++)
        {
            rockpool.GetComponent<EverythingPool>().GetItem(new Vector3(i - 1, -1, 0));
            rockpool.GetComponent<EverythingPool>().GetItem(new Vector3(i - 1, length +1, 0));
        }
        for(int i = 0;i<=length+1;i++)
        {
            rockpool.GetComponent<EverythingPool>().GetItem(new Vector3(-1, i-1, 0));
            rockpool.GetComponent<EverythingPool>().GetItem(new Vector3(width+1, i-1, 0));
        }
    }
}
