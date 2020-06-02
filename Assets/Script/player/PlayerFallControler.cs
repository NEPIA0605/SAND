using System.Collections;
using UnityEngine;

public class PlayerFallControler : MonoBehaviour
{
    //　レイを飛ばす場所
    [SerializeField]
    private Transform rayPosition;
    //　レイを飛ばす距離
    [SerializeField]
    private float rayRange = 0.1f;
    //　落ちた場所
    private float fallenPosition;

    //　落ちた地点を設定したかどうか
    private bool isFall;
    //　落下してから地面に落ちるまでの距離
    private float fallenDistance;
    //　どのぐらいの高さからダメージを与えるか
    [SerializeField]
    private float takeDamageDistance = 2f;

    bool GameoverFlg;
    void Start()
    {
        fallenDistance = 0f;
        fallenPosition = transform.position.y;
        isFall = false;
        GameoverFlg = false;
    }

    void Update()
    {
        Debug.DrawLine(rayPosition.position, rayPosition.position + Vector3.down * rayRange, Color.blue);
        Debug.Log(GameoverFlg);

        //　落ちている状態
        if (isFall)
        {

            //　落下地点と現在地の距離を計算（ジャンプ等で上に飛んで落下した場合を考慮する為の処理）
            fallenPosition = Mathf.Max(fallenPosition, transform.position.y);
            Debug.Log(fallenPosition);

            //　地面にレイが届いていたら
            if (Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange, LayerMask.GetMask("Field", "Block")))
            {
                //　落下距離を計算
                fallenDistance = fallenPosition - transform.position.y;
                //　落下によるダメージが発生する距離を超える場合ダメージを与える
                if (fallenDistance >= takeDamageDistance)
                {
                    GameoverFlg = true;
                    //myHp.TakeDamage((int)(fallenDistance - takeDamageDistance));
                }
                isFall = false;
            }
        }
        else
        {
            //　地面にレイが届いていなければ落下地点を設定
            if (!Physics.Linecast(rayPosition.position, rayPosition.position + Vector3.down * rayRange, LayerMask.GetMask("Field", "Block")))
            {
                //　最初の落下地点を設定
                fallenPosition = transform.position.y;
                fallenDistance = 0;
                isFall = true;
            }
        }
    }
}
