using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthModel : MonoBehaviour, IAttackable
{
    [SerializeField] public HealthBar healthBar;

    [SerializeField] Timer healthCounter;

    [SerializeField] Rigidbody2D body;
    public Vector3 safePos;

    int reviveAmount = 0;

    [SerializeField]Animator anim;

    public bool HasFullHealth() => healthCounter.IsFull();
    public bool Damaged() => healthCounter.IsLowerThanMax();
    public Timer GetCounter() => healthCounter;

    void Start()
    {
        Debug.Assert(body != null);
        Debug.Assert(anim != null);
        healthBar = GameMenu.ConnectToHealthBar();
        healthBar.Initialize(healthCounter.MaxValue, healthCounter.CurrentValue);
        healthCounter.Initialize(()=> { }, Timer.TimerBehavior.START_FULL, ()=> { healthBar.UpdateHealthBar(healthCounter.CurrentValue); });
        Game.AttachPlayer(gameObject);
        safePos = transform.position;
    }

    public void TakeDamage(int damage)
    {
        healthCounter.Subtract(damage);
        AudioManager.PlaySFX("TakeDamage");
        AudioManager.PlaySFX("DamageBubbles");
        AudioManager.PlaySFX("VoiceHurt");
        if (healthCounter.IsEmpty() && reviveAmount <= 0)
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
        if(healthCounter.IsFull()){return;}
        healthCounter.Add(heal);
        AudioManager.PlaySFX("PickUpHeart");
        AudioManager.PlaySFX("VoiceHeal");
    }

    public void OnBeAttacked(int value, Vector2 dir)
    {
        TakeDamage(value);
        body.AddForce(dir, ForceMode2D.Impulse);
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
