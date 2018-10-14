using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(ParticleSystem))]
public class Player : MonoBehaviour
{
    enum Mode
    {
        Normal,
        Big,
        Fire,
    }

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jump;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite bigSprite;
    SpriteRenderer sr = null;
    Rigidbody2D rb = null;
    CircleCollider2D cc = null;
    ParticleSystem ps = null;
    bool isGround = false;
    Mode mode = Mode.Normal;
    float speedMultiplier = 1f;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        cc = this.GetComponent<CircleCollider2D>();
        ps = this.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        float speed = walkSpeed;
        if (Input.GetKey(KeyCode.X))
        {
            speed = runSpeed;
        }
        speed *= speedMultiplier;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.localScale = new Vector3(-1f, 1f, 1f);
            rb.angularVelocity += speed;
            if (!isGround && rb.velocity.x > -3f)
            {
                rb.velocity -= new Vector2(speed * 0.001f, 0f);
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.localScale = new Vector3(1f, 1f, 1f);
            rb.angularVelocity -= speed;
            if (!isGround && rb.velocity.x < 3f)
            {
                rb.velocity += new Vector2(speed * 0.001f, 0f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Z) && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            rb.velocity -= new Vector2(0f, Mathf.Abs(rb.velocity.y / 2f));
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.SetMode(Mode.Normal);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.SetMode(Mode.Big);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.SetMode(Mode.Fire);
        }
    }

    void FixedUpdate()
    {
        Vector2 size = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);

        Vector3 cameraPosition = Vector3.Lerp(Camera.main.transform.position, this.transform.position, 0.1f);

        cameraPosition = new Vector3(Mathf.Max(size.x, cameraPosition.x), Mathf.Max(size.y, cameraPosition.y), 0f);
        Camera.main.transform.position = cameraPosition;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isGround = false;

        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (collision.contacts[i].normal.y > 0.5f)
            {
                isGround = true;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }

    void SetMode(Mode mode)
    {
        this.mode = mode;
        ParticleSystem.ShapeModule shape = ps.shape;

        switch (mode)
        {
            case Mode.Normal:
                cc.radius = 0.5f;
                sr.sprite = normalSprite;
                sr.color = Color.white;
                shape.radius = 0.6f;
                ps.Stop();
                speedMultiplier = 1f;
                break;

            case Mode.Big:
                cc.radius = 0.75f;
                sr.sprite = bigSprite;
                sr.color = Color.white;
                shape.radius = 0.8f;
                ps.Stop();
                speedMultiplier = 1f;
                break;

            case Mode.Fire:
                sr.color = new Color(0.5f, 0.15f, 0f);
                ps.Play();
                speedMultiplier = 3f;
                break;
        }
    }
}
