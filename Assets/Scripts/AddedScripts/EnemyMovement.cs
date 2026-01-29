using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Скрипт управляет передвижением врага.
/// Как только включён, скрипт определяет, в какую сторону враг способен двигаться. Метод EnemyDirection() проверяет, способен ли враг двигаться в ту же сторону, куда он движется сейчас. Если нет - он движется в обратную.
/// Метод TryToBite() позволяет врагу попробовать укусить игрока и тем самым снизить его уровень здоровья.
/// 
/// Дополнительная настройка: Нужно указать в скрипте пустой объект TargetPosition, являющийся дочерним объектом врага. 
/// Также выбрать два слоя: whatAllowsMovement - поля, которые позволяют врагу ходить, playerLayer - слой игрока, по которому определяется способность укусить игрока.
/// </summary>
public class EnemyMovement : PauseBehaviour
{
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] private bool isMoving = false;
    orient orientation;
    bool isActive = true;
    [SerializeField] private Transform targetPosition;
    private Vector3 direction;
    private SpriteRenderer sprRen;
    public LayerMask whatAllowsMovement;
    public LayerMask playerLayer;

    enum orient
    {
        vert, hor
    }

    void Start()
    {
        EnemyManager.Instance.PutInList(this);
        sprRen = GetComponent<SpriteRenderer>();
        targetPosition.position = this.gameObject.transform.position;
        targetPosition.parent = null;
        direction = new Vector3(0, 0, 0);

        if (Physics2D.OverlapCircle(transform.position + new Vector3(0, 1, 0), .01f, whatAllowsMovement) ||
            Physics2D.OverlapCircle(transform.position + new Vector3(0, -1, 0), .01f, whatAllowsMovement))
        {
            orientation = orient.vert;
        }
        else if (Physics2D.OverlapCircle(transform.position + new Vector3(1, 0, 0), .01f, whatAllowsMovement) ||
            Physics2D.OverlapCircle(transform.position + new Vector3(-1, 0, 0), .01f, whatAllowsMovement))
        {
            orientation = orient.hor;
        }
        //Debug.Log($"orientation is {orientation}");
        if (orientation == orient.vert)
        {
            direction = new Vector3(0, 1, 0);
        }
        else if (orientation == orient.hor)
        {
            direction = new Vector3(1, 0, 0);
        }
    }
    public override void OnGamePaused(bool isGamePaused)
    {
        isActive = !isGamePaused;
    }

    void EnemyDirection()
    {
        if (!isActive) return;
        if (!isMoving &&
             Physics2D.OverlapCircle(targetPosition.position + direction, .01f, whatAllowsMovement))
        {
            targetPosition.position += direction; 
        }
        else if (!Physics2D.OverlapCircle(targetPosition.position + direction, .01f, whatAllowsMovement))
        {
            direction *= -1;
            targetPosition.position += direction;
            sprRen.flipX = !sprRen.flipX;
        }
    }
    public void SetIsMoving(bool value)
    {
        isMoving = value;
    }
    void TryToBite()
    {
        if (Physics2D.OverlapCircle(transform.position, .01f, playerLayer))
        {
            Hunger.Instance.CallHungerDown();
            Debug.Log("Bitten");
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition.position, walkSpeed*Time.fixedDeltaTime);
            if (transform.position == targetPosition.position)
            {
                isMoving = false;
                EnemyDirection();
                TryToBite();
            }
        }
    }
}
