using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(ParticleSystem))]
public class Player : Character
{
    public enum Form
    {
        Normal,
        Big,
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
    float fireTimer = 0f;

    Form _form = Form.Normal;
    public Form form
    {
        get
        {
            return _form;
        }
        set
        {
            _form = value;

            ParticleSystem.ShapeModule shape = ps.shape;
            switch (_form)
            {
                case Form.Normal:
                    cc.radius = 0.5f;
                    sr.sprite = normalSprite;
                    shape.radius = 0.6f;
                    break;

                case Form.Big:
                    cc.radius = 0.75f;
                    sr.sprite = bigSprite;
                    shape.radius = 0.8f;
                    break;
            }
        }
    }

    bool _isFire;
    public bool isFire
    {
        get
        {
            return _isFire;
        }
        set
        {
            _isFire = value;

            if (_isFire)
            {
                sr.color = new Color(0.5f, 0.15f, 0f);
                ps.Play();
                fireTimer = 10f;
            }
            else
            {
                sr.color = Color.white;
                ps.Stop();
                fireTimer = 0f;
            }
        }
    }

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
            speed = runSpeed;

        if (isFire)
        {
            speed *= 3f;

            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
                isFire = false;
        }

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
    }

    void FixedUpdate()
    {
        Vector2 size = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);

        Vector3 cameraPosition = Vector3.Lerp(Camera.main.transform.position, this.transform.position, 0.1f);

        cameraPosition = new Vector3(Mathf.Max(size.x, cameraPosition.x), Mathf.Max(size.y, cameraPosition.y), 0f);
        Camera.main.transform.position = cameraPosition;
    }
}
