using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TextBoxController : MonoBehaviour
{

    public GameObject textBox;
    public TextMeshProUGUI texto;
    public TextAsset falaZe;
    public TextAsset falaNPC;
    public TextAsset textoTutorial;
    //PRECISA ESTAR NA FASE
    public GameObject coletavelTutorial;
    public GameObject inimigoTutorial;
    public GameObject barreiraTutorial;
    public GameObject barreiraProjetil;
    public GameObject barreiraAntiInimigo;
    public Image quemFala;
    public Image NPC;

    public string[] textLines;
    public string[] textLinesNPC;

    GameObject jogador;
    CharController jogadorScript;
    GameObject check;

    public int currentLine = 0;
    public int currentLineNPC = 0;
    public int EndLine;
    public int EndLineNPC;
    public int[] pauseAt;
    public int[] pauseAtNPC;

    public bool ativado;
    //public bool dialogoNPC;
    public bool falaSozinho;
    public bool tutorial;
    public bool falaComNPC;
    public bool teste;

    public bool jogadorFalaPrimeiro;
    public bool npcFalaPrimeiro;

    int i = 0;
    int j = 0;

    //Variaveis que precisam ser globais pra função tutorial funcionar
    int cw = 0, ca = 0, cs = 0, cd = 0;
    int parte4Counter = 0;
    bool parte1;
    bool parte2;
    bool parte3;
    bool parte3_1;
    bool parte3_2;
    bool parte4;
    bool parte4_1;
    bool parte4_2;
    bool parte5;
    bool parte6;
    bool parte6_1;
    int contadorBalas;
    float velocidadeInimigo;

    // Use this for initialization

    void Awake()
    {
        parte1 = true;
        parte2 = true;
        parte3 = true;
        parte3_1 = true;
        parte3_2 = true;
        parte4 = true;
        parte4_1 = true;
        parte4_2 = true;
        parte5 = true;
        parte6 = true;
        parte6_1 = true;
    }

    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player");
        jogadorScript = jogador.GetComponent<CharController>();
        velocidadeInimigo = inimigoTutorial.GetComponent<EnemyBehaviour>().velocidade;

        currentLine = 0;

        coletavelTutorial.SetActive(false);
        inimigoTutorial.SetActive(false);
        barreiraTutorial.SetActive(false);
        barreiraProjetil.SetActive(false);

        /*
        if (!dialogoNPC)
        {
            NPC.gameObject.SetActive(false);
        }
        */

        if (NPC != null)
        {
            NPC.transform.localScale = new Vector3(-NPC.transform.localScale.x,
                NPC.transform.localScale.y, NPC.transform.localScale.z);
        }

        if (ativado)
        {
            EnableTextBox();
            //Ativando a barreira anti inimigo
            barreiraAntiInimigo.SetActive(true);
        }


        //Debug.Log(EndLine);
    }

    // Update is called once per frame
    void Update()
    {

        if (tutorial)
        {
            Tutorial(textoTutorial);
			quemFala.gameObject.SetActive(true);
			NPC.gameObject.SetActive(false);
            barreiraTutorial.SetActive(true);
            barreiraProjetil.SetActive(true);
            //
            jogadorScript.podePausar = false;
        }

        if (!tutorial)
        {
            barreiraTutorial.SetActive(false);
            barreiraProjetil.SetActive(false);
        }

        if (falaSozinho)
        {
            FalaSozinho(falaZe);
        }

        if (falaComNPC)
        {
            FalaComNPC(falaZe, falaNPC);
        }

        if (!ativado)
        {
            textBox.SetActive(false);
        }
    }

    void Tutorial(TextAsset textoTutorial)
    {

        if (textoTutorial != null)
        {
            textLines = textoTutorial.text.Split('\n');
        }

        if (EndLine == 0)
        {
            EndLine = textLines.Length - 1;
        }

        //Seta o texto que vai aparecer já trocando as tags pelo o que eu quero
        texto.text = textLines[currentLine].Replace("<br>", "\n");


        if (parte4_1)
        {
            Resume();
        }


        //Primeira parte
        //Movimentação
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            cw++;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ca++;
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            cs++;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            cd++;
        }

        //Debug.Log("ca: " + ca + " cs: " + cs + " cw: " + cw + " cd: " + cd);

        if (ca >= 1 && cd >= 1 && cs >= 1 && cw >= 1 && parte1)
        {
            parte1 = !parte1;
            currentLine += 1;
        }

        //Segunda parte
        //Tiro
        if (Input.GetKeyDown(KeyCode.Mouse0) && parte2 && !parte1)
        {
            parte2 = !parte2;
            currentLine += 1;
            //Ativando o coletável para o jogador pegar
            coletavelTutorial.SetActive(true);
        }

        //Dá pra melhorar essa porra aqui escrevendo um texto melhor e explicando mais, mas eu não vou fazer

        //Parte 3
        //Explicando as balas
        contadorBalas = jogadorScript.listaDeBalas.Count;
        if (contadorBalas > 1 && parte3 && !parte2 && !parte1)
        {
            parte3 = !parte3;
            currentLine += 1;
        }

        //Parte 3.1
        //Explicando ainda sobre balas
        if (Input.GetKeyDown(KeyCode.Space) && parte3_1 && !parte3 && !parte2 && !parte1)
        {
            parte3_1 = !parte3_1;
            currentLine += 1;
        }

        //Parte 3.2
        //Explicando ainda sobre balas
        //É else if pq se não for dá erro
        else if (Input.GetKeyDown(KeyCode.Space) && parte3_2 && !parte3_1 && !parte3 && !parte2 && !parte1)
        {
            parte3_2 = !parte3_2;
            currentLine += 1;
        }

        //Parte 4
        //Trocando de balas
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") > Mathf.Abs(0)) &&
            parte4 && !parte3_2 && !parte3_1 && !parte3 && !parte2 && !parte1)
        {
            parte4Counter++;
            if (parte4Counter >= 2)
            {
                parte4 = !parte4;
                currentLine += 1;
            }
        }

        //Parte 4.1
        //Explicando sobre balas
        if (Input.GetKeyDown(KeyCode.Space) && parte4_1 && !parte4 && !parte3_2 && !parte3_1 && !parte3 && !parte2 && !parte1)
        {
            parte4_1 = !parte4_1;
            currentLine += 1;
        }

        //Parte 4.2
        //Explicando sobre debuffs
        else if (Input.GetKeyDown(KeyCode.Space) && parte4_2 && !parte4_1 && !parte4 && !parte3_2 && !parte3_1 && !parte3 && !parte2 && !parte1)
        {
            parte4_2 = !parte4_2;
            currentLine += 1;
            //Ativando o inimigo, só que parado
            inimigoTutorial.SetActive(true);
            Pause();
        }

        //Parte 5
        //Enfrentando o inimigo
        else if (Input.GetKeyDown(KeyCode.Space) && parte5 &&!parte4_2 && !parte4_1 && !parte4 && !parte3_2 && !parte3_1 && !parte3 && !parte2 && !parte1)
        {
            parte5 = !parte5;
            currentLine += 1;
            //Ativando o inimigo, só que parado
            inimigoTutorial.GetComponent<EnemyBehaviour>().velocidade = velocidadeInimigo;
            //Desativando a barreira anti inimigo
            barreiraAntiInimigo.SetActive(false);
            DisableTextBox();
        }
        if (!parte5)
        {
            check = GameObject.Find("Inimigo Tutorial");
        }

        if (!parte5 && !check)
        {
            EnableTextBox();
            //Parte 6
            //Inimigo foi derrotado e la vem outro tutorial
            if (Input.GetKeyDown(KeyCode.Escape) && parte6 && !parte5 && !parte4_1 && !parte4 && !parte3_2 && !parte3_1 && !parte3 && !parte2 && !parte1)
            {
                parte6 = !parte6;
                string tutorialName;
                tutorialName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(tutorialName);
            }

            //parte 6.1
            ////Inimigo foi derrotado e sigamos
            if (Input.GetKeyDown(KeyCode.Space) && parte6_1 && !parte5 && !parte4_2 && !parte4_1 && !parte4 && !parte3_2 && !parte3_1 && !parte3 && !parte2 && !parte1)
            {
                parte5 = !parte5;
                barreiraTutorial.SetActive(false);
                barreiraProjetil.SetActive(false);
                tutorial = !tutorial;
                jogadorScript.podePausar = true;
                DisableTextBox();
            }
        }

        Debug.Log("Linha: " + currentLine);

    }

    void FalaComNPC(TextAsset falaZe, TextAsset falaNPC)
    {

        jogadorScript.podePausar = false;

        //Parte do NPC
        if (falaNPC != null)
        {
            textLinesNPC = falaNPC.text.Split('\n');
        }

        if (EndLineNPC == 0)
        {
            EndLineNPC = textLinesNPC.Length - 1;
        }

        //Parte do personagem
        if (falaZe != null)
        {
            textLines = falaZe.text.Split('\n');
        }

        if (EndLine == 0)
        {
            EndLine = textLines.Length - 1;
        }

        if (EndLineNPC == 0)
        {
            EndLineNPC = textLinesNPC.Length - 1;
        }

        if (currentLineNPC > EndLineNPC && currentLine > EndLine)
        {
            jogadorScript.podePausar = true;
            DisableTextBox();
            falaComNPC = !falaComNPC;
            //Desativando o script para não bugar
            //this.enabled = false;
        }

        if (jogadorFalaPrimeiro)
        {
            //Seta o texto que vai aparecer já trocando as tags pelo o que eu quero
            texto.text = textLines[currentLine].Replace("<br>", "\n");

            quemFala.gameObject.SetActive(true);
            NPC.gameObject.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
            {
                currentLine += 1;
            }

            if (i < pauseAt.Length)
            {
                if (currentLine == pauseAt[i])
                {

                    if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
                    {
                        i++;
                        texto.text = textLinesNPC[currentLineNPC].Replace("<br>", "\n");
                        jogadorFalaPrimeiro = !jogadorFalaPrimeiro;
                        npcFalaPrimeiro = !npcFalaPrimeiro;
                    }
                }
            }


            if (currentLine > EndLine)
            {
                //Espera antes de passar para a próxima linha
                //texto.text = textLines[currentLine - 1].Replace("<br>", "\n");

                if (currentLineNPC <= EndLineNPC && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space)))
                {
                    texto.text = textLinesNPC[currentLineNPC].Replace("<br>", "\n");
                    jogadorFalaPrimeiro = !jogadorFalaPrimeiro;
                    npcFalaPrimeiro = !npcFalaPrimeiro;
                }
            }
        }

        else if (npcFalaPrimeiro)
        {
            //Seta o texto que vai aparecer já trocando as tags pelo o que eu quero
            texto.text = textLinesNPC[currentLineNPC].Replace("<br>", "\n");

            quemFala.gameObject.SetActive(false);
            NPC.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
            {
                currentLineNPC += 1;
            }

            if (j < pauseAtNPC.Length)
            {
                if (currentLineNPC == pauseAtNPC[j])
                {

                    if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
                    {
                        j++;
                        texto.text = textLines[currentLine].Replace("<br>", "\n");
                        jogadorFalaPrimeiro = !jogadorFalaPrimeiro;
                        npcFalaPrimeiro = !npcFalaPrimeiro;
                    }
                }
            }


            if (currentLineNPC > EndLineNPC)
            {
                //Espera antes de passar para a próxima linha
                //texto.text = textLines[currentLine - 1].Replace("<br>", "\n");

                if (currentLine <= EndLine && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space)))
                {
                    texto.text = textLines[currentLine].Replace("<br>", "\n");
                    jogadorFalaPrimeiro = !jogadorFalaPrimeiro;
                    npcFalaPrimeiro = !npcFalaPrimeiro;
                }
            }
        }
    }

    //Função que mostra as falas do jogador
    void FalaSozinho(TextAsset Fala)
    {
        if (Fala != null)
        {
            textLines = Fala.text.Split('\n');
        }

        if (EndLine == 0)
        {
            EndLine = textLines.Length - 1;
        }

        //Seta o texto que vai aparecer já trocando as tags pelo o que eu quero
        texto.text = textLines[currentLine].Replace("<br>", "\n");

        //Mudar isso
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
        {
            currentLine += 1;
        }

        if (currentLine > EndLine)
        {
            ativado = !ativado;
            DisableTextBox();
        }
    }


    //
    void FalaSozinho(TextAsset Fala, int comecaEm)
    {
        if (Fala != null)
        {
            textLines = Fala.text.Split('\n');
        }

        if (EndLine == 0)
        {
            EndLine = textLines.Length - 1;
        }

        //Seta o texto que vai aparecer já trocando as tags pelo o que eu quero
        texto.text = textLines[comecaEm].Replace("<br>", "\n");

        //Mudar isso
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
        {
            currentLine += 1;
        }

        if (comecaEm > EndLine)
        {
            ativado = !ativado;
            DisableTextBox();
        }
    }


    void EnableTextBox()
    {
        textBox.SetActive(true);
        Pause();
    }

    void DisableTextBox()
    {
        textBox.SetActive(false);
        Resume();
    }

    void Pause()
    {
        jogador.GetComponent<CharController>().pausado = true;
        Time.timeScale = 0.0f;
    }

    void Resume()
    {
        jogador.GetComponent<CharController>().pausado = false;
        Time.timeScale = 1;
    }

    /*
    void OnGUI()
    {
        texto.text = GUI.TextArea(new Rect(0, 140, 200, 100), textLines[currentLine]);
    }
    */
}
