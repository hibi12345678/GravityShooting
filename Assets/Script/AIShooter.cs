using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
public class AIShooter : MonoBehaviourPunCallbacks
{
    public float maxDistance = 100f; // Ray�̍ő勗��
    public int damage; // �^����_���[�W��

    public GameObject effectPrefab;  // �G�t�F�N�g�̃v���n�u
    public GameObject firePrefab;  // �G�t�F�N�g�̃v���n�u
    public GameObject hibanaEffect;  // �G�t�F�N�g�̃v���n�u
    public Transform camConTransform;
    
    
    public GameObject firePoint; // �e�𔭎˂���ʒu
    Quaternion rotation;
    int bulletCount;
    public Camera myCamera;
    private bool myBool, timeBool, continuBool;
    int errorCount = 0;
    float sumRecoil;
    float[] recoilX = { 0.0f, 0.0f, 0.2f, 0.3f, 0.3f, 0.7f, 0.1f, 0.2f, 0.3f, 0.7f, 0.5f, 0.1f, 0.2f, 0.3f, 0.7f, 0.7f, 0.6f, 0.6f, 0.6f, 0.4f, 0.3f, 0.6f, 0.7f, 0.2f, 0.3f, 0.7f };
    float[] recoilY = { 0.0f, 0.3f, 0.1f, 0.5f, 0.0f, -0.1f, 0.2f, 0.4f, -0.4f, 0.2f, 0.3f, 0.14f, -0.3f, -0.2f, -0.1f, 0.4f, 0.2f, 0.0f, -0.3f, 0.0f, 0.0f, -0.0f, -0.1f, 0.0f, 0.3f, 0.1f };
    float[] errorX = { 0.0f, 0.0f, 0.0f, 0.1f, 0.1f, 0.1f, -0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.0f, 0.0f, -0.1f, 0.1f, 0.1f, 0.0f, 0.1f, 0.05f, 0.1f, 0.0f, 0.0f, 0.1f, 0.1f, -0.1f, 0.0f };
    float[] errorY = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.1f, 0.0f, 0.0f, 0.05f, 0.1f, -0.1f, 0.0f, 0.0f, 0.1f, 0.1f, 0.0f, 0.0f, 0.05f, -0.1f, 0.0f, 0.1f, 0.0f, 0.0f, 0.05f, -0.1f, 0.1f };
    private Coroutine currentCoroutine = null;  // ���ݎ��s���̃R���[�`����ǐ�

    // �e�L�X�g�̐F��ύX���邽�߂̕ϐ�
    private Color textColor = Color.black;
    public Animator animator;
    private AudioSource[] audioSources;
    public int num;
    public int frames = 60; // ��]�ɂ�����t���[����
    private Quaternion targetRotation;
    private Quaternion initialRotation;
    private int currentFrame;
    public GameObject parent;
    public float detectionRadius = 12f; // ���o���a
    private float timer;
    public float changeInterval = 1f;
    void Start()
    {
        // �����̃v���C���[�ł���΃J������L���ɂ��A���̃v���C���[�ł���Ζ����ɂ���
        if (photonView.IsMine)
        {
            myCamera.enabled = true;
        }
        else
        {
            myCamera.enabled = false;
        }

        myBool = true;
        timeBool = true;
        continuBool = true;
        bulletCount = 25;
        audioSources = GetComponents<AudioSource>();
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

    void Update()
    {

        if (photonView.IsMine)
        {

            timer += Time.deltaTime;
            // �q�I�u�W�F�N�g�̃��[�J�����W�n�ł̕������擾
           
            // �ړ��x�N�g����������
            Vector3 moveDirection = Vector3.zero;
            int randomNumber = 0;
            if (timer >= changeInterval)
            {
                randomNumber = Random.Range(1, 2); // ����͔���
                timer = 0f;
            }

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
                            if (randomNumber  == 1 && myBool == true && bulletCount != 0 && timeBool == true) // �}�E�X�̍��{�^�����������Ƃ�
                            {
                                errorCount++;
                                bulletCount--;
                                PhotonView photonView = GetComponent<PhotonView>();

                                photonView.RPC(nameof(ShootRayFromCenter), RpcTarget.All);
                                // Ray�����������I�u�W�F�N�g�ɏ��𑗂�
                                
                                animator.SetTrigger("Fire");

                                if (continuBool == false)
                                {
                                    StopCoroutine(currentCoroutine);
                                }
                                currentCoroutine = StartCoroutine(recoilCoroutine());


                            }

                            if (bulletCount == 0 && timeBool == true)
                            {
                                StartCoroutine(Relord());

                            }
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
                    if (obj.layer == 7)
                    {
                        // ���g�ƃ^�[�Q�b�g�Ԃ̋������v�Z
                        float distanceToTarget = Vector3.Distance(transform.position, obj.transform.position);

                        // �^�[�Q�b�g�����o���a���ɂ��邩�`�F�b�N
                        if (distanceToTarget <= detectionRadius)
                        {
                            if (randomNumber == 1 && myBool == true && bulletCount != 0 && timeBool == true) // �}�E�X�̍��{�^�����������Ƃ�
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

                            if (bulletCount == 0 && timeBool == true)
                            {
                                StartCoroutine(Relord());

                            }
                        }
                    }
                }



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
        // ��ʂ̒��S����Ray���΂�
        Ray ray = myCamera.ScreenPointToRay(new Vector3(Screen.width / 2 + errorX[errorCount], Screen.height / 2 + errorY[errorCount]));
        RaycastHit hit;
        Vector3 targetPosition;
        transform.localRotation *= Quaternion.Euler(-recoilX[errorCount], -recoilY[errorCount], 0);
        sumRecoil += recoilY[errorCount];
        StartCoroutine(DelayCoroutine());
        if (gameObject.layer == 7)
        {
            layerMask = ((1 << 8) | (1 << 13) | (1 << 14));
        }
        else if (gameObject.layer == 8)
        {
            layerMask = ((1 << 7) | (1 << 13) | (1 << 14));
        }
        if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
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
                    targetPhotonView.RPC("TakeDamage", RpcTarget.All, damage, targetPhotonView.CreatorActorNr, num , photonView.ViewID);
                    targetPhotonView.RPC("TakeDirection", RpcTarget.All, targetPhotonView.CreatorActorNr, camConTransform.position, num);
                    
                }

                
                
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
                    targetPhotonView.RPC("TakeDamage", RpcTarget.All, damage, aItargetHealth.num, num, photonView.ViewID);

                }

            }

            // �q�b�g�����I�u�W�F�N�g��Health�R���|�[�l���g������ꍇ�A�_���[�W��^����
            // �v���n�u���w��̈ʒu�ɃC���X�^���X��
            GameObject hibanaInstance = Instantiate(hibanaEffect, targetPosition, rotation);

            // Particle System���Đ�
            ParticleSystem hibana = hibanaInstance.GetComponent<ParticleSystem>();
            if (hibana != null)
            {
                hibana.Play();

                // �G�t�F�N�g���I�������玩���ō폜�����悤�ɐݒ�
                Destroy(hibanaInstance, hibana.main.duration + hibana.main.startLifetime.constantMax);
            }


            // �q�b�g�����I�u�W�F�N�g��Transform���擾
            Transform hitTransform = hit.transform;

            // "Canvas"�Ƃ������O�̎q�I�u�W�F�N�g�ɃA�N�Z�X
            Transform canvas = hitTransform.Find("Canvas");

        }
        else
        {
            targetPosition = ray.origin + ray.direction * 100f; // �f�t�H���g�̃^�[�Q�b�g�ʒu��ݒ�
        }



        // �e���������������v�Z���A���x��ݒ肷��
        Vector3 direction = (targetPosition - firePoint.transform.position).normalized;

        // direction�x�N�g�������]�s����쐬
        rotation = Quaternion.LookRotation(direction);

        // �v���n�u���w��̈ʒu�ɃC���X�^���X��
        GameObject effectInstance = Instantiate(effectPrefab, firePoint.transform.position, rotation);

        GameObject fireInstance = Instantiate(firePrefab, firePoint.transform.position, rotation);
        // ���݂̃X�P�[�����擾
        Vector3 currentScale = effectInstance.transform.localScale;

        // x���̃X�P�[����ύX
        currentScale.z = (targetPosition - firePoint.transform.position).magnitude;

        // �ύX�����X�P�[�����I�u�W�F�N�g�ɓK�p
        effectInstance.transform.localScale = currentScale;
        // Particle System���Đ�
        ParticleSystem ps = effectInstance.GetComponent<ParticleSystem>();

        ParticleSystem fire = fireInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            fire.Play();
            // �G�t�F�N�g���I�������玩���ō폜�����悤�ɐݒ�
            Destroy(effectInstance, ps.main.duration + ps.main.startLifetime.constantMax);
            // �G�t�F�N�g���I�������玩���ō폜�����悤�ɐݒ�
            Destroy(fireInstance, fire.main.duration + fire.main.startLifetime.constantMax);
        }
        else
        {
            // �G�t�F�N�g�̃I�u�W�F�N�g���̂��폜�����悤�ɐݒ�iParticle System���Ȃ��ꍇ�j
            Destroy(effectInstance, 0.1f);  // 5�b��ɍ폜
            Destroy(fireInstance, 0.1f);  // 5�b��ɍ폜
        }

    }

    IEnumerator Relord()
    {
        photonView.RPC(nameof(Relo), RpcTarget.All);
        // 3.5�b�ԑ҂�
        timeBool = false;
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(2.5f);
        photonView.RPC(nameof(ReloFin), RpcTarget.All);
        yield return new WaitForSeconds(1.0f);
        timeBool = true;
        bulletCount = 25;
        errorCount = 0;
    }

    // �R���[�`���{��
    IEnumerator DelayCoroutine()
    {
        myBool = false;
        // 0.16�b�ԑ҂�
        yield return new WaitForSeconds(0.16f);
        myBool = true;
    }

    // �R���[�`���{��
    IEnumerator recoilCoroutine()
    {
        continuBool = false;
        // 0.3�b�ԑ҂�

        yield return new WaitForSeconds(0.3f);
        initialRotation = transform.localRotation;
        errorCount = 0;
        currentFrame = 0;

        continuBool = true;
        currentCoroutine = null;  // �R���[�`���̏I��������
    }
}