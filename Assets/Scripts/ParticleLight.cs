using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ParticleLight : MonoBehaviour
{
    ParticleSystem particleSystem;

    public Light2D lightPrefab;
    Light2D[] lights = new Light2D[50];

    
    private void Start() 
    {
        particleSystem = GetComponent<ParticleSystem>();
        for(int i = 0; i < lights.Length; i++)
        {
            lights[i] = Instantiate(lightPrefab, Vector2.zero, Quaternion.identity, transform);
            lights[i].intensity = 0;
        }
    }
    private void FixedUpdate() 
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.particleCount];
        particleSystem.GetParticles(particles);
        for(int i = 0; i < lights.Length; i++)
        {
            if(i >= particles.Length)
            {
                lights[i].intensity = 0;
            }
            else
            {
                lights[i].transform.position = particles[i].position;
                lights[i].intensity = 0.25f;
            }
        }
    }
}
