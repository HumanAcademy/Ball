using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : Character
{
    protected Rigidbody2D rb = null;
    protected Collider2D c = null;

    protected virtual void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        c = this.GetComponent<Collider2D>();
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);

        foreach (var contact in collision.contacts)
        {
            Player player = contact.collider.gameObject.GetComponent<Player>();
            if (player != null)
            {
                if (player.isFire)
                {
                    rb.constraints = RigidbodyConstraints2D.None;
                    rb.velocity = new Vector2(Random.Range(-3f, 3f), 3f);
                    rb.angularVelocity = 1000f;
                    c.enabled = false;
                    Destroy(this);
                    Destroy(this.gameObject, 5f);
                    continue;
                }

                if (contact.normal.y <= -0.7f)
                {
                    this.transform.localScale = new Vector3(1f, 0.25f, 1f);
                    contact.rigidbody.velocity += new Vector2(0f, 5f);
                    Destroy(this);
                    Destroy(this.gameObject, 5f);
                }

                if (contact.normal.x >= 0.7f || contact.normal.x <= -0.7f)
                {
                    Destroy(player.gameObject);
                }
            }
        }
    }
}
