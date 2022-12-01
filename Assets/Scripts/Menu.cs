using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menupausa;
    private KeyCode pause = KeyCode.Escape;
    private bool pauseActivo = false;
    PlayerController player;
    ControladorJuego controladorJuego;
    Patrullar enemigo;
    public Conexion conexion;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        enemigo = FindObjectOfType<Patrullar>();
        controladorJuego = FindObjectOfType<ControladorJuego>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(pause) && !pauseActivo) MenuPausaActivar();
    }


    public void MenuPausaActivar()
    {
        pauseActivo = true;
        menupausa.SetActive(true);
        player.setMovimiento(!pauseActivo);
        enemigo.movimientoEnemigo = false;
        controladorJuego.CambiarTemporizador(false);
    }

    public void MenuPausaDesactivar()
    {
        pauseActivo = false;
        menupausa.SetActive(false);
        player.setMovimiento(!pauseActivo);
        enemigo.movimientoEnemigo = true;
        controladorJuego.CambiarTemporizador(true);
    }

    public void RegresarMenuPrincipal()
    {
        ResetearYGuardar();
    }

    public void ResetearYGuardar()
    {
        PlayerPrefs.SetInt("Vidas", 3);
        PlayerPrefs.SetInt("Flama", 1);
        PlayerPrefs.SetInt("Bomba", 1);
        PlayerPrefs.SetInt("Velocidad", 1);
        if(PlayerPrefs.GetInt("Score") > PlayerPrefs.GetInt("HighScorePJ")) conexion.Guardar();
        PlayerPrefs.SetInt("Score", 0);
    }
}
