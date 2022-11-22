using System.Collections;
using UnityEngine;
using TMPro;


public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rgbd2d;
    private Vector2 direccion = Vector2.down, positionInit;
    [SerializeField] private float velocidad = 5f;
    private Animator animacion;
    private bool anim = true, movimiento = true;
    private int estado = 0, vidas = 3;
    private Collider2D colider;
    private ControladorJuego tiempo;
    public TextMeshProUGUI txtVidas, txtPuntaje, txtVelocidad;
    public int enemigosDerrotados = 0;



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
        if (vidas == 0 || !movimiento)
        {
            return;
        }

        if (Input.GetKey(Up)){
            estado = 1;
            setDireccion(Vector2.up);
        }else if (Input.GetKey(Down)){
            estado = 2;
            setDireccion(Vector2.down);
        }else if (Input.GetKey(Left)){
            estado = 3;
            setDireccion(Vector2.left);
        }
        else if (Input.GetKey(Right))
        {
            estado = 4;
            setDireccion(Vector2.right);
        }
        else{
            disableAnimation();
            estado = 0;
            setDireccion(Vector2.zero);
        }
        enableAnimation();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion") && vidas > 0)
        {
            movimiento = false;
            SetDeath();
        }
        if (collision.CompareTag("Cristal")) { 
            cristal = true;
            Destroy(collision.gameObject);
            portal.SetActive(cristal);
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
        if (vidas > 0)
        {
            transform.position = positionInit;
            colider.isTrigger = false;
            tiempo.ActivarTemporizador();
            gameObject.SetActive(true);
            movimiento = true;
        }
        ActualizaVidas();
    }

    private void ActualizaVidas()
    {
        txtVidas.SetText(vidas.ToString());
    }

    public void AddSpeed()
    {
        if (velocidad < 11) velocidad++;
        txtVelocidad.SetText((velocidad-4).ToString());
    }
}
