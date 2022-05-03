using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthModel : MonoBehaviour, Attackable
{
    [SerializeField] public HealthBar healthBar;

    [SerializeField] public int currentHealth;
    [SerializeField] public int maxHealth;
    public Vector3 safePos;

    int reviveAmount = 0;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar = Game.ConnectToHealthBar();
        healthBar.Initialize(maxHealth, currentHealth);
        Game.AttachPlayer(gameObject);
        safePos = transform.position;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage < 0 ? 0 : currentHealth - damage;
        healthBar.UpdateHealthBar(currentHealth);
        AudioManager.PlaySFX("TakeDamage");
        AudioManager.PlaySFX("DamageBubbles");
        AudioManager.PlaySFX("VoiceHurt");
        if (currentHealth <= 0 && reviveAmount <= 0)
        {
            //GetComponent<Input>().SetControllable(false);
            Heal(12);
            ReturnToSafe();
            Game.GameOver();
        }
    }
    public void Heal(int heal)
    {
        currentHealth = currentHealth + heal > maxHealth ? maxHealth : currentHealth + heal;
        healthBar.UpdateHealthBar(currentHealth);
        AudioManager.PlaySFX("PickUpHeart");
        AudioManager.PlaySFX("VoiceHeal");
    }

    public void OnBeAttacked(int value)
    {
        TakeDamage(value);
    }

    public bool Damaged()
    {
        return currentHealth < maxHealth;
    }

    public void ReturnToSafe()
    {
        transform.position = safePos;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        AudioManager.PlaySFX("VoiceDeath");
        // fade in and out black thingy
    }
}
