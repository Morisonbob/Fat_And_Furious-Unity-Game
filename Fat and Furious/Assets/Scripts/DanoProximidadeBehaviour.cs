using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanoProximidadeBehaviour : MonoBehaviour
{

    public float tempoEmTela;
    public bool desaparece;
    public float forcaEmpurrao;
    public GameObject efeitoHit;

    CharController player;
    public int dano;
    float tempo;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Precisa destruir o transform junto
        if (desaparece)
        {
            if (tempo < tempoEmTela)
            {
                tempo += Time.deltaTime;
				//Mudar depois
                if (tempo >= tempoEmTela)
                {
					Destroy(transform.parent.gameObject);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8) //Layer do Jogador
        {
            player = collision.gameObject.GetComponent<CharController>();
            GameObject novoEfeito = Instantiate(efeitoHit, transform.position, Quaternion.identity);
            novoEfeito.GetComponent<ParticleSystem>().Play();
            player.LevarDano(dano);
            //Criar função que empurra o jogador
            //player.forcaEmpurrao = forcaEmpurrao;
        }
    }
}