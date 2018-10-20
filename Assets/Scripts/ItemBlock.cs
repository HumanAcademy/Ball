using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ItemBlock : MonoBehaviour
{
    [SerializeField] GameObject item;
    [SerializeField] AudioClip itemSound;

    [HideInInspector] public AudioSource audioSource = null;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y >= 0.7f)
            {
                GameObject prefab = Instantiate(item);
                prefab.transform.position = this.transform.position + new Vector3(0f, 1f, 0f);
                prefab.GetComponent<Rigidbody2D>().velocity = new Vector2(3f, 3f);
                audioSource.PlayOneShot(itemSound);
                Destroy(this);
            }
        }
    }
}
