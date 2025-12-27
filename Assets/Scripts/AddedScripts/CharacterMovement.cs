using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 7f;
    [SerializeField] private bool isMoving = false;
    bool canMove = true;
    [SerializeField] public Transform targetPosition;
    public LayerMask whatAllowsMovement;

    private void Awake()
    {
        targetPosition.parent = null;
    }
    void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;

    }
    void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }
    void OnGameStateChanged(bool isGamePaused)
    {
        canMove = !isGamePaused;
    }
    public void OnWalk_Up()
    {
        if (!canMove) return;
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
        if (!canMove) return;
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
        if (!canMove) return;
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
        if (!canMove) return;
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
