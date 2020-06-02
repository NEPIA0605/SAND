using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //目標FPS設定
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
