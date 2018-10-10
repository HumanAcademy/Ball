using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    [SerializeField] Sprite[] walkSprites;
    SpriteRenderer sr = null;
    int walkSpriteIndex = 0;
    float animationTimer = 0f;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        animationTimer += Time.deltaTime;

        if (animationTimer > 0.3f)
        {
            animationTimer -= 0.3f;

            sr.sprite = walkSprites[walkSpriteIndex];

            walkSpriteIndex++;
            if (walkSpriteIndex > walkSprites.Length - 1)
                walkSpriteIndex = 0;
        }
    }
}
