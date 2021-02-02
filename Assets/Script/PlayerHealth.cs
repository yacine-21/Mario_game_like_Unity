using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public float invincibilityTimeAfterHit = 3f;
    public float invincibilityFlashDelay = 0.2f;
    public bool isInvincible = false;

    public SpriteRenderer graphics;
    public HealthBar healthBar;

    public static PlayerHealth instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerHealth dans la scène");
            return;
        }

        instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDammage(60);
        }
    }

    public void HealPlayer(int amount)
    {
        if ((currentHealth + amount) > maxHealth)
        {
            currentHealth = maxHealth;
        } else
        {
            currentHealth += amount;
        }

        healthBar.SetHealth(currentHealth);
    }

    public void TakeDammage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);

            // vérifier si le jouer est toujours vivant
            if(currentHealth <= 0)
            {
                Death();
                return;
            }


            isInvincible = true;
            StartCoroutine(InvincibilityFlash());
            StartCoroutine(HandleInvincibiltyDelay());
        }
    }

    public void Death()
    {
        PlayerMovement.instance.enabled = false;
        PlayerMovement.instance.animator.SetTrigger("Death");
        PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Kinematic;
        PlayerMovement.instance.PlayerCollider.enabled = false;
        GameOverManager.instance.OnPlayerDeath();
    }

    public void Respawn()
    {
        PlayerMovement.instance.enabled = true;
        PlayerMovement.instance.animator.SetTrigger("Respawn");
        PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Dynamic;
        PlayerMovement.instance.PlayerCollider.enabled = true;
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
    }

    public IEnumerator InvincibilityFlash()
    {
        while (isInvincible)
        {
            graphics.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
            graphics.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
        }
    }

    public IEnumerator HandleInvincibiltyDelay()
    {
        yield return new WaitForSeconds(invincibilityTimeAfterHit);
        isInvincible = false;
    }
}
