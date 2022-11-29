using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RockCrab : MonoBehaviour
{
    public enum Behavior
    {
        NONE,
        WALKING,
        CLAWATTACK
    }
    GoBackAndForth movement;
    [SerializeField] GameObject[] leftLeg;
    [SerializeField] GameObject[] rightLeg;
    [SerializeField] GameObject[] leftClaw;
    [SerializeField] GameObject[] rightClaw;
    [SerializeField] Animator[] clawAnimators; //0 == Left, 1 == Right

    Animator anim;
    Behavior behavior;
    Transform player;
    Timer attackCounter;
    [SerializeField]float attackRadius;
    //When animating, animate transforms, that have the body parts as children. The children then switch transform

    private void Awake()
    {
        movement = GetComponent<GoBackAndForth>();
        anim = GetComponent<Animator>();

        movement.OnAwake(false, false);
        player = Game.GetCurrentPlayer().transform;
        attackCounter = new Timer(()=> { }, 150, Timer.TimerBehavior.NONE);
        behavior = Behavior.WALKING;
    }

    private void Update()
    {
        if(behavior == Behavior.WALKING)
        {
            attackCounter.Increment();
            movement.OnUpdate();
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance < attackRadius && attackCounter.IsFull())
            {
                behavior = Behavior.CLAWATTACK;
                attackCounter.Reset();
                Attack();
            }
        }
    }
    public void Attack()
    {
        movement.Stop();
        float distanceLeftClaw = Vector2.Distance(leftClaw[0].transform.position, player.transform.position);
        float distanceRightClaw = Vector2.Distance(rightClaw[0].transform.position, player.transform.position);
        int index = distanceLeftClaw < distanceRightClaw ? 0 : 1;
        clawAnimators[index].SetTrigger("Attack");
        anim.SetBool("Walking", false);
    }


    public void AttackDone()
    {
        anim.SetBool("Walking", true);
        behavior = Behavior.WALKING;
    }
    private void FixedUpdate()
    {
        if(behavior == Behavior.WALKING)
        {
            movement.OnFixedUpdate();
        }
    }
    public void SetWalking(Behavior value)
    {
        behavior = value;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = behavior == Behavior.WALKING ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}