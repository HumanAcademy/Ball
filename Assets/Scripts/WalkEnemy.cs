using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEnemy : Enemy
{
    [SerializeField] protected Sprite[] walkSprites;
    [SerializeField] protected float speed;

    protected int walkSpriteIndex = 0;
    protected float animationTimer = 0f;
    protected float moveDirection = -1f;

    // 移動と歩きアニメーション
    protected override void Update()
    {
        base.Update();

        Vector2 velocity = rigidbody2D.velocity;

        velocity.x += speed * moveDirection;
        velocity.x *= 0.9f;

        rigidbody2D.velocity = velocity;

        animationTimer += Time.deltaTime;

        if (animationTimer > 0.1f)
        {
            animationTimer -= 0.1f;

            spriteRenderer.sprite = walkSprites[walkSpriteIndex];

            walkSpriteIndex++;
            if (walkSpriteIndex > walkSprites.Length - 1)
                walkSpriteIndex = 0;
        }
    }

    // ステージか敵に当たったら反対に進む処理
    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);

        foreach (var contact in collision.contacts)
        {
            if (contact.collider.gameObject.layer == LayerMask.NameToLayer("Stage") ||
                contact.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (contact.normal.x > slopeThreshold.x)
                {
                    moveDirection = Mathf.Abs(moveDirection);
                    spriteRenderer.flipX = true;
                }
                if (contact.normal.x < -slopeThreshold.x)
                {
                    moveDirection = -Mathf.Abs(moveDirection);
                    spriteRenderer.flipX = false;
                }
            }
        }
    }
}
