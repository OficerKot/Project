using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectsInteraction : MonoBehaviour
{
    [SerializeField] public bool interacted = false;

    public void OnInteract()
    {
        Debug.Log("Interacted!");
        interacted = true;
        if (interacted)
            interacted = false;
    }

}
