using System;
using TMPro;
using UnityEngine;

public class Microgame : MonoBehaviour
{
    [Header("Microgame Settings")] 
    [SerializeField, Tooltip("Text component for the timer.")]
    private TMP_Text timerText;
    [SerializeField, Tooltip("The deadline that the timer has to abide by in seconds.")]
    private float deadline;
    
    private float _currentTimer;
    
    private void Update()
    {
        _currentTimer += Time.deltaTime;
        
        timerText.SetText((deadline - _currentTimer).ToString("0"));

        MicrogameUpdate(_currentTimer);
        
        if (_currentTimer >= deadline)
        {
            OnDeadline();
            _currentTimer = 0;
        }
    }

    protected virtual void MicrogameUpdate(float pCurrentTime) {}
    
    protected virtual void OnDeadline() {}
}