using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Classe que contém todos os atributos referentes as balas
//Serializable faz com que a classe possa ser vista
[System.Serializable]
public class Balas
{
    public string nomeBala;
    public string efeito;
    public float fireRate;
    public bool equipado;
    public int maxBullets;
    public int Bullets;
    public GameObject bala;

    //Variavies que irão sobreescrever os valores dentro do script 'BalaBehaviour'
    //Apenas por motivos de organização no inspector
    public float velocidadeBala;
    public float dano;
    public float danoDeEfeito;
    public int tempoParaDestruir;
    public float tempoDeEfeito;
}


[System.Serializable]
public class Granadas
{
    public string nomeGranada;
    public string efeito;
    //Fire rate de todas as granadas vai ser igual
    //public float fireRate;
    public bool equipado;
    public int maxBullets;
    public int Bullets;
    public GameObject granada;

    //Variavies que irão sobreescrever os valores dentro do script 'GranadaBehaviour'
    //Apenas por motivos de organização no inspector
    //Velocidade da granada vai ser igual para todas
    //public float velocidadeGranada;
    public float danoGranada;
    public float danoDeEfeito;
    public int tempoParaDestruir;
    public float tempoDeEfeito;
    public float raioDeExplosao;
}

public class CharController : MonoBehaviour
{

    public float velocidadeJogador;
    public Transform posicaoBala;
    public int vidaJogador;
    public float frameDeInvencibilidade;


    float moveX;
    float moveY;
    float nextFire;
    float nextGranada;
    Rigidbody2D rb;

    public Balas[] arrayBalas;
    public Granadas[] arrayGranadas;

    bool facingRight = true;

    // Use this for initialization
    void Start()
    {
        //Começa equipado com a arma padrão
        arrayBalas[0].equipado = true;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(velocidadeJogador * moveX, rb.velocity.y);
        rb.velocity = new Vector2(rb.velocity.x, velocidadeJogador * moveY);

        if (moveX < 0 && facingRight)
        {
            Flip();
        }
        else if (moveX > 0 && !facingRight)
        {
            Flip();
        }


        //chacagem de que bala o jogador vai atirar
        //Checar classe "bala"
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Checa qual arma está equipada, se a arma ainda está no tempo de refresh do tiro
            //e se existem balas para atirar
            foreach (Balas b in arrayBalas)
            {
                if (b.equipado && Time.time > nextFire && b.Bullets != 0)
                {
                    nextFire = Time.time + b.fireRate;
                    GameObject novaBala = (GameObject)Instantiate(b.bala, posicaoBala.position, Quaternion.identity);
                    //Coloca os valores que estão no INSPECTOR DO JOGADOR NA AREA DE ARRAY BALAS
                    //dentro do script 'BalaBehaviour'
                    novaBala.GetComponent<BalaBehaviour>().dano = b.dano;
                    novaBala.GetComponent<BalaBehaviour>().danoDeEfeito = b.danoDeEfeito;
                    novaBala.GetComponent<BalaBehaviour>().velocidadeBala = b.velocidadeBala;
                    novaBala.GetComponent<BalaBehaviour>().tempoParaDestruir = b.tempoParaDestruir;
                    novaBala.GetComponent<BalaBehaviour>().tempoDeEfeito = b.tempoDeEfeito;
                    novaBala.transform.localScale = transform.localScale;
                    b.Bullets--;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            foreach (Granadas g in arrayGranadas)
            {
                if (g.equipado && Time.time > nextGranada && g.Bullets != 0)
                {
                    nextGranada = Time.time + 1;
                    GameObject novaGranada = (GameObject)Instantiate(g.granada, posicaoBala.position, Quaternion.identity);
                    novaGranada.GetComponent<GranadaBehaviour>().danoGranada = g.danoGranada;
                    novaGranada.GetComponent<GranadaBehaviour>().danoDeEfeito = g.danoDeEfeito;
                    novaGranada.GetComponent<GranadaBehaviour>().tempoDeEfeito = g.tempoDeEfeito;
                    novaGranada.GetComponent<GranadaBehaviour>().raioDeExplosao = g.raioDeExplosao;
                    novaGranada.transform.localScale = transform.localScale;
                }
            }

        }

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 novaEscala = transform.localScale;
        novaEscala.x *= -1;
        transform.localScale = novaEscala;
    }

    public void LevarDano(int dano)
    {
        if (Time.time > frameDeInvencibilidade)
        {
            vidaJogador -= dano;
        }

    }
}
