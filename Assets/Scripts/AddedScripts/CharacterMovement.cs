using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : PauseBehaviour
{
    [SerializeField] float walkSpeed = 7f;
    [SerializeField] private bool isMoving = false;
    bool isActive = true;
    [SerializeField] public Transform targetPosition;
    public LayerMask whatAllowsMovement;

    private void Awake()
    {
        targetPosition.parent = null;
    }
    public override void OnGamePaused(bool isGamePaused)
    {
        isActive = !isGamePaused;
    }
    public void OnWalk_Up()
    {
        if (!isActive) return;
        //Debug.Log("Throwing upwards");
        if (!isMoving && !GameManager.Instance.WhatInHand() && 
            Physics2D.OverlapCircle(targetPosition.position + new Vector3(0, 1, 0), .01f, whatAllowsMovement))
        {
            Clock.Instance.TimeTick();
            targetPosition.position += new Vector3(0, 1, 0);
            isMoving = true;
        }
        //else
        //    targetPosition.position = gameObject.transform.position;
    }

    public void OnWalk_Down()
    {
        if (!isActive) return;
        //Debug.Log("Throwing downwards");
        if (!isMoving && !GameManager.Instance.WhatInHand() &&
            Physics2D.OverlapCircle(targetPosition.position + new Vector3(0, -1, 0), .01f, whatAllowsMovement))
        {
            Clock.Instance.TimeTick();
            targetPosition.position += new Vector3(0, -1, 0);
            isMoving = true;
        }
        //else
        //    targetPosition.position = gameObject.transform.position;
    }

    public void OnWalk_Left()
    {
        if (!isActive) return;
        //Debug.Log("Throwing left");
        if (!isMoving && !GameManager.Instance.WhatInHand() &&
             Physics2D.OverlapCircle(targetPosition.position + new Vector3(-1, 0, 0), .01f, whatAllowsMovement))
        {
            Clock.Instance.TimeTick();
            targetPosition.position += new Vector3(-1, 0, 0);
            isMoving = true;
        }
        //else
        //    targetPosition.position = gameObject.transform.position;
    }

    public void OnWalk_Right()
    {
        if (!isActive) return;
        //Debug.Log("Throwing right");
        if (!isMoving && !GameManager.Instance.WhatInHand() &&
            Physics2D.OverlapCircle(targetPosition.position + new Vector3(1, 0, 0), .01f, whatAllowsMovement))
        {
            Clock.Instance.TimeTick();
            targetPosition.position += new Vector3(1, 0, 0);
            isMoving = true;
        }
        //else
        //    targetPosition.position = gameObject.transform.position;
    }
    
    void FixedUpdate()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition.position, walkSpeed*Time.fixedDeltaTime);
            if (transform.position.x == targetPosition.position.x && transform.position.y == targetPosition.position.y)
            {
                isMoving = false;
            }
        }
    }
}
