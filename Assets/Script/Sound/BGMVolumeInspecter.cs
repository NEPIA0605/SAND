using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMVolumeInspecter : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource BGMAudioSource;   //音量調整対象のAudioSource

    // Start is called before the first frame update
    void Start()
    {
        //読み込み
        BGMAudioSource = this.GetComponent<AudioSource>();
        BGMVolChange();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //オプション用　スライダーを動かしたときに呼んで更新
    public void BGMVolChange()
    {
        float vol;
        vol = (float)PlayerPrefs.GetInt("VOLUME_BGM", 5);
        BGMAudioSource.volume = vol * 0.1f;                //0.0~1.0なので、0~10に0.1を掛ける
    }
}
