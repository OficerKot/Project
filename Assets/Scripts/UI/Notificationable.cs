using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Для объектов, которые в зависимости от игровых событий могут быть визуально выделены для обращения внимания пользователя.
/// </summary>
public class Notificationable : MonoBehaviour
{
    public GameObject notification;


    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(HideNotification);
    }

    /// <summary>
    /// Отображение объекта-флага
    /// </summary>
    public void ShowNotification()
    {
        notification.SetActive(true);
    }

    /// <summary>
    /// Скрытие объекта-флага
    /// </summary>
    public void HideNotification()
    {
        notification.SetActive(false);
    }

}
