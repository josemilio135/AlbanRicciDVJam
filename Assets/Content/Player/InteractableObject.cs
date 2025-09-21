using UnityEngine;

public enum InteractableType
{
    Interact,
    Pickup
}

public class InteractableObject : MonoBehaviour
{
    [Header("Interaction Settings")]
    public InteractableType objectType = InteractableType.Interact;
    
    [Header("Optional Settings")]
    public string interactionPrompt = "Press F to interact";
    
    void Start()
    {
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }
    
    public virtual void OnInteract()
    {
        Debug.Log($"Interacting with {gameObject.name} (Type: {objectType})");
    }
}
