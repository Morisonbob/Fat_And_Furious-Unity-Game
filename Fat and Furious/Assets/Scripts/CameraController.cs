using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public Transform leftEdge;
    public Transform finalDaFase;
    Vector3 distancia;

	// Use this for initialization
	void Start () {
        distancia = transform.position - player.transform.position;
    }

    //Atualiza após cada movimento ter sido feito
    void LateUpdate () { 
		transform.position = new Vector3(Mathf.Clamp(player.transform.position.x + distancia.x,leftEdge.position.x, finalDaFase.position.x),
            transform.position.y, transform.position.z);
        
    }
}
