using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{

    [SerializeField] float Velocidad;
    [SerializeField] float VelocidadSalto;
    public GameObject BalaObj;
    public GameObject Disparador;
    [SerializeField] float firerate;

    bool Caer;
    float nextfire;
    bool saltos = false;

    Animator Myanimator;
    Rigidbody2D MyRigidbody;
    BoxCollider2D Colisionador;


    //-----------------------------------------------------------mecanica dash

    IEnumerator dashCoroutine;
    float movDash = 1;
    bool isDashing;
    bool canDash = true;
    private float direccion = 1;
    float gravedadNormal;


    void Start()
    {
        Myanimator = GetComponent<Animator>();
        Myanimator.SetBool("Is running", false);


        MyRigidbody = GetComponent<Rigidbody2D>();


        Colisionador = GetComponent<BoxCollider2D>();

        gravedadNormal = MyRigidbody.gravityScale; // mecanica del dash

    }

    // Update is called once per frame
    void Update()
    {
      Correr();
      Saltar();
      Disparar();
      Caida();



        if (movDash != 0)
        {
            direccion = movDash;
        }
        if (Input.GetKeyDown(KeyCode.X) && canDash == true && Colisionador.IsTouchingLayers(LayerMask.GetMask("Terreno")))
        {
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }
            dashCoroutine = Dash(0.1f, 0);
            StartCoroutine(dashCoroutine);
            Myanimator.SetBool("Dash", true);
        }
        if (Input.GetKey(KeyCode.X) && canDash == true && Colisionador.IsTouchingLayers(LayerMask.GetMask("Terreno")))
        {
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }
            dashCoroutine = Dash(0.3f, 1);
            StartCoroutine(dashCoroutine);
            Myanimator.SetBool("Dash", true);
        }
    }

    private void FixedUpdate()
    {
        //algunos arreglos codigo dash (en especifico aqui es la fuerza de empuje)
        if (isDashing)
        {
            MyRigidbody.AddForce(new Vector2(direccion * 5, 0), ForceMode2D.Impulse);
        }
    }

    void Correr ()
    {
        float movh = Input.GetAxis("Horizontal");

        Vector2 movimiento = new Vector2(movh * Time.deltaTime * Velocidad, 0);

        transform.Translate(movimiento);

        movDash = movh;// importante para que el dash sirva 

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

            Disparador.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (movh > 0)
        {
            Vector2 Escala = transform.localScale;
            Escala.x = 1f;
            transform.localScale = Escala;

            Disparador.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    void Saltar()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(Colisionador.IsTouchingLayers(LayerMask.GetMask("Terreno")))
            {
                MyRigidbody.velocity = new Vector2(0, 0);
                MyRigidbody.AddForce(new Vector2(0, VelocidadSalto * 100f));
                Myanimator.SetTrigger("Jumping");
                saltos = true;
            }
            else if(!Colisionador.IsTouchingLayers(LayerMask.GetMask("Terreno")))
            {

                if(saltos==true)
                {
                    MyRigidbody.velocity = new Vector2(0, 0);
                    MyRigidbody.AddForce(new Vector2(0, VelocidadSalto * 100f));
                    Myanimator.SetTrigger("Jumping");
                    saltos = false;
                }
                
            }
        }



    }

    void Disparar()
    {
        if (Input.GetKeyDown
            (KeyCode.Z) && Time.time >= nextfire)
        {
            Instantiate(BalaObj, Disparador.transform.position, Disparador.transform.rotation);
            nextfire = Time.time + firerate;
        }

        if (Time.time < nextfire)
        {
            Myanimator.SetLayerWeight(1, 1);
        }
        else if(Time.time >= nextfire)
        {
            Myanimator.SetLayerWeight(1, 0);
        }
    }

    void Caida()
    {
        if (MyRigidbody.velocity.y >= 0)
        {
            Caer = false;
        }
        else if (MyRigidbody.velocity.y < 0)
        {
            Caer = true;
        }

        if (Caer == true)
        {
            Myanimator.SetBool("Falling", true);
        }
        else if (Caer == false)
        {
            Myanimator.SetBool("Falling", false);
        }
    }

    public void ReiniciarSaltos()
    {
        saltos = false;
    }




    /*concatenador para que sirva el dash donde se definen los parametros para modificarlos
       arriba de la duracion y el cool down del dash*/
    IEnumerator Dash(float duracionDash, float dashCooldown)
    {
        Vector2 velocidadOriginal = MyRigidbody.velocity;
        isDashing = true;
        canDash = false;
        Myanimator.SetBool("Dash", true);
        MyRigidbody.gravityScale = 0;
        MyRigidbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(duracionDash);
        isDashing = false;
        Myanimator.SetBool("Dash", false);
        MyRigidbody.gravityScale = gravedadNormal;
        MyRigidbody.velocity = velocidadOriginal;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


}
