using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : MonoBehaviour
{
    public float maxLength;
    public float fallSpeed;
    public LineRenderer lineRenderer;
    public BoxCollider2D coll;
    public LayerMask mask;
    
    float lastLength;
    public GameObject splashEffectObject;

    void Start()
    {
        lineRenderer.positionCount = 2;
        for(int i = 0; i < 2; i++)
        {
            lineRenderer.SetPosition(i, transform.position);
        }
    }
    private void Update() 
    {
        GetCurrentLength(out float currentMaxLength);
        CalculateActualLength(currentMaxLength, out float currentLength);
        ResizeLine(currentLength);
        RescaleCollider(currentLength);
        CheckForSplashEffect(currentLength, currentMaxLength);
    }

    void GetCurrentLength(out float currentMaxLength)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, maxLength, mask);
        if(hit)
        {
            currentMaxLength = Vector3.Distance(transform.position, hit.point);
        }
        else
        {
            currentMaxLength = maxLength;
        }
    }
    void CalculateActualLength(float currentMaxLength, out float currentLength)
    {
        currentLength = lastLength + Time.deltaTime * fallSpeed;
        currentLength = Mathf.Clamp(currentLength, 0, currentMaxLength);
        lastLength = currentLength;
    }
    void ResizeLine(float currentLength)
    {
        lineRenderer.SetPosition(1, transform.position - Vector3.up * currentLength);
        lineRenderer.material.SetFloat("Length", currentLength);
    }
    void RescaleCollider(float currentLength)
    {
        coll.size = new Vector2(coll.size.x, currentLength);
        coll.offset = new Vector2(0, -currentLength/2);
    }
    void CheckForSplashEffect(float currentLength, float currentMaxLength)
    {
        splashEffectObject.gameObject.SetActive(currentLength >= currentMaxLength);
        splashEffectObject.transform.position = transform.position - Vector3.up * currentLength;
    }
}
