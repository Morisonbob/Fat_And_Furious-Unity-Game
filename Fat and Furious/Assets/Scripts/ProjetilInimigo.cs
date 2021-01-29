using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjetilInimigo : MonoBehaviour {

    public string nomeProjetil;
    public string efeitoProjetil;
    public GameObject Projetil;
    public float fireRate;
    public float nextFire;
    public int bullets;

    //Checagem de efeitos
    public bool podeAtordoar;
    public bool podeQueimar;
    public bool empuraOJogador;
    public bool podeCongelar;

    //Variavies que irão sobreescrever os valores dentro do script 'BalaBehaviour'
    //Apenas por motivos de organização no inspector
    public float velocidadeProjetil;
    public int danoProjetil;
    public float danoDeEfeitoDoProjetil;
    public int tempoParaDestruirProjetil;
    public float tempoDeEfeitoDoProjetil;
    public GameObject efeitoHit;

    //Chance do ataque causar status negativo
    public float chance;
    public float forcaEmpurrao;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
