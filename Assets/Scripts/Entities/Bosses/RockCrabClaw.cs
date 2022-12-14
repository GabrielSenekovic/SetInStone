using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RockCrabClaw : MonoBehaviour
{
    [SerializeField] RockCrab crab;
    CinemachineImpulseSource impulseSource;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void AttackDone()
    {
        crab.AttackDone();
    }
    public void TriggerScreenShake()
    {
        impulseSource.GenerateImpulse();
    }
}
