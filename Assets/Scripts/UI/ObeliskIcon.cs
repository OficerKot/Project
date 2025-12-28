using UnityEngine;
using UnityEngine.UI;

public class ObeliskIcon : MonoBehaviour
{
    [SerializeField] ObeliskColor color;

    void OnEnable ()
    {
        ObeliskManager.OnObeliskCollected += OnObeliskCollected;
    }
    private void OnDisable()
    {
        ObeliskManager.OnObeliskCollected -= OnObeliskCollected;
    }

    void OnObeliskCollected(ObeliskColor c)
    {
        if(c == color)
        {
            Debug.Log("Highlight");
            Image image = GetComponent<Image>();
            Color imageColor = image.color;
            imageColor = Color.white;
            image.color = imageColor;
        }
    }
}
