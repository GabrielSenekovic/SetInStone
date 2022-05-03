using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : MonoBehaviour
{
    HealthModel healthModel;

    public float rotationSpeed;
    public float swaySpeed;

    float currentRotation;
    public float currentSway;
    public AnimationCurve curve;

    float startY;

    private void Start() 
    {
        startY = transform.localPosition.y;
    }

    private void FixedUpdate() 
    {
        currentRotation += rotationSpeed;
        currentSway += swaySpeed;
        if(currentSway > 1)
        {
            currentSway = 0;
        }
        transform.localRotation = Quaternion.Euler(0, currentRotation, 0);
        transform.localPosition = new Vector3(transform.localPosition.x, startY + curve.Evaluate(currentSway), transform.localPosition.z);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided");
            healthModel = collision.GetComponent<HealthModel>();
            if(healthModel == null)
            {
                healthModel = collision.transform.parent.transform.parent.GetComponent<HealthModel>();
            }
            healthModel.maxHealth += 4;
            healthModel.Heal(healthModel.maxHealth - healthModel.currentHealth);
            healthModel.healthBar.HeartContainerPickedUp();
            healthModel.healthBar.UpdateHealthBar(healthModel.currentHealth);
            Destroy(gameObject);
        }
    }
}
