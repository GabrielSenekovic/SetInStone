using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public HealthBar playerHealthBar;
    public CanvasGroup canvas;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }
    
    public void TurnOff()
    {
        playerHealthBar.ResetHealthbar();
    }
}
