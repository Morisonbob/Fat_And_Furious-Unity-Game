using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaBehaviour : MonoBehaviour
{
    //Variaveis que são public apenas para poderem ser acessadas dentro de
    //outros scripts
    //NÃO MUDAR ESSES VALORES NO INSPECTOR DOS GAMEOBJECT BALA!!!
    //OS VALORES DEVEM SER MUDADOS NO OBJETO JOGADOR
    [HideInInspector]
    public float velocidadeBala;
    [HideInInspector]
    public float dano;
    [HideInInspector]
    public float danoDeEfeito;
    [HideInInspector]
    public int tempoParaDestruir;
    [HideInInspector]
    public float tempoDeEfeito;
    [HideInInspector]
    public float raioDeExplosao;
    [HideInInspector]
    public float danoDeExplosao;
    [HideInInspector]
    public bool explode;
    [HideInInspector]
    public float penaDeVelocidade;
    [HideInInspector]
    public float forcaEmpurrao;
    [HideInInspector]
    public AudioClip somDoTiro;
    [HideInInspector]
    public GameObject efeitoTiro;
    [HideInInspector]
    public GameObject efeitoAtingido;

    EnemyBehaviour enemy;
    GiantBoss gBoss;
    //BossParts bPart;

    AudioSource audioS;

    void Awake()
    {
        audioS = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        if (somDoTiro != null)
        {
            audioS.PlayOneShot(somDoTiro);
        }
        
        GameObject novoEfeito = Instantiate(efeitoTiro,transform.position,Quaternion.identity);
        novoEfeito.GetComponent<ParticleSystem>().Play();

        if (transform.localScale.x > 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(velocidadeBala, 0);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-velocidadeBala, 0);
        }

        Destroy(gameObject, tempoParaDestruir);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, raioDeExplosao);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            enemy = other.GetComponent<EnemyBehaviour>();
            //Toca o efeito quando a bala atinge o inimigo
            GameObject novoEfeito = Instantiate(efeitoAtingido, transform.position, Quaternion.identity);
            novoEfeito.GetComponent<ParticleSystem>().Play();
            enemy.TomarDano(dano);
			enemy.LevarEmpurao(forcaEmpurrao, transform.position - enemy.transform.position);

            if (explode)
            {
                Collider2D[] atingidos = Physics2D.OverlapCircleAll(gameObject.transform.position, raioDeExplosao);
                foreach (Collider2D atingido in atingidos)
                {
                    if (atingido.tag == "Enemy")
                    {
                        enemy.TomarDano(danoDeExplosao);
                    }
                }
            }

            if (gameObject.tag == "Tiro De Fogo" && enemy.status == "Normal" && !enemy.imuneAFogo)
            {
                enemy.status = "Queimando";
                enemy.danoEfeito = danoDeEfeito;
                enemy.TomarDano(danoDeEfeito);
                enemy.maxTempoEfeito = tempoDeEfeito;
                enemy.tempoDeEfeito = 0;
            }
            else if (gameObject.tag == "Tiro De Gelo" && enemy.status == "Normal")
            {
                enemy.status = "Congelado";
                enemy.danoEfeito = danoDeEfeito;
                enemy.TomarDano(danoDeEfeito);
                enemy.maxTempoEfeito = tempoDeEfeito;
                enemy.tempoDeEfeito = 0;
            }
            else if (gameObject.tag == "Tiro Lento" && enemy.status == "Normal")
            {
                enemy.status = "Lento";
                enemy.danoEfeito = danoDeEfeito;
                enemy.TomarDano(danoDeEfeito);
                enemy.maxTempoEfeito = tempoDeEfeito;
                enemy.tempoDeEfeito = 0;
                enemy.penaDeVelocidade = penaDeVelocidade;
            }

            Destroy(gameObject);
        }

        //Checagem para o Boss **********************************************************

        if (other.tag == "Tiro Inimigo")
        {
            GameObject novoEfeito = Instantiate(efeitoAtingido, transform.position, Quaternion.identity);
            novoEfeito.GetComponent<ParticleSystem>().Play();
        }

        if (other.tag == "Boss")
        {
            GameObject novoEfeito = Instantiate(efeitoAtingido, transform.position, Quaternion.identity);
            novoEfeito.GetComponent<ParticleSystem>().Play();
            //Se for um pedaço do boss
            if (other.transform.parent)
            {
                gBoss = other.transform.parent.GetComponent<GiantBoss>();
                //Kako Layer
                /*
                if (other.gameObject.layer == 13)
                {
                    audioS.PlayOneShot(kakohit1);
                }
                */
            }
            //Se for a parte principal do boss
            if(other.transform.childCount > 0)
            {
                gBoss = other.GetComponent<GiantBoss>();
            }

            gBoss.TomarDanoChefe(dano);

            if (explode)
            {
                Collider2D[] atingidos = Physics2D.OverlapCircleAll(gameObject.transform.position, raioDeExplosao);
                foreach (Collider2D atingido in atingidos)
                {
                    if (atingido.tag == "Enemy")
                    {
                        gBoss.TomarDanoChefe(danoDeExplosao);
                    }
                }
            }

            if (gameObject.tag == "Tiro De Fogo" && gBoss.status == "Normal" && !gBoss.imuneAFogo)
            {
                gBoss.status = "Queimando";
                gBoss.danoEfeito = danoDeEfeito;
                gBoss.TomarDanoChefe(danoDeEfeito);
                gBoss.maxTempoEfeito = tempoDeEfeito;
                gBoss.tempoDeEfeito = 0;
            }
            else if (gameObject.tag == "Tiro De Gelo" && gBoss.status == "Normal" && !gBoss.imuneAGelo)
            {
                gBoss.status = "Congelado";
                gBoss.danoEfeito = danoDeEfeito;
                gBoss.TomarDanoChefe(danoDeEfeito);
                gBoss.maxTempoEfeito = tempoDeEfeito;
                gBoss.tempoDeEfeito = 0;
            }
            else if (gameObject.tag == "Tiro Lento" && gBoss.status == "Normal")
            {
                gBoss.danoEfeito = danoDeEfeito;
                gBoss.TomarDanoChefe(danoDeEfeito);
                gBoss.tempoDeEfeito = 0;
            }

            Destroy(gameObject);
        }
        /*
        //Checagem para pedaços dos Boss **********************************

        if (other.tag == "Boss Part")
        {
            bPart = other.GetComponent<BossParts>();
            bPart.TomarDano(dano);

            if (explode)
            {
                Collider2D[] atingidos = Physics2D.OverlapCircleAll(gameObject.transform.position, raioDeExplosao);
                foreach (Collider2D atingido in atingidos)
                {
                    if (atingido.tag == "Enemy")
                    {
                        bPart.TomarDano(danoDeExplosao);
                    }
                }
            }

            if (gameObject.tag == "Tiro De Fogo" && bPart.status == "Normal" && !bPart.imuneAFogo)
            {
                bPart.status = "Queimando";
                bPart.danoEfeito = danoDeEfeito;
                bPart.TomarDano(danoDeEfeito);
                bPart.maxTempoEfeito = tempoDeEfeito;
                bPart.tempoDeEfeito = 0;
            }
            else if (gameObject.tag == "Tiro De Gelo" && bPart.status == "Normal" && !bPart.imuneAGelo)
            {
                bPart.status = "Congelado";
                bPart.danoEfeito = danoDeEfeito;
                bPart.TomarDano(danoDeEfeito);
                bPart.maxTempoEfeito = tempoDeEfeito;
                bPart.tempoDeEfeito = 0;
            }
            Destroy(gameObject);
        }*/
    }
}
