using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjetilInimigoBehaviour : MonoBehaviour
{
    //Variaveis que são public apenas para poderem ser acessadas dentro de
    //outros scripts
    //NÃO MUDAR ESSES VALORES NO INSPECTOR DOS GAMEOBJECT PROJETIL!!!
    //OS VALORES DEVEM SER MUDADOS NO OBJETO INIMIGO
    [HideInInspector]
    public float velocidadeProjetil;
    [HideInInspector]
    public int danoProjetil;
    [HideInInspector]
    public float danoDeEfeitoDoProjetil;
    [HideInInspector]
    public int tempoParaDestruirProjetil;
    [HideInInspector]
    public float tempoDeEfeitoDoProjetil;
    [HideInInspector]
    public float chance;
    [HideInInspector]
    public float forcaEmpurrao;
    [HideInInspector]
    public bool podeAtordoar;
    [HideInInspector]
    public bool podeQueimar;
    [HideInInspector]
    public bool empuraOJogador;
    [HideInInspector]
    public bool podeCongelar;
    [HideInInspector]
    public GameObject efeitoHit;
    public float rand;

    [HideInInspector]
    public Vector3 dir;

    CharController player;

    // Use this for initialization
    void Start()
    {
        var jogador = GameObject.FindGameObjectWithTag("Player");
        dir = transform.position - jogador.transform.position;

        if (dir.x < 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(velocidadeProjetil, 0);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-velocidadeProjetil, 0);
        }

		if (transform.parent != null) {
			Destroy (transform.parent.gameObject, tempoParaDestruirProjetil);
		} else {
			Destroy (gameObject, tempoParaDestruirProjetil);
		}
    }

    // Update is called once per frame
    void Update()
    {
        rand = Random.Range(1, 100);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 8) //Layer do Jogador
        {
            player = other.gameObject.GetComponent<CharController>();
            player.LevarDano(danoProjetil);
            GameObject novoEfeito = Instantiate(efeitoHit, transform.position, Quaternion.identity);
            novoEfeito.GetComponent<ParticleSystem>().Play();
            //player.LevarEmpurao(forcaEmpurrao);

            if (podeCongelar)
            {
                if (rand <= chance)
                {
                    if (player.status == "Normal")
                    {
                        player.status = "Congelado";
                        player.maxTempoEfeito = tempoDeEfeitoDoProjetil;
                        player.danoEfeito = danoDeEfeitoDoProjetil;
                    }
                }
            }
            else if (podeQueimar)
            {
                if (rand <= chance)
                {
                    if (player.status == "Normal")
                    {
                        player.status = "Queimado";
                        player.maxTempoEfeito = tempoDeEfeitoDoProjetil;
                        //Como nesse caso o dano é por tempo, é necessário passar o dano de efeito dessa dorma
                        player.danoEfeito = danoDeEfeitoDoProjetil;
                    }
                }
            }
            else if (podeAtordoar)
            {
                if (rand <= chance)
                {
                    if (player.status == "Normal")
                    {
                        player.status = "Atordoado";
                        player.maxTempoEfeito = tempoDeEfeitoDoProjetil;
                        player.danoEfeito = danoDeEfeitoDoProjetil;
                    }
                }
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == 9) //Layer das balas inimigas
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
