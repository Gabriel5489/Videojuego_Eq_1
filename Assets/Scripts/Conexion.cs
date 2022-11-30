using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

using MySql.Data;
using MySql.Data.MySqlClient;
using UnityEngine.SceneManagement;
using TMPro;

public class Conexion : MonoBehaviour
{
    public TMP_InputField txtUsuario, txtPassword, txtUsuarioR, txtPasswordR;
    private string host = "198.23.57.166", database= "gabbau0_dblogingame", user= "gabbau0_dblogingame", password= "o11wXgEH1=", cadenacon;
    [SerializeField] GameObject aviso, avisoRegistro, avisoR;
    [SerializeField] TextMeshProUGUI txtAviso, txtAvisoR;
    private bool usuarioRegistrado = false;

    private MySqlConnection con = null;
    private MySqlCommand cmd = null;
    private MySqlDataReader rdr = null;
    private string URL = "https://pruebas-gabriel-uthh.000webhostapp.com/UnityWebService/";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void login()
    {
        StartCoroutine(GetLogin());
    }

    private IEnumerator GetLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("nom_user", txtUsuario.text);
        form.AddField("nom_pass", txtPassword.text);

        UnityWebRequest www = UnityWebRequest.Post(URL + "login.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            aviso.SetActive(true);
            txtAviso.SetText("Error al conectarse a la API");
        }
        else
        {
            if(www.downloadHandler.text != "0")
            {
                StartCoroutine(ObtenerHS());
                string[] datos = www.downloadHandler.text.Split('-');
                int puntajepj = Int32.Parse(datos[1].ToString());
                PlayerPrefs.SetInt("HighScorePJ", puntajepj);
                PlayerPrefs.SetInt("Score", 0);
                PlayerPrefs.SetString("Jugador", datos[0].ToString());
                PlayerPrefs.SetInt("Vidas", 3);
                PlayerPrefs.SetInt("Flama", 1);
                PlayerPrefs.SetInt("Bomba", 1);
                PlayerPrefs.SetInt("Velocidad", 1);
                SceneManager.LoadScene("Menu");
            }
            else
            {
                aviso.SetActive(true);
                txtAviso.SetText("Datos incorrectos, reviselos e intente de nuevo");
            }

        }
    }

    public void Registrar()
    {
        StartCoroutine(GetRegistrar());
    }

    private IEnumerator GetRegistrar()
    {
        WWWForm form = new WWWForm();
        form.AddField("nom_user", txtUsuarioR.text);
        form.AddField("nom_pass", txtPasswordR.text);

        UnityWebRequest www = UnityWebRequest.Post(URL + "registrar.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            aviso.SetActive(true);
            txtAviso.SetText("Error al conectarse a la API");
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (www.downloadHandler.text != "0")
            {
                avisoRegistro.SetActive(true);
            }
            else
            {
                txtAvisoR.SetText("El usuario ya existe, o los datos ingresados son incorrectos");
                avisoR.SetActive(true);
                gameObject.SetActive(false);
            }

        }
    }

    public void Guardar() { StartCoroutine(GetGuardar()); }

    private IEnumerator GetGuardar()
    {
        int score = PlayerPrefs.GetInt("Score");
        string usuario = PlayerPrefs.GetString("Jugador");

        WWWForm form = new WWWForm();
        form.AddField("nom_user", usuario);
        form.AddField("score", score);

        UnityWebRequest www = UnityWebRequest.Post(URL + "guardapuntaje.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            aviso.SetActive(true);
            txtAviso.SetText("Error al conectarse a la API");
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

    private IEnumerator ObtenerHS()
    {

        UnityWebRequest www = UnityWebRequest.Get(URL + "highscore.php");
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            aviso.SetActive(true);
            txtAviso.SetText("Error al conectarse a la API");
        }
        else
        {
            if (www.downloadHandler.text != "0")
            {
                PlayerPrefs.SetInt("HighScoreGame", Int32.Parse(www.downloadHandler.text));
            }
        }
    }

    public void Cerrar()
    {
        Application.Quit();
    }
}
