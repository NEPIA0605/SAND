using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarValueInspect : MonoBehaviour
{
    [Header("参照するスライダー")]
    public Slider InsSlider;  //値を参照するスライダー
    private Text SliValue;    //値

    // Start is called before the first frame update
    void Start()
    {
        SliValue = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //桁数そろえる(正攻法だとゼロ埋めされる)
        if (InsSlider.value > 9)
        {
            SliValue.text = InsSlider.value.ToString();
        }
        else if (InsSlider.value < 10)
        {
            SliValue.text = " " + InsSlider.value.ToString();
        }
    }
}
