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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            Debug.Log("There's a path");
            //charMov.SetMovable(true);
=======
<<<<<<< Updated upstream
=======
>>>>>>> Stashed changes
<<<<<<< Updated upstream
            Debug.Log("Path found!");
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
            switch (Direction)
            {
                case 'U':
                    {
                        dominoScript = other.transform.GetComponent<DominoPart>();
                        if (dominoScript.IsBeingPlaced())
                        {
                            characterMovement.up_available = true;
                            characterMovement.collision_up = other.gameObject;
                        }
                        break;
                    }
                case 'D':
                    {
                        dominoScript = other.transform.GetComponent<DominoPart>();
                        if (dominoScript.IsBeingPlaced())
                        {
                            characterMovement.down_available = true;
                            characterMovement.collision_down = other.gameObject;
                        }
                        break;
                    }
                case 'L':
                    {
                        dominoScript = other.transform.GetComponent<DominoPart>();
                        if (dominoScript.IsBeingPlaced())
                        {
                            characterMovement.left_available = true;
                            characterMovement.collision_left = other.gameObject;
                        }
                        break;
                    }
                case 'R':
                    {
                        dominoScript = other.transform.GetComponent<DominoPart>();
                        if (dominoScript.IsBeingPlaced())
                        {
                            characterMovement.right_available = true;
                            characterMovement.collision_right = other.gameObject;
                        }
                        break;
                    }
                default: break;
            }
>>>>>>> Stashed changes
        }
        else
        {
            Debug.Log("No path, heading back...");
            //charMov.SetMovable(false);
        }
    }
}
