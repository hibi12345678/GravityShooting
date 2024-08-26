using UnityEngine;
using Photon.Pun;
public class Gravity : MonoBehaviourPunCallbacks
{
    
    public float gravity = 0.01f;
    private Rigidbody rb;
    Transform planet;
    GameObject sphere;
    bool isGrounded;
    bool flag = true;

    public float groundCheckDistance = 0.001f; // 地面との距離をチェックする長さ
    public LayerMask groundLayer;           // 地面のレイヤー
    public GameObject obj;

    
    private void Start()
    {
        isGrounded = false;
        sphere = GameObject.Find("Planet");
        planet = sphere.transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            
            // 惑星の中心に向かってキャラクターを向ける
            Vector3 direction = (transform.position - planet.position).normalized;
            if (isGrounded == false)
            {
                Vector3 gravityDirection = (transform.position - planet.position).normalized;

                rb.AddForce(-gravityDirection * gravity, ForceMode.Acceleration);
            }
            

            // ジャンプの入力をチェック
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {

                 rb.AddForce(direction * 250 * 1.0f, ForceMode.Acceleration);

                 

            }

           

            
        }

        

        // 地面に接触しているかどうかをRaycastで判定
        isGrounded = Physics.Raycast(obj.transform.position, planet.position - transform.position, groundCheckDistance, groundLayer);

        

        // ジャンプなどの処理を実行
        if (isGrounded)
        {
            rb.velocity = Vector3.zero;

        }



    }

}
