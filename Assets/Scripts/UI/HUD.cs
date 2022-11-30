using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public HealthBar playerHealthBar;
    public CanvasGroup canvas;

    public Text currencyName;
    public Image currencyIcon;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }
    
    public void TurnOff()
    {
        playerHealthBar.ResetHealthbar();
    }
    public void UpdateCurrency(Inventory.Currency currency, int amount)
    {
        //Check if current area uses this currency. If so, show
        currencyName.text = currency.ToString() + ": " + amount.ToString();
        currencyIcon.enabled = true;
    }
}
