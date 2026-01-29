using System.Collections;
using UnityEngine;

/// <summary>
/// Скрипт, отвечающий за плавное перемещение игровой камеры за игроком.
/// Метод FixedUpdate() 50 раз за секунду вызывает метод SmoothDamp класса Vector3, чтобы плавно передвинуть камеру на позицию игрока
/// 
/// Дополнительная настройка: Указать игрока в поле targetPosition и скорость смещения в поле damping.
/// </summary>
public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;
    private Vector3 targetPosition;

    public Transform target;
    public Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        targetPosition.z = transform.position.z;
        transform.position = targetPosition;
    }

    void FixedUpdate()
    {
        targetPosition = target.position;
        targetPosition.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, damping);
    }
}
