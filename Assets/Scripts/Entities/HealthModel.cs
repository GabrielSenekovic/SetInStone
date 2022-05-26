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

    Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar = Game.ConnectToHealthBar();
        healthBar.Initialize(maxHealth, currentHealth);
        Game.AttachPlayer(gameObject);
        safePos = transform.position;
        anim = GetComponent<Animator>();
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
            GetComponent<Input>().SetControllable(false);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if(GetComponent<Movement>().submerged)
            {
                anim.SetBool("swimming", false);
            }
            anim.SetBool("death", true);
            AudioManager.PlaySFX("VoiceDeath");
            Game.GameOver();
        }
    }
    public void Heal(int heal)
    {
        if(currentHealth == maxHealth){return;}
        currentHealth = currentHealth + heal > maxHealth ? maxHealth : currentHealth + heal;
        healthBar.UpdateHealthBar(currentHealth);
        AudioManager.PlaySFX("PickUpHeart");
        AudioManager.PlaySFX("VoiceHeal");
    }

    public void OnBeAttacked(int value, Vector2 dir)
    {
        TakeDamage(value);
        if(GetComponent<Rigidbody2D>())
        {
            GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);
        }
    }

    public bool Damaged()
    {
        return currentHealth < maxHealth;
    }

    public void ReturnToSafe()
    {
        Heal(12);
        transform.position = safePos;
        GetComponent<Input>().SetControllable(true);
        anim.SetBool("death", false);
        // fade in and out black thingy
    }
}
