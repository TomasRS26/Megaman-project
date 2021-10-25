using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    private Rigidbody2D RigidBala;
    public float velocidad;

    Animator BalaAnimator;
    // Start is called before the first frame update
    void Start()
    {
        RigidBala = GetComponent<Rigidbody2D>();

        BalaAnimator = GetComponent<Animator>();
        BalaAnimator.SetBool("Destroy", false);
    }

    // Update is called once per frame
    void Update()
    {

        RigidBala.velocity = transform.right * (velocidad * Time.deltaTime);

    }

    public void DestruirBala()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject objeto = collision.gameObject;

        string etiqueta = objeto.tag;

        BalaAnimator.SetBool("Destroy", true);

    }
}
