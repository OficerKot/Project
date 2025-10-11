using UnityEngine;

public class MoveAvailable : MonoBehaviour
{
    private Collider2D collider;
    private CharacterMovement characterMovement;
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
                        characterMovement.up_available = true;
                        break;
                    }
                case 'D':
                    {
                        characterMovement.down_available = true;
                        break;
                    }
                case 'L':
                    {
                        characterMovement.left_available = true;
                        break;
                    }
                case 'R':
                    {
                        characterMovement.right_available = true;
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
