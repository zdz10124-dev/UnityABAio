using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AllControl;

public class GameControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = AllControl.GameManager.Instance.MaxFPS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
