using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    Rigidbody rb;

    public float speed = 0.25f;
    public float rotSpeed = 100f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        rb.position += new Vector3(moveX * speed, 0, moveY * speed) * 2;
    }
}
