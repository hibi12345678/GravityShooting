using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;
public class Health : MonoBehaviourPunCallbacks
{
    
    public int currentHealth;
    private AudioSource audioSource;
    public GameObject uiPrefab;
    public GameObject killuiPrefab;
    public TextMeshProUGUI killtextPrefab = default;
    
    
    public Transform uiParent;
    float angle;
    GameObject uiInstance;
    GameObject killuiInstance;
    TextMeshProUGUI killtextInstance;  
    GameObject deathuiInstance;
    TextMeshProUGUI deathtextInstance;
    Vector3 localDirection;
    public GameObject mainScene;
    public GameObject parentObject;
    ScoreProperty scoreProperty;
    float azimuthAngle;
    
    PhotonView targetPhotonView;
    [SerializeField]
    private Image GreenGauge;
    int num;
    float value;
    public Camera myCamera;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = 100;
        mainScene = GameObject.Find("MainScene");
        scoreProperty = mainScene.GetComponent<ScoreProperty>();
        targetPhotonView = mainScene.GetComponent<PhotonView>();
        DOTween.Init();

    }

    [PunRPC]
    public void TakeDamage(int damage, int myNum, int senderActorNumber ,int shooterViewID)
    {

        if(myNum == photonView.CreatorActorNr)
        {
            // Rayを発射したプレイヤーのPhotonViewを取得
            PhotonView shooterPhotonView = PhotonView.Find(shooterViewID);
            GameObject shooter = shooterPhotonView.gameObject;
            if (gameObject.layer == 7)
            {
                if (senderActorNumber == 2 || senderActorNumber == 4)
                {
                    currentHealth -= damage;

                    if (currentHealth <= 0)
                    {
                       
                        Die(photonView.CreatorActorNr, senderActorNumber, shooter);
                    }
                    
                    scoreProperty.ScoreAdd(damage,senderActorNumber);
                    

                }
            }

            else if (gameObject.layer == 8)
            {
                if (senderActorNumber == 1 || senderActorNumber == 3)
                {
                    currentHealth -= damage;

                    if (currentHealth <= 0)
                    {
                        
                        Die(photonView.CreatorActorNr, senderActorNumber, shooter);
                    }
                    
                    scoreProperty.ScoreAdd(damage,senderActorNumber);
                    
                }
            }
        }

    }


    [PunRPC]
    public void TakeDirection(int myNum, Vector3 direction, int senderActorNumber)
    {
        if (myNum == photonView.CreatorActorNr)
        {
            if (gameObject.layer == 7)
            {
                if (senderActorNumber == 2 || senderActorNumber == 4)
                {
                    // ワールド空間での方向ベクトルを計算
                    Vector3 worldDirection = direction - transform.position;

                    // ローカル空間での方向ベクトルに変換
                    localDirection = transform.InverseTransformDirection(worldDirection);

                    // UI要素のインスタンスを作成し、親を設定する
                    uiInstance = Instantiate(uiPrefab, uiParent);
                    // 必要に応じて、位置や回転を調整する（ローカル座標系で）
                    uiInstance.transform.localPosition = Vector3.zero;
                    if (uiInstance != null)
                    {
                        // XZ平面での方向ベクトルを取得（Y成分は無視）
                        Vector3 flattenedDirection = new Vector3(localDirection.x, 0, localDirection.z);

                        // 方位角をラジアンで計算し、度に変換
                        azimuthAngle = Mathf.Atan2(flattenedDirection.z, flattenedDirection.x) * Mathf.Rad2Deg;

                        uiInstance.transform.localRotation = Quaternion.Euler(0, 0, azimuthAngle - 90f + parentObject.transform.localEulerAngles.y);
                    }
                    Destroy(uiInstance, 0.7f);
                    

                }
            }

            else if (gameObject.layer == 8)
            {
                if (senderActorNumber == 1 || senderActorNumber == 3)
                {
                    // ワールド空間での方向ベクトルを計算
                    Vector3 worldDirection = direction - transform.position;

                    // ローカル空間での方向ベクトルに変換
                    localDirection = transform.InverseTransformDirection(worldDirection);

                    // UI要素のインスタンスを作成し、親を設定する
                    uiInstance = Instantiate(uiPrefab, uiParent);
                    // 必要に応じて、位置や回転を調整する（ローカル座標系で）
                    uiInstance.transform.localPosition = Vector3.zero;
                    if (uiInstance != null)
                    {
                        // XZ平面での方向ベクトルを取得（Y成分は無視）
                        Vector3 flattenedDirection = new Vector3(localDirection.x, 0, localDirection.z);

                        // 方位角をラジアンで計算し、度に変換
                        azimuthAngle = Mathf.Atan2(flattenedDirection.z, flattenedDirection.x) * Mathf.Rad2Deg;

                        uiInstance.transform.localRotation = Quaternion.Euler(0, 0, azimuthAngle - 90f-parentObject.transform.localEulerAngles.y);
                    }
                    Destroy(uiInstance, 0.7f);

                }
            }

        }

    }

    Vector3[] points = new Vector3[6]
    {
        new Vector3(0, -16, 0),
        new Vector3(0, 16, 0),
        new Vector3(0, 0, -16),    
        new Vector3(0, 0, 16),
        new Vector3(-16, 0, 0),
        new Vector3(16, 0, 0)
    };
    // 近いとみなす距離のしきい値
    public float minDistance = 10.0f;

    
    void Die(int death,int kill, GameObject shooter)
    {
        scoreProperty.KDAdd(death, kill);
        currentHealth = 100;
        
        GreenGauge.fillAmount = 1;
        
        List<Vector3> validPoints = new List<Vector3>();
        PhotonView shooterPhotonView = shooter.GetComponent<PhotonView>();
        KillSound targetHealth = shooter.GetComponent<KillSound>();

        if (targetHealth != null)
        {
            shooterPhotonView.RPC("KillEnemy", RpcTarget.All, death, kill);
        }
        if (gameObject.layer == 7)
        {

            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            List<GameObject> objectsWithLayer7 = new List<GameObject>();
            
            foreach (GameObject obj in allObjects)
            {
                if (obj.name == "Player2(Clone)" || obj.name == "Player4(Clone)" || obj.name == "Player3-1(Clone)" || obj.name == "Player3-3(Clone)")
                {
                    objectsWithLayer7.Add(obj);
                    
                }

                

            }

            // 現在の位置から十分に離れた点を収集
            foreach (var point in points)
            {
                if (Vector3.Distance(objectsWithLayer7[0].transform.position, point) > minDistance && Vector3.Distance(objectsWithLayer7[1].transform.position, point) > minDistance)
                {
                    validPoints.Add(point);
                }
            }

            // 有効な点がない場合は処理を終了
            if (validPoints.Count == 0)
            {
                Debug.LogWarning("No valid points found that are far enough from the current position.");
                return;
            }


            if (photonView.IsMine)
            {
                killuiInstance = Instantiate(killuiPrefab, uiParent);
                killuiInstance.transform.localPosition = new Vector3(0, 200, 0);
                killuiInstance.transform.localRotation = Quaternion.identity;
                Destroy(killuiInstance, 2.0f);
                killtextInstance = Instantiate(killtextPrefab, uiParent);
                killtextInstance.text = "Killed by Player" + kill.ToString();
                killtextInstance.transform.localPosition = new Vector3(0, 200, 0);
                killtextInstance.transform.localRotation = Quaternion.identity;
                Destroy(killtextInstance, 2.0f);
                
                
                // ランダムに選ばれた位置に移動
                int randomIndex = Random.Range(0, validPoints.Count);
                transform.position = validPoints[randomIndex];
                
            }

        }
        else if (gameObject.layer == 8)
        {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            List<GameObject> objectsWithLayer8 = new List<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (obj.name == "Player1(Clone)" || obj.name == "Player3(Clone)" || obj.name == "Player3-2(Clone)" )
                {
                    objectsWithLayer8.Add(obj);
                }
            

            }
            


            // 現在の位置から十分に離れた点を収集
            foreach (var point in points)
            {
                if (Vector3.Distance(objectsWithLayer8[0].transform.position, point) > minDistance && Vector3.Distance(objectsWithLayer8[1].transform.position, point) > minDistance)
                {
                    validPoints.Add(point);
                }
            }

            // 有効な点がない場合は処理を終了
            if (validPoints.Count == 0)
            {
                Debug.LogWarning("No valid points found that are far enough from the current position.");
                return;
            }

            if (photonView.IsMine)
            {
                killuiInstance = Instantiate(killuiPrefab, uiParent);
                killuiInstance.transform.localPosition = new Vector3(0, 200, 0);
                killuiInstance.transform.localRotation = Quaternion.identity;
                Destroy(killuiInstance, 2.0f);
                killtextInstance = Instantiate(killtextPrefab, uiParent);
                killtextInstance.text = "Killed by Player" + kill.ToString();
                killtextInstance.transform.localPosition = new Vector3(0, 200, 0);
                killtextInstance.transform.localRotation = Quaternion.identity;
                Destroy(killtextInstance, 2.0f);
                
                
                // ランダムに選ばれた位置に移動
                int randomIndex = Random.Range(0, validPoints.Count);
                transform.position = validPoints[randomIndex];
                
            }
                


            
            
            
        }
    }

    void Update()
    {
       
        value = currentHealth;
        var valueFrom = value / 100;
        
        GreenGauge.fillAmount = valueFrom;
    }


   
}

