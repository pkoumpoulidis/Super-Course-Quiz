using System.Collections;
using AudioSystem;
using ChestSystem;
using QuizSystem;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private Color defaultTextColor;
    [SerializeField] private Color correctTextColor;
    [SerializeField] private Color incorrectTextColor;
    //[SerializeField] private CharacterMovementController characterController;
    [SerializeField] private CinemachineVirtualCameraBase quizCamera;
    [FormerlySerializedAs("questionText")] [SerializeField] private TMP_Text displayText;
    [SerializeField] private DecisionWheel decisionWheel;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip incorrectSound;
    [SerializeField] private AudioClip textSound;
    [SerializeField] private ParticleSystem successParticlePrefab;
    
    private AnimalQuiz _quiz;
    private CharacterMovementController _characterController;
    private QuizQuestion _currentQuestion;
    private bool _isAQuestionDisplayed = false;
    private bool _isTextDisplayed = false;
    private QuizChest _targetChest;
    public static QuizManager Instance { get; private set; }
    public AnimalQuiz Quiz => _quiz;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private ParticleSystem _successParticle;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void InitializeQuiz(CharacterMovementController characterController)
    {
        _quiz = characterController.BaseCharacter.quiz;
        quizCamera.Follow = characterController.CharacterBody;
        _characterController = characterController;
    }

    public void BeginDisplayQuestion(QuizChest targetChest)
    {
        if (_isAQuestionDisplayed) return;
        if(_isTextDisplayed) return;
        _targetChest = targetChest;
        StartCoroutine(DisplayQuestion());
    }

    IEnumerator DisplayText(string text, Color textColor,  bool clearTextAfterDisplay = false)
    {
        _isTextDisplayed = true;
        displayText.color = textColor;
        displayText.text = "";
        int counter = 0;
        foreach (var c in text)
        {
            counter++;
            displayText.text += c;
            if(counter % 3 == 0)
                SoundManager.Instance.PlaySfx(textSound, _characterController.CharacterBody, 1f);
            yield return new WaitForSeconds(0.02f);
        }

        if (clearTextAfterDisplay)
        {
            yield return new WaitForSeconds(1f);
            displayText.text = "";
        }
        _isTextDisplayed = false;
    }

    IEnumerator DisplayQuestion()
    {
        Cursor.lockState = CursorLockMode.None;
        quizCamera.Priority = 10;
        _characterController.CharacterCamera.gameObject.SetActive(false);
        _characterController.enabled = false;
        _isAQuestionDisplayed = true;
        _currentQuestion = _quiz.GetRandomQuestion();
        yield return StartCoroutine(DisplayText(_currentQuestion.Question, defaultTextColor));
        decisionWheel.SetAnswers(_currentQuestion.Answers);
        decisionWheel.gameObject.SetActive(true);
        
    }

    public void AnswerQuestion(string answer)
    {
        var isAnsweredCorrectly = _currentQuestion.Answer(answer);
        if (isAnsweredCorrectly)
        {
            SoundManager.Instance.PlaySfx(correctSound, _targetChest.transform, 1f);
            _successParticle = Instantiate(successParticlePrefab, _characterController.CharacterBody.transform.position, Quaternion.identity);
            _successParticle.transform.position += Vector3.up * 2f;
            _successParticle.Play();
            StartCoroutine(DisplayText("Correct!", correctTextColor, true));
            _targetChest.InteractWithChest(true);
        }
        else
        {
            SoundManager.Instance.PlaySfx(incorrectSound, _targetChest.transform, 1f);
            StartCoroutine(DisplayText("Incorrect!", incorrectTextColor, true));
            _targetChest.InteractWithChest(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        _isAQuestionDisplayed = false;
        _characterController.CharacterCamera.gameObject.SetActive(true);
        quizCamera.Priority = -10;
        _characterController.enabled = true;
    }
}
