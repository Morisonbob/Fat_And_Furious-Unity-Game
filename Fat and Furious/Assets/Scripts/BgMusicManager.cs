using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMusicManager : MonoBehaviour
{

    public AudioClip clip1;
    public AudioClip clip2;

    bool jaMudou;

    AudioSource audioS;

    // Use this for initialization
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        audioS.PlayOneShot(clip1, 1f);
        audioS.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioS.isPlaying && !jaMudou)
        {
            audioS.PlayOneShot(clip1, 1f);
            audioS.loop = true;
        }
        else if (!audioS.isPlaying && jaMudou)
        {
            audioS.PlayOneShot(clip2, 0.8f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !jaMudou)
        {
            audioS.Stop();
            jaMudou = true;
            audioS.PlayOneShot(clip2, 0.8f);
        }
    }
}
