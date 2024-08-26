using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;

public class AIHealth : MonoBehaviourPunCallbacks
{

    public int currentHealth;
    private AudioSource audioSource;
    Vector3 localDirection;
    public GameObject mainScene;
    public GameObject parentObject;
    ScoreProperty scoreProperty;  
    public GameObject parent;
    PhotonView targetPhotonView;
    public int num;
    float value;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = 100;
        mainScene = GameObject.Find("MainScene");
        scoreProperty = mainScene.GetComponent<ScoreProperty>();
        targetPhotonView = mainScene.GetComponent<PhotonView>();
        
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
    }

    [PunRPC]
    public void TakeDamage(int damage, int myNum, int senderActorNumber, int shooterViewID)
    {

        if (myNum == num)
        {
            // Ray�𔭎˂����v���C���[��PhotonView���擾
            PhotonView shooterPhotonView = PhotonView.Find(shooterViewID);
            GameObject shooter = shooterPhotonView.gameObject;
            if (gameObject.layer == 7)
            {
                if (senderActorNumber == 2 || senderActorNumber == 4)
                {
                    currentHealth -= damage;

                    if (currentHealth <= 0)
                    {

                        Die(num, senderActorNumber,shooter);
                    }

                    scoreProperty.ScoreAdd(damage, senderActorNumber);


                }
            }

            else if (gameObject.layer == 8)
            {
                if (senderActorNumber == 1 || senderActorNumber == 3)
                {
                    currentHealth -= damage;

                    if (currentHealth <= 0)
                    {

                        Die(num, senderActorNumber, shooter);
                    }

                    scoreProperty.ScoreAdd(damage, senderActorNumber);

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
    // �߂��Ƃ݂Ȃ������̂������l
    public float minDistance = 10.0f;


    void Die(int death, int kill, GameObject shooter)
    {
        scoreProperty.KDAdd(death, kill);
        currentHealth = 100;

        
        List<Vector3> validPoints = new List<Vector3>();
        PhotonView shooterPhotonView =shooter.GetComponent<PhotonView>();
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
                if (obj.layer == 8)
                {
                    objectsWithLayer7.Add(obj);
                }


            }

            // ���݂̈ʒu����\���ɗ��ꂽ�_�����W
            foreach (var point in points)
            {
                if (Vector3.Distance(objectsWithLayer7[0].transform.position, point) > minDistance && Vector3.Distance(objectsWithLayer7[1].transform.position, point) > minDistance)
                {
                    validPoints.Add(point);
                }
            }

            // �L���ȓ_���Ȃ��ꍇ�͏������I��
            if (validPoints.Count == 0)
            {
                Debug.LogWarning("No valid points found that are far enough from the current position.");
                return;
            }

            // ���ׂẴR���W�����𖳌���
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }
            // Rigidbody�̕������Z�𖳌���
            Rigidbody rb = GetComponent<Rigidbody>();
            
            rb.isKinematic = true;
            parentObject.SetActive(false);
            // �����_���ɑI�΂ꂽ�ʒu�Ɉړ�
            int randomIndex = Random.Range(0, validPoints.Count);
            transform.position = validPoints[randomIndex];
            //yield return new WaitForSeconds(1f);
            parentObject.SetActive(true);
            
            // �ēx�A���ׂẴR���W������L����
            foreach (Collider collider in colliders)
            {
                collider.enabled = true;
            }
            rb.isKinematic = false;
        }
        else if (gameObject.layer == 8)
        {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            List<GameObject> objectsWithLayer8 = new List<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (obj.layer == 7)
                {
                    objectsWithLayer8.Add(obj);
                }
            }



            // ���݂̈ʒu����\���ɗ��ꂽ�_�����W
            foreach (var point in points)
            {
                if (Vector3.Distance(objectsWithLayer8[0].transform.position, point) > minDistance && Vector3.Distance(objectsWithLayer8[1].transform.position, point) > minDistance)
                {
                    validPoints.Add(point);
                }
            }

            // �L���ȓ_���Ȃ��ꍇ�͏������I��
            if (validPoints.Count == 0)
            {
                Debug.LogWarning("No valid points found that are far enough from the current position.");
                return;
            }

            // ���ׂẴR���W�����𖳌���
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }
            // Rigidbody�̕������Z�𖳌���
            Rigidbody rb = GetComponent<Rigidbody>();
            
            rb.isKinematic = true;
            parentObject.SetActive(false);
            // �����_���ɑI�΂ꂽ�ʒu�Ɉړ�
            int randomIndex = Random.Range(0, validPoints.Count);
            transform.position = validPoints[randomIndex];
            //yield return new WaitForSeconds(1f);
            parentObject.SetActive(true);
            
            // �ēx�A���ׂẴR���W������L����
            foreach (Collider collider in colliders)
            {
                collider.enabled = true;
            }
            rb.isKinematic = false;

        }

        

    }

    IEnumerator CameraOff()
    {
        // ���ׂẴR���W�����𖳌���
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        // Rigidbody�̕������Z�𖳌���
        Rigidbody rb = GetComponent<Rigidbody>();
        
        rb.isKinematic = true;
        parentObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        parentObject.SetActive(true);
        
        // �ēx�A���ׂẴR���W������L����
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
        rb.isKinematic = false;

    }
}


