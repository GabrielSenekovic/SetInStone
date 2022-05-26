using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HeartContainer : MonoBehaviour
{
    HealthModel healthModel;

    public float swaySpeed;

    public float currentSway;
    public AnimationCurve curve;

    public ParticleSystem particles;

    float startY;

    Animator anim;

    bool destroy = false;

    Transform playerTemp;

    private void Start() 
    {
        startY = transform.localPosition.y;
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate() 
    {
        currentSway += swaySpeed;
        if(currentSway > 1)
        {
            currentSway = 0;
        }
        transform.localPosition = new Vector3(transform.localPosition.x, startY + curve.Evaluate(currentSway), transform.localPosition.z);
        if(destroy && particles.particleCount == 0)
        {
        Destroy(gameObject);
        }
    }
    public void Disappear()
    {
        destroy = true;
        Game.Instance.cinemachineVirtualCamera.Follow = playerTemp;
        Game.Instance.cinemachineVirtualCamera.gameObject.GetComponent<CinemachineConfiner>().m_ConfineScreenEdges = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && swaySpeed > 0)
        {
            healthModel = collision.GetComponent<HealthModel>();
            if(healthModel == null)
            {
                healthModel = collision.transform.parent.GetComponent<HealthModel>();
            }
            healthModel.maxHealth += 4;
            healthModel.Heal(healthModel.maxHealth - healthModel.currentHealth);
            healthModel.healthBar.HeartContainerPickedUp();
            healthModel.healthBar.UpdateHealthBar(healthModel.currentHealth);
            anim.SetTrigger("Take");
            swaySpeed = 0;
            anim.speed = 2;
            Game.Instance.cinemachineVirtualCamera.gameObject.GetComponent<CinemachineConfiner>().m_ConfineScreenEdges = false;
            StartCoroutine("TakeCutscene");
        }
    }
    public IEnumerator TakeCutscene()
    {
        playerTemp = Game.Instance.cinemachineVirtualCamera.Follow;

        Game.Instance.cinemachineVirtualCamera.Follow = transform;
        yield return new WaitForSeconds(1.0f);
        anim.speed = 1;
        anim.SetTrigger("Take");
    }
}
