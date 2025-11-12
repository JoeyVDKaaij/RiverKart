using TMPro;
using UnityEngine;

public class SpeedCheckMicrogame : MonoBehaviour
{
    [Header("Speed Check Microgame Settings")]
    [SerializeField, Tooltip("Set the speedometer.")]
    private Speedometer speedometer;
    [SerializeField, Tooltip("Set the text component that shows the max speed.")]
    private TMP_Text text;
    [SerializeField, Tooltip("Set the deadline.")]
    private float deadline;
    [SerializeField, Tooltip("Set the goals that the player should reach.")]
    private int[] goals;
    [SerializeField, Tooltip("Set the margin of errors.")]
    private float marginOfError;
    
    private float _timer;
    private int _currentGoal;
    private int _pointsGained;
    
    private void Start()
    {
        if (text == null) return;
            
        _currentGoal = goals[Random.Range(0, goals.Length)];
        
        text.SetText(_currentGoal.ToString());
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= deadline)
        {
            SetupNextGoal();    
            _timer = 0;
        }
    }
    
    private void SetupNextGoal()
    {
        if (text == null && speedometer) return;

        if (speedometer.CurrentSpeed >= _currentGoal - marginOfError &&
            speedometer.CurrentSpeed <= _currentGoal + marginOfError)
        {
            _pointsGained++;
        }
        Debug.Log(_pointsGained);
            
        _currentGoal = goals[Random.Range(0, goals.Length)];
        
        text.SetText(_currentGoal.ToString());
    }
}
