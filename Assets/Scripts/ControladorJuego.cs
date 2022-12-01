using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorJuego : MonoBehaviour
{
    public float tiempoMaximo;

    private float tiempoActual;
    private bool tiempoActivado;
    private PlayerController pj;
    public TextMeshProUGUI txtTiempo;
    public TextMeshProUGUI txtHigh;

    // Start is called before the first frame update
    void Start()
    {
        pj = FindObjectOfType<PlayerController>();
        txtHigh.SetText(PlayerPrefs.GetInt("HighScoreGame").ToString());
        ActivarTemporizador();
    }

    // Update is called once per frame
    void Update()
    {
        if (tiempoActivado) CambiarContador();   
    }

    private void CambiarContador()
    {
        tiempoActual -= Time.deltaTime;

        if (tiempoActual >= 0) {
            SegundosAMinutos((int) tiempoActual);
        }

        if (tiempoActual <= 0)
        {
            CambiarTemporizador(false);
            pj.SetDeath();
        }
    }

    private void ActualizarTiempo(int mins, int sec)
    {
        string cadena = cadena = "0" + mins + ":" + sec;
        if (sec.ToString().Length < 2) cadena = cadena = "0" + mins + ":0" + sec;
        txtTiempo.SetText(cadena);
    }

    public void CambiarTemporizador(bool estado)
    {
        tiempoActivado = estado;
    }

    public void ActivarTemporizador()
    {
        tiempoActual = tiempoMaximo;
        txtTiempo.SetText(tiempoMaximo.ToString());
        CambiarTemporizador(true);
    }

    public void DesactivarTemporizador()
    {
        CambiarTemporizador(false);
    }

    private void SegundosAMinutos(int segundos)
    {
        int min, sec;
        min = segundos / 60;
        if (min >= 1)
        {
            sec = segundos % 60;
            ActualizarTiempo(min, sec);
        }
        else
        {
            ActualizarTiempo(min, segundos);
        }
    }
}
