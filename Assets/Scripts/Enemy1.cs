using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy1 : Enemy
{
    [SerializeField] float speed;
    [SerializeField] Sprite[] walkSprites;
    SpriteRenderer sr = null;
    int walkSpriteIndex = 0;
    float animationTimer = 0f;

    protected override void Start()
    {
        base.Start();

        sr = this.GetComponent<SpriteRenderer>();
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

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);

        foreach (var contact in collision.contacts)
        {
            if (contact.normal.x > 0.5f)
            {
                speed = Mathf.Abs(speed);
                sr.flipX = true;
            }
            if (contact.normal.x < -0.5f)
            {
                speed = -Mathf.Abs(speed);
                sr.flipX = false;
            }
        }
    }
}
