using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rgbd2d;
    private Vector2 direccion = Vector2.down;
    public float velocidad = 5f;
    private Animator animacion;
    private bool anim = true;
    private int estado = 0;

    public KeyCode Up = KeyCode.W;
    public KeyCode Down = KeyCode.S;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    // Start is called before the first frame update
    void Start()
    {
        rgbd2d = GetComponent<Rigidbody2D>();
        animacion = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(Up)){
            estado = 1;
            setDireccion(Vector2.up);
        }else if (Input.GetKey(Down)){
            estado = 2;
            setDireccion(Vector2.down);
        }else if (Input.GetKey(Left)){
            estado = 3;
            setDireccion(Vector2.left);
        }else if (Input.GetKey(Right)){
            estado = 4;
            setDireccion(Vector2.right);
        }else{
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
                break;
            default:
                break;
        }
    }
}
