using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Character : MonoBehaviour
{
    public SpriteRenderer spriteRenderer = null;
    public Rigidbody2D rigidbody2D = null;

    protected bool isGround = false;

    protected virtual void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
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

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        isGround = false;
    }
}
