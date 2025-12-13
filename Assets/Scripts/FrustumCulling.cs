using UnityEngine;

public class FrustumCulling : MonoBehaviour
{
    private Camera targetCamera;
    private Renderer targetRenderer;
    private Collider2D col;

    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }
        col = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        Vector2 cameraMin = targetCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector2 cameraMax = targetCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Rect cameraViewRect = new Rect(cameraMin.x, cameraMin.y, cameraMax.x - cameraMin.x, cameraMax.y - cameraMin.y);

        Bounds objectBounds = targetRenderer.bounds;
        Rect objectRect = new Rect(objectBounds.min.x, objectBounds.min.y, objectBounds.size.x, objectBounds.size.y);

        bool isVisible = cameraViewRect.Overlaps(objectRect);
        targetRenderer.enabled = isVisible;
        col.enabled = isVisible;
    }
}
