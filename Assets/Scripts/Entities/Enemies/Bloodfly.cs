using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bloodfly : MonoBehaviour, IAttackable
{
    public int contactDamage = 1;
    // Start is called before the first frame update

    public int currentHealth;
    public int maxHealth;
    [SerializeField] public GameObject bubble;
    Rigidbody2D body;

    //[SerializeField]VisualEffectEntry deathCloud;

    public VisualEffect VFX_prefab;

    void Start()
    {
        currentHealth = maxHealth;
        body = GetComponent<Rigidbody2D>();

        //deathCloud.effect = Instantiate(VFX_prefab, transform.position, Quaternion.identity, transform);
       // Game.Instance.visualEffects.Add(deathCloud, false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IAttackable>().OnBeAttacked(contactDamage, (collision.transform.position-transform.position ).normalized * 4);
            AudioManager.PlaySFX("ButterflyTouch");
            if(TryGetComponent<ChasePlayer>(out ChasePlayer chase))
            {
                chase.chaseBehavior |= ChasePlayer.ChaseBehavior.LEAVING;
                GetComponent<Collider2D>().enabled = false;
            }
        }
        if (collision.CompareTag("Water"))
        {
            Die();
        }
    }

    public void OnBeAttacked(int value, Vector2 dir)
    {
        currentHealth -= value;
        body.AddForce(dir, ForceMode2D.Impulse);
        Die();
    }
    public void Die()
    {
        if (currentHealth <= 0)
        {
            // Game.Instance.visualEffects.ChangePosition(deathCloud, transform.position);
            // deathCloud.effect.Play();
            Instantiate(bubble, transform.position, transform.rotation);
            AudioManager.PlaySFX("ButterflyDeath");
            gameObject.SetActive(false);
        }
    }
}
