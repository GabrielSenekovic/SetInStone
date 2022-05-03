using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;
using UnityEngine.VFX;

public class Butterfly : MonoBehaviour, Attackable
{
    public int contactDamage = 1;
    [System.NonSerialized] public PathFollower pathFollow;
    [System.NonSerialized] public PathCreator pathCreator;
    public float turnAroundSpeed = 10;
    public GameObject pathPrefab;
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
        Game.Instance.visualEffects.Add(deathCloud, false);

        if(!pathPrefab){return;}
        pathFollow = GetComponent<PathFollower>();
        GameObject path = GameObject.Instantiate(pathPrefab, transform.position, Quaternion.identity);
        pathFollow.pathCreator = path.GetComponent<PathCreator>();
        pathCreator = pathFollow.pathCreator;
    }

    // Update is called once per frame
    void Update()
    {
        if(!pathCreator){return;}
        Quaternion rotQ = pathCreator.path.GetRotationAtDistance(pathFollow.distanceTravelled, pathFollow.endOfPathInstruction);
        Vector3 rotE = rotQ.eulerAngles;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotE.y, 0), Time.deltaTime * turnAroundSpeed);
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
            Game.Instance.visualEffects.ChangePosition(deathCloud, transform.position);
            deathCloud.effect.Play();
            Instantiate(bubble, transform.position, transform.rotation);
            AudioManager.PlaySFX("ButterflyDeath");
            gameObject.SetActive(false);
        }
    }
}
