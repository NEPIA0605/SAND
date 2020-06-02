using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEVolumeInspecter : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource SEAudioSource;   //音量調整対象のAudioSource

    // Start is called before the first frame update
    void Start()
    {
        //読み込み
        SEAudioSource = this.GetComponent<AudioSource>();
        SEVolChange();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //オプション用　スライダーを動かしたときに呼んで更新
    public void SEVolChange()
    {
        float vol;
        vol = (float)PlayerPrefs.GetInt("VOLUME_SE", 5);
        SEAudioSource.volume = vol * 0.1f;                //0.0~1.0なので、0~10に0.1を掛ける
    }
}
