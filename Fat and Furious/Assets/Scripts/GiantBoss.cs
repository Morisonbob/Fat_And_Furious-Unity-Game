using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Apenas pra demo
using UnityEngine.SceneManagement;

public class GiantBoss : MonoBehaviour
{
    [Tooltip("Tá no nome")]
    public bool imuneAFogo;
    public bool imuneAGelo;
    public bool usaBuff;
    public bool invoca;
    public bool usaAtaqueProximidade;
    public bool lancaProjetil;
    public bool ataqueFisico;

    public Transform posicaoProjetil;
    public GameObject bracoDireito;
    public GameObject bracoEsquerdo;
    public GameObject minion;
    public Transform minionPosicao;
    public GameObject ataqueProximidade;
    public Transform proximidadeTransform;
    public ProjetilInimigo projetilScript;
    public AudioClip hit1;
    public AudioClip hit2;
    public AudioClip hit3;

    public float bossLife;
    public int dano;
    public int danoExtra;
    public float vidaExtra;

    public string status;
    public float tempoDeEfeito;
    public float maxTempoEfeito;
    public float danoEfeito;
    public float porcentagemBuff;
    public float aleatorio;
    public float aleatorio2;
    public float tempoInvocacao;
    public float chanceInvocacao;
    public float chanceProjetil;
    public float distanciaAtProximidade;
    public float tempoProximoAtaque;
    public float chanceAtaqueFisico;
    public float volumeEfeitos;

    //Efeitos dos ataques
    public GameObject efeitoSummon;
    public GameObject efeitoBuff;
    public GameObject efeitoMorte;

    float proximaInvocacao = 0;
    float maxBossLife;
    int numeroBuff = 1;
    GameObject jogador;
    ProjetilInimigo projetil;
    float distancia;
    float tempo;
    float esperaUmSegundo;
    Animator anim;
    AudioSource audioS;
    float qualHit;

    // Use this for initialization
    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player");
        anim = gameObject.GetComponentInParent<Animator>();
        status = "Normal";
        projetil = Instantiate(projetilScript);
        maxBossLife = bossLife;
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        //Fazendo com que seja gerado só um random por segundo
        esperaUmSegundo += Time.deltaTime;

        if (esperaUmSegundo >= 1)
        {
            aleatorio = Random.Range(1, 100);
            aleatorio2 = Random.Range(1, 100);
            qualHit = Random.Range(1, 4);
            esperaUmSegundo = 0;
        }


        Mathf.Abs(distancia = Vector2.Distance(transform.position, jogador.transform.position));
        //Debug.Log("Distancia do jogador ao Boss: " + distancia);


        //LancaProjetil(projetil, posicaoProjetil);

        //Variavel de controle
        tempo += Time.deltaTime;


        //Ataque de projetil
        if (lancaProjetil && aleatorio2 <= chanceProjetil)
        {
            LancaProjetil(projetil, posicaoProjetil);
            return;
        }

        //Ataque usando arma
        if (ataqueFisico && aleatorio <= chanceAtaqueFisico)
        {
            //Animação
            anim.SetBool("AtaqueFisico", true);
        }

        if (aleatorio > chanceAtaqueFisico)
        {
            anim.SetBool("AtaqueFisico", false);
        }

        //Ataque de proximidade
        if (usaAtaqueProximidade && distancia <= distanciaAtProximidade && tempo >= tempoProximoAtaque)
        {
            anim.SetTrigger("AtaqueProximidade");
            ataqueDeProximidade(ataqueProximidade, proximidadeTransform);
            tempo = 0;

        }

        //Summmon
        if (invoca && Time.time > proximaInvocacao && aleatorio <= chanceInvocacao)
        {
            proximaInvocacao = Time.time + tempoInvocacao;
            Summon(minion, minionPosicao);
        }

        //Buff
        if (bossLife <= ((maxBossLife) * porcentagemBuff) && usaBuff && numeroBuff >= 1)
        {
            numeroBuff--;
            Buff(danoExtra, vidaExtra);
        }

        //anim.SetBool("AtaqueFisico", false);
    }

    public void ataqueDeProximidade(GameObject objeto, Transform posicao)
    {
        Instantiate(objeto, posicao.position, Quaternion.identity);
    }

    public void TomarDanoChefe(float dano)
    {
        bossLife -= dano;

        if (!audioS.isPlaying && qualHit == 1)
        {
            audioS.PlayOneShot(hit1, volumeEfeitos);
        }
        else if (!audioS.isPlaying && qualHit == 2)
        {
            audioS.PlayOneShot(hit2, volumeEfeitos);
        }
        else if (!audioS.isPlaying)
        {
            audioS.PlayOneShot(hit3 , volumeEfeitos);
        }
        if (bossLife <= 0.0f)
        {
            //Destroy(gameObject);
            //Apenas para a demo
            //Tocar animação
            GameObject novoEfeito = Instantiate(efeitoMorte, transform.position, Quaternion.identity);
            novoEfeito.GetComponent<ParticleSystem>().Play();
            anim.Play("Miku_Dying");
        }
    }

    void AplicaStatus()
    {
        if (status == "Queimando")
        {
            tempoDeEfeito += Time.deltaTime;
            //danoSofrido.text = "Queimando";
            if (tempoDeEfeito >= maxTempoEfeito)
            {
                status = "Normal";
            }
            //Levando dano por tempo
            TomarDanoChefe((danoEfeito * Time.deltaTime));
            //enemyLife -= danoEfeito * Time.deltaTime;
        }
        else if (status == "Congelado")
        {
            //Paraliza os movimentos do mebro
            //Desativa as animações
            //Desativa ataque fisico
        }
    }

    //Deixa o Boss mais forte e aumenta a vida dele
    void Buff(int danoExtra, float vidaExtra)
    {
        GameObject novoEfeito = Instantiate(efeitoBuff, transform.position, Quaternion.identity);
        novoEfeito.GetComponent<ParticleSystem>().Play();
        dano += danoExtra;
        bossLife += vidaExtra;
    }

    void Summon(GameObject minion, Transform minionPosicion)
    {
        EnemyBehaviour minionInvocado = minion.GetComponent<EnemyBehaviour>();
        Instantiate(minionInvocado, minionPosicion.position, Quaternion.identity);
        GameObject novoEfeito = Instantiate(efeitoSummon, minionPosicao.transform.position, Quaternion.identity);
        novoEfeito.GetComponent<ParticleSystem>().Play();
    }

    void LancaProjetil(ProjetilInimigo projetil, Transform posicaoProjetil)
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
}
