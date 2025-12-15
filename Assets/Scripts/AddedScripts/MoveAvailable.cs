using UnityEngine;

public class MoveAvailable : MonoBehaviour
{
    private Collider2D collider_;
    private CharacterMovement charMov;
    private DominoPart dominoScript;

    void Awake()
    {
        charMov = transform.parent.GetComponent<CharacterMovement>();
        collider_ = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Path")
        {
            Debug.Log("There's a path");
            //charMov.SetMovable(true);
        }
        else
        {
            Debug.Log("No path, heading back...");
            //charMov.SetMovable(false);
        }
    }
}
