
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]

    // Velocidade do projétil.
    public float speed = 12f;

    // Dano causado ao inimigo.
    public int damage = 20;

    // Tempo até o projétil desaparecer.
    public float lifetime = 3f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Destrói o projétil automaticamente
        // caso ele não atinja nenhum objeto.
        Destroy(gameObject, lifetime);

        // Move na direção do eixo X local,
        // que acompanha a rotação do FirePoint.
        rb.linearVelocity =
            transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Procura o sistema de vida do inimigo.
        EnemyHealth enemyHealth =
            collision.GetComponentInParent<EnemyHealth>();

        // Se atingiu um inimigo, causa dano.
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);

            // Destrói o projétil após acertar.
            Destroy(gameObject);
            return;
        }

        // Se atingiu o cenário, destrói o projétil.
        if (collision.gameObject.layer ==
            LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
