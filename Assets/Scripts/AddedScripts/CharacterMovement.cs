using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : PauseBehaviour
{
    [SerializeField] float walkSpeed = 7f;
    [SerializeField] private bool isMoving = false;
    bool isActive = true, movementIsAlloved = true;
    [SerializeField] public Transform targetPosition;
    public LayerMask whatAllowsMovement;
    public static event Action<bool> OnMovementAttempted;
    private CharacterStates charStates;


    private void Awake()
    {
        int x = UnityEngine.Random.Range(-50, 51);
        int y = UnityEngine.Random.Range(-50, 51);
        transform.position = new Vector3(x, y, 0);
        targetPosition.parent = null;
        charStates = GetComponent<CharacterStates>();
    }
    public override void OnGamePaused(bool isGamePaused)
    {
        isActive = !isGamePaused;
    }
    //public void OnWalk_Up()
    //{
    //    if (!isActive) return;
    //    if (!isMoving && !GameManager.Instance.WhatInHand() && 
    //        Physics2D.OverlapCircle(targetPosition.position + new Vector3(0, 1, 0), .01f, whatAllowsMovement))
    //    {
    //        Clock.Instance.TimeTick();
    //        targetPosition.position += new Vector3(0, 1, 0);
    //        isMoving = true;
    //    }
    //}

    //public void OnWalk_Down()
    //{
    //    if (!isActive) return;
    //    if (!isMoving && !GameManager.Instance.WhatInHand() &&
    //        Physics2D.OverlapCircle(targetPosition.position + new Vector3(0, -1, 0), .01f, whatAllowsMovement))
    //    {
    //        Clock.Instance.TimeTick();
    //        targetPosition.position += new Vector3(0, -1, 0);
    //        isMoving = true;
    //    }
    //}

    //public void OnWalk_Left()
    //{
    //    if(CanMove(Vector3.left))
    //    {
    //        Clock.Instance.TimeTick();
    //        targetPosition.position += new Vector3(-1, 0, 0);
    //        isMoving = true;
    //    }
    //}

    //public void OnWalk_Right()
    //{
    //    if(CanMove(Vector3.right))
    //    {
    //        Clock.Instance.TimeTick();
    //        targetPosition.position += Vector3.right;
    //        isMoving = true;
    //    }
    //} 
    
    //
    public void OnWalk(InputValue inputValue) //Ќичего не мен€ла, только вынесла в 1 метод
    {
        if (!isActive || isMoving) return;
        Vector2 destination = inputValue.Get<Vector2>();
        if(AvailableDestinaton(destination))
        {
            Clock.Instance.TimeTick();
            EnemyManager.Instance.MakeStep();
            Move(destination);
        }
        AudioManager.Instance.Step();
    }
    void Move(Vector2 destination)
    {
        if (movementIsAlloved)
        {
            if(OnMovementAttempted != null) OnMovementAttempted.Invoke(true);
            targetPosition.position += new Vector3(destination.x, destination.y, 0);
            isMoving = true;
        }
        else
        {
            if (OnMovementAttempted != null) OnMovementAttempted.Invoke(false);
            charStates.CheckState();
        }
    }
    public void ControlMovement(bool val)
    {
        movementIsAlloved = val;
    }
    bool AvailableDestinaton(Vector3 destination)
    {
        return Mathf.Abs(destination.x + destination.y) == 1 && !GameManager.Instance.WhatInHand() && Physics2D.OverlapCircle(targetPosition.position + destination, .01f, whatAllowsMovement);
    }
    void FixedUpdate()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition.position, walkSpeed*Time.fixedDeltaTime);
            if (transform.position.x == targetPosition.position.x && transform.position.y == targetPosition.position.y)
            {
                isMoving = false;
                charStates.CheckState();
                SignsManager.Instance.CastSignsRays();
            }
        }
    }
}
