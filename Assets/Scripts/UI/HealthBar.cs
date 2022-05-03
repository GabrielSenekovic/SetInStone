using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject HeartPrefab;

    List<Image> hearts = new List<Image>();
    public void Initialize(int health, int currentHealth)
    {
        for(int i = 0; i < (health/4); i++)
        {
            GameObject temp = Instantiate(HeartPrefab, transform);
            hearts.Add(temp.transform.GetChild(1).GetComponent<Image>());
            SetHeartFillAmount(currentHealth, i);
        }
    }

    public void UpdateHealthBar(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            SetHeartFillAmount(currentHealth, i);
        }
    }

    public void HeartContainerPickedUp()
    {
        GameObject temp = Instantiate(HeartPrefab, transform);
        hearts.Add(temp.transform.GetChild(1).GetComponent<Image>());
    }

    void SetHeartFillAmount(int currentHealth, int i)
    {
        hearts[i].fillAmount = (float)(currentHealth - (i * 4.0f)) / 4.0f;
    }

    public void ResetHealthbar() 
    {
        for(int i = 0; i < hearts.Count; i++)
        {
            Destroy(hearts[i].transform.parent.gameObject);
        }
        hearts.Clear();
    }
}
