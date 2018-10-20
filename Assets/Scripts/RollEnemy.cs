using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class RollEnemy : WalkEnemy
{
    [SerializeField] Sprite rollSprite;
    [SerializeField] CircleCollider2D circleCollider2D;
    [SerializeField] AudioClip collisionSound;

    private bool _isRolling = false;
    protected bool isRolling
    {
        get
        {
            return _isRolling;
        }
        set
        {
            _isRolling = value;

            if (_isRolling)
            {
                spriteRenderer.sprite = rollSprite;
                rigidbody2D.mass = 100f;
                rigidbody2D.constraints = RigidbodyConstraints2D.None;
                collider2D.enabled = false;
                circleCollider2D.enabled = true;
            }
            else
            {
                this.gameObject.layer = LayerMask.NameToLayer("Enemy");
                rigidbody2D.mass = 1f;
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                collider2D.enabled = true;
                circleCollider2D.enabled = false;
            }
        }
    }

    // 回転状態になったら勝手に転がるので
    // アニメーションと移動をしないようにする
    protected override void Update()
    {
        if (isRolling)
        {
            if (Mathf.Abs(rigidbody2D.velocity.x) > 0.1f)
                this.gameObject.layer = LayerMask.NameToLayer("Object");
            else
                this.gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
        else
        {
            base.Update();
        }
    }

    // 踏まれた時の処理
    public override void Tread(ContactPoint2D contact)
    {
        // BoxColliderからCircleColliderに切り替えたら時に
        // CircleColliderにプレイヤーがめり込んでこの関数が2回呼ばれ
        // 1回踏んだだけで転がってしまうのでプレイヤーをちょっと上に移動して
        // CircleColliderにめり込まないようにする
        if (!isRolling)
            contact.collider.transform.position += new Vector3(0f, 0.5f);

        isRolling = true;

        contact.rigidbody.velocity = new Vector2(contact.rigidbody.velocity.x, 5f);

        if (Mathf.Abs(rigidbody2D.velocity.x) < 0.1f)
        {
            float rollDirection = Mathf.Sign(this.transform.position.x - contact.collider.transform.position.x);
            rigidbody2D.velocity = new Vector2(10f * rollDirection, rigidbody2D.velocity.y);
        }
        else
        {
            rigidbody2D.velocity = new Vector2(0f, rigidbody2D.velocity.y);
            rigidbody2D.angularVelocity = 0f;
        }

        audioSource.PlayOneShot(treadSound);
    }

    // 吹っ飛んだ時にCircleColliderを無効にする
    public override void Knock(ContactPoint2D contact)
    {
        base.Knock(contact);

        circleCollider2D.enabled = false;
    }

    // 回転状態で回転してないときプレイヤーを消さない
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (isRolling && Mathf.Abs(rigidbody2D.velocity.x) < 0.1f)
        {
            foreach (var contact in collision.contacts)
            {
                if (contact.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    Tread(contact);
                }
            }
        }
        else
        {
            base.OnCollisionEnter2D(collision);
        }
    }

    // 回転状態の時、壁に当たったら跳ね返す
    protected override void OnCollisionStay2D(Collision2D collision)
    {
        if (isRolling)
        {
            foreach (var contact in collision.contacts)
            {
                if (contact.normal.x > slopeThreshold.x)
                {
                    rigidbody2D.velocity = new Vector2(10f, rigidbody2D.velocity.y);
                    audioSource.PlayOneShot(collisionSound);
                }
                if (contact.normal.x < -slopeThreshold.x)
                {
                    rigidbody2D.velocity = new Vector2(-10f, rigidbody2D.velocity.y);
                    audioSource.PlayOneShot(collisionSound);
                }
            }
        }
        else
        {
            base.OnCollisionStay2D(collision);
        }
    }
}
