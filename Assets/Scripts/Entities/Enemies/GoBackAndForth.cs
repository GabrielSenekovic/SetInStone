using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[ExecuteAlways]
public class GoBackAndForth : MonoBehaviour
{
    [Flags]
    enum Behavior
    {
        NONE = 0, //Doesn't move
        HORIZONTAL = 1, //Goes back and forth horizontally
        VERTICAL = 1 << 1,
        FOLLOWPLAYER = 1 << 2 //Switch direction towards player
    }
    [SerializeField] float movementSpeed;
    Rigidbody2D body;
    int direction;
    bool pushable;
    bool turnAround;
    [SerializeField] Behavior behavior;
    Vector2 startPosition;
    [SerializeField] int distanceToTravel; //Goes out from the middle
    GameObject player;

    public void OnAwake(bool pushable, bool turnAround)
    {
        startPosition = transform.position;
        body = GetComponent<Rigidbody2D>();
        ChooseRandomDir();
        this.pushable = pushable;
        this.turnAround = turnAround;
        if(!pushable)
        {
            body.bodyType = RigidbodyType2D.Kinematic;
        }
    }
    private void Start()
    {
        player = Game.GetCurrentPlayer();
    }
    public void OnUpdate()
    {
        int x = behavior.HasFlag(Behavior.HORIZONTAL) ? direction : 0;
        int y = behavior.HasFlag(Behavior.VERTICAL) ? direction : 0;
        if(pushable)
        {
            body.AddForce(new Vector2(x, y) * Time.deltaTime * movementSpeed, ForceMode2D.Impulse);
        }
        else
        {
            body.velocity = new Vector2(x, y) * Time.deltaTime * movementSpeed;
        }
    }
    public void OnFixedUpdate()
    {
        if(behavior.HasFlag(Behavior.FOLLOWPLAYER))
        {
            FollowPlayer();
        }
        else if(CheckIfFinishedTravel())
        {
            ChangeDir();
        }
    }
    public Vector2 GetDirection()
    {
        int x = behavior == Behavior.HORIZONTAL ? direction : 0;
        int y = behavior == Behavior.VERTICAL ? direction : 0;
        return new Vector2(x, y);
    }

    void ChooseRandomDir()
    {
        int[] dirs = new int[2]{ -1,1};
        direction = dirs[UnityEngine.Random.Range(0, 1)];
        FlipEntity();
    }
    void ChangeDir()
    {
        body.velocity = Vector2.zero;
        direction = direction == 1 ? -1 : 1;
        FlipEntity();
    }
    void ChangeDir(int dir)
    {
        body.velocity = Vector2.zero;
        direction = dir;
        FlipEntity();
    }
    void FlipEntity()
    {
        if(turnAround)
        {
            transform.localScale = new Vector3(direction, 1, 1);
        }
    }
    void FollowPlayer()
    {
        //Change dir toward player if the current dir is wrong
        if (CheckIfFinishedTravel())
        {
            direction = 0;

            int directionToPlayer = (int)Mathf.Sign((player.transform.position - transform.position).x);
            int directionFromStart = (int)Mathf.Sign(transform.position.x - startPosition.x);
            if (directionToPlayer != directionFromStart)
            {
                ChangeDir(directionToPlayer);
            }
        }
        else if(direction != 0)
        {
            direction = (int)Mathf.Sign((player.transform.position - transform.position).x);
        }
    }
    bool CheckIfFinishedTravel()
    {
        int distanceTravelled = (int)(startPosition - new Vector2(transform.position.x, transform.position.y)).magnitude;
        int directionFromStart = (int)Mathf.Sign(transform.position.x - startPosition.x);
        return distanceTravelled >= distanceToTravel && (directionFromStart == direction || direction == 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        int x = behavior.HasFlag(Behavior.HORIZONTAL) ? distanceToTravel : 0;
        int y = behavior.HasFlag(Behavior.VERTICAL) ? distanceToTravel : 0;
#if UNITY_EDITOR
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + x, transform.position.y + y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x - x, transform.position.y - y));
        Gizmos.DrawSphere(new Vector2(transform.position.x + x, transform.position.y + y), 0.1f);
        Gizmos.DrawSphere(new Vector2(transform.position.x - x, transform.position.y - y), 0.1f);
#endif
    }
}
