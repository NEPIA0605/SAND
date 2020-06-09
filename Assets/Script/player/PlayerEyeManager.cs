using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEyeManager : MonoBehaviour
{

    Animator animator;
    float BlinkTime;
    float BlinkCnt;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        BlinkTime = 5.0f;
        BlinkCnt = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        BlinkCnt += Time.deltaTime;
        if (BlinkCnt >= BlinkTime)
        {
            animator.SetBool("Blink", true);
        }

    }

    //まばたき終わり
    public void BlinkEnd()
    {
        Debug.Log("まばたき終わり");
        animator.SetBool("Blink", false);

        BlinkCnt = 0.0f;
    }
}
