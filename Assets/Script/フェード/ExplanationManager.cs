using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class ExplanationManager : MonoBehaviour
{
    public GameObject ExplanationObj;

    [SerializeField] AudioClip[] clips;//サウンド

    //SEです。
    protected AudioSource Source;

    float Exit_Time;
    float Exit_MAX = 6f;
    bool Exit_check;

    // Start is called before the first frame update
    void Start()
    {
        //サウンド
        Source = GetComponent<AudioSource>();
        Exit_Time = 0;
        Exit_check = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 1"))
        {
            Debug.Log("B!");
            Exit_check = true;
        }
        if (Exit_check)
        {
            Exit_Time++;
            if (Exit_Time > Exit_MAX)
            {
                ExplanationObj.SetActive(false);
                Exit_check = false;
            }
        }
    }
}
