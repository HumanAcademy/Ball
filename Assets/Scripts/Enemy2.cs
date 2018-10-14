using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Enemy2 : Character
{
    enum Mode
    {
        Normal,
        Roll,
    }

    [SerializeField] float speed;
    [SerializeField] Sprite[] walkSprites;
    [SerializeField] Sprite rollSprites;
    SpriteRenderer sr = null;
    Rigidbody2D rb = null;
    BoxCollider2D bc = null;
    CircleCollider2D cc = null;
    Mode mode = Mode.Normal;
    int walkSpriteIndex = 0;
    float animationTimer = 0f;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        bc = this.GetComponent<BoxCollider2D>();
        cc = this.GetComponent<CircleCollider2D>();
        speed = -Mathf.Abs(speed);
    }

    void Update()
    {
        if (mode == Mode.Roll)
            return;

        animationTimer += Time.deltaTime;

        if (animationTimer > 0.1f)
        {
            animationTimer -= 0.1f;

            sr.sprite = walkSprites[walkSpriteIndex];

            walkSpriteIndex++;
            if (walkSpriteIndex > walkSprites.Length - 1)
                walkSpriteIndex = 0;
        }

        Vector2 velocity = rb.velocity;

        velocity.x += speed;
        velocity.x *= 0.9f;

        rb.velocity = velocity;
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);

        foreach (var contact in collision.contacts)
        {
            if (contact.normal.x >= 0.7f)
            {
                if (mode == Mode.Roll)
                {
                    rb.velocity = new Vector2(5f, rb.velocity.y);
                    rb.angularVelocity = -1000f;
                }
                else
                {
                    speed = Mathf.Abs(speed);
                    sr.flipX = true;
                }
            }
            if (contact.normal.x <= -0.7f)
            {
                if (mode == Mode.Roll)
                {
                    rb.velocity = new Vector2(-5f, rb.velocity.y);
                    rb.angularVelocity = 1000f;
                }
                else
                {
                    speed = -Mathf.Abs(speed);
                    sr.flipX = false;
                }
            }

            Player player = contact.collider.gameObject.GetComponent<Player>();
            if (player != null)
            {
                if (player.isFire)
                {
                    rb.constraints = RigidbodyConstraints2D.None;
                    rb.velocity = new Vector2(Random.Range(-3f, 3f), 3f);
                    rb.angularVelocity = 1000f;
                    bc.enabled = false;
                    cc.enabled = false;
                    Destroy(this);
                    Destroy(this.gameObject, 5f);
                    continue;
                }

                if (contact.normal.y <= -0.7f)
                {
                    contact.rigidbody.velocity += new Vector2(0f, 5f);
                    sr.sprite = rollSprites;
                    rb.constraints = RigidbodyConstraints2D.None;
                    if (player.transform.position.x < this.transform.position.x)
                    {
                        rb.velocity = new Vector2(5f, rb.velocity.y);
                        rb.angularVelocity = -1000f;
                    }
                    else
                    {
                        rb.velocity = new Vector2(-5f, rb.velocity.y);
                        rb.angularVelocity = 1000f;
                    }
                    bc.enabled = false;
                    cc.enabled = true;
                    mode = Mode.Roll;
                }

                if (contact.normal.x >= 0.7f || contact.normal.x <= -0.7f)
                {
                    if (player.form == Player.Form.Big)
                        player.form = Player.Form.Normal;
                    else
                        Destroy(player.gameObject);
                }
                continue;
            }

            Character character = contact.collider.gameObject.GetComponent<Character>();
            if (mode == Mode.Roll && character != null)
            {
                if (contact.normal.x >= 0.7f || contact.normal.x <= -0.7f)
                {
                    contact.rigidbody.constraints = RigidbodyConstraints2D.None;
                    contact.rigidbody.velocity = new Vector2(Random.Range(-3f, 3f), 3f);
                    contact.rigidbody.angularVelocity = 1000f;
                    character.GetComponent<Collider2D>().enabled = false;
                    Destroy(character);
                    Destroy(character.gameObject, 5f);
                }
            }
        }
    }
}
