using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    [Tooltip("Faz com que ele seja imune ao status de 'Queimando'")]
    public bool imuneAFogo;
    [Tooltip("Atira algo no jogador, esse projetil tem seus status e atributos alterados no prefab do projeto." +
        "Habilidades que estão inclusas nessa categoria são: 'Ataque do Desespero', 'Selfie', 'Jalapeño', 'Descarga Eletrica'" +
        "e 'Jogar Sorvete'")]
    public bool usaProjetil;
    [Tooltip("Ataque que se ativa depois que o zumbi morre, exemplo: 'Isso não devia ser um ataque'")]
    public bool usaScooter;
    [Tooltip("Ataque básico de todos os zumbis. Habilidades que estão inclusas nessa categoria são: " +
        "'Mordida', 'Comidaaaa', 'Arriba!' e 'Fome além do alcance'")]
    public bool mordida;
    [Tooltip("Variação de mordida que congela o jogador.")]
    public bool mordidaGelada;
    [Tooltip("Skill que ativa quando o inimigo morre, dando dano a todos ao redor.")]
    public bool explosaoDeSabores;
    [Tooltip("Ataque físico usando uma arma branca. Habilidades que estão inclusas nessa categoria são: "
        + "'Espada de baunilha', 'Plástico justiceiro' e 'Mjölnir'")]
    public bool ataqueFisico;
    [Tooltip("Desmarque essa opção caso queira que o inimigo siga o jogador apenas no X.")]
    public bool segueXY;
    [Tooltip("Se o inimigo dropa ou não uma bala ao morrer. ")]
    public bool dropItem;


    public GameObject drop;

    public Transform checaInimigo;
    public LayerMask paraQuando;
    public float enemyLife;
    public float maxEnemyLife;
    public string status;
    public float tempoDeEfeito;
    public float maxTempoEfeito;
    public float danoEfeito;
    public float penaDeVelocidade;
    public float velocidade;
    public int dano;
    public float impulsoAtaque;
    public float distanciaParaAtacar;
    [HideInInspector]
    public Vector2 dir;
    //Números aleatórios
    public float chanceEfeito;
    public float aleatorio;

    //Chance do ataque causar status negativo
    [Tooltip("Chance de se acertar um status negativo, tanto por projetil, " +
        "quanto por ataque que tenha esse atributo. Varia de 0 a 100")]
    public float chance;
    [Tooltip("Chance de usar um ataque fisico, dá um fator de aleatóriedade aos ataques.")]
    public float chanceAtaqueF;
    public float danoDaExplosao;
    public float tempoDeEspera;
    public float raioDeExplosao;
    public float drawBack;
    public float maxTempoParado;

    //Coisas relacionadas aos projeteis
    public ProjetilInimigo projetilScript;
    ProjetilInimigo projetil;

    //public Projeteis projetil;
    public Transform posicaoProjetil;
    public Transform mostrarDanoSofrido;
    public Text danoSofrido;
    public float distanciaMaxima;
    public float porcentagemProjetil;
    public float maxTempEfeitoJogador;

    public AudioClip grunhindo;
    public GameObject efeitoMorte;
    public GameObject efeitoAtaque;

    float tempoParado = 0;
    float tempoParaAndar = 0;
    float maxTempoParaAndar = 2;
    bool ataquei;
    float velocidadeMax;
    float distanciaAoJogador;
    bool facingRight = false;
    bool podeAndar = true;
    Collider2D[] checaInimigoAFrente;
    Rigidbody2D rb;
    GameObject jogador;
    CharController jogadorScript;
    float danoRecebido;
    float temp = 0;
    float esperaUmSegundo;
    Animator anim;

    AudioSource audioS;

    //Coisa pro checa status
    int count = 1;

    //Coisa pra animação não bugar
    int animCount = 0;

    // Use this for initialization
    void Start()
    {
        status = "Normal";
        //Trick one, como está sendo usado um prefab para varios inimigos e se fazendo operações com ele
        //não se quer que o prefab mude, por isso é necessário trabalhar com uma cópia dele, porém um a=b
        //não é uma cópia e sim uma referencia, para uma copia mesmo em um novo local de memória é necessário
        //nesse caso usar um instanciate
        projetil = Instantiate(projetilScript);
        rb = GetComponent<Rigidbody2D>();
        jogador = GameObject.FindGameObjectWithTag("Player");
        jogadorScript = jogador.GetComponent<CharController>();
        velocidadeMax = velocidade;
        audioS = GetComponent<AudioSource>();
        maxEnemyLife = enemyLife;
        anim = GetComponent<Animator>();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(checaInimigo.position, 0.2f);
        Gizmos.DrawWireSphere(gameObject.transform.position, raioDeExplosao);
    }


    // Update is called once per frame
    void Update()
    {
        

        //danoRecebido = maxEnemyLife - enemyLife;
        dir = transform.position - jogador.transform.position;
        Mathf.Abs(distanciaAoJogador = Vector2.Distance(transform.position, jogador.transform.position));
        //Debug.Log("Distancia ao jogador: " + distanciaAoJogador);

        checaInimigoAFrente = Physics2D.OverlapCircleAll(checaInimigo.position, 0.2f, paraQuando);


        //Fazendo com que seja gerado só um random por segundo
        esperaUmSegundo += Time.deltaTime;

        if (esperaUmSegundo >= 1)
        {
            chanceEfeito = Random.Range(1, 100);
            aleatorio = Random.Range(1, 100);
            esperaUmSegundo = 0;
        }

        //Gera um número aleatório de forma constante
        //chanceEfeito = Random.Range(1, 100);
        //aleatorio = Random.Range(1, 100); ;

        if (distanciaAoJogador >= distanciaMaxima)
        {
            podeAndar = false;
        }
        else
        {
            podeAndar = true;
        }


        foreach (Collider2D col in checaInimigoAFrente)
        {
            if (col != GetComponent<Collider2D>())
            {
                podeAndar = false;
                rb.velocity = Vector2.zero;
            }
        }

        //Fazer o inimigo virar
        if (dir.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (dir.x < 0 && facingRight)
        {
            Flip();
        }


        //Faz andar até o player referenciado
        if ((distanciaAoJogador > distanciaParaAtacar || Mathf.Round(transform.position.y) != Mathf.Round(jogador.transform.position.y))
            && tempoParado == 0 && podeAndar)
        {
            Vector2 velocidadeAplicada = velocidade * (-dir.normalized);

            //Toca o efeito de grunhindo
            if (!audioS.isPlaying)
            {
                audioS.Play();
            }

            //Para no eixo x para alinhar no eixo y
            if (distanciaAoJogador <= distanciaParaAtacar && segueXY)
            {
                velocidadeAplicada.x = 0;
            }
            rb.velocity = velocidadeAplicada;
        }

        else
        {
            //Checagem de ataque
            if (!ataquei)
            {
                if (podeAndar)
                {
                    //Tempo que o inimigo fica parado antes de voltar a se mover
                    if (tempoParado < maxTempoParado)
                    {
                        rb.velocity = Vector2.zero;
                        tempoParado += Time.deltaTime;
                    }
                    //Se não tiver que ficar parado ele ataca dependo da direção em que está virado
                    //Se tiver com 50% ou menos de vida, dá um ataque especial, mas só uma vez
                    else if (enemyLife <= ((maxEnemyLife) * porcentagemProjetil) && projetil.bullets > 0 && usaProjetil && status!= "Congelado")
                    {
                        LancaProjetil(projetil, posicaoProjetil);
                    }
                    else if (ataqueFisico && aleatorio <= chanceAtaqueF)
                    {
                        AtaqueFisico();
                    }
                    //Se não tiver que ficar parado ele ataca com uma mordida
                    else
                    {
                        if ((mordida || mordidaGelada) && !ataquei)
                        {
                            Mordida();
                        }
                    }
                }
            }
            else
            {
                EsperaParaAndar();
            }
        }

        //Skill de morte
        if (explosaoDeSabores)
        {
            ExplosaoDeSabores();
        }
        else if (usaScooter)
        {
            NotAAttack();
        }

        //Checagem de status negativos 
        ChecaStatus();

    }

    /*
     * 
     * FUNÇÕES DA CLASSE
     * 
     */


    //Vira o personagem e todos os seus parents
    //checar exemplo de usagem nesse código
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 novaEscala = transform.localScale;
        novaEscala.x *= -1;
        transform.localScale = novaEscala;
    }


    //O nome explica o que ela faz, pelo amor de deus
    void AplicarForcaParaTras(float forcaAplicada)
    {
        if (dir.x > 0)
        {
            rb.AddForce(new Vector2(forcaAplicada, 0), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(-forcaAplicada, 0), ForceMode2D.Impulse);
        }
    }

    //Confere se esta sob efeito de algum estado negativo e aplica seu efeito
    void ChecaStatus()
    {
        if (status == "Queimando")
        {
            tempoDeEfeito += Time.deltaTime;
            if (animCount == 0)
            {
                anim.SetBool("Fogo", true);
                animCount++;
            }
            
            //danoSofrido.text = "Queimando";
            if (tempoDeEfeito >= maxTempoEfeito)
            {
                anim.SetBool("Fogo", false);
                status = "Normal";
                animCount = 0;
            }
            //Levando dano por tempo
            enemyLife -= danoEfeito * Time.deltaTime;
            if (enemyLife <= 0.0f)
            {
                //Dropa coisa antes de morrer
                if (dropItem)
                {
                    Instantiate(drop, transform.position, Quaternion.identity);
                }
                GameObject novoEfeito = Instantiate(efeitoMorte, transform.position, Quaternion.identity);
                novoEfeito.GetComponent<ParticleSystem>().Play();
                Destroy(gameObject);
            }
        }
        else if (status == "Lento")
        {
            if (count == 1)
            {
                velocidade -= penaDeVelocidade;
                count++;
            }
            tempoDeEfeito += Time.deltaTime;

            if (animCount == 0)
            {
                anim.SetBool("Lento", true);
                animCount++;
            }

            if (tempoDeEfeito >= maxTempoEfeito)
            {
                status = "Normal";
                velocidade = velocidadeMax;
                anim.SetBool("Lento", false);
                animCount = 0;
                count = 1;
            }
        }
        else if (status == "Congelado")
        {
            tempoDeEfeito += Time.deltaTime;
            rb.velocity = Vector2.zero;
            anim.SetBool("Congelado", true);
            if (tempoDeEfeito >= maxTempoEfeito)
            {
                status = "Normal";
                velocidade = velocidadeMax;
                anim.SetBool("Congelado", false);
            }
        }
    }


    void EsperaParaAndar()
    {
        if (tempoParaAndar < maxTempoParaAndar)
        {
            tempoParaAndar += Time.deltaTime;
            //rb.velocity = Vector2.zero;
        }
        else
        {
            ataquei = false;
            tempoParado = 0;
            tempoParaAndar = 0;
        }
    }


    //ataque básico compartilhado por praticamente todo zumbi
    void Mordida()
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
        //Obs: dir == dir = transform.position - jogador.transform.position;
    }

    //Ataque que lança um projétil, varia de cada inimigo
    //O projetil lançado tem que ter um script próprio linkado a ele

    public void LancaProjetil(ProjetilInimigo projetil, Transform posicaoProjetil)
    {
        if (Time.time > projetil.nextFire)
        {
            projetil.nextFire = Time.time + projetil.fireRate;
            projetil.bullets--;
            GameObject novoProjetil = (GameObject)Instantiate(projetil.Projetil, posicaoProjetil.position, Quaternion.identity);
            //Tentar reduzir ao máximo o número de getcomponents
            ProjetilInimigoBehaviour projetilLancado = novoProjetil.GetComponent<ProjetilInimigoBehaviour>();
            projetilLancado.danoProjetil = projetil.danoProjetil;
            projetilLancado.danoDeEfeitoDoProjetil = projetil.danoDeEfeitoDoProjetil;
            projetilLancado.tempoDeEfeitoDoProjetil = projetil.tempoDeEfeitoDoProjetil;
            projetilLancado.tempoParaDestruirProjetil = projetil.tempoParaDestruirProjetil;
            projetilLancado.velocidadeProjetil = projetil.velocidadeProjetil;
            projetilLancado.chance = projetil.chance;
            projetilLancado.podeAtordoar = projetil.podeAtordoar;
            projetilLancado.podeQueimar = projetil.podeQueimar;
            projetilLancado.podeCongelar = projetil.podeCongelar;
            projetilLancado.empuraOJogador = projetil.empuraOJogador;
            projetilLancado.forcaEmpurrao = projetil.forcaEmpurrao;
            projetilLancado.efeitoHit = projetil.efeitoHit;
        }

    }

    /*
	public void ShowDamage(float dano, Text danoSofrido, Transform mostrarDanoSofrido)
	{
		danoSofrido.text = "" + dano.ToString();
		Instantiate (danoSofrido, mostrarDanoSofrido);
	}
	*/

    public void TomarDano(float dano)
    {
        enemyLife -= dano;

        if (enemyLife <= 0.0f && !explosaoDeSabores && !usaScooter)
        {
            if (dropItem)
            {
                Instantiate(drop, transform.position, Quaternion.identity);
            }            
            GameObject novoEfeito = Instantiate(efeitoMorte, transform.position, Quaternion.identity);
            novoEfeito.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }
        //danoRecebido = dano;
        //ShowDamage (danoRecebido, danoSofrido, mostrarDanoSofrido);
    }

    /*
    void MordidaGelada()
    {
        if (dir.x > 0)
        {
            rb.AddForce(new Vector2(-impulsoAtaque, 0), ForceMode2D.Impulse);
            ataquei = true;

            if (chanceEfeito <= chance)
            {
                if (jogadorScript.status == "Normal")
                {
                    jogadorScript.status = "Congelado";
                    jogadorScript.maxTempoEfeito = maxTempEfeitoJogador;
                }
            }
        }
        else
        {
            rb.AddForce(new Vector2(impulsoAtaque, 0), ForceMode2D.Impulse);
            ataquei = true;
            if (chanceEfeito <= chance)
            {
                if (jogadorScript.status == "Normal")
                {
                    jogadorScript.status = "Congelado";
                    jogadorScript.maxTempoEfeito = maxTempEfeitoJogador;
                }
            }
        }
    }
    */

    void NotAAttack()
    {
        if (enemyLife <= 0)
        {
            //Pode só ser substituido por uma animação
            temp += Time.deltaTime;
            rb.velocity = Vector2.zero;
            Debug.Log(temp);


            if (temp >= tempoDeEspera)
            {
                Collider2D[] atingidos = Physics2D.OverlapCircleAll(gameObject.transform.position, raioDeExplosao);
                foreach (Collider2D atingido in atingidos)
                {
                    if (atingido.tag == "Enemy")
                    {
                        TomarDano(danoDaExplosao);
                    }
                    if (atingido.tag == "Player")
                    {
                        jogadorScript.LevarDano(Mathf.RoundToInt(danoDaExplosao));
                    }
                }
                if (dropItem)
                {
                    Instantiate(drop, transform.position, Quaternion.identity);
                }
                GameObject novoEfeito = Instantiate(efeitoMorte, transform.position, Quaternion.identity);
                novoEfeito.GetComponent<ParticleSystem>().Play();
                Destroy(gameObject);
            }
        }
    }

    void ExplosaoDeSabores()
    {
        if (enemyLife <= 0)
        {
            Collider2D[] atingidos = Physics2D.OverlapCircleAll(gameObject.transform.position, raioDeExplosao);
            foreach (Collider2D atingido in atingidos)
            {
                if (atingido.tag == "Player")
                {
                    jogadorScript.LevarDano(Mathf.RoundToInt(danoDaExplosao));
                }
            }

            if (dropItem)
            {
                Instantiate(drop, transform.position, Quaternion.identity);
            }
            GameObject novoEfeito = Instantiate(efeitoMorte, transform.position, Quaternion.identity);
            novoEfeito.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }
    }

    void AtaqueFisico()
    {
        //Chama a animação de ataque
        //O script de proximidade dá jeito
    }

    //Tá no nome o que faz
    public void LevarEmpurao(float forcaEmpurrao, Vector2 dirBala)
    {
        transform.position = new Vector2(transform.position.x - forcaEmpurrao * dirBala.normalized.x, transform.position.y);
    }

    //Rever
    void MordidaNinja(float chanceDeQueda)
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

        if (chanceDeQueda <= aleatorio)
        {
            rb.velocity = Vector2.zero;
            //Animação de queda
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            rb.velocity = Vector2.zero;
            col.gameObject.GetComponent<CharController>().LevarDano(dano);
            GameObject novoEfeito = Instantiate(efeitoAtaque, new Vector3(transform.position.x-1, transform.position.y + 2,
                transform.position.z), Quaternion.identity);
            novoEfeito.GetComponent<ParticleSystem>().Play();
            if (mordidaGelada)
            {
                if (chanceEfeito <= chance)
                {
                    if (jogadorScript.status == "Normal")
                    {
                        jogadorScript.status = "Congelado";
                        jogadorScript.maxTempoEfeito = maxTempEfeitoJogador;
                    }
                }
            }
            //Faz ele ir para trás após se chocar contra o jogador
            //Usada para ele não atravessar o jogador
            AplicarForcaParaTras((float)drawBack);
        }
    }
}