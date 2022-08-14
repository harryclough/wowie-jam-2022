using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public HealthController healthController;
    public Slider healthBar;
    public RectTransform healthBarTransform;
    public float maxWidthScale = 0.5f;

    void Start()
    {
        float width = healthController.maxHealth * maxWidthScale;

        healthBarTransform.sizeDelta = new Vector2(width, healthBarTransform.sizeDelta.y);
    }

    void Update()
    {
        healthBar.value = healthController.currentHealth / healthController.maxHealth;
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
