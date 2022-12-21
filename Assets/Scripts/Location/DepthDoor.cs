using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DepthDoor : MonoBehaviour, IInteractable
{
    [SerializeField] bool unlocked;
    [SerializeField] List<GameObject> outside;
    [SerializeField] List<GameObject> inside;
    public bool CanBeInteractedWith() => unlocked;

    public void Interact()
    {
        if(outside[0].activeSelf)
        {
            outside.ForEach(g => g.SetActive(false));
            inside.ForEach(g => Activate(g));
        }
        else
        {
            outside.ForEach(g => Activate(g));
            inside.ForEach(g => g.SetActive(false));
        }
    }
    public void Activate(GameObject g)
    {
        g.SetActive(true);
        if(g.TryGetComponent(out PolygonCollider2D collider) && g.CompareTag("PassThrough"))
        {
            Game.SetCameraCollider(collider);
        }
    }
}
