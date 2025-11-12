using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Speedometer : MonoBehaviour
{
    [Header("Speedometer Settings")]
    [SerializeField, Tooltip("Set up the keybind for speeding up.")]
    private InputActionReference speedUpActionReference;
    [SerializeField, Tooltip("Set the speed in which your actual speed increases per second by holding down a certain input."), Min(0f)]
    private float increasingSpeed;
    [SerializeField, Tooltip("Set the speed in which your actual speed decreases per second automatically."), Min(0f)]
    private float decreasingSpeed;
    
    private TMP_Text _speedText;
    private float _currentIncrementingSpeed;
    private float _currentSpeed;
    
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
        if (speedUpActionReference.action.IsPressed())
            _currentIncrementingSpeed += increasingSpeed * Time.deltaTime;
        else
            _currentIncrementingSpeed -= decreasingSpeed * Time.deltaTime;

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
