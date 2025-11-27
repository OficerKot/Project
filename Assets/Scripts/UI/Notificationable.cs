using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notificationable : MonoBehaviour
{
    public GameObject notification;


    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(HideNotification);
    }
    public void ShowNotification()
    {
        notification.SetActive(true);
    }

    public void HideNotification()
    {
        notification.SetActive(false);
    }

}
