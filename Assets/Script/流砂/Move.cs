using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [Header("移動・回転のベース")]
    public float PlayerMoveSpeed;
    public float PlayerAngleSpeed;

    //移動力・回転角度
    private Vector3 PlayerVec3Move;
    private Vector3 PlayerVec3Angle;

    //流砂の速度を保存
    private Vector3 SandVec;

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        PlayerVec3Move = PlayerVec3Angle = SandVec = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //前後左右の移動
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                PlayerVec3Move.z = PlayerMoveSpeed;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                PlayerVec3Move.z = -PlayerMoveSpeed;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                PlayerVec3Move.x = -PlayerMoveSpeed;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                PlayerVec3Move.x = PlayerMoveSpeed;
            }
        }

        {
            //RegitBody無しの場合は上下移動も使える
            //else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            //{
            //    if (Input.GetKey(KeyCode.UpArrow))
            //    {
            //        PlayerVec3Move.y = PlayerMoveSpeed;
            //    }
            //    else if (Input.GetKey(KeyCode.DownArrow))
            //    {
            //        PlayerVec3Move.y = -PlayerMoveSpeed;
            //    }
            //}

            //左右回転　今は使わない
            //if (Input.GetKey(KeyCode.A))
            //{
            //    PlayerVec3Angle.y = -PlayerAngleSpeed;
            //}
            //else if (Input.GetKey(KeyCode.D))
            //{
            //    PlayerVec3Angle.y = PlayerAngleSpeed;
            //}

            //if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            //{
            //    //上下回転(空中専用想定)
            //    if (Input.GetKey(KeyCode.W))
            //    {
            //        PlayerVec3Angle.x = -PlayerAngleSpeed;
            //    }
            //    else if (Input.GetKey(KeyCode.S))
            //    {
            //        PlayerVec3Angle.x = PlayerAngleSpeed;
            //    }
            //}
        }

        //移動止める
        if (Input.GetKeyUp(KeyCode.UpArrow) ||
           Input.GetKeyUp(KeyCode.DownArrow) ||
           Input.GetKeyUp(KeyCode.LeftArrow) ||
           Input.GetKeyUp(KeyCode.RightArrow))
        {
            PlayerVec3Move = new Vector3(0.0f, 0.0f, 0.0f);
        }

        //回転止める
        if (Input.GetKeyUp(KeyCode.W) ||
            Input.GetKeyUp(KeyCode.A) ||
            Input.GetKeyUp(KeyCode.S) ||
            Input.GetKeyUp(KeyCode.D))
        {
            PlayerVec3Angle = new Vector3(0.0f, 0.0f, 0.0f);
        }

        //回転リセット
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Transform>().eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }

        //移動・回転反映
        this.transform.Translate(PlayerVec3Move + SandVec);
        this.transform.Rotate(PlayerVec3Angle);
    }

    //流砂の上にいるときに流砂の移動力を受け取る
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "QuickSand")
        {
            this.GetComponent<Rigidbody>().useGravity = false;
            SandVec = collision.gameObject.GetComponent<Quicksand>().GetSandMove();

            //上に乗っている場合はyを無視する
            if(collision.gameObject.transform.position.y * 2.0f <= this.transform.position.y)
            {
                SandVec.y = 0.0f;
            }
            else if(collision.gameObject.transform.position.y * 2.0f >= this.transform.position.y)
            {
                SandVec.x = 0.0f;
                SandVec.z = 0.0f;
            }
        }
    }

    //流砂から離れるときに流砂の影響を消す
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "QuickSand")
        {
            this.GetComponent<Rigidbody>().useGravity = true;
            SandVec = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
}
