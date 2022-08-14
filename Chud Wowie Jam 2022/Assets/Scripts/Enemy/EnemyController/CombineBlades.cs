using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineBlades : MonoBehaviour
{
    public AudioSource bladeSound;

    void OnTriggerEnter2D(Collider2D collider)
    {
        string tag = collider.gameObject.tag;
        if (tag == "Sheep")
        {
            collider.gameObject.GetComponent<SheepController>().Die();
            playBladeSound();
        }
        else if (tag == "Enemy")
        {
            collider.gameObject.GetComponent<EnemyController>().Die();
            playBladeSound();
        }
    }

    void playBladeSound()
    {
        if (bladeSound.isPlaying)
        {
            bladeSound.PlayOneShot(bladeSound.clip, Random.Range(0.5f, 0.75f));
        }
        else
        {
            bladeSound.pitch = Random.Range(0.9f, 1.1f);
            bladeSound.Play();
        }
    }
}
