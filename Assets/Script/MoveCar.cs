using UnityEngine;

public class MoveCar : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float rotationSpeed;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    float vertical;

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) > 0.1)
        {
            transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * vertical * speed);
        }
    }
}