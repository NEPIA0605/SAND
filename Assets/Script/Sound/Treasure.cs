using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Treasure : MonoBehaviour
{
    //サウンド用
    [SerializeField] AudioClip[] clips;

    //SEです。
    protected AudioSource Source;


    // Start is called before the first frame update
    void Start()
    {
        Source = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySE_Treasure()
    {
            Source.PlayOneShot(clips[0]);
    }

}
