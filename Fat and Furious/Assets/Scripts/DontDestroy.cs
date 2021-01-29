using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{

    string fase;

    private static DontDestroy instancia;

    // Use this for initialization
    void Start()
    {
        //if we don't have an [_instance] set yet
         if (!instancia)
            instancia = this;
        //otherwise, if we do, kill this thing
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        fase = SceneManager.GetActiveScene().name;

        if (fase != "Tela_Inicial" && fase != "Options")
        {
            Destroy(transform.gameObject);
        }
    }
}
