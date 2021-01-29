using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Balas
{
    public string nomeBala;
    public string efeito;
    public float fireRate;
    public bool equipado;
    public int maxBullets;
    public int bullets;
    public int numeroReferencia;
    public string anotação;
    public GameObject bala;
    public Sprite spriteHud;

    //Variavies que irão sobreescrever os valores dentro do script 'BalaBehaviour'
    //Apenas por motivos de organização no inspector
    public AudioClip somDoTiro;
    public GameObject efeitoTiro;
    public GameObject efeitoAtingido;
    public float velocidadeBala;
    public float dano;
    public float tempoDeEfeito;
    public float danoDeEfeito;
    public int tempoParaDestruir;
    public float raioDeExplosao;
    public float danoDeExplosao;
    public bool explode;
    public float penaDeVelocidade;
    public float forcaEmpurrao;
}

public class ClasseBalas : MonoBehaviour {

    public List<Balas> balasDoJogo = new List<Balas>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
