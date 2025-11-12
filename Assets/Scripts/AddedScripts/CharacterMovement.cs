using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] public bool up_available;
    [SerializeField] public bool down_available;
    [SerializeField] public bool left_available;
    [SerializeField] public bool right_available;
    [SerializeField] public GameObject collision_up;
    [SerializeField] public GameObject collision_down;
    [SerializeField] public GameObject collision_left;
    [SerializeField] public GameObject collision_right;
    private float walkSpeed = 7f;
    [SerializeField] private bool isMoving = false;
    private Vector2 targetPosition;

    void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    void OnGameStateChanged(bool isGameOver)
    {
        enabled = !isGameOver;
    }
    public void OnWalk_Up()
    {
        Debug.Log("Trying to walk upwards");
        if (up_available && !isMoving)
        {
            Hunger.Instance.MakeStep();
            Clock.Instance.TimeTick();
            targetPosition = new Vector2(transform.position.x, collision_up.transform.position.y);
            isMoving = true;
        }
    }

    public void OnWalk_Down()
    {
        Debug.Log("Trying to walk downwards");
        if (down_available && !isMoving)
        {
            Hunger.Instance.MakeStep();
            Clock.Instance.TimeTick();
            targetPosition = new Vector2(transform.position.x, collision_down.transform.position.y);
            isMoving = true;
        }
    }

    public void OnWalk_Left()
    {
        Debug.Log("Trying to walk left");
        if (left_available && !isMoving)
        {
            Hunger.Instance.MakeStep();
            Clock.Instance.TimeTick();
            targetPosition = new Vector2(collision_left.transform.position.x, transform.position.y);
            isMoving = true;
        }
    }

    public void OnWalk_Right()
    {
        Debug.Log("Trying to walk right");
        if (right_available && !isMoving)
        {
            Hunger.Instance.MakeStep();
            Clock.Instance.TimeTick();
            targetPosition = new Vector2(collision_right.transform.position.x, transform.position.y);
            isMoving = true;
        }
    }

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
