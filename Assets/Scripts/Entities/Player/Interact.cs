using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] GameObject icon;
    IInteractable currentInteractable;
    private void Awake()
    {
        icon.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable) && interactable.CanBeInteractedWith())
        {
            currentInteractable = interactable;
            icon.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            currentInteractable = null;
            icon.SetActive(false);
        }
    }

    public void OnInteract()
    {
        currentInteractable?.Interact();
        if (!currentInteractable.CanBeInteractedWith())
        {
            currentInteractable = null;
            icon.SetActive(false);
        }
        
    }
}
