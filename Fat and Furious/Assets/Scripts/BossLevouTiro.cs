using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevouTiro : MonoBehaviour
{

    public AudioClip hit1;
    AudioSource audioS;

    // Use this for initialization
    void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9)
        {
            audioS.PlayOneShot(hit1);
        }
    }
}
