using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{

    public bool start;
    public bool options;
    public bool ending;

    public GameObject telaDeLoad;
    public TextMeshProUGUI fraseLoading;

    bool telaDeLoadBool = false;
    bool carregando = false;

    GameObject[] pauseObjects;
    GameObject[] gameOverObjects;
    public GameObject botaoOptionsGO;
    public GameObject botaoQuitGO;
    public GameObject botaoStartGO;
    Button botaoOptions;
    Button botaoQuit;
    Button botaoStart;
    public Button botaoVoltar;
    public TextMeshProUGUI textoVoltar;
    CharController jogador;

    int index = 0;
    int indexStart = 0;

    bool mouseOver;

    void Awake()
    {
        if (!start && !options && !ending)
        {
            jogador = GameObject.FindGameObjectWithTag("Player").GetComponent<CharController>();
        }

        pauseObjects = GameObject.FindGameObjectsWithTag("Pause");
        HideOnPause();
        gameOverObjects = GameObject.FindGameObjectsWithTag("Game Over");
        HideOnGameOver();
    }

    // Use this for initialization
    void Start()
    {

        //Prevenção de erros
        if (options)
        {
            return;
        }

        botaoOptions = botaoOptionsGO.GetComponent<Button>();
        botaoQuit = botaoQuitGO.GetComponent<Button>();

        indexStart = 0;

        if (start)
        {
            botaoStart = botaoStartGO.GetComponent<Button>();
            StartMenu();
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (telaDeLoadBool && !carregando)
        {
            //
            carregando = true;

            //Faz o texto aparecer
            fraseLoading.text = "Loading...";

            StartCoroutine(LoadSceneWithLoadingScreen());     
        }

        if (carregando)
        {
            //Muda a visibilidade do texto através do alpha
            fraseLoading.color = new Color(fraseLoading.color.r, fraseLoading.color.g, fraseLoading.color.b,
    Mathf.PingPong(Time.time, 1));
        }


        if (start)
        {
            StartMenu();
        }

        if (options)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                botaoVoltar.Select();
                textoVoltar.color = Color.red;
            }

        }

        if (ending)
        {
            EndMenu();
        }
    }

    public void PauseMenu(bool paused)
    {
        if (!paused)
        {
            paused = true;
            Time.timeScale = 0.0f;
            ShowOnPause();
        }
        else if (paused)
        {

            paused = false;
            Time.timeScale = 1.0f;
            HideOnPause();
        }
    }

    public void ShowOnPause()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    public void HideOnPause()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    public void ShowOnGameOver()
    {
        foreach (GameObject g in gameOverObjects)
        {
            g.SetActive(true);
        }
    }

    public void HideOnGameOver()
    {
        foreach (GameObject g in gameOverObjects)
        {
            g.SetActive(false);
        }
    }

    public void GameOverMenu()
    {
        jogador.podePausar = false;
        jogador.pausado = true;
        jogador.gameOver = true;
        Time.timeScale = 0.5f;
        ShowOnGameOver();

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            index++;
            if (index > 1)
            {
                index = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            index--;
            if (index < 0)
            {
                index = 1;
            }
        }

        if (index == 0)
        {
            botaoOptions.Select();

        }

        if (index == 1)
        {
            botaoQuit.Select();
        }

        if (index == 0 && Input.GetKeyDown(KeyCode.Return))
        {
            Restart();
        }

        else if (index == 1 && Input.GetKeyDown(KeyCode.Return))
        {
            Quit();
        }

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Scene cena = SceneManager.GetActiveScene();
        jogador.podePausar = true;
        jogador.pausado = false;
        jogador.gameOver = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(cena.name);
    }

    public void MouseOver(int num)
    {
        index = num;
        indexStart = num;
    }

    public void LoadLevel(string nomeFase)
    {
        SceneManager.LoadScene(nomeFase);
    }

    IEnumerator LoadSceneWithLoadingScreen()
    {

        yield return new WaitForSeconds(1);

        // The Application loads the Scene in the background as the current Scene runs.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Debug Clean");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void LoadSceneLS()
    {
        //Ativa a tela de load
        telaDeLoad.SetActive(true);
        telaDeLoadBool = true;
    }

    public void EndMenu()
    {
        botaoStart = botaoStartGO.GetComponent<Button>();
        botaoQuit = botaoQuitGO.GetComponent<Button>();

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            indexStart++;
            if (indexStart > 1)
            {
                indexStart = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            indexStart--;
            if (indexStart < 0)
            {
                indexStart = 1;
            }
        }

        //Highlight Start
        if (indexStart == 0)
        {
            botaoStart.Select();
            botaoStart.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            botaoStart.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }

        //Highlight Start
        if (indexStart == 1)
        {
            botaoQuit.Select();
            botaoQuit.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            botaoQuit.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
    }

    public void StartMenu()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            indexStart++;
            if (indexStart > 2)
            {
                indexStart = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            indexStart--;
            if (indexStart < 0)
            {
                indexStart = 2;
            }
        }

        //Highlight Start
        if (indexStart == 0)
        {
            botaoStart.Select();
            botaoStart.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            botaoStart.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }

        //Highlight Options
        if (indexStart == 1)
        {
            botaoOptions.Select();
            botaoOptions.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            botaoOptions.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }

        //Highlight Quit
        if (indexStart == 2)
        {
            botaoQuit.Select();
            botaoQuit.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            botaoQuit.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }

        //Começar o jogo
        if (indexStart == 0 && Input.GetKeyDown(KeyCode.Return))
        {
            LoadLevel("Debug");
        }

        //Ir para o menu de opções
        else if (indexStart == 1 && Input.GetKeyDown(KeyCode.Return))
        {
            LoadLevel("Options");
        }

        //Sair do jogo
        else if (indexStart == 2 && Input.GetKeyDown(KeyCode.Return))
        {
            Quit();
        }

    }
}
