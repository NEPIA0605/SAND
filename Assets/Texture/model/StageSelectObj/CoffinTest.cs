using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffinTest : MonoBehaviour
{
    [Header("アニメーター")]
    [SerializeField] private Animator TestAnima;    //テストするアニメーター
    public bool of;
    public bool cf;

    // Start is called before the first frame update
    void Start()
    {
        TestAnima = this.GetComponent<Animator>();
        of = cf = false;
        of = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            of = true;
            cf = false;
            TestAnima.SetBool("Open", true);
            TestAnima.SetBool("Close", false);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            of = false;
            cf = true;
            TestAnima.SetBool("Open", false);
            TestAnima.SetBool("Close", true);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            TestAnima.Play("Open");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TestAnima.Play("Close");
        }
    }
}
