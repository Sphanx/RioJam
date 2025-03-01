using System;
using UnityEngine;
using UnityEngine.UI;

public class PoweBar : MonoBehaviour
{
    public Slider powerBar;
    public Rigidbody2D _rigidbody;
    public float powerSpeed = 2f;
    public float minValue = 2f;
    public float maxValue = 10f;
    private bool barYon = true;
    public bool hasGameStarted = false;

    void Update()
    {
        float movement =  powerSpeed * Time.deltaTime;
        powerBar.value += barYon ? movement : -movement;

        if (powerBar.value >= powerBar.maxValue || powerBar.value <= powerBar.minValue)
        {
            barYon = !barYon;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasGameStarted = true;
            ApplyForce();
        }
    }

    void ApplyForce()
    {
        float powerValue = powerBar.value;  // Barın o anki değeri
    float middle = (powerBar.maxValue + powerBar.minValue) / 2;  // Barın tam orta noktası
    float forceMultiplier;

    // Güce göre kuvvet hesapla
    if (Mathf.Abs(powerValue - middle) < 0.1f) // Eğer bar tam ortadaysa
    {
        forceMultiplier = 1f; 
    }
    else if (powerValue > middle) 
    {
        forceMultiplier = Mathf.Lerp(1f, 2f, (powerValue - middle) / (powerBar.maxValue - middle));  
    }
    else 
    {
        forceMultiplier = Mathf.Lerp(0.5f, 1f, (powerValue - powerBar.minValue) / (middle - powerBar.minValue));
    }

    float appliedForce = Mathf.Lerp(minValue, maxValue, forceMultiplier); // Kuvveti belirle
    _rigidbody.linearVelocity = Vector2.zero; 
    _rigidbody.AddForce(Vector2.up * appliedForce, ForceMode2D.Impulse);
    }


}
