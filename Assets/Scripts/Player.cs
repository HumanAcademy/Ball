using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jump;
    Rigidbody2D rb = null;
    bool isGround = false;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float speed = walkSpeed;
        if (Input.GetKey(KeyCode.X))
        {
            speed = runSpeed;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.angularVelocity += speed;
            if (!isGround && rb.velocity.x > -3f)
            {
                rb.velocity -= new Vector2(speed * 0.001f, 0f);
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.angularVelocity -= speed;
            if (!isGround && rb.velocity.x < 3f)
            {
                rb.velocity += new Vector2(speed * 0.001f, 0f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Z) && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            rb.velocity -= new Vector2(0f, Mathf.Abs(rb.velocity.y / 2f));
        }
    }

    void FixedUpdate()
    {
        Vector2 size = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);

        Vector3 cameraPosition = Vector3.Lerp(Camera.main.transform.position, this.transform.position, 0.1f);

        cameraPosition = new Vector3(Mathf.Max(size.x, cameraPosition.x), Mathf.Max(size.y, cameraPosition.y), 0f);
        Camera.main.transform.position = cameraPosition;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isGround = false;

        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (collision.contacts[i].normal.y > 0.5f)
            {
                isGround = true;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }
}
