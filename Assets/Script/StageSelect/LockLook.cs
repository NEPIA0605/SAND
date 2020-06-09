using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class LockLook : MonoBehaviour
{
    //定数定義
    private const int LOCK_MAX = 4;

    [Header("錠前リスト")]
    public Image[] Locks;
    public int WorldNo;     //ワールドNo

    private bool FirstFlg;

    float Lock_time;
    float Lock_MAX = 0.65f;
    bool Lock_flag;

    //サウンド用
    [SerializeField] AudioClip[] clips;

    //SEです。
    protected AudioSource Source;


    // Start is called before the first frame update
    void Start()
    {
        FirstFlg = true;
        Source = GetComponent<AudioSource>();

        Lock_time = 0;
        Lock_flag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (FirstFlg)
        {
            //使うところ以外をfalseに
            for (int i = 0; i < LOCK_MAX; i++)
            {
                if (i != WorldNo)
                {
                    Locks[i].gameObject.SetActive(false);
                }
            }
            FirstFlg = false;
        }
    }

    //アニメーションさせる
    public void LockMove()
    {
        if (Lock_time == 0 && !Lock_flag)
        {
            Locks[WorldNo].GetComponent<Animator>().Play("ChainMove");

            Source.PlayOneShot(clips[0]);
            Lock_flag = true;
        }
        if (Lock_flag)
        {
            Lock_time += Time.deltaTime;
            if(Lock_time > Lock_MAX)
            {
                Lock_time = 0;
                Lock_flag = false;
            }
        }

    }

    //WorldNumのSetter
    public void SetWorldNum(int wn)
    {
        WorldNo = wn - 1;
    }
}
