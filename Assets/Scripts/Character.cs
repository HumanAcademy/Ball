using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Character : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer spriteRenderer = null;
    [HideInInspector] public Rigidbody2D rigidbody2D = null;

    protected readonly Vector2 slopeThreshold = new Vector2(Mathf.Sin(50f * Mathf.Deg2Rad), 1f - Mathf.Sin(50f * Mathf.Deg2Rad));
    protected bool isGround = false;

    protected virtual void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        //rigidbody2D.mass = rigidbody2D.velocity.sqrMagnitude;
    }

    // 着地判定
    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        isGround = false;

        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (collision.contacts[i].normal.y > slopeThreshold.y)
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
