using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : Character
{
    [SerializeField] protected float speed;

    public Collider2D collider2D;

    float moveDirection = -1f;

    protected override void Start()
    {
        base.Start();

        collider2D = this.GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        Vector2 velocity = rigidbody2D.velocity;

        velocity.x += moveDirection * speed;
        velocity.x *= 0.9f;

        rigidbody2D.velocity = velocity;
    }

    public virtual void Tread(ContactPoint2D contact)
    {
        this.transform.position += new Vector3(0f, -0.25f, 0f);
        this.transform.localScale = new Vector3(1f, 0.25f, 1f);
        contact.rigidbody.velocity = new Vector2(contact.rigidbody.velocity.x, 5f);
        Destroy(this);
        Destroy(this.gameObject, 5f);
    }

    public virtual void Knock(ContactPoint2D contact)
    {
        rigidbody2D.constraints = RigidbodyConstraints2D.None;
        rigidbody2D.velocity = new Vector2(contact.rigidbody.velocity.x, 5f);
        rigidbody2D.angularVelocity = contact.rigidbody.velocity.x * -100f;
        collider2D.enabled = false;
        Destroy(this);
        Destroy(this.gameObject, 5f);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            if (contact.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Player player = contact.collider.GetComponent<Player>();
                if (player.isFire)
                {
                    Knock(contact);
                }
                else
                {
                    if (contact.normal.y < -0.9f)
                        Tread(contact);

                    if (contact.normal.x > 0.9f || contact.normal.x < -0.9f)
                    {
                        if (player.form == Player.Form.Big)
                            player.form = Player.Form.Normal;
                        else
                            Destroy(player.gameObject);
                    }
                }
            }
        }
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);

        foreach (var contact in collision.contacts)
        {
            if (contact.collider.gameObject.layer == LayerMask.NameToLayer("Stage") ||
                contact.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (contact.normal.x > 0.9f)
                {
                    moveDirection = Mathf.Abs(moveDirection);
                    spriteRenderer.flipX = true;
                }
                if (contact.normal.x < -0.9f)
                {
                    moveDirection = -Mathf.Abs(moveDirection);
                    spriteRenderer.flipX = false;
                }
            }
        }
    }
}
