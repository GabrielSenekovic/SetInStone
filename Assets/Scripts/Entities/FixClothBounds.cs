using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixClothBounds : MonoBehaviour 
{

    private SkinnedMeshRenderer[] Renderers;
    public Vector3 properBounds;

    void Awake() 
    {
        this.Renderers = this.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    void OnRenderObject () 
    {
        foreach (SkinnedMeshRenderer smr in this.Renderers) 
        {
            smr.localBounds = new Bounds(Vector3.zero, properBounds); // Paste your meshes real bounds here
        }
    }

}
