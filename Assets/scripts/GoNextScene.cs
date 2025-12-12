using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AllControl;
public class GoNextScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NextScene()//加载下一个场景
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;//最后一个的下一个是第一个
        SceneManager.LoadScene(nextSceneIndex);
    }
    public void SetMode1()
    {
        GameManager.Instance.Mode = 1;
    }
    public void SetMode2()
    {
        GameManager.Instance.Mode = 2;
    }
}
