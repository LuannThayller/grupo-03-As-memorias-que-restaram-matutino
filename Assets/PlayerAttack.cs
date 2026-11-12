
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 0.3f;

    private float nextFireTime;
    private bool isAttacking;

    private void Update()
    {
        // Dispara continuamente enquanto
        // o botão estiver pressionado.
        if (isAttacking && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    // Chamado pelo Input System.
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isAttacking = true;
        }
        else if (context.canceled)
        {
            isAttacking = false;
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
            return;

        Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayShoot();
        }
    }

    private void OnDisable()
    {
        isAttacking = false;
    }
}
