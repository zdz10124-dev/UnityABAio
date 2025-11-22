using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildings
{
    int GenerateWeight{ get; set; }
    void Generate(int x, int y);//生成位置中心点
}