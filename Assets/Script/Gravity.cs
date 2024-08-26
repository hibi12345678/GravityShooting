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

    public float groundCheckDistance = 0.001f; // �n�ʂƂ̋������`�F�b�N���钷��
    public LayerMask groundLayer;           // �n�ʂ̃��C���[
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
            
            // �f���̒��S�Ɍ������ăL�����N�^�[��������
            Vector3 direction = (transform.position - planet.position).normalized;
            if (isGrounded == false)
            {
                Vector3 gravityDirection = (transform.position - planet.position).normalized;

                rb.AddForce(-gravityDirection * gravity, ForceMode.Acceleration);
            }
            

            // �W�����v�̓��͂��`�F�b�N
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {

                 rb.AddForce(direction * 250 * 1.0f, ForceMode.Acceleration);

                 

            }

           

            
        }

        

        // �n�ʂɐڐG���Ă��邩�ǂ�����Raycast�Ŕ���
        isGrounded = Physics.Raycast(obj.transform.position, planet.position - transform.position, groundCheckDistance, groundLayer);

        

        // �W�����v�Ȃǂ̏��������s
        if (isGrounded)
        {
            rb.velocity = Vector3.zero;

        }



    }

}
