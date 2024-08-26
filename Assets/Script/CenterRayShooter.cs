using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
public class CenterRayShooter : MonoBehaviourPunCallbacks
{
    public float maxDistance = 100f; // Rayの最大距離
    public int damage; // 与えるダメージ量

    public GameObject effectPrefab;  // エフェクトのプレハブ
    public GameObject firePrefab;  // エフェクトのプレハブ
    public GameObject hibanaEffect;  // エフェクトのプレハブ
    public Transform camConTransform;
    public Transform rotationObjRotation;
    
    public GameObject firePoint; // 弾を発射する位置
    public Image mouseKarsor;
    Quaternion rotation;
    int bulletCount;
    public Camera myCamera;
    private bool myBool,timeBool, continuBool;
    int errorCount = 0;
    float sumRecoil;
    float[] recoilX = { 0.0f, 0.0f, 0.2f, 0.3f, 0.3f, 0.7f, 0.1f, 0.2f, 0.3f, 0.7f, 0.5f, 0.1f, 0.2f, 0.3f, 0.7f, 0.7f, 0.6f, 0.6f, 0.6f, 0.4f, 0.3f, 0.6f, 0.7f, 0.2f, 0.3f, 0.7f};
    float[] recoilY = { 0.0f, 0.3f, 0.1f, 0.5f, 0.0f, -0.1f, 0.2f, 0.4f, -0.4f, 0.2f, 0.3f, 0.14f, -0.3f, -0.2f, -0.1f, 0.4f, 0.2f, 0.0f, -0.3f, 0.0f, 0.0f, -0.0f, -0.1f, 0.0f, 0.3f, 0.1f };
    float[] errorX = { 0.0f, 0.0f, 0.0f, 0.1f, 0.1f, 0.1f, -0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.0f, 0.0f, -0.1f, 0.1f, 0.1f, 0.0f, 0.1f, 0.05f, 0.1f, 0.0f, 0.0f, 0.1f, 0.1f, -0.1f, 0.0f };
    float[] errorY = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.1f, 0.0f, 0.0f, 0.05f, 0.1f, -0.1f, 0.0f, 0.0f, 0.1f, 0.1f, 0.0f, 0.0f, 0.05f, -0.1f, 0.0f, 0.1f, 0.0f, 0.0f, 0.05f, -0.1f, 0.1f };
    private Coroutine currentCoroutine = null;  // 現在実行中のコルーチンを追跡
                                                
    // UI Textへの参照
    public Text uiText;
    
    // テキストの色を変更するための変数
    private Color textColor = Color.black;
    public Animator animator;
    private AudioSource[] audioSources;
    public AudioListener audioListener;
    public int frames = 60; // 回転にかけるフレーム数
    private Quaternion targetRotation;
    private Quaternion initialRotation;
    private int currentFrame;
    void Start()
    {     
        // 自分のプレイヤーであればカメラを有効にし、他のプレイヤーであれば無効にする
        if (photonView.IsMine)
        {
            myCamera.enabled = true;
        }
        else
        {
            myCamera.enabled = false;
        }
        if (photonView.IsMine)
        {
            audioListener.enabled = true;
        }
        else
        {
            audioListener.enabled = false;
        }
        myBool = true;
        timeBool = true;
        continuBool = true;
        bulletCount = 25;
        audioSources = GetComponents<AudioSource>();
    }

    void Update()
    {
        
        if (photonView.IsMine)
        {
            
            
            // 変数の値をテキストに表示
            uiText.text = bulletCount.ToString() + " / 25 ";
            // 条件に応じて色を変更する例
            if (bulletCount <= 10)
            {
                uiText.color = Color.red;
            }
            else
            {
                uiText.color = Color.black;
            }

            if (Input.GetMouseButton(0) && myBool == true && bulletCount != 0 && timeBool == true) // マウスの左ボタンを押したとき
            {
                errorCount++;
                bulletCount--;
                PhotonView photonView = GetComponent<PhotonView>();


                photonView.RPC(nameof(ShootRayFromCenter), RpcTarget.All);
                animator.SetTrigger("Fire");
                
                if (continuBool == false)
                {
                    StopCoroutine(currentCoroutine);
                }
                currentCoroutine = StartCoroutine(recoilCoroutine());


            }

            if (Input.GetMouseButton(0) && bulletCount == 0 && timeBool == true)
            {
                StartCoroutine(Relord());

            }

            if (Input.GetKeyDown(KeyCode.R) && bulletCount != 25 && timeBool == true)
            {

                StartCoroutine(Relord());
            }
        }

        if (currentFrame < frames)
        {
            currentFrame++;
            float t = (float)currentFrame / frames;
            transform.localRotation = Quaternion.Lerp(initialRotation, Quaternion.Euler(Vector3.zero), t);
        }
    }

    [PunRPC]
    void Relo()
    {
        audioSources[1].Play();
    }

    [PunRPC]
    void ReloFin()
    {
        audioSources[2].Play();
    }

    int layerMask;
    [PunRPC]
    public void ShootRayFromCenter()
    {
        audioSources[0].Play();
        // 画面の中心からRayを飛ばす
        Ray ray = myCamera.ScreenPointToRay(new Vector3(Screen.width / 2+errorX[errorCount], Screen.height / 2+errorY[errorCount]));
        RaycastHit hit;
        Vector3 targetPosition;
        transform.localRotation *= Quaternion.Euler(-recoilX[errorCount], -recoilY[errorCount], 0);
        sumRecoil += recoilY[errorCount];
        StartCoroutine(DelayCoroutine());

        if(gameObject.layer == 7)
        {
            layerMask = ((1 << 8) | (1 << 13) | (1 << 14));
        }
        else if(gameObject.layer == 8)
        {
            layerMask = ((1 << 7) | (1 << 13) | (1 << 14));
        }
        if (Physics.Raycast(ray, out hit, maxDistance , layerMask))
        {
            
            targetPosition = hit.point;
                   
            Health targetHealth = hit.collider.GetComponent<Health>();
            AIHealth aItargetHealth = hit.collider.GetComponent<AIHealth>();
            PhotonView targetPhotonView = hit.collider.GetComponent<PhotonView>();
            

            if (targetHealth != null)
            {
                Collider hitCollider = hit.collider;
                

                if (hitCollider.isTrigger)
                {
                    damage = 20;
                }
                else
                {
                    damage = 10;
                }
                
                if (targetPhotonView != null)
                {
                    targetPhotonView.RPC("TakeDamage", RpcTarget.All, damage, targetPhotonView.CreatorActorNr, photonView.CreatorActorNr, photonView.ViewID);
                    targetPhotonView.RPC("TakeDirection", RpcTarget.All,  targetPhotonView.CreatorActorNr, camConTransform.position, photonView.CreatorActorNr);
                }
                
                // Materialの色を赤に変更
                mouseKarsor.color = Color.red;
                StartCoroutine(ChangeColor()); 
            }
            if (aItargetHealth != null)
            {
                Collider hitCollider = hit.collider;


                if (hitCollider.isTrigger)
                {
                    damage = 30;
                }
                else
                {
                    damage = 10;
                }

                if (targetPhotonView != null)
                {
                    targetPhotonView.RPC("TakeDamage", RpcTarget.All, damage, aItargetHealth.num, photonView.CreatorActorNr, photonView.ViewID);
                    
                }

                // Materialの色を赤に変更
                mouseKarsor.color = Color.red;
                StartCoroutine(ChangeColor());
            }


            // ヒットしたオブジェクトにHealthコンポーネントがある場合、ダメージを与える
            // プレハブを指定の位置にインスタンス化
            GameObject hibanaInstance = Instantiate(hibanaEffect, targetPosition, rotation);

            // Particle Systemを再生
            ParticleSystem hibana = hibanaInstance.GetComponent<ParticleSystem>();
            if (hibana != null)
            {
                hibana.Play();

                // エフェクトが終了したら自動で削除されるように設定
                Destroy(hibanaInstance, hibana.main.duration + hibana.main.startLifetime.constantMax);
            }


            

            
        }
        else
        {
            targetPosition = ray.origin + ray.direction * 100f; // デフォルトのターゲット位置を設定
        }

        

        // 弾が向かう方向を計算し、速度を設定する
        Vector3 direction = (targetPosition - firePoint.transform.position).normalized;

        // directionベクトルから回転行列を作成
        rotation = Quaternion.LookRotation(direction);

        // プレハブを指定の位置にインスタンス化
        GameObject effectInstance = Instantiate(effectPrefab, firePoint.transform.position, rotation);
        
        GameObject fireInstance = Instantiate(firePrefab, firePoint.transform.position, rotation);
        // 現在のスケールを取得
        Vector3 currentScale = effectInstance.transform.localScale;

        // x軸のスケールを変更
        currentScale.z = (targetPosition - firePoint.transform.position).magnitude;

        // 変更したスケールをオブジェクトに適用
        effectInstance.transform.localScale = currentScale;
        // Particle Systemを再生
        ParticleSystem ps = effectInstance.GetComponent<ParticleSystem>();
        
        ParticleSystem fire = fireInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            fire.Play();
            // エフェクトが終了したら自動で削除されるように設定
            Destroy(effectInstance, ps.main.duration + ps.main.startLifetime.constantMax);
            // エフェクトが終了したら自動で削除されるように設定
            Destroy(fireInstance, fire.main.duration + fire.main.startLifetime.constantMax);
        }
        else
        {
            // エフェクトのオブジェクト自体が削除されるように設定（Particle Systemがない場合）
            Destroy(effectInstance, 0.1f);  // 5秒後に削除
            Destroy(fireInstance, 0.1f);  // 5秒後に削除
        }

    }
    IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(0.35f);
        mouseKarsor.color = new Color(0f, 1f, 0f);
    }
    IEnumerator Relord()
    {
        photonView.RPC(nameof(Relo), RpcTarget.All);
        // 3.5秒間待つ
        timeBool = false;
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(2.5f);
        photonView.RPC(nameof(ReloFin), RpcTarget.All);
        yield return new WaitForSeconds(1.0f);
        timeBool = true;
        bulletCount = 25;
        errorCount = 0;
    }

    // コルーチン本体
    IEnumerator DelayCoroutine()
    {
        myBool = false;
        // 0.16秒間待つ
        yield return new WaitForSeconds(0.23f);
        myBool = true;
    }

    // コルーチン本体
    IEnumerator recoilCoroutine()
    {
        continuBool = false;
        // 0.3秒間待つ
        
        yield return new WaitForSeconds(0.3f);
        initialRotation = transform.localRotation;
        errorCount = 0;
        currentFrame = 0;
        
        continuBool = true;
        currentCoroutine = null;  // コルーチンの終了を示す
    }
}