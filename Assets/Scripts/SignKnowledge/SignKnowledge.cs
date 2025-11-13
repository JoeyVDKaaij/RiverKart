using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public struct SignKnowledgeQuestion
{
    [Tooltip("Set the question that needs to be asked.")]
    public string question;
    [Tooltip("Set the sprite of the correct answer.")]
    public Sprite answerSprite;
    [Tooltip("Set the sprite of the wrong answer.")]
    public Sprite wrongSprite;
} 

public class SignKnowledge : Microgame
{
    [Header("Sign Knowledge Settings")] 
    [SerializeField, Tooltip("Set how many questions should be asked.")]
    private int amountOfQuestions = 5;
    [SerializeField, Tooltip("Set the possible questions.")]
    private SignKnowledgeQuestion[] questions;
    [SerializeField, Tooltip("Set the possible questions.")]
    private Button[] buttons;
    [SerializeField, Tooltip("Set the possible questions.")]
    private TMP_Text questionText;

    private int _questionsAsked = 0;
    private int _questionsCorrect = 0;
    private List<SignKnowledgeQuestion> _questionsLeft = new List<SignKnowledgeQuestion>();
    private SignKnowledgeQuestion _currentQuestion;
    private bool _questionCorrect = false;
    
    private void OnValidate()
    {
        _questionsLeft = questions.ToList();
    }

    private void Start()
    {
        _questionsLeft = questions.ToList();
        
        int nextQuestionIndex = Random.Range(0, _questionsLeft.Count);
        _currentQuestion = _questionsLeft[nextQuestionIndex];
        _questionsLeft.RemoveAt(nextQuestionIndex);
        
        Debug.Log(_questionsLeft.Count);
        
        SetupButtons();
    }

    protected override void OnDeadline()
    {
        if (_questionCorrect)
            _questionsCorrect++;
        
        _questionCorrect = false;
        
        if (_questionsAsked >= amountOfQuestions || _questionsLeft.Count <= 0)
        {
            // Result Screen code
            
            // Remove load scene once the result screen has been implemented
            SceneManager.LoadScene(0);
            return;
        }
        
        _questionsAsked++;
        
        int nextQuestionIndex = Random.Range(0, _questionsLeft.Count);
        _currentQuestion = _questionsLeft[nextQuestionIndex];
        _questionsLeft.RemoveAt(nextQuestionIndex);
        
        SetupButtons();
    }

    private void SetupButtons()
    {
        if (questionText == null || buttons == null || buttons.Length < 2) return;

        foreach (Button button in buttons)
            button.interactable = true;
        
        questionText.SetText(_currentQuestion.question);
        
        int chosenAnswerIndex = Random.Range(0, buttons.Length);
        buttons[chosenAnswerIndex].image.sprite = _currentQuestion.answerSprite;
        buttons[chosenAnswerIndex == 0 ? 1 : 0].image.sprite = _currentQuestion.wrongSprite;
        
        buttons[chosenAnswerIndex].onClick.RemoveAllListeners();
        buttons[chosenAnswerIndex].onClick.AddListener(() => CorrectAnswerChosen(chosenAnswerIndex));
        buttons[chosenAnswerIndex == 1 ? 0 : 1].onClick.RemoveAllListeners();
        buttons[chosenAnswerIndex == 1 ? 0 : 1].onClick.AddListener(() => WrongAnswerChosen(chosenAnswerIndex == 1 ? 0 : 1));
    }

    private void CorrectAnswerChosen(int pButtonId)
    {
        buttons[pButtonId].interactable = false;
        buttons[pButtonId == 1 ? 0 : 1].interactable = true;

        _questionCorrect = true;
    }

    private void WrongAnswerChosen(int pButtonId)
    {
        buttons[pButtonId].interactable = false;
        buttons[pButtonId == 1 ? 0 : 1].interactable = true;

        _questionCorrect = true;
    }
}