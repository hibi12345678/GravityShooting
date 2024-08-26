using Photon.Pun;
using UnityEngine;
using System.Collections;

public class SphereCharacterController : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f; // �W�����v��
    

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

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
            
            photonView.RPC(nameof(ChangeLayer), RpcTarget.All);
           if (photonView.IsMine)
           {
               photonView.RPC(nameof(StartSound), RpcTarget.All);
           }

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

        if (photonView.CreatorActorNr == 1 || photonView.CreatorActorNr == 3)
            gameObject.layer = 7;
        else if (photonView.CreatorActorNr == 2 || photonView.CreatorActorNr == 4)
            gameObject.layer = 8;
        newLayer = gameObject.layer;
        // �e�I�u�W�F�N�g�Ƃ��̎q�I�u�W�F�N�g���ׂĂɃ��C���[��K�p
        SetLayerRecursively(gameObject, newLayer);
    }

   
    void Update()
    {
        if (photonView.IsMine)
        {

            
            // �q�I�u�W�F�N�g�̃��[�J�����W�n�ł̕������擾
            Vector3 childForward = childTransform.forward;
            Vector3 childRight = childTransform.right;
            // �ړ��x�N�g����������
            Vector3 moveDirection = Vector3.zero;
            // W�L�[�őO�i
            if (Input.GetKey(KeyCode.W))
            {
                photonView.RPC(nameof(WalkSound), RpcTarget.All);
                //WalkSound();
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
                moveSpeed = 2.5f;

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    photonView.RPC(nameof(RunSound), RpcTarget.All);
                    //RunSound();
                    moveSpeed = 5f;

                    if (!animator.GetBool("Run"))
                    {
                        animator.SetBool("Run", true);
                    }

                    if (!animatorleg.GetBool("Run"))
                    {
                        animatorleg.SetBool("Run", true);
                    }
                    if (!animatormid.GetBool("Run"))
                    {
                        animatormid.SetBool("Run", true);
                    }



                }
            }
            // S�L�[�Ō��
            else if (Input.GetKey(KeyCode.S))
            {
                photonView.RPC(nameof(WalkSound), RpcTarget.All);
                //WalkSound();
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
                moveSpeed = 2.5f;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    moveSpeed = 3.5f;
                }
            }
            // A�L�[�ō��ړ�
            else if (Input.GetKey(KeyCode.A))
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
                moveSpeed = 2.5f;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    moveSpeed = 3.5f;
                }
            }
            // D�L�[�ŉE�ړ�
            else if (Input.GetKey(KeyCode.D))
            {
                photonView.RPC(nameof(WalkSound), RpcTarget.All);
                //WalkSound();
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
                moveSpeed = 2.5f;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    moveSpeed = 3.5f;
                }
            }

            else if (moveDirection == Vector3.zero)
            {
                photonView.RPC(nameof(StopSound), RpcTarget.All);
                //StopSound();
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

    }

    [PunRPC]
    void WalkSound()
    {
        if (photonView.IsMine)
        {
            audioSources[0].UnPause();
            audioSources[1].Pause();
        }
            
        
    }

    [PunRPC]
    void RunSound()
    {
        if (photonView.IsMine)
        {
            audioSources[1].UnPause();
            audioSources[0].Pause();
        }
           
        
    }
    [PunRPC]
    void StartSound()
    {
        if (photonView.IsMine)
        {
            audioSources[0].Play();
            audioSources[1].Play();
        }
            
        
    }

    [PunRPC]
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
