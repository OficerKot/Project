using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] GameObject messageObj;
    TextMeshProUGUI messageText;

    private void Start()
    {
        messageText = messageObj.GetComponent<TextMeshProUGUI>();
    }
    public void ShowMessage(string text, float duration)
    {
        StartCoroutine(ShowText(text, duration));
    }

    IEnumerator ShowText(string text, float duration)
    {
        messageObj.SetActive(true);
        messageText.text = text;
        yield return new WaitForSeconds(duration);
        messageText.text = "";
        messageObj.SetActive(false);
    }
}
