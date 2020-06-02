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


    // Start is called before the first frame update
    void Start()
    {
        //サウンド
        Source = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {    
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 1"))
        {
            Debug.Log("B!");
            ExplanationObj.SetActive(false);
        }
    }
}
