using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fondo : MonoBehaviour
{
    [SerializeField] private Renderer fondo1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fondo1.material.mainTextureOffset += new Vector2(0.05f, 0) * Time.deltaTime;
    }
}
