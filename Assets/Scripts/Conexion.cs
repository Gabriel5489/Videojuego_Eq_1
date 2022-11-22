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
    public TMP_InputField txtUsuario, txtPassword;
    public string host, database, user, password, cadenacon;

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
            Debug.Log(e);
        }
    }

    public void login()
    {
        string consulta = "CALL spLogin('" + txtUsuario.text + "', '" + txtPassword.text + "')";

        try
        {
            cmd = new MySqlCommand(consulta, con);
            rdr = cmd.ExecuteReader();

            if (rdr.Read())
            {
                PlayerPrefs.SetString("Jugador", txtUsuario.text);
                SceneManager.LoadSceneAsync("Stage 1");
            }

            rdr.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
