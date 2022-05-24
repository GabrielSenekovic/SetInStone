using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bloodfly : MonoBehaviour, Attackable
{
    public int contactDamage = 1;
    // Start is called before the first frame update

    public int currentHealth;
    public int maxHealth;
    [SerializeField] public GameObject bubble;

    [SerializeField]VisualEffectEntry deathCloud;

    public VisualEffect VFX_prefab;
    void Start()
    {
        currentHealth = maxHealth;

        deathCloud.effect = Instantiate(VFX_prefab, transform.position, Quaternion.identity, transform);
       // Game.Instance.visualEffects.Add(deathCloud, false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Butterfly collided with: " + collision.gameObject.name);
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Attackable>().OnBeAttacked(contactDamage);
            AudioManager.PlaySFX("ButterflyTouch");
        }
    }

    public void OnBeAttacked(int value)
    {
        currentHealth -= value;
        if(currentHealth == 0)
        {
           // Game.Instance.visualEffects.ChangePosition(deathCloud, transform.position);
           // deathCloud.effect.Play();
            Instantiate(bubble, transform.position, transform.rotation);
            AudioManager.PlaySFX("ButterflyDeath");
            gameObject.SetActive(false);
        }
    }
}
