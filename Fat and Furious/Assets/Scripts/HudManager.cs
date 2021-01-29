using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudManager : MonoBehaviour
{

    //Imagem bruta que vai ser alterada
    public Image armaEquipada;
    public Image armaEsquerda;
    public Image armaDireita;
    //Array de imagem que serão mudadas a medida que o jogador apertar 'Q' e 'E'
    public List<Sprite> arrayArmaEquipada;
    public List<Sprite> arrayArmaEsquerda;
    public List<Sprite> arrayArmaDireita;
    //Número que equivale a cada arma
    int indexArmaEquipada;
    int indexArmaEsquerda;
    int indexArmaDireita;

    public CharController ScriptCharacter;

    //Texto que mostra o número de balas que o jogador ainda tem
    //Defe ser filho da respectiva imagem
    public TextMeshProUGUI nBalasEquipada;
    public TextMeshProUGUI nBalasEsquerda;
    public TextMeshProUGUI nBalasDireita;

    //Sprites que mostram a vida do personagem
    public Image vidas;
    public Sprite[] vidasSprite;

    // Use this for initialization
    void Start()
    {
        //ScriptCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<CharController>();
        RestartHud();
        ContaBalas();
        AtualizaVida();
    }

    // Update is called once per frame
    void Update()
    {

        ContaBalas();
        AtualizaVida();

        //Mexendo puramente nos numeros das armas no array
        if (Input.GetKeyDown(KeyCode.E) && !ScriptCharacter.pausado && !ScriptCharacter.gameOver ||
            Input.GetAxis("Mouse ScrollWheel") > 0 && !ScriptCharacter.pausado && !ScriptCharacter.gameOver)
        {
            indexArmaEquipada = indexArmaEquipada - 1;
            indexArmaEsquerda = indexArmaEsquerda - 1;
            indexArmaDireita = indexArmaDireita - 1;
            if (indexArmaEquipada < 0)
            {
                indexArmaEquipada = arrayArmaEquipada.Count - 1;
            }
            if (indexArmaDireita < 0)
            {
                indexArmaDireita = arrayArmaEquipada.Count - 1;
            }
            if (indexArmaEsquerda < 0)
            {
                indexArmaEsquerda = arrayArmaEquipada.Count - 1;
            }
            armaEquipada.sprite = arrayArmaEquipada[indexArmaEquipada];
            armaEsquerda.sprite = arrayArmaEsquerda[indexArmaEsquerda];
            armaDireita.sprite = arrayArmaDireita[indexArmaDireita];
            //Setando para falso todos os 'equipado' das outras balas
            Unequip();

            //Deixando como verdadeiro o 'equipado' da bala em questão
            ScriptCharacter.listaDeBalas[indexArmaEquipada].equipado = true;
        }

        if (Input.GetKeyDown(KeyCode.Q) && !ScriptCharacter.pausado && !ScriptCharacter.gameOver ||
            Input.GetAxis("Mouse ScrollWheel") < 0 && !ScriptCharacter.pausado && !ScriptCharacter.gameOver)
        {
            indexArmaEquipada = indexArmaEquipada + 1;
            indexArmaEsquerda = indexArmaEsquerda + 1;
            indexArmaDireita = indexArmaDireita + 1;
            if (indexArmaEquipada > arrayArmaEquipada.Count - 1)
            {
                indexArmaEquipada = 0;
            }
            if (indexArmaDireita > arrayArmaEquipada.Count - 1)
            {
                indexArmaDireita = 0;
            }
            if (indexArmaEsquerda > arrayArmaEquipada.Count - 1)
            {
                indexArmaEsquerda = 0;
            }

            armaEquipada.sprite = arrayArmaEquipada[indexArmaEquipada];
            armaEsquerda.sprite = arrayArmaEsquerda[indexArmaEsquerda];
            armaDireita.sprite = arrayArmaDireita[indexArmaDireita];

            //Setando para falso todos os 'equipado' das outras balas
            Unequip();

            //Deixando como verdadeiro o 'equipado' da bala em questão
            ScriptCharacter.listaDeBalas[indexArmaEquipada].equipado = true;

        }
    }

    public void RestartHud()
    {
        //indexArmaEquipada = 0;
        indexArmaEsquerda = indexArmaEquipada - 1;
        indexArmaDireita = indexArmaEquipada + 1;

        if (indexArmaEsquerda < 0)
        {
            indexArmaEsquerda = arrayArmaEquipada.Count - 1;
        }
        if (indexArmaDireita > arrayArmaEquipada.Count - 1)
        {
            indexArmaDireita = 0;
        }

        armaEquipada.sprite = arrayArmaEquipada[indexArmaEquipada];
        armaEsquerda.sprite = arrayArmaEsquerda[indexArmaEsquerda];
        armaDireita.sprite = arrayArmaDireita[indexArmaDireita];
    }

    void Unequip()
    {
        for (int i = 0; i < arrayArmaEquipada.Count; i++)
        {
            ScriptCharacter.listaDeBalas[i].equipado = false;
        }
    }

    void ContaBalas()
    {

        nBalasEquipada.text = ScriptCharacter.listaDeBalas[indexArmaEquipada].bullets.ToString();
        nBalasDireita.text = ScriptCharacter.listaDeBalas[indexArmaDireita].bullets.ToString();
        nBalasEsquerda.text = ScriptCharacter.listaDeBalas[indexArmaEsquerda].bullets.ToString();

        if (indexArmaEquipada == 0)
        {
            nBalasEquipada.text = "max";
        }
        if (indexArmaEsquerda == 0)
        {
            nBalasEsquerda.text = "max";
        }

        if (indexArmaDireita == 0)
        {
            nBalasDireita.text = "max";
        }
    }

    void ScaleText(Text texto, Image Imagem)
    {
        texto.rectTransform.sizeDelta = new Vector2(Imagem.rectTransform.sizeDelta.x, (Imagem.rectTransform.sizeDelta.y / 2));
    }

    void AtualizaVida()
    {
        if (ScriptCharacter.vidaJogador <0)
        {
            vidas.sprite = vidasSprite[0];
        }
        else
        {
            vidas.sprite = vidasSprite[ScriptCharacter.vidaJogador];
        }  
    }
}
