using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AIController : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    public GameObject parent;
    public Transform groundCheck; // キャラクターの足元の位置を指定するTransform
    public Transform amatureleg;
    Transform planet;
    private bool isGrounded;
    Vector3 localMove;
    private Rigidbody rb; // Rigidbodyコンポーネント

    // 子オブジェクトを設定
    public Transform childTransform;

    public Animator animator;
    public Animator animatorleg;
    public Animator animatormid;
    float currentLegYPosition;

    public AudioSource[] audioSources;
    public int num;
    GameObject sphere;
    public bool freezeX = true;
    public bool freezeY = false;
    public bool freezeZ = true;
    void Start()
    {

        // Rigidbodyコンポーネントを取得
        rb = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.Euler(0, 0, 0);
        audioSources = GetComponents<AudioSource>();

        sphere = GameObject.Find("Planet");
        planet = sphere.transform;
        Invoke("DelaySound", 0.1f);
       
        if (parent.name == "Player3-1(Clone)")
        {
            num = 2;

        }
        else if (parent.name == "Player3-2(Clone)")
        {
            num = 3;
        }
        else if (parent.name == "Player3-3(Clone)")
        {
            num = 4;
        }
        photonView.RPC(nameof(ChangeLayer), RpcTarget.All);
    }

    void DelaySound()
    {

        StartSound();
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        // オブジェクト自身のレイヤーを設定
        obj.layer = newLayer;

        // すべての子オブジェクトに対してレイヤーを設定
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    [PunRPC]
    public void ChangeLayer()
    {
        int newLayer;

        if (num == 1 || num == 3)
            gameObject.layer = 7;
        else if (num == 2 || num == 4)
            gameObject.layer = 8;
        newLayer = gameObject.layer;
        // 親オブジェクトとその子オブジェクトすべてにレイヤーを適用
        SetLayerRecursively(gameObject, newLayer);
    }
    public float detectionRadius = 12f; // 検出半径
    public float changeDirectionInterval = 1f; // 移動方向を変更する間隔（秒）
    private float timer;
    public float rotationSpeed = 5f; // 回転速度
    public GameObject rotationObj;
    public GameObject camCom;
    int randomNumber = 0;
    void Update()
    {

            if (gameObject.layer == 7)
            {

                GameObject[] allObjects = FindObjectsOfType<GameObject>();
                List<GameObject> objectsWithLayer7 = new List<GameObject>();

                foreach (GameObject obj in allObjects)
                {

                    if (obj.layer == 8)
                    {
                        // 自身とターゲット間の距離を計算
                        float distanceToTarget = Vector3.Distance(transform.position, obj.transform.position);

                        // ターゲットが検出半径内にあるかチェック
                        if (distanceToTarget <= detectionRadius)
                        {
                           // 親オブジェクトのローカル座標系でのターゲットの位置を計算
                           Vector3 localTargetPosition = rotationObj.transform.parent.InverseTransformPoint(obj.transform.position);
                           // ターゲットの方向を向く（Y軸のみ）
                           Quaternion targetRotation = Quaternion.LookRotation(new Vector3(localTargetPosition.x, 0, localTargetPosition.z));
                           // 自分の前方ベクトル
                           Vector3 forward = rotationObj.transform.forward;

                           // 対象オブジェクトへの方向ベクトル
                           Vector3 toTarget = obj.transform.position - rotationObj.transform.position;

                           // 前方ベクトルと対象への方向ベクトルのドット積を計算
                           float dotProduct = Vector3.Dot(forward, toTarget.normalized);
                           
                           // ドット積が正の場合、対象オブジェクトは前方にいる
                           if (dotProduct > 0)
                           {

                               rotationObj.transform.localRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
                           }

                            // 親オブジェクトのローカル座標系でのターゲットの位置を計算
                            Vector3 localTargetPositionx = camCom.transform.parent.InverseTransformPoint(obj.transform.position);

                            
                            // ターゲットの方向を向く（X軸のみ）
                            Quaternion targetRotationx = Quaternion.LookRotation(new Vector3(0, localTargetPositionx.y, localTargetPositionx.z));

                           

                            // rotationObj を親オブジェクト基準でX軸周りに回転させる
                            camCom.transform.localRotation = Quaternion.Euler(targetRotationx.x + 15, 0, 0);
                        }
                    }
                }
            }
            else if (gameObject.layer == 8)
            {
                GameObject[] allObjects = FindObjectsOfType<GameObject>();
                List<GameObject> objectsWithLayer8 = new List<GameObject>();

                foreach (GameObject obj in allObjects)
                {
                    if (obj.layer == 7 &&  obj.name == "AmatureUpper")
                    {
                    
                        // 自身とターゲット間の距離を計算
                        float distanceToTarget = Vector3.Distance(transform.position, obj.transform.position);

                        // ターゲットが検出半径内にあるかチェック
                        if (distanceToTarget <= detectionRadius)
                        {
                       
                           // 親オブジェクトのローカル座標系でのターゲットの位置を計算
                           Vector3 localTargetPosition = rotationObj.transform.parent.InverseTransformPoint(obj.transform.position);
                           // ターゲットの方向を向く（Y軸のみ）
                           Quaternion targetRotation = Quaternion.LookRotation(new Vector3(localTargetPosition.x, 0, localTargetPosition.z));
                           // 自分の前方ベクトル
                           Vector3 forward = rotationObj.transform.forward;

                           // 対象オブジェクトへの方向ベクトル
                           Vector3 toTarget = obj.transform.position - rotationObj.transform.position;
                           
                           // 前方ベクトルと対象への方向ベクトルのドット積を計算
                           float dotProduct = Vector3.Dot(forward, toTarget.normalized);
                            
                           // ドット積が正の場合、対象オブジェクトは前方にいる
                           if (dotProduct > 0)
                           {
                              
                                rotationObj.transform.localRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
                           }
                           
                           
                           
                       
                           // 親オブジェクトのローカル座標系でのターゲットの位置を計算
                           Vector3 localTargetPositionx = camCom.transform.parent.InverseTransformPoint(obj.transform.position);
                           
                           // ターゲットの方向を向く（X軸のみ）
                           Quaternion targetRotationx = Quaternion.LookRotation(new Vector3(0, localTargetPositionx.y, localTargetPositionx.z));

                           

                           // rotationObj を親オブジェクト基準でX軸周りに回転させる
                           camCom.transform.localRotation = Quaternion.Euler(targetRotationx.x +15, 0, 0);
                        }
                    }
                }



            }
            
            timer += Time.deltaTime;
            // 子オブジェクトのローカル座標系での方向を取得
            Vector3 childForward = childTransform.forward;
            Vector3 childRight = childTransform.right;
            
            // 移動ベクトルを初期化
            Vector3 moveDirection = Vector3.zero;
            if (timer >= changeDirectionInterval)
            {
                randomNumber = Random.Range(1, 6); // 上限は非包含
                
                timer = 0f;
            }
            // Wキーで前進
            if (randomNumber == 1)
            {
                //photonView.RPC(nameof(WalkSound), RpcTarget.All);
                WalkSound();
                moveDirection += childForward;
                if (!animator.GetBool("Walk"))
                {
                    animator.SetBool("Walk", true);
                }
                if (!animatorleg.GetBool("Walk"))
                {
                    animatorleg.SetBool("Walk", true);
                }
                if (!animatormid.GetBool("Walk"))
                {
                    animatormid.SetBool("Walk", true);
                }


                animator.SetBool("Run", false);
                animatorleg.SetBool("WalkRight", false);
                animatorleg.SetBool("WalkLeft", false);
                animatorleg.SetBool("WalkBack", false);
                animatorleg.SetBool("Run", false);
                animatormid.SetBool("WalkRight", false);
                animatormid.SetBool("WalkLeft", false);
                animatormid.SetBool("WalkBack", false);
                animatormid.SetBool("Run", false);
                moveSpeed = 1.5f;

               
            }
            // Sキーで後退
            else if (randomNumber == 2)
            {
                //photonView.RPC(nameof(WalkSound), RpcTarget.All);
                WalkSound();
                moveDirection -= childForward;
                if (!animator.GetBool("Walk"))
                {
                    animator.SetBool("Walk", true);
                }
                if (!animatorleg.GetBool("WalkBack"))
                {
                    animatorleg.SetBool("WalkBack", true);
                }
                if (!animatormid.GetBool("WalkBack"))
                {
                    animatormid.SetBool("WalkBack", true);
                }


                animator.SetBool("Run", false);
                animatorleg.SetBool("Walk", false);
                animatorleg.SetBool("WalkRight", false);
                animatorleg.SetBool("WalkLeft", false);

                animatorleg.SetBool("Run", false);
                animatormid.SetBool("Walk", false);
                animatormid.SetBool("WalkRight", false);
                animatormid.SetBool("WalkLeft", false);

                animatormid.SetBool("Run", false);
                moveSpeed = 1.5f;
                
            }
            // Aキーで左移動
            else if (randomNumber == 3)
            {
                //photonView.RPC(nameof(WalkSound), RpcTarget.All);
                WalkSound();
                moveDirection -= childRight;
                if (!animator.GetBool("Walk"))
                {
                    animator.SetBool("Walk", true);
                }
                if (!animatorleg.GetBool("WalkLeft"))
                {
                    animatorleg.SetBool("WalkLeft", true);
                }
                if (!animatormid.GetBool("WalkLeft"))
                {
                    animatormid.SetBool("WalkLeft", true);
                }


                animator.SetBool("Run", false);
                animatorleg.SetBool("Walk", false);
                animatorleg.SetBool("WalkRight", false);

                animatorleg.SetBool("WalkBack", false);
                animatorleg.SetBool("Run", false);
                animatormid.SetBool("Walk", false);
                animatormid.SetBool("WalkRight", false);

                animatormid.SetBool("WalkBack", false);
                animatormid.SetBool("Run", false);
                moveSpeed = 1.5f;
                
            }
            // Dキーで右移動
            else if (randomNumber == 4)
            {
                //photonView.RPC(nameof(WalkSound), RpcTarget.All);
                WalkSound();
                moveDirection += childRight;
                if (!animator.GetBool("Walk"))
                {
                    animator.SetBool("Walk", true);
                }
                if (!animatorleg.GetBool("WalkRight"))
                {
                    animatorleg.SetBool("WalkRight", true);
                }
                if (!animatormid.GetBool("WalkRight"))
                {
                    animatormid.SetBool("WalkRight", true);
                }


                animator.SetBool("Run", false);
                animatorleg.SetBool("Walk", false);

                animatorleg.SetBool("WalkLeft", false);
                animatorleg.SetBool("WalkBack", false);
                animatorleg.SetBool("Run", false);
                animatormid.SetBool("Walk", false);

                animatormid.SetBool("WalkLeft", false);
                animatormid.SetBool("WalkBack", false);
                animatormid.SetBool("Run", false);
                moveSpeed =1.5f;
                
            }

            else if (moveDirection == Vector3.zero)
            {
                //photonView.RPC(nameof(StopSound), RpcTarget.All);
                StopSound();
                animator.SetBool("Walk", false);
                animator.SetBool("Run", false);
                animatorleg.SetBool("Walk", false);
                animatorleg.SetBool("WalkRight", false);
                animatorleg.SetBool("WalkLeft", false);
                animatorleg.SetBool("WalkBack", false);
                animatorleg.SetBool("Run", false);
                animatormid.SetBool("Walk", false);
                animatormid.SetBool("WalkRight", false);
                animatormid.SetBool("WalkLeft", false);
                animatormid.SetBool("WalkBack", false);
                animatormid.SetBool("Run", false);

                Vector3 currentLocalPosition = transform.localPosition;

                // 固定する軸のローカルポジションを維持
                transform.localPosition = new Vector3(
                    freezeX ? currentLocalPosition.x : currentLocalPosition.x,
                    freezeY ? currentLocalPosition.y : currentLocalPosition.y,
                    freezeZ ? currentLocalPosition.z : currentLocalPosition.z
                );

            }
            // 移動ベクトルを正規化して、移動速度をかける
            moveDirection = moveDirection.normalized * moveSpeed * Time.deltaTime;

            // 親オブジェクトを移動させる

            transform.localPosition += moveDirection;

            // グローバルな移動方向に変換
            moveDirection = localMove;

            Vector3 gravityDirection = (transform.position - planet.position).normalized;

            // 重力方向を考慮して回転を調整
            transform.rotation = Quaternion.FromToRotation(transform.up, gravityDirection) * transform.rotation;




    }


    void WalkSound()
    {
        if (photonView.IsMine)
        {
            audioSources[0].UnPause();
            audioSources[1].Pause();
        }


    }


    void RunSound()
    {
        if (photonView.IsMine)
        {
            audioSources[1].UnPause();
            audioSources[0].Pause();
        }


    }

    void StartSound()
    {
        if (photonView.IsMine)
        {
            audioSources[0].Play();
            audioSources[1].Play();
        }


    }


    void StopSound()
    {

        if (photonView.IsMine)
        {
            audioSources[0].Pause();
            audioSources[1].Pause();
        }



    }


    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (isGrounded == false)
            {
                audioSources[2].Play();
            }
            isGrounded = true;

        }

    }

    void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;

        }


    }


}
