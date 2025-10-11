using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] public bool up_available;
    [SerializeField] public bool down_available;
    [SerializeField] public bool left_available;
    [SerializeField] public bool right_available;
    private float walkSpeed = 7f;
    private bool isMoving = false;
    private Vector2 targetPosition;

    public void OnWalk_Up()
    {
        Debug.Log("Trying to walk upwards");
        if (up_available && !isMoving)
        {
            Debug.Log("Walked!");
            targetPosition = new Vector2(transform.position.x, transform.position.y + Vector2.up.y);
            isMoving = true;
        }
    }

    public void OnWalk_Down()
    {
        Debug.Log("Trying to walk downwards");
        if (down_available && !isMoving)
        {
            Debug.Log("Walked!");
            targetPosition = new Vector2(transform.position.x, transform.position.y + Vector2.down.y);
            isMoving = true;
        }
    }

    public void OnWalk_Left()
    {
        Debug.Log("Trying to walk left");
        if (left_available && !isMoving)
        {
            Debug.Log("Walked!");
            targetPosition = new Vector2(transform.position.x + Vector2.left.x, transform.position.y);
            isMoving = true;
        }
    }

    public void OnWalk_Right()
    {
        Debug.Log("Trying to walk right");
        if (right_available && !isMoving)
        {
            Debug.Log("Walked!");
            targetPosition = new Vector2(transform.position.x + Vector2.right.x, transform.position.y);
            isMoving = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, walkSpeed*Time.fixedDeltaTime);
            if (transform.position.x == targetPosition.x && transform.position.y == targetPosition.y)
                isMoving = false;
        }
    }
}
