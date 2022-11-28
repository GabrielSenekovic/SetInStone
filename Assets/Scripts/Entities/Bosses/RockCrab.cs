using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCrab : MonoBehaviour
{
    public enum Behavior
    {
        NONE,
        WALKING,
        CLAWATTACK
    }
    GoBackAndForth movement;
    [SerializeField] Rigidbody2D[] leftLeg;
    [SerializeField] Rigidbody2D[] rightLeg;
    [SerializeField] Rigidbody2D[] leftClaw;
    [SerializeField] Rigidbody2D[] rightClaw;
    Animator anim;
    Animator[] clawAnimators; //0 == Left, 1 == Right
    Behavior behavior;
    Transform player;
    Timer attackCounter;
    //When animating, animate transforms, that have the body parts as children. The children then switch transform

    private void Awake()
    {
        movement = GetComponent<GoBackAndForth>();
        movement.OnAwake(false, false);
        player = Game.GetCurrentPlayer().transform;
        attackCounter = new Timer(null, 50, Timer.TimerBehavior.NONE);
    }

    private void Update()
    {
        if(behavior == Behavior.WALKING)
        {
            attackCounter.Increment();
            movement.OnUpdate();
            if(Vector2.Distance(transform.position, player.position) < 4 && attackCounter.IsFull())
            {
                behavior = Behavior.CLAWATTACK;
                attackCounter.Reset();
            }
        }
    }
    public void Attack()
    {
        clawAnimators[Random.Range(0, clawAnimators.Length)].SetTrigger("Attack");
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
}