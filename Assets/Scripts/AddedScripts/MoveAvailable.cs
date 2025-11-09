using UnityEngine;

public class MoveAvailable : MonoBehaviour
{
    private Collider2D collider;
    private CharacterMovement characterMovement;
    private DominoBone dominoScript;
    public char Direction;

    void Awake()
    {
        
        characterMovement = this.transform.parent.GetComponent<CharacterMovement>();
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Path")
        {
            switch (Direction)
            {
                case 'U':
                    {
                        dominoScript = other.transform.GetComponent<DominoBone>();
                        if (dominoScript.IsBeingPlaced())
                        {
                            characterMovement.up_available = true;
                            characterMovement.collision_up = other.gameObject;
                        }
                        break;
                    }
                case 'D':
                    {
                        dominoScript = other.transform.GetComponent<DominoBone>();
                        if (dominoScript.IsBeingPlaced())
                        {
                            characterMovement.down_available = true;
                            characterMovement.collision_down = other.gameObject;
                        }
                        break;
                    }
                case 'L':
                    {
                        dominoScript = other.transform.GetComponent<DominoBone>();
                        if (dominoScript.IsBeingPlaced())
                        {
                            characterMovement.left_available = true;
                            characterMovement.collision_left = other.gameObject;
                        }
                        break;
                    }
                case 'R':
                    {
                        dominoScript = other.transform.GetComponent<DominoBone>();
                        if (dominoScript.IsBeingPlaced())
                        {
                            characterMovement.right_available = true;
                            characterMovement.collision_right = other.gameObject;
                        }
                        break;
                    }
                default: break;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Path")
        {
            switch (Direction)
            {
                case 'U':
                    {
                        characterMovement.up_available = false;
                        break;
                    }
                case 'D':
                    {
                        characterMovement.down_available = false;
                        break;
                    }
                case 'L':
                    {
                        characterMovement.left_available = false;
                        break;
                    }
                case 'R':
                    {
                        characterMovement.right_available = false;
                        break;
                    }
                default: break;
            }
        }
    }
}
