using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedCheckMicrogame : Microgame
{
    [Header("Speed Check Microgame Settings")]
    [SerializeField, Tooltip("Set the speedometer.")]
    private Speedometer speedometer;
    [SerializeField, Tooltip("Set the text component that shows the max speed.")]
    private TMP_Text text;
    [SerializeField, Tooltip("Set the goals that the player should reach.")]
    private int[] goals;
    [SerializeField, Tooltip("Set the margin of errors.")]
    private float marginOfError;
    [SerializeField, Tooltip("Set the margin of errors."), Min(1)]
    private int maxAmountOfTries;
    
    private int _currentGoal;
    private int _pointsGained;
    private int _amountOfTries;
    
    private void Start()
    {
        if (text == null) return;
            
        _currentGoal = goals[Random.Range(0, goals.Length)];
        
        text.SetText(_currentGoal.ToString());

        _amountOfTries++;
    }
    
    protected override void OnDeadline()
    {
        if (text == null && speedometer) return;

        if (speedometer.CurrentSpeed >= _currentGoal - marginOfError &&
            speedometer.CurrentSpeed <= _currentGoal + marginOfError)
        {
            _pointsGained++;
        }
        Debug.Log(_pointsGained);

        if (_amountOfTries >= maxAmountOfTries)
        {
            // Result screen code
            
            SceneManager.LoadScene(0);
            return;
        }
        _amountOfTries++;
        
        _currentGoal = goals[Random.Range(0, goals.Length)];
        
        text.SetText(_currentGoal.ToString());
    }
}
