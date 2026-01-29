using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Иконки обелисков, отображающиеся в правом верхнем углу. Позволяют отслеживать информацию о количестве собранных обелисков. 
/// Во время сбора обелиска подсвечивается иконка с его цветом.
/// </summary>
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

    /// <summary>
    /// Подсветка иконки.
    /// </summary>
    /// <param name="c"></param>
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
