using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ParticleLight : MonoBehaviour
{
    ParticleSystem particleSystem;

    public Light2D lightPrefab;

    public int amountOfLights;
    List<Light2D> lights = new List<Light2D>();

    
    private void Start() 
    {
        particleSystem = GetComponent<ParticleSystem>();
        for(int i = 0; i < amountOfLights; i++)
        {
            lights.Add(Instantiate(lightPrefab, transform.position, Quaternion.identity, transform));
            lights[i].intensity = 0;
        }
    }
    private void Update() 
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.particleCount];
        particleSystem.GetParticles(particles);
        for(int i = 0; i < lights.Count; i++)
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
