using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarreiraDeProjetil : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            collision.gameObject.SetActive(false);
        }
    }
}
