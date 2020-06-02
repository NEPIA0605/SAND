using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeChange : MonoBehaviour
{
    [Header("対象のバー")]
    public Slider TargetBar;    //対象のバー

    [Header("ボリューム")]
    public float NowVolume;            //今のボリューム
    private float OldVolume;           //前のボリューム

    // Start is called before the first frame update
    void Start()
    {
        //スライダー取得
        TargetBar = this.GetComponent<Slider>();
        //セーブデータ取得
        if (this.tag == "BGMBar")
        {
            NowVolume = PlayerPrefs.GetInt("VOLUME_BGM", 5);
        }
        else if (this.tag == "SEBar")
        {
            NowVolume = PlayerPrefs.GetInt("VOLUME_SE", 5);
        }
    }

    // Update is called once per frame
    void Update()
    {
        TargetBar.value = NowVolume;
    }

    //ボリュームUp
    public void VolUp()
    {
        if (NowVolume < 10)
        {
            NowVolume += 1.0f;

            if (this.tag == "BGMBar")
            {
                PlayerPrefs.SetInt("VOLUME_BGM", (int)NowVolume);
            }
            else if (this.tag == "SEBar")
            {
                PlayerPrefs.SetInt("VOLUME_SE", (int)NowVolume);
            }

            PlayerPrefs.Save();
        }
    }

    //ボリュームDown
    public void VolDown()
    {
        if (NowVolume > 0)
        {
            NowVolume -= 1.0f;

            if (this.tag == "BGMBar")
            {
                PlayerPrefs.SetInt("VOLUME_BGM", (int)NowVolume);
            }
            else if (this.tag == "SEBar")
            {
                PlayerPrefs.SetInt("VOLUME_SE", (int)NowVolume);
            }

            PlayerPrefs.Save();
        }
    }
}
