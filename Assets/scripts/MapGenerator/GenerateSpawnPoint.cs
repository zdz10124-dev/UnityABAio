using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AllControl;

public class GenerateSpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    public int GenerateWeight = 100;
    public List<Vector3> SpawnPoints;
    void Start()
    {
        
    }
    public void NewASpawnPoint(Vector3 pos)
    {
        if (Random.Range(0, 1000) < GenerateWeight) { SpawnPoints.Add(pos); }//十分之一的灌木丛会设置为重生点
    }
    public void AvoidEmpty(int x, int y)
    {
        if (SpawnPoints.Count == 0) { SpawnPoints.Add(new Vector3(x, y, 0)); }
        Debug.LogFormat("一共有{0}个重生点", SpawnPoints.Count);
    }
    public Vector3 FindAPlaceToSpawn()
    {
        List<float> spawnRanges = GameManager.Instance.SpawnRanges;
        List<Vector3> spawnPoints = SpawnPoints;

        // 遍历每一个半径
        foreach (float radius in spawnRanges)
        {
            Debug.LogFormat("重生中，当前半径检测是{0}", radius);
            List<Vector3> validSpawnPoints = new List<Vector3>();

            // 检查每个重生点是否在当前半径下是“安全”的
            foreach (Vector3 spawnPoint in spawnPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(spawnPoint.x,spawnPoint.y), radius);
                bool playerFound = false;
                //Debug.Log("检测中");
                foreach (Collider2D col in colliders)
                {
                    //Debug.LogFormat("我检测到了物体{0}", col.gameObject.name);
                    if (col.CompareTag("Player"))
                    {
                        playerFound = true;
                        break;
                    }
                }

                if (!playerFound)
                {
                    validSpawnPoints.Add(spawnPoint);
                }
            }

            // 如果在这个半径下找到了至少一个安全点，随机选一个返回
            if (validSpawnPoints.Count > 0)
            {
                int randomIndex = Random.Range(0, validSpawnPoints.Count);
                return validSpawnPoints[randomIndex];
            }

            // 否则继续尝试下一个更大的半径
        }

        // 所有半径都没有找到安全点，从原始列表中随机返回一个作为最后兜底
        Debug.LogWarning("No safe spawn point found at any range! Falling back to random spawn point.");
        int fallbackIndex = Random.Range(0, spawnPoints.Count);
        return spawnPoints[fallbackIndex];
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
