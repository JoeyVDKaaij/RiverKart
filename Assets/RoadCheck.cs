using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoadCheck : MonoBehaviour
{
    [SerializeField] 
    private float yPosChecker = -100;
    [SerializeField] 
    private float yPosDeath = -100;
    
    private bool _safe = false;
    private ReactionTimeMicrogame _reactionTimeMicrogame;
    
    public void SetSafety(bool pSafe, ReactionTimeMicrogame reactionTimeMicrogame)
    {
        _safe = pSafe;
        _reactionTimeMicrogame = reactionTimeMicrogame;
    }

    private void Update()
    {
        if (_safe) return;
        
        if (transform.position.y < yPosChecker)
        {
            if (_reactionTimeMicrogame.GetSpeed == 0)
            {
                _safe = false;
                return;
            }
            
            if (transform.position.y < yPosDeath)
            {
                // Replace this with animation code.
                SceneManager.LoadScene(0);
            }
        }
    }
}
