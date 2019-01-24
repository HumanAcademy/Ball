using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(ParticleSystem))]
public class Player : Character
{
    public enum Form
    {
        Normal,
        Big,
    }

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jump;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite bigSprite;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip stageAudio;
    [SerializeField] private AudioClip fireAudio;

    [HideInInspector] public CircleCollider2D circleCollider2D = null;
    [HideInInspector] public ParticleSystem particleSystem = null;

    private float fireTimer = 0f;

    private Form _form = Form.Normal;
    public Form form
    {
        get
        {
            return _form;
        }
        set
        {
            _form = value;

            ParticleSystem.ShapeModule shape = particleSystem.shape;
            switch (_form)
            {
                case Form.Normal:
                    circleCollider2D.radius = 0.5f;
                    spriteRenderer.sprite = normalSprite;
                    shape.radius = 0.6f;
                    break;

                case Form.Big:
                    circleCollider2D.radius = 0.75f;
                    spriteRenderer.sprite = bigSprite;
                    shape.radius = 0.8f;
                    break;
            }
        }
    }

    private bool _isFire = false;

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
                spriteRenderer.color = new Color(0.5f, 0.15f, 0f);
                rigidbody2D.mass = 1000f;
                particleSystem.Play();
                SetAudio(fireAudio);
                fireTimer = 10f;
            }
            else
            {
                spriteRenderer.color = Color.white;
                rigidbody2D.mass = 1f;
                particleSystem.Stop();
                SetAudio(stageAudio, true);
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        circleCollider2D = this.GetComponent<CircleCollider2D>();
        particleSystem = this.GetComponent<ParticleSystem>();

        SetAudio(stageAudio);
    }

    protected override void Update()
    {
        base.Update();

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
            rigidbody2D.angularVelocity += speed;

            if (!isGround && rigidbody2D.velocity.x > -3f)
                rigidbody2D.velocity -= new Vector2(speed * 0.001f, 0f);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.localScale = new Vector3(1f, 1f, 1f);
            rigidbody2D.angularVelocity -= speed;

            if (!isGround && rigidbody2D.velocity.x < 3f)
                rigidbody2D.velocity += new Vector2(speed * 0.001f, 0f);
        }

        if (isGround && Input.GetKeyDown(KeyCode.Z))
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jump);
            audioSource.PlayOneShot(jumpSound);
        }

        if (Input.GetKeyUp(KeyCode.Z))
            rigidbody2D.velocity -= new Vector2(0f, Mathf.Abs(rigidbody2D.velocity.y / 2f));
    }

    private void FixedUpdate()
    {
        Vector2 size = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);

        Vector3 cameraPosition = Vector3.Lerp(Camera.main.transform.position, this.transform.position, 0.1f);

        cameraPosition = new Vector3(Mathf.Max(size.x, cameraPosition.x), Mathf.Max(size.y, cameraPosition.y), 0f);
        Camera.main.transform.position = cameraPosition;
    }

    private void SetAudio(AudioClip audio, bool fade = false)
    {
        if (fade)
        {
            StartCoroutine(FadeAudio(audio));
        }
        else
        {
            audioSource.volume = 1f;
            audioSource.clip = audio;
            audioSource.Play();
        }
    }

    private IEnumerator FadeAudio(AudioClip audio)
    {
        float count = 1f;
        while (count > 0f)
        {
            count -= 0.05f;
            audioSource.volume = count;
            yield return null;
        }

        audioSource.clip = audio;
        audioSource.Play();

        while (count < 1f)
        {
            count += 0.05f;
            audioSource.volume = count;
            yield return null;
        }

        audioSource.volume = 1f;
    }
}
