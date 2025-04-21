using System.Collections;
using System.Linq;
using AudioSystem;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PlayerSystems
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Transform menuPanel;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button returnToMainMenu;
        [SerializeField] private Button quitGame;
        [SerializeField] private TMP_Text currentScoreTxt;
        [SerializeField] private TMP_Text totalScoreTxt;
        [SerializeField] private TMP_Text chestScoreTxt;
        [SerializeField] private RectTransform gemUIImage;
        [SerializeField] private RectTransform gatheredGem;
        [SerializeField] private RectTransform  canvas;
        [SerializeField] private Transform interactionUI;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private AudioClip clickSound;
    
        [Header("GameOver")]
        [SerializeField] private Transform gameOverPanel;
        [SerializeField] private Button backToMenuButton;
        [SerializeField] private Button restartGameButton;
        [SerializeField] private TMP_Text gameOverScoreTxt;
        [SerializeField] private Transform interactInfoUI;
    
        public static PlayerUI Instance { get; private set; }

        private int _currentScore;
        private int _totalScore;

        private int _maxScore;
        private int _chestCounter;
        private CursorLockMode _lastLockMode;

        private Animator _menuAnimator;
        private Animator _gameOverAnimator;
    
        public Transform InteractInfoUI => interactInfoUI;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            _menuAnimator = menuPanel.GetComponent<Animator>();
            _gameOverAnimator = gameOverPanel.GetComponent<Animator>();
            resumeButton.onClick.AddListener(ResumeGame);
            returnToMainMenu.onClick.AddListener(ReturnToMainMenu);
            quitGame.onClick.AddListener(ExitGame);
            backToMenuButton.onClick.AddListener(ReturnToMainMenu);
            restartGameButton.onClick.AddListener(RestartGame);
            _totalScore = QuizManager.Instance.Quiz.QuizQuestions.Count(q => q.HasBeenAnsweredCorrectly);
            _maxScore = QuizManager.Instance.Quiz.QuizQuestions.Count();
            _chestCounter = _maxScore;
            _currentScore = 0;
            currentScoreTxt.text = $"{_currentScore}";
            totalScoreTxt.text = $"{_totalScore}";
            chestScoreTxt.text = $"{_maxScore}";
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(Cursor.lockState);

            if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
            if(!menuPanel.gameObject.activeSelf)
                OpenMenu();
            else
                CloseMenu();
        }

        private void OpenMenu()
        {
            if (gameOverPanel.gameObject.activeSelf) return;
        
            SoundManager.Instance.PlaySfx(clickSound, transform, 1f);
            _lastLockMode = Cursor.lockState;
            interactionUI.gameObject.SetActive(false);
            menuPanel.gameObject.SetActive(true);
            _menuAnimator.Play("MenuPopIn", 0);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }

        private void CloseMenu()
        {
            SoundManager.Instance.PlaySfx(clickSound, transform, 1f);
            Cursor.lockState = _lastLockMode;
            Time.timeScale = 1;
            interactionUI.gameObject.SetActive(true);
            menuPanel.gameObject.SetActive(false);
        }

        private void ResumeGame()
        {
            CloseMenu();
        }

        private void ReturnToMainMenu()
        {
            Time.timeScale = 1;
            GameManager.Instance.SavePlayerData();
            SceneManager.LoadScene("MainMenu");
        }

        private void ExitGame()
        {
            Time.timeScale = 1;
            GameManager.Instance.SavePlayerData();
            Application.Quit();
        }

        private void RestartGame()
        {
            Time.timeScale = 1;
            GameManager.Instance.SavePlayerData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void GatherGem(Transform gemSpawnPoint)
        {
            if (_totalScore < _maxScore)
                _totalScore++;
            _currentScore++;
        
            GemGatherAnimation(gemSpawnPoint);
        }

        private void GemGatherAnimation(Transform gemSpawnPoint)
        {
            Vector3 screenPosition = playerCamera.WorldToScreenPoint(gemSpawnPoint.position);
            gatheredGem.transform.position = screenPosition;
            gatheredGem.localScale = Vector3.one;
            gatheredGem.gameObject.SetActive(true);
            StartCoroutine(LerpUIPosition(gatheredGem, gemUIImage, 0.5f));
        }

        public IEnumerator LerpUIPosition(RectTransform movingUI, RectTransform targetUI, float duration)
        {
            Vector3 startPos = movingUI.localPosition;
        
            // Get target position in screen space
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, targetUI.position);

            // Convert to local position relative to movingUI's parent
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                movingUI.parent as RectTransform,
                screenPos,
                null,
                out var targetLocalPos
            );

            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                movingUI.localPosition = Vector3.Lerp(startPos, targetLocalPos, t);
                movingUI.localScale = Vector3.Lerp(Vector3.one, targetUI.localScale, t);
                yield return null;
            }

            // Ensure it ends exactly at the target
            movingUI.localPosition = targetLocalPos;
            movingUI.gameObject.SetActive(false);
            currentScoreTxt.text = $"{_currentScore}";
            totalScoreTxt.text = $"{_totalScore}";
        }

        public void UpdateChestText()
        {
            _chestCounter--;
            chestScoreTxt.text = $"{_chestCounter}";
            CheckForGameOver();
        }

        public void CheckForGameOver()
        {
            if (_chestCounter <= 0)
            {
                gameOverScoreTxt.text = $"You answered {_currentScore}/{_maxScore} questions!";
                interactionUI.gameObject.SetActive(false);
                menuPanel.gameObject.SetActive(false);
                gameOverPanel.gameObject.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                _gameOverAnimator.Play("GameoverPopIn", 0);
            }
        }
    }
}
