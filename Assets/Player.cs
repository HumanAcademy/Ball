using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] float walkSpeed = 50f;
    [SerializeField] float runSpeed = 100f;
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
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.angularVelocity -= speed;
        }

        if (Input.GetKeyDown(KeyCode.Z) && isGround)
        {
            rb.AddForce(new Vector2(0f, jump), ForceMode2D.Impulse);
        }
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
