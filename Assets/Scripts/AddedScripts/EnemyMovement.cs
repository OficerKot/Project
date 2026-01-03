using System;
using UnityEngine;

public class EnemyMovement : PauseBehaviour
{
    [SerializeField] float walkSpeed = 7f;
    [SerializeField] private bool isMoving = false;
    orient orientation;
    bool isActive = true;
    private Transform targetPosition;
    private Vector3 direction;
    public LayerMask whatAllowsMovement;

    enum orient
    {
        vert, hor
    }

    void Awake()
    {
        targetPosition = this.gameObject.transform;
        direction = new Vector3(0, 0, 0);
        orientation = (orient)UnityEngine.Random.Range(0, 2);
        if (orientation == orient.vert)
        {
            direction = new Vector3(0, 1, 0);
        }
        else if (orientation == orient.hor)
        {
            direction = new Vector3(1, 0, 0);
        }
        EnemyMove();
    }
    public override void OnGamePaused(bool isGamePaused)
    {
        isActive = !isGamePaused;
    }

    void EnemyMove()
    {
        if (!isActive) return;
        if (!isMoving &&
             Physics2D.OverlapCircle(targetPosition.position + direction, .01f, whatAllowsMovement))
        {
            targetPosition.position += direction;
            isMoving = true;
        }
        else if (!Physics2D.OverlapCircle(targetPosition.position + direction, .01f, whatAllowsMovement))
        {
            direction *= -1;
            targetPosition.position += direction;
            isMoving = true;
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition.position, walkSpeed * Time.fixedDeltaTime);
            if (transform.position.x == targetPosition.position.x && transform.position.y == targetPosition.position.y)
            {
                isMoving = false;
                EnemyMove();
            }
        }
    }
}
