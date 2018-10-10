using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy1 : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Sprite[] walkSprites;
    SpriteRenderer sr = null;
    Rigidbody2D rb = null;
    int walkSpriteIndex = 0;
    float animationTimer = 0f;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
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

    void OnCollisionStay2D(Collision2D collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (collision.contacts[i].collider.tag == "Player")
            {
                if (collision.contacts[i].normal.y < -0.5f)
                {
                    this.transform.localScale = new Vector3(1f, 0.25f, 1f);
                    collision.contacts[i].rigidbody.velocity += new Vector2(0f, 2f);
                    Destroy(this);
                }
            }
            else
            {
                if (collision.contacts[i].normal.x > 0.5f)
                {
                    speed = Mathf.Abs(speed);
                    sr.flipX = true;
                }
                if (collision.contacts[i].normal.x < -0.5f)
                {
                    speed = -Mathf.Abs(speed);
                    sr.flipX = false;
                }
            }
        }
    }

}
