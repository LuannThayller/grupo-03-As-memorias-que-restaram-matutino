
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Damage Settings")]

    // Quantidade de dano causada ao jogador.
    public int damage = 20;

    // Tenta causar dano quando a colisão começa.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamagePlayer(collision);
    }

    // Continua verificando o contato.
    // Isso permite causar outro dano quando
    // a invulnerabilidade do jogador terminar.
    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamagePlayer(collision);
    }

    // Verifica se o objeto atingido possui vida.
    private void TryDamagePlayer(Collision2D collision)
    {
        // Procura PlayerHealth no objeto atingido
        // ou em algum de seus objetos pais.
        PlayerHealth playerHealth =
            collision.collider.GetComponentInParent<PlayerHealth>();

        // Se encontrou o componente de vida,
        // envia a quantidade de dano.
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
