using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NowMove : MonoBehaviour
{
    [Header("拡大率")]
    [Range(0.5f, 2.0f)] public float rate = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!this.GetComponent<CursorMove>().GetMoveEnd())
        {
            this.GetComponent<Image>().transform.localScale = new Vector3(1.0f * rate, 1.0f * rate, 1.0f);
        }
        else
        {
            this.GetComponent<Image>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}
