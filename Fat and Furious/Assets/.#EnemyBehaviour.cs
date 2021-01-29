using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public float enemyLife;
    public string status;
    public float tempoDeEfeito;
    public float maxTempoEfeito;
    public float danoEfeito;
    public float velocidade;
    public int dano;
    public float impulsoAtaque;
    public float distanciaParaAtacar;
	[HideInInspector]
	public Vector2 dir;
	public Transform checaInimigo;
	public LayerMask paraQuando;

    float tempoParado = 0;
    float maxTempoParado = 1;
    float tempoParaAndar = 0;
    float maxTempoParaAndar = 1;
    bool ataquei;
    float velocidadeMax;
    float distanciaAoJogador;
    bool facingRight = true;
	bool podeAndar = true;
	Collider2D[] checaInimigoAFrente;
    Rigidbody2D rb;
    GameObject jogador;


    // Use this for initialization
    void Start()
    {
        status = "Normal";
        rb = GetComponent<Rigidbody2D>();
        jogador = GameObject.FindGameObjectWithTag("Player");
        velocidadeMax = velocidade;
    }

	//
	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (checaInimigo.position, 0.2f);
	}

    // Update is called once per frame
    void Update()
    {
        dir = transform.position - jogador.transform.position;
        Mathf.Abs(distanciaAoJogador = Vector2.Distance(transform.position, jogador.transform.position));
        Debug.Log("Distancia ao jogador: " + distanciaAoJogador);

		checaInimigoAFrente = Physics2D.OverlapCircleAll(checaInimigo.position, 0.2f, paraQuando);
		podeAndar = true;
		foreach (Collider2D col in checaInimigoAFrente) 
		{
			if(col != GetComponent<Collider2D>())
			{
				podeAndar = false;
				rb.velocity = Vector2.zero;
			}
		}

		//Fazer o inimigo virar
		if (dir.x > 0 && !facingRight)
		{
			Flip ();
		}
		else if (dir.x < 0 && facingRight)
		{
			Flip ();
		}

        //Faz andar até o player referenciado
        if ((distanciaAoJogador > distanciaParaAtacar || Mathf.Round(transform.position.y) != Mathf.Round(jogador.transform.position.y))
			&& tempoParado == 0 && podeAndar)
        {
            Vector2 velocidadeAplicada = velocidade * (-dir.normalized);


            //Movimentação do inimigo
            if (distanciaAoJogador <= distanciaParaAtacar)
            {
                velocidadeAplicada.x = 0;
            }
            rb.velocity = velocidadeAplicada;
        }
        else
        {
			//Checagem de ataque
			if (!ataquei && podeAndar)
            {
				//Tempo que o inimigo fica parado antes de voltar a se mover
                if (tempoParado < maxTempoParado)
                {
                    rb.velocity = Vector2.zero;
                    tempoParado += Time.deltaTime;
                }
				//Se não tiver que ficar parado ele ataca dependo da direção em que está virado
                else
                {
                    if (dir.x > 0)
                    {
                        rb.AddForce(new Vector2(-impulsoAtaque, 0), ForceMode2D.Impulse);
                        ataquei = true;
                    }
                    else
                    {
                        rb.AddForce(new Vector2(impulsoAtaque, 0), ForceMode2D.Impulse);
                        ataquei = true;
                    }
                }
            }
            else
            {
                if (tempoParaAndar < maxTempoParaAndar)
                {
                    tempoParaAndar += Time.deltaTime;
                }
                else
                {
                    ataquei = false;
                    tempoParado = 0;
                    tempoParaAndar = 0;
                }
            }
        }


        //Checagem de status negativos 
        if (status == "Queimando")
        {
            tempoDeEfeito += Time.deltaTime;
            if (tempoDeEfeito >= maxTempoEfeito)
            {
                status = "Normal";
            }

          