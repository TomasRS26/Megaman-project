using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{

    [SerializeField] float Velocidad;
    [SerializeField] float VelocidadSalto;

    bool Caer;
    bool Salto;

    Animator Myanimator;
    Rigidbody2D MyRigidbody;
    BoxCollider2D Colisionador;

    void Start()
    {
        Myanimator = GetComponent<Animator>();
        Myanimator.SetBool("Is running", false);


        MyRigidbody = GetComponent<Rigidbody2D>();


        Colisionador = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Correr();
        Saltar();
    }

    void Correr ()
    {
        float movh = Input.GetAxis("Horizontal");

        Vector2 movimiento = new Vector2(movh * Time.deltaTime * Velocidad, 0);

        transform.Translate(movimiento);

        if (movh != 0)
        {
            Myanimator.SetBool("Is running", true);
        }
        else if (movh == 0)
        {
            Myanimator.SetBool("Is running", false);
        }

        if (movh < 0)
        {
            Vector2 Escala = transform.localScale;
            Escala.x = -1f;
            transform.localScale = Escala;
        }
        else if (movh > 0)
        {
            Vector2 Escala = transform.localScale;
            Escala.x = 1f;
            transform.localScale = Escala;
        }
    }

    void Saltar()
    {
        if(MyRigidbody.velocity.y >= 0)
        {
            Caer = false;
        }
        else if (MyRigidbody.velocity.y < 0)
        {
            Caer = true;
        }


        if(Input.GetKeyDown(KeyCode.Space) && Colisionador.IsTouchingLayers(LayerMask.GetMask("Terreno")))
        {
            MyRigidbody.AddForce(new Vector2 (0, VelocidadSalto * 100f));
            Myanimator.SetTrigger("Jumping");
        }

        if(Caer == true)
        {
            Myanimator.SetBool("Falling", true);
        }
        else if(Caer == false)
        {
            Myanimator.SetBool("Falling", false);
        }
    }

}
