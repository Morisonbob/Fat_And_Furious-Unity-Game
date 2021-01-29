using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadaBehaviour : MonoBehaviour
{

    //Variaveis que são public apenas para poderem ser acessadas dentro de
    //outros scripts
    //NÃO MUDAR ESSES VALORES NO INSPECTOR DOS GAMEOBJECT GRANADA!!!
    //OS VALORES DEVEM SER MUDADOS NO OBJETO JOGADOR
    [HideInInspector]
    public float velocidadeGranada = 8;
    [HideInInspector]
    public float danoGranada;
    [HideInInspector]
    public float danoDeEfeito;
    [HideInInspector]
    public float tempoDeEfeito;
    [HideInInspector]
    public float raioDeExplosao;

    Collider2D[] atingidos;

    // Use this for initialization
    void Start()
    {
        if (transform.localScale.x > 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(velocidadeGranada, 0);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-velocidadeGranada, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Desenha o raio de explosão da granada
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, raioDeExplosao);
    }

	void OnTriggerEnter2D(Collider2D col)
    {
        atingidos = Physics2D.OverlapCircleAll(gameObject.transform.position, raioDeExplosao);
        foreach (Collider2D atingido in atingidos)
        {
            if (atingido.tag == "Enemy")
            {
                atingido.GetComponent<EnemyBehaviour>().enemyLife = atingido.GetComponent<EnemyBehaviour>().enemyLife - danoGranada;
                if (gameObject.tag == "Tiro De Fogo" && atingido.GetComponent<EnemyBehaviour>().status == "Normal")
                {
                    atingido.GetComponent<EnemyBehaviour>().status = "Queimando";
                    atingido.GetComponent<EnemyBehaviour>().danoEfeito = danoDeEfeito;
                    atingido.GetComponent<EnemyBehaviour>().maxTempoEfeito = tempoDeEfeito;
                    atingido.GetComponent<EnemyBehaviour>().tempoDeEfeito = 0;
                }
                else if (gameObject.tag == "Tiro De Gelo" && atingido.GetComponent<EnemyBehaviour>().status == "Normal")
                {
                    atingido.GetComponent<EnemyBehaviour>().status = "Congelado";
                    atingido.GetComponent<EnemyBehaviour>().danoEfeito = danoDeEfeito;
                    atingido.GetComponent<EnemyBehaviour>().maxTempoEfeito = tempoDeEfeito;
                    atingido.GetComponent<EnemyBehaviour>().tempoDeEfeito = 0;
                }
                if (atingido.GetComponent<EnemyBehaviour>().enemyLife <= 0)
                {
                    Destroy(atingido.gameObject);
                }
                Destroy(gameObject);
            }
			Destroy (gameObject, 3);
        }
    }
}
