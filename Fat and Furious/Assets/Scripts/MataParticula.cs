using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MataParticula : MonoBehaviour {

    public float tempoParaSumir;
    GameObject jogador;

    void Awake()
    {
        jogador = GameObject.Find("Jogador");
    }

    // Use this for initialization
    void Start () {

        if (jogador.transform.localScale.x < 0)
        {
            transform.rotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, 180);
            //transform.localRotation.Set(180, transform.localRotation.y, 180, transform.localRotation.w);
            //transform.Rotate(new Vector3(transform.localRotation.x, transform.localRotation.y, 180), Space.Self);
        }
    }
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject,tempoParaSumir);
	}
}
