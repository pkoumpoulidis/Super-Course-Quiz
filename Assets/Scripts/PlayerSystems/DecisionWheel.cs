using System;
using System.Collections.Generic;
using AudioSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DecisionWheel : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private RectTransform arrow;
    [SerializeField] private Image[] wheelSections;
    [SerializeField] private TMP_Text[] wheelAnswers;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightColor;

    [Header("SFX")] 
    [SerializeField] private AudioClip animationSfx;
    [SerializeField] private AudioClip hoverSfx;
    
    private List<string> _currentAnswers;
    private string _highlightedAnswer;
    private bool _canAnswer = false;
    private Image _currentImage;
    private void Update()
    {
        PointAtAnswer();
        if (!_canAnswer) return;
        if (!Input.GetMouseButtonDown(0)) return;
        

        QuizManager.Instance.AnswerQuestion(_highlightedAnswer);
        _canAnswer = false;
        animator.Play("DecisionWheelPopOut", 0);
    }

    private void OnEnable()
    {
        animator.Play("DecisionWheelPopIn", 0);
    }

    public void DisableWheel()
    {
        _canAnswer = false;
        gameObject.SetActive(false);
    }

    public void FlagCanAnswer()
    {
        _canAnswer = true;
    }

    private void PointAtAnswer()
    {
        var angle = CalculateAngle();
        arrow.rotation = Quaternion.Euler(0, 0, angle);
        if (!_canAnswer) return;
        HighlightSelected(angle);
    }
    
    void HighlightSelected(float angle)
    {
        if (angle >= 0 && angle < 90)
            Highlight(wheelSections[0], wheelAnswers[0]);
        else if (angle >= -90 && angle < 0)
            Highlight(wheelSections[1], wheelAnswers[1]);
        else if (angle >= 90 && angle < 180)
            Highlight(wheelSections[3], wheelAnswers[3]);
        else
            Highlight(wheelSections[2], wheelAnswers[2]);
    }
    
    void Highlight(Image wheelSection, TMP_Text wheelAnswer)
    {
        if(_currentImage == wheelSection) return;
        ResetAllHighlights();
        _currentImage = wheelSection;
        SoundManager.Instance.PlaySfx(hoverSfx, transform, 1f);
        wheelSection.color = highlightColor;
        wheelAnswer.color = highlightColor;
        _highlightedAnswer = wheelAnswer.text;
    }
    
    void ResetAllHighlights()
    {
        foreach (var c in wheelSections)
            c.color = normalColor;
        foreach(var c in wheelAnswers)
            c.color = normalColor;
    }
    
    private float CalculateAngle()
    {
        Vector3 mousePos = Input.mousePosition;
        
        // Convert mouse position to canvas space
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            arrow.parent as RectTransform, 
            mousePos, 
            null, 
            out Vector2 localMousePos);
        
        // Calculate direction from UI element to mouse
        Vector2 direction = localMousePos - arrow.anchoredPosition;
        
        // Calculate angle (in degrees)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return angle;
    }

    public void SetAnswers(List<string> answers)
    {
        _currentAnswers = new List<string>(answers);
        ShuffleAnswers(_currentAnswers);
        _highlightedAnswer = _currentAnswers[0];
        for(int i = 0; i < _currentAnswers.Count; i++)
        {
            wheelAnswers[i].text = _currentAnswers[i];
        }
    }
    
    private void ShuffleAnswers(List<String> answers)
    {
        for (int i = answers.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            // Swap elements
            (answers[i], answers[randomIndex]) = (answers[randomIndex], answers[i]);
        }
    }

    public void PlayAnimationSfx()
    {
        SoundManager.Instance.PlaySfx(animationSfx, transform, 1f);
    }
}
