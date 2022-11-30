using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    private MySqlConnection con = null;
    private MySqlCommand cmd = null;
    private MySqlDataReader rdr = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Awake()
    {
        //PlayerPrefs.DeleteAll();
        cadenacon = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password;
        try
        {
            con = new MySqlConnection(cadenacon);
            con.Open();
            Debug.Log("Estado de la conexión: " + con.State);
        }
        catch (Exception e)
        {
            txtAviso.SetText(e.ToString());
            aviso.SetActive(true);
            Debug.Log(e.Message);
        }
    }

    public void login()
    {
        string consulta = "CALL spLogin('" + txtUsuario.text + "', '" + txtPassword.text + "')";

        Debug.Log(consulta);
        try
        {
            cmd = new MySqlCommand(consulta, con);
            rdr = cmd.ExecuteReader();
            Debug.Log(consulta);

            if (rdr.Read())
            {
                int puntajepj = rdr.GetInt32(3);
                ObtenerHS();
                PlayerPrefs.SetInt("HighScorePJ", puntajepj);
                PlayerPrefs.SetInt("Score", 0);
                PlayerPrefs.SetString("Jugador", txtUsuario.text);
                PlayerPrefs.SetInt("Vidas", 3);
                PlayerPrefs.SetInt("Flama", 1);
                PlayerPrefs.SetInt("Bomba", 1);
                PlayerPrefs.SetInt("Velocidad", 1);
                SceneManager.LoadScene("Menu");
            }
            else
            {
                aviso.SetActive(true);
                gameObject.SetActive(false);
            }

            rdr.Close();
        }
        catch (Exception e)
        {
            txtAviso.SetText(e.Message);
            aviso.SetActive(true);
            gameObject.SetActive(false);
            Debug.Log(e.Message);
        }
    }

    public void Registrar()
    {
        string consulta = "CALL spRegistro('" + txtUsuarioR.text + "', '" + txtPasswordR.text + "')";

        Debug.Log(consulta);
        try
        {
            

            if (Usuarios(txtUsuarioR.text))
            {
                txtAvisoR.SetText("El usuario ya existe, intente con otro");
                avisoR.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                rdr.Close();
                cmd = new MySqlCommand(consulta, con);
                rdr = cmd.ExecuteReader();
                Debug.Log(consulta);
                avisoRegistro.SetActive(true);
            }

            rdr.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private bool Usuarios(string usuario)
    {
        string consulta = "CALL spUsuarios('" + usuario + "')";

        Debug.Log(consulta);
        try
        {
            cmd = new MySqlCommand(consulta, con);
            rdr = cmd.ExecuteReader();
            Debug.Log(consulta);

            if (rdr.Read())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    public void Guardar()
    {
        int score = PlayerPrefs.GetInt("Score");
        string usuario = PlayerPrefs.GetString("Jugador");
        string consulta = "CALL spAddPuntaje('" + score + "', '" + usuario + "')";

        try
        {
            cmd = new MySqlCommand(consulta, con);
            rdr = cmd.ExecuteReader();
            ObtenerHS();

            rdr.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void ObtenerHS()
    {
        string consulta = "CALL spGetHS()";

        try
        {
            cmd = new MySqlCommand(consulta, con);
            rdr = cmd.ExecuteReader();

            if (rdr.Read())
            {
                PlayerPrefs.SetInt("HighScoreGame", (int)rdr.GetValue(0));
                //SceneManager.LoadScene("Menu");
            }

            rdr.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Cerrar()
    {
        Application.Quit();
    }
}
