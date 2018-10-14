using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    protected Rigidbody2D rb = null;

    protected virtual void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.x >= 0.7f)
            {
                rb.velocity = new Vector2(3f, rb.velocity.y);
            }
            if (contact.normal.x <= -0.7f)
            {
                rb.velocity = new Vector2(-3f, rb.velocity.y);
            }
        }
    }
}
