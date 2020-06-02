using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ray : MonoBehaviour
{
    //public Transform target;

    //RaycastHit hit;

    //////　レイを飛ばす位置
    ////[SerializeField]
    ////private Transform rayPosition;
    //////　レイの距離
    ////[SerializeField]
    ////private float rayRange = 0.85f;
    //////　レイが地面に到達しているかどうか
    ////private bool isGround = false;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //void FixedUpdate()
    //{
    //    // Face to target direction
    //    transform.rotation =
    //    Quaternion.LookRotation(target.position - transform.position);

    //    // Create 'Ray' to forward direction
    //    Ray ray = new Ray(transform.position, transform.forward);

    //    // If Ray hit someting
    //    if (Physics.Raycast(ray, out hit, 15))
    //    {

    //        // If the object is "Player"
    //        if (hit.collider.tag == "Block")
    //        {

    //            // Draw Red Line
    //            Debug.DrawLine(ray.origin, hit.point, Color.red);

    //        }
    //    }
    //}

    RaycastHit hit;

    void Update()

    {

        // レイ飛ばす

        if (Physics.BoxCast(transform.position, Vector3.one * 0.5f, transform.TransformDirection(Vector3.down), out hit))
        {

            Debug.Log(hit.collider.gameObject.name);

        }

    }





    // 表示（シーンビューのみ）
    void OnDrawGizmosSelected()

    {

        Gizmos.DrawWireCube(hit.point, Vector3.one * 0.5f);

    }
}
