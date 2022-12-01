using System.Collections;
using UnityEngine;
using TMPro;


public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rgbd2d;
    private Vector2 direccion = Vector2.down, positionInit;
    public float velocidad = 5f;
    private Animator animacion;
    private bool anim = true, movimiento = true;
    private int estado = 0, vidas = 3;
    private Collider2D colider;
    private ControladorJuego tiempo;
    public TextMeshProUGUI txtVidas, txtPuntaje, txtVelocidad, txtPFinal;
    public int enemigosDerrotados = 0;
    public GameObject PanelGameOver;

    [Header("Portal")]
    private bool cristal = false;

    public GameObject portal;


    public KeyCode Up = KeyCode.W;
    public KeyCode Down = KeyCode.S;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    // Start is called before the first frame update
    void Start()
    {
        velocidad = PlayerPrefs.GetInt("Velocidad") + 4;
        txtPuntaje.SetText(PlayerPrefs.GetInt("Score").ToString());
        ActualizaVidas();
        txtVelocidad.SetText((velocidad - 4).ToString());
        positionInit = transform.position;
        rgbd2d = GetComponent<Rigidbody2D>();
        animacion = GetComponent<Animator>();
        colider = GetComponent<Collider2D>();
        tiempo = FindObjectOfType<ControladorJuego>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (vidas > 0 && movimiento)
        {
            if (Input.GetKey(Up))
            {
                estado = 1;
                setDireccion(Vector2.up);
            }
            else if (Input.GetKey(Down))
            {
                estado = 2;
                setDireccion(Vector2.down);
            }
            else if (Input.GetKey(Left))
            {
                estado = 3;
                setDireccion(Vector2.left);
            }
            else if (Input.GetKey(Right))
            {
                estado = 4;
                setDireccion(Vector2.right);
            }
            else
            {
                disableAnimation();
                estado = 0;
                setDireccion(Vector2.zero);
            }
            enableAnimation();
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rgbd2d.position;
        Vector2 translation = direccion * velocidad * Time.fixedDeltaTime;

        rgbd2d.MovePosition(position + translation);

    }

    private void setDireccion(Vector2 nuevaDireccion)
    {
        direccion = nuevaDireccion;
    }

    private void disableAnimation()
    {
        animacion.SetBool("arriba", !anim);
        animacion.SetBool("abajo", !anim);
        animacion.SetBool("izquierda", !anim);
    }

    private void enableAnimation()
    {
        switch (estado)
        {
            case 1:
                animacion.SetBool("arriba", anim);
                break;
            case 2:
                animacion.SetBool("abajo", anim);
                break;
            case 3:
                animacion.SetBool("izquierda", anim);
                break;
            case 4:
                animacion.SetBool("izquierda", anim);
                break;
            default:
                break;
        }
        
    }
    private void Girar()
    {
        Vector3 escala = transform.localScale;
        
        if (estado == 3)
        {
            escala.x = 1;
        }
        else if (estado == 4)
        {
            escala.x = -1;
        }
        transform.localScale = escala;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            collision.isTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion") && vidas > 0)
        {
            movimiento = false;
            disableAnimation();
            rgbd2d.constraints = RigidbodyConstraints2D.FreezePosition;
            SetDeath();
        }
        if (collision.CompareTag("Cristal")) { 
            cristal = true;
            Destroy(collision.gameObject);
            portal.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.tag == "Enemigo")
        {
            colider.isTrigger = true;
            SetDeath();
        }

    }

    public void SetDeath()
    {
        animacion.SetBool("Death", anim);
        Invoke(nameof(Death), 1.6f);
    }

    private void Death()
    {
        gameObject.SetActive(false);
        vidas--;
        PlayerPrefs.SetInt("Vidas", vidas);
        if (vidas > 0)
        {
            transform.position = positionInit;
            colider.isTrigger = false;
            tiempo.ActivarTemporizador();
            gameObject.SetActive(true);
            rgbd2d.constraints = RigidbodyConstraints2D.None;
            rgbd2d.freezeRotation = true;
            movimiento = true;
        }
        else
        {

            PanelGameOver.SetActive(true);
            tiempo.CambiarTemporizador(false);
            txtPFinal.SetText("Tu puntaje fue de: \n" + PlayerPrefs.GetInt("Score"));
        }
        ActualizaVidas();
    }

    private void ActualizaVidas()
    {
        txtVidas.SetText(PlayerPrefs.GetInt("Vidas").ToString());
    }

    public void ActualizaPuntaje()
    {
        int puntaje = PlayerPrefs.GetInt("Score") + 1000;
        PlayerPrefs.SetInt("Score", puntaje);
        txtPuntaje.SetText(PlayerPrefs.GetInt("Score").ToString());
    }

    public void AddSpeed()
    {
        if (velocidad < 9) velocidad++;
        PlayerPrefs.SetInt("Velocidad", (int)(velocidad - 4));
        txtVelocidad.SetText(PlayerPrefs.GetInt("Velocidad").ToString());
    }

    public void AddBomb()
    {
        tiempo.GetComponent<BombController>().AddBomb();
    }

    public void AddFlame()
    {
        tiempo.GetComponent<BombController>().AddFlame();
    }

    public void setMovimiento(bool pausa) { movimiento = pausa; }
    public bool GetMovimiento() { return movimiento; }
}
