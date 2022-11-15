using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthModel : MonoBehaviour, IAttackable
{
    [SerializeField] public HealthBar healthBar;

    [SerializeField] public int currentHealth;
    [SerializeField] public int maxHealth;
    [SerializeField] Rigidbody2D body;
    public Vector3 safePos;

    int reviveAmount = 0;

    [SerializeField]Animator anim;

    void Start()
    {
        Debug.Assert(body != null);
        Debug.Assert(anim != null);
        currentHealth = maxHealth;
        healthBar = GameMenu.ConnectToHealthBar();
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
            transform.parent.GetComponent<Input>().SetControllable(false);
            body.velocity = Vector2.zero;
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
        body.AddForce(dir, ForceMode2D.Impulse);
    }

    public bool Damaged()
    {
        return currentHealth < maxHealth;
    }

    public void ReturnToSafe()
    {
        Heal(12);
        transform.parent.position = safePos;
        transform.parent.GetComponent<Input>().SetControllable(true);
        anim.SetBool("death", false);
        anim.SetBool("swimming", false);
        anim.SetTrigger("jump");
        // fade in and out black thingy
    }
}
