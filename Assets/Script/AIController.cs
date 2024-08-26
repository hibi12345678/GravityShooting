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
    public Transform groundCheck; // �L�����N�^�[�̑����̈ʒu���w�肷��Transform
    public Transform amatureleg;
    Transform planet;
    private bool isGrounded;
    Vector3 localMove;
    private Rigidbody rb; // Rigidbody�R���|�[�l���g

    // �q�I�u�W�F�N�g��ݒ�
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

        // Rigidbody�R���|�[�l���g���擾
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

        // �I�u�W�F�N�g���g�̃��C���[��ݒ�
        obj.layer = newLayer;

        // ���ׂĂ̎q�I�u�W�F�N�g�ɑ΂��ă��C���[��ݒ�
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
        // �e�I�u�W�F�N�g�Ƃ��̎q�I�u�W�F�N�g���ׂĂɃ��C���[��K�p
        SetLayerRecursively(gameObject, newLayer);
    }
    public float detectionRadius = 12f; // ���o���a
    public float changeDirectionInterval = 1f; // �ړ�������ύX����Ԋu�i�b�j
    private float timer;
    public float rotationSpeed = 5f; // ��]���x
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
                        // ���g�ƃ^�[�Q�b�g�Ԃ̋������v�Z
                        float distanceToTarget = Vector3.Distance(transform.position, obj.transform.position);

                        // �^�[�Q�b�g�����o���a���ɂ��邩�`�F�b�N
                        if (distanceToTarget <= detectionRadius)
                        {
                           // �e�I�u�W�F�N�g�̃��[�J�����W�n�ł̃^�[�Q�b�g�̈ʒu���v�Z
                           Vector3 localTargetPosition = rotationObj.transform.parent.InverseTransformPoint(obj.transform.position);
                           // �^�[�Q�b�g�̕����������iY���̂݁j
                           Quaternion targetRotation = Quaternion.LookRotation(new Vector3(localTargetPosition.x, 0, localTargetPosition.z));
                           // �����̑O���x�N�g��
                           Vector3 forward = rotationObj.transform.forward;

                           // �ΏۃI�u�W�F�N�g�ւ̕����x�N�g��
                           Vector3 toTarget = obj.transform.position - rotationObj.transform.position;

                           // �O���x�N�g���ƑΏۂւ̕����x�N�g���̃h�b�g�ς��v�Z
                           float dotProduct = Vector3.Dot(forward, toTarget.normalized);
                           
                           // �h�b�g�ς����̏ꍇ�A�ΏۃI�u�W�F�N�g�͑O���ɂ���
                           if (dotProduct > 0)
                           {

                               rotationObj.transform.localRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
                           }

                            // �e�I�u�W�F�N�g�̃��[�J�����W�n�ł̃^�[�Q�b�g�̈ʒu���v�Z
                            Vector3 localTargetPositionx = camCom.transform.parent.InverseTransformPoint(obj.transform.position);

                            
                            // �^�[�Q�b�g�̕����������iX���̂݁j
                            Quaternion targetRotationx = Quaternion.LookRotation(new Vector3(0, localTargetPositionx.y, localTargetPositionx.z));

                           

                            // rotationObj ��e�I�u�W�F�N�g���X������ɉ�]������
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
                    
                        // ���g�ƃ^�[�Q�b�g�Ԃ̋������v�Z
                        float distanceToTarget = Vector3.Distance(transform.position, obj.transform.position);

                        // �^�[�Q�b�g�����o���a���ɂ��邩�`�F�b�N
                        if (distanceToTarget <= detectionRadius)
                        {
                       
                           // �e�I�u�W�F�N�g�̃��[�J�����W�n�ł̃^�[�Q�b�g�̈ʒu���v�Z
                           Vector3 localTargetPosition = rotationObj.transform.parent.InverseTransformPoint(obj.transform.position);
                           // �^�[�Q�b�g�̕����������iY���̂݁j
                           Quaternion targetRotation = Quaternion.LookRotation(new Vector3(localTargetPosition.x, 0, localTargetPosition.z));
                           // �����̑O���x�N�g��
                           Vector3 forward = rotationObj.transform.forward;

                           // �ΏۃI�u�W�F�N�g�ւ̕����x�N�g��
                           Vector3 toTarget = obj.transform.position - rotationObj.transform.position;
                           
                           // �O���x�N�g���ƑΏۂւ̕����x�N�g���̃h�b�g�ς��v�Z
                           float dotProduct = Vector3.Dot(forward, toTarget.normalized);
                            
                           // �h�b�g�ς����̏ꍇ�A�ΏۃI�u�W�F�N�g�͑O���ɂ���
                           if (dotProduct > 0)
                           {
                              
                                rotationObj.transform.localRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
                           }
                           
                           
                           
                       
                           // �e�I�u�W�F�N�g�̃��[�J�����W�n�ł̃^�[�Q�b�g�̈ʒu���v�Z
                           Vector3 localTargetPositionx = camCom.transform.parent.InverseTransformPoint(obj.transform.position);
                           
                           // �^�[�Q�b�g�̕����������iX���̂݁j
                           Quaternion targetRotationx = Quaternion.LookRotation(new Vector3(0, localTargetPositionx.y, localTargetPositionx.z));

                           

                           // rotationObj ��e�I�u�W�F�N�g���X������ɉ�]������
                           camCom.transform.localRotation = Quaternion.Euler(targetRotationx.x +15, 0, 0);
                        }
                    }
                }



            }
            
            timer += Time.deltaTime;
            // �q�I�u�W�F�N�g�̃��[�J�����W�n�ł̕������擾
            Vector3 childForward = childTransform.forward;
            Vector3 childRight = childTransform.right;
            
            // �ړ��x�N�g����������
            Vector3 moveDirection = Vector3.zero;
            if (timer >= changeDirectionInterval)
            {
                randomNumber = Random.Range(1, 6); // ����͔���
                
                timer = 0f;
            }
            // W�L�[�őO�i
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
            // S�L�[�Ō��
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
            // A�L�[�ō��ړ�
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
            // D�L�[�ŉE�ړ�
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

                // �Œ肷�鎲�̃��[�J���|�W�V�������ێ�
                transform.localPosition = new Vector3(
                    freezeX ? currentLocalPosition.x : currentLocalPosition.x,
                    freezeY ? currentLocalPosition.y : currentLocalPosition.y,
                    freezeZ ? currentLocalPosition.z : currentLocalPosition.z
                );

            }
            // �ړ��x�N�g���𐳋K�����āA�ړ����x��������
            moveDirection = moveDirection.normalized * moveSpeed * Time.deltaTime;

            // �e�I�u�W�F�N�g���ړ�������

            transform.localPosition += moveDirection;

            // �O���[�o���Ȉړ������ɕϊ�
            moveDirection = localMove;

            Vector3 gravityDirection = (transform.position - planet.position).normalized;

            // �d�͕������l�����ĉ�]�𒲐�
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
