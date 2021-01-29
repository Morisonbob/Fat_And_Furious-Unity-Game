using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//Classe que contém todos os atributos referentes as balas
//Serializable faz com que a classe possa ser vista



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
    public float tempoDeEfeito;
    public float maxTempoEfeito;
    public int extraLifes;
    public string status = "Normal";
    [HideInInspector]
    public float danoEfeito;

    float maxTempoInvencibilidade = 0;
    SpriteRenderer sprite;

    float moveX;
    float moveY;
    float nextFire;
    float nextGranada;
    int maxVidaJogador;
    [HideInInspector]
    public float velocidadeMax;
    Rigidbody2D rb;
    GameObject classeBalas;

    public Granadas[] arrayGranadas;

    bool facingRight = true;

    [HideInInspector]
    public bool pausado = false;
    public bool podePausar = true;
    [HideInInspector]
    public bool gameOver;

    Menus menuController;

    [HideInInspector]
    public List<Balas> listaDeBalas = new List<Balas>();

    public GameObject bang;

    public Image imagemDano;

    Animator anim;

    // Use this for initialization
    void Start()
    {
        //Começa equipado com a arma padrão
        //arrayBalas[0].equipado = true;
        classeBalas = GameObject.Find("Class Balas");
        listaDeBalas.Add(classeBalas.GetComponent<ClasseBalas>().balasDoJogo[0]);
        listaDeBalas[0].equipado = true;
        rb = GetComponent<Rigidbody2D>();
        menuController = GameObject.FindObjectOfType<Menus>();
        velocidadeMax = velocidadeJogador;
        maxVidaJogador = vidaJogador;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (gameOver)
        {
            rb.velocity = Vector2.zero;
            menuController.GameOverMenu();
        }

        //Só pode se mover se não tiver acontecido um game over
        if (!gameOver)
        {
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(velocidadeJogador * moveX, rb.velocity.y);
            rb.velocity = new Vector2(rb.velocity.x, velocidadeJogador * moveY);
        }

        //Virando o personagem caso seja nescessário
        if (moveX < 0 && facingRight)
        {
            Flip();
        }
        else if (moveX > 0 && !facingRight)
        {
            Flip();
        }

        //Setando a animação de andar
        anim.SetFloat("Velocidade", Mathf.Abs(moveX + moveY));

        //chacagem de que bala o jogador vai atirar
        //Checar classe "bala"
        if (Input.GetButton("Fire1") && !pausado && !gameOver)
        {
            //Checa qual arma está equipada, se a arma ainda está no tempo de refresh do tiro
            //e se existem balas para atirar
            foreach (Balas b in listaDeBalas)
            {
                if (b.equipado && Time.time > nextFire && b.bullets != 0)
                {
                    nextFire = Time.time + b.fireRate;
                    GameObject novaBala = Instantiate(b.bala, posicaoBala.position, Quaternion.identity);
                    BalaBehaviour novaBalaScript = novaBala.GetComponent<BalaBehaviour>();
                    //Coloca os valores que estão no INSPECTOR DO CLASS BALA (importados para o jogador) NA AREA DE ARRAY BALAS
                    //dentro do script 'BalaBehaviour'
                    novaBalaScript.dano = b.dano;
                    novaBalaScript.danoDeEfeito = b.danoDeEfeito;
                    novaBalaScript.velocidadeBala = b.velocidadeBala;
                    novaBalaScript.tempoParaDestruir = b.tempoParaDestruir;
                    novaBalaScript.tempoDeEfeito = b.tempoDeEfeito;
                    novaBalaScript.raioDeExplosao = b.raioDeExplosao;
                    novaBalaScript.danoDeExplosao = b.danoDeExplosao;
                    novaBalaScript.explode = b.explode;
                    novaBalaScript.penaDeVelocidade = b.penaDeVelocidade;
                    novaBalaScript.forcaEmpurrao = b.forcaEmpurrao;
                    novaBalaScript.somDoTiro = b.somDoTiro;
                    novaBalaScript.efeitoTiro = b.efeitoTiro;
                    novaBalaScript.efeitoAtingido = b.efeitoAtingido;
                    novaBala.transform.localScale = transform.localScale;
                    b.bullets--;
                }
            }
        }

        /*
        //Criando a granada que ele atira
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
        */

        //Caso aperte pause, pausa o jogo
        if (Input.GetKeyDown(KeyCode.Return) && podePausar)
        {
            menuController.PauseMenu(pausado);
            pausado = !pausado;
        }

        //Checando se foi atingido por algum status negativo
        ChecaStatus();

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
        //Ativa a imagem de que levou dano e põe seu alpha no máximo
        imagemDano.gameObject.SetActive(true);
        imagemDano.color = new Color(imagemDano.color.r, imagemDano.color.g, imagemDano.color.b, 1);

        if (Time.time > maxTempoInvencibilidade)
        {
            maxTempoInvencibilidade = Time.time + frameDeInvencibilidade;
			InvokeRepeating ("Pisca", 0,0.3f);
			Invoke ("ParaDePiscar",(frameDeInvencibilidade - 0.1f));
            vidaJogador -= dano;
        }

        if (vidaJogador <=0)
        {
            menuController.GameOverMenu();
        }
    }

	public void Pisca(){

        //Vai diminuindo o alpha da imagem
        imagemDano.color = new Color(imagemDano.color.r, imagemDano.color.g, imagemDano.color.b, imagemDano.color.a - 0.1f);

        if (GetComponent<SpriteRenderer>().enabled){
			GetComponent<SpriteRenderer> ().enabled = false;
	}
		else{
			GetComponent<SpriteRenderer>().enabled = true;
		}
	}

	public void ParaDePiscar(){
        //Desaparece a imagem de dano
        imagemDano.gameObject.SetActive(false);
        CancelInvoke ("Pisca");
		GetComponent<SpriteRenderer>().enabled = true;
	}


	float danoTemporario;

    public void ChecaStatus()
    {
        if (status == "Congelado")
        {
            tempoDeEfeito += Time.deltaTime;
            velocidadeJogador = 0;
            anim.SetBool("Congelado", true);

            if (tempoDeEfeito >= maxTempoEfeito)
            {
                status = "Normal";
                velocidadeJogador = velocidadeMax;
                tempoDeEfeito = 0;
                anim.SetBool("Congelado", false);
            }
        }
        else if (status == "Queimado")
        {
            tempoDeEfeito += Time.deltaTime;
            //danoSofrido.text = "Queimando";

            //Levando dano por tempo
			//float danoTotal = danoEfeito * tempoDeEfeito;

			danoTemporario += Time.deltaTime;
			if (danoTemporario >= 1) {
				vidaJogador -= (int)danoEfeito;
				danoTemporario = 0;
			}

            if (tempoDeEfeito >= maxTempoEfeito)
            {
                status = "Normal";
                //int temp =  (int)(danoTotal);
                //vidaJogador -= temp;
                tempoDeEfeito = 0;
				danoTemporario = 0;
            }
        }
        else if (status == "Atordoado")
        {
            tempoDeEfeito += Time.deltaTime;
            velocidadeJogador = 0;

            //animação de atordoado

            if (tempoDeEfeito >= maxTempoEfeito)
            {
                status = "Normal";
                velocidadeJogador = velocidadeMax;
                tempoDeEfeito = 0;
            }
        }
    }

    //SEI NÃO VÉI
    /*
    public void LevarEmpurao(float forcaEmpurrao)
    {
        if (facingRight)
        {
            transform.position = new Vector2(transform.position.x + forcaEmpurrao, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - forcaEmpurrao, transform.position.y);
        }
    }
    */
}
