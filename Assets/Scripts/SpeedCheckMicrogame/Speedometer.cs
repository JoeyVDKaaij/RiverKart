using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Speedometer : MonoBehaviour
{
    [Header("Keybind Settings")]
    [SerializeField, Tooltip("Set up the keybind for speeding up.")]
    private InputActionReference speedUpActionReference;
    [Header("Speed Settings")]
    [SerializeField, Tooltip("Set the speed in which your actual speed increases per second by holding down a certain input."), Min(0f)]
    private float increasingSpeed;
    [SerializeField, Tooltip("Set the speed in which your actual speed decreases per second automatically."), Min(0f)]
    private float decreasingSpeed;
    [SerializeField, Tooltip("Set the maximum increasing speed."), Min(0f)]
    private float maximumIncreasingSpeed;
    [SerializeField, Tooltip("Set the maximum decreasing speed."), Min(0f)]
    private float maximumDecreasingSpeed;
    [SerializeField, Tooltip("Set the halting speed."), Min(1f)]
    private float haltingSpeed = 10;
    
    private TMP_Text _speedText;
    private float _currentIncrementingSpeed;
    private float _currentSpeed;

    public float CurrentSpeed
    {
        get { return _currentSpeed; }
    }
    
    private void Start()
    {
        _speedText = GetComponent<TMP_Text>();
        
        if (speedUpActionReference != null)
            speedUpActionReference.action.Enable();
    }

    private void OnDestroy()
    {
        if (speedUpActionReference != null)
            speedUpActionReference.action.Disable();
    }

    private void Update()
    {
        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        if (speedUpActionReference.action.IsPressed())
        {
            if (_currentIncrementingSpeed < 0) _currentIncrementingSpeed -= _currentIncrementingSpeed / haltingSpeed;
            _currentIncrementingSpeed += (increasingSpeed / 10) * Time.deltaTime;
        }
        else
        {
            if (_currentIncrementingSpeed > 0) _currentIncrementingSpeed -= _currentIncrementingSpeed / haltingSpeed;
            _currentIncrementingSpeed -= (decreasingSpeed / 10) * Time.deltaTime;
        }

        _currentIncrementingSpeed = Mathf.Clamp(_currentIncrementingSpeed, -maximumDecreasingSpeed, maximumIncreasingSpeed);
        
        _currentSpeed += _currentIncrementingSpeed;
        _currentSpeed = Mathf.Max(_currentSpeed, 0);

        if (_currentSpeed == 0)
        {
            _currentIncrementingSpeed = 0;
        }
        
        if (_speedText != null)
            _speedText.SetText(_currentSpeed.ToString("0.00"));
    }
}
