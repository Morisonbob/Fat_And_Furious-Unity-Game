using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColetavelBehaviour : MonoBehaviour
{

    GameObject bala;
    GameObject hud;
    public int numeroDeReferencia;
    public int vidaExtra;

    // Use this for initialization
    void Start()
    {
        bala = GameObject.Find("Class Balas");
        hud = GameObject.Find("BalaEquipada");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && this.tag != "Cura")
        {
            //Checa se essa bala já está em posse do jogador
            if (other.GetComponent<CharController>().listaDeBalas.Contains(bala.GetComponent<ClasseBalas>().balasDoJogo[numeroDeReferencia]))
            {
                bala.GetComponent<ClasseBalas>().balasDoJogo[numeroDeReferencia].bullets = bala.GetComponent<ClasseBalas>().balasDoJogo[numeroDeReferencia].maxBullets;
                Destroy(gameObject);
            }
            else
            {
                other.GetComponent<CharController>().listaDeBalas.Add(bala.GetComponent<ClasseBalas>().balasDoJogo[numeroDeReferencia]);
                hud.GetComponent<HudManager>().arrayArmaEquipada.Add(bala.GetComponent<ClasseBalas>().balasDoJogo[numeroDeReferencia].spriteHud);
                hud.GetComponent<HudManager>().arrayArmaDireita.Add(bala.GetComponent<ClasseBalas>().balasDoJogo[numeroDeReferencia].spriteHud);
                hud.GetComponent<HudManager>().arrayArmaEsquerda.Add(bala.GetComponent<ClasseBalas>().balasDoJogo[numeroDeReferencia].spriteHud);
                hud.GetComponent<HudManager>().RestartHud();
                Destroy(gameObject);
            }
        }

        else if (other.tag == "Player" && this.tag == "Cura")
        {
            other.GetComponent<CharController>().vidaJogador += vidaExtra;
            if (other.GetComponent<CharController>().vidaJogador > 5)
            {
                other.GetComponent<CharController>().vidaJogador = 5;
            }
            Destroy(gameObject);
        }
    }
}
