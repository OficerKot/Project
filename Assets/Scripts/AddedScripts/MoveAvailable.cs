using UnityEngine;

public class MoveAvailable : MonoBehaviour
{
    private Collider2D collider_;
    private CharacterMovement characterMovement;
    private DominoPart dominoScript;
    public char Direction;

    void Awake()
    {
        
        characterMovement = transform.parent.GetComponent<CharacterMovement>();
        collider_ = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Path")
        {
            //Debug.Log("Path found!");
            switch (Direction)
            {
                case 'U':
                    {
                        dominoScript = other.transform.GetComponent<DominoPart>();
                        Debug.Log($"DominoPart is {dominoScript}");
                        if (dominoScript.IsBeingPlaced() && !GameManager.Instance.WhatInHand())
                        {
                            characterMovement.up_available = true;
                            characterMovement.collision_up = other.gameObject;
                        }
                        break;
                    }
                case 'D':
                    {
                        dominoScript = other.transform.GetComponent<DominoPart>();
                        if (dominoScript.IsBeingPlaced() && !GameManager.Instance.WhatInHand())
                        {
                            characterMovement.down_available = true;
                            characterMovement.collision_down = other.gameObject;
                        }
                        break;
                    }
                case 'L':
                    {
                        dominoScript = other.transform.GetComponent<DominoPart>();
                        if (dominoScript.IsBeingPlaced() && !GameManager.Instance.WhatInHand())
                        {
                            characterMovement.left_available = true;
                            characterMovement.collision_left = other.gameObject;
                        }
                        break;
                    }
                case 'R':
                    {
                        dominoScript = other.transform.GetComponent<DominoPart>();
                        if (dominoScript.IsBeingPlaced() && !GameManager.Instance.WhatInHand())
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
                        if (!GameManager.Instance.WhatInHand())
                            characterMovement.up_available = false;
                        break;
                    }
                case 'D':
                    {
                        if (!GameManager.Instance.WhatInHand())
                            characterMovement.down_available = false;
                        break;
                    }
                case 'L':
                    {
                        if (!GameManager.Instance.WhatInHand())
                            characterMovement.left_available = false;
                        break;
                    }
                case 'R':
                    {
                        if (!GameManager.Instance.WhatInHand())
                            characterMovement.right_available = false;
                        break;
                    }
                default: break;
            }
        }
    }
}
