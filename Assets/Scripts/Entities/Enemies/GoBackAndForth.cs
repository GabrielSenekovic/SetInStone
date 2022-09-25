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
        VERTICAL = 1 << 1
    }
    [SerializeField] float movementSpeed;
    Rigidbody2D body;
    int direction;
    [SerializeField] Behavior behavior;
    Vector2 startPosition;
    [SerializeField] int distanceToTravel; //Goes out from the middle

    public void OnAwake()
    {
        startPosition = transform.position;
        body = GetComponent<Rigidbody2D>();
        ChooseRandomDir();
    }
    public void OnUpdate()
    {
        int x = behavior == Behavior.HORIZONTAL ? direction : 0;
        int y = behavior == Behavior.VERTICAL ? direction : 0;
        body.AddForce(new Vector2(x, y) * Time.deltaTime * movementSpeed, ForceMode2D.Impulse);
    }
    public void OnFixedUpdate()
    {
        CheckIfFinishedTravel();
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
    void FlipEntity()
    {
        transform.localScale = new Vector3(direction, 1, 1);
    }
    void CheckIfFinishedTravel()
    {
        int distanceTravelled = (int)(startPosition - new Vector2(transform.position.x, transform.position.y)).magnitude;
        int directionFromStart = (int)Mathf.Sign(transform.position.x - startPosition.x);
        if(distanceTravelled >= distanceToTravel && directionFromStart == direction )
        {
            ChangeDir();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        int x = behavior == Behavior.HORIZONTAL ? distanceToTravel : 0;
        int y = behavior == Behavior.VERTICAL ? distanceToTravel : 0;
        Gizmos.DrawLine(startPosition, new Vector2(startPosition.x + x, startPosition.y + y));
        Gizmos.DrawLine(startPosition, new Vector2(startPosition.x - x, startPosition.y - y));
        Gizmos.DrawSphere(new Vector2(startPosition.x + x, startPosition.y + y), 0.1f);
        Gizmos.DrawSphere(new Vector2(startPosition.x - x, startPosition.y - y), 0.1f);
    }
}
