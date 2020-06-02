using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRotate : MonoBehaviour
{
    [Header("回転速度")]
    public float RotSpd;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0.0f, RotSpd, 0.0f);
    }
}
