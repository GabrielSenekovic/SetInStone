using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyProjectile : MonoBehaviour
{
    Rigidbody2D body;
    public bool hit;
    public Snail snailScript;

    public LightEntry myLight;
    public VisualEffectEntry VFX;

    public int damageDealt;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        VFX.effect = GetComponentInChildren<VisualEffect>();
        myLight.light = GetComponentInChildren<Light>();
        Game.Instance.visualEffects.Add(myLight, VFX);
    }
    public void Shoot(Vector2 atkDir, float atkSpeed, float atkAngle, Vector2 bodyPosition)
    {
        hit = false;
        transform.position = bodyPosition;
        transform.rotation = Quaternion.Euler(0, 0, atkAngle * Mathf.Rad2Deg);
        GetComponent<Rigidbody2D>().velocity = atkDir.normalized * atkSpeed; //* give it velocity in that direction
        Game.Instance.visualEffects.SetFollowSource(VFX, true);
        Game.Instance.visualEffects.ToggleVFXAndLight(VFX, myLight, true);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("PassThrough")){return;}

        if(!hit && other.CompareTag("Shield"))
        {
            hit = true;
            other.transform.parent.transform.parent.gameObject.GetComponent<Rigidbody2D>().AddForce
                (GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
                Debug.Log("Projectile added force");
            AudioManager.PlaySFX("Block");
        }
        //AudioManager.PlaySFX("SnailShotHit");
        Debug.Log("Snail projectile hit: " + other.gameObject.name);
        Game.Instance.visualEffects.SetFollowSource(VFX, false);
        Game.Instance.visualEffects.ToggleVFXAndLight(VFX, myLight, false);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(!hit && other.gameObject.CompareTag("Player"))
        {
            hit = true;
            Debug.Log("Shot player!");
            other.gameObject.GetComponent<Attackable>().OnBeAttacked(damageDealt);
        }
        //AudioManager.PlaySFX("SnailShotHit");
        Debug.Log("Snail projectile hit: " + other.gameObject.name);
        Game.Instance.visualEffects.SetFollowSource(VFX, false);
        Game.Instance.visualEffects.ToggleVFXAndLight(VFX, myLight, false);
        AudioManager.PlaySFX("SnailShotHit");
        gameObject.SetActive(false);
    }
}
