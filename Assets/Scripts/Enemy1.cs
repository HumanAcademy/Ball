using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Enemy1 : Enemy
{
    [SerializeField] Sprite[] walkSprites;
    SpriteRenderer sr = null;
    Rigidbody2D rb = null;
    BoxCollider2D bc = null;
    int walkSpriteIndex = 0;
    float animationTimer = 0f;

    protected override void Start()
    {
        base.Start();

        sr = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        bc = this.GetComponent<BoxCollider2D>();
        //speed = -Mathf.Abs(speed);
    }

    protected override void Update()
    {
        base.Update();

        animationTimer += Time.deltaTime;

        if (animationTimer > 0.1f)
        {
            animationTimer -= 0.1f;

            sr.sprite = walkSprites[walkSpriteIndex];

            walkSpriteIndex++;
            if (walkSpriteIndex > walkSprites.Length - 1)
                walkSpriteIndex = 0;
        }

        //Vector2 velocity = rb.velocity;

        //velocity.x += speed;
        //velocity.x *= 0.9f;

        //rb.velocity = velocity;
    }

    //protected override void OnCollisionStay2D(Collision2D collision)
    //{
    //    base.OnCollisionStay2D(collision);

    //    foreach (var contact in collision.contacts)
    //    {
    //        if (contact.normal.x >= 0.7f)
    //        {
    //            speed = Mathf.Abs(speed);
    //            sr.flipX = true;
    //        }
    //        if (contact.normal.x <= -0.7f)
    //        {
    //            speed = -Mathf.Abs(speed);
    //            sr.flipX = false;
    //        }

    //        Player player = contact.collider.gameObject.GetComponent<Player>();
    //        if (player != null)
    //        {
    //            if (player.isFire)
    //            {
    //                rb.constraints = RigidbodyConstraints2D.None;
    //                rb.velocity = new Vector2(Random.Range(-3f, 3f), 3f);
    //                rb.angularVelocity = 1000f;
    //                bc.enabled = false;
    //                Destroy(this);
    //                Destroy(this.gameObject, 5f);
    //                continue;
    //            }

    //            if (contact.normal.y <= -0.7f)
    //            {
    //                this.transform.localScale = new Vector3(1f, 0.25f, 1f);
    //                contact.rigidbody.velocity += new Vector2(0f, 5f);
    //                Destroy(this);
    //                Destroy(this.gameObject, 5f);
    //            }

    //            if (contact.normal.x >= 0.7f || contact.normal.x <= -0.7f)
    //            {
    //                if (player.form == Player.Form.Big)
    //                    player.form = Player.Form.Normal;
    //                else
    //                    Destroy(player.gameObject);
    //            }
    //        }
    //    }
    //}
}
