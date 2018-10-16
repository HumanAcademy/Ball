using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Enemy : Character
{
    [HideInInspector] public Collider2D collider2D;

    protected override void Start()
    {
        base.Start();

        collider2D = this.GetComponent<Collider2D>();
    }

    // 踏まれた時の処理
    public virtual void Tread(ContactPoint2D contact)
    {
        this.transform.position += new Vector3(0f, -0.25f, 0f);
        this.transform.localScale = new Vector3(1f, 0.25f, 1f);
        contact.rigidbody.velocity = new Vector2(contact.rigidbody.velocity.x, 5f);
        Destroy(this);
        Destroy(this.gameObject, 5f);
    }

    // 吹っ飛ばされた時の処理
    public virtual void Knock(ContactPoint2D contact)
    {
        Debug.Log("Knock");
        rigidbody2D.constraints = RigidbodyConstraints2D.None;
        rigidbody2D.velocity = new Vector2(contact.rigidbody.velocity.x, 5f);
        rigidbody2D.angularVelocity = contact.rigidbody.velocity.x * -500f;
        collider2D.enabled = false;
        Destroy(this);
        Destroy(this.gameObject, 5f);
    }

    // プレイヤーに当たった時の処理
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
                    if (contact.normal.y < -slopeThreshold.y)
                        Tread(contact);

                    if (contact.normal.x > slopeThreshold.x || contact.normal.x < -slopeThreshold.x)
                    {
                        if (player.form == Player.Form.Big)
                            player.form = Player.Form.Normal;
                        else
                            Destroy(player.gameObject);
                    }
                }
            }

            if (contact.collider.gameObject.layer == LayerMask.NameToLayer("Object"))
            {
                if (contact.rigidbody.velocity.sqrMagnitude > Mathf.Pow(10f, 2f))
                    Knock(contact);
            }
        }
    }
}
