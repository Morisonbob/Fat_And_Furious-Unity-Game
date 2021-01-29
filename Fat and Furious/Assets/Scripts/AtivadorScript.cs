using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivadorScript : MonoBehaviour {

	public GameObject[] objetosInimigos;
	public GameObject[] objetosBoss;

    void Start()
    {
        objetosInimigos = GameObject.FindGameObjectsWithTag("Enemy");
        objetosBoss = GameObject.FindGameObjectsWithTag("Boss");

		/*
		foreach(GameObject obj in objetosInimigos)
        {
			gameObject.GetComponent<EnemyBehaviour> ().enabled = false;
        }

		foreach(GameObject obj in objetosBoss)
		{
			gameObject.GetComponent<GiantBoss> ().enabled = false;
		}
		*/
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
		if(collision.tag == "Enemy" && collision.gameObject.GetComponent<EnemyBehaviour>().enabled == false)
		{
			collision.gameObject.GetComponent<EnemyBehaviour> ().enabled = true;
		}

		else if(collision.tag == "Boss" && collision.gameObject.GetComponent<GiantBoss>().enabled == false)
        {
			collision.gameObject.GetComponent<GiantBoss> ().enabled = true;
		}
    }
}
