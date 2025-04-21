using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AudioSystem;
using Models;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class CharacterSelection : MonoBehaviour
    {
        private static readonly int IsLocked = Animator.StringToHash("isLocked");
        [SerializeField] private Camera menuCamera;
        [SerializeField] private Player player;
        [SerializeField] private List<AnimalViewPoint> animals;
        [SerializeField] private TMP_Text animalNameTxt;
        [SerializeField] private TMP_Text animalScoreTxt;
        [SerializeField] private Button nextAnimalButton;
        [SerializeField] private Button prevAnimalButton;
        [SerializeField] private Button playButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private TMP_Text lockedPanelTxt;
        [SerializeField] private Animator lockedPanelAnimator;
        [SerializeField] private Color playButtonEnabledColor;
        [SerializeField] private Color playButtonDisabledColor;

        [SerializeField] private TMP_Text playerNameTxt;
        [SerializeField] private TMP_Text playerScoreTxt;
        
        [SerializeField] private RectTransform fistTimePanel;
        [SerializeField] private TMP_InputField playerNameInputField;
        [SerializeField] private Toggle playTutorialToggle;
        [SerializeField] private Button submitButton;
        [SerializeField] private List<RectTransform> panels;
        [SerializeField] private GameObject settingsMenu;
        [Header("SFX")]
        [SerializeField] private AudioClip prevNextButtonSound;
        [SerializeField] private AudioClip failSound;

        private int _index = 0;
        private Color _playButtonColor;
        private Image _playButtonImage;
        private void Start()
        {
            SoundManager.Instance.SetMenuMusic();
            Cursor.visible = true;
            _playButtonImage = playButton.GetComponent<Image>();
            nextAnimalButton.onClick.AddListener(NextAnimal);
            prevAnimalButton.onClick.AddListener(PreviousAnimal);
            submitButton.onClick.AddListener(SubmitName);
            exitButton.onClick.AddListener(ExitGame);
            playButton.onClick.AddListener(EnterGame);
            optionsButton.onClick.AddListener(OpenSettingsMenu);
            UpdateAnimalUI();
            CheckIfIsFirstTime();
            CalculatePlayerScore();
            CheckIfAnyCharacterUnlocked();
            playerNameTxt.text = player.PlayerName;
        }

        private void EnterGame()
        {
            if (animals[_index].Animal.isLocked)
            {
                SoundManager.Instance.PlaySfx(failSound, transform, 1f);
                return;
            }
            
            player.SetLastSelectedAnimalId(animals[_index].Animal.AnimalId);
            GameManager.Instance.SavePlayerData();
            SceneManager.LoadScene("MainScene");
        }

        private void CheckIfIsFirstTime()
        {
            if (player.IsFirstTime)
            {
                foreach(var panel in panels)
                    StartCoroutine(LerpUISize(panel, Vector3.one, Vector3.zero, 0.5f));
                StartCoroutine(LerpUISize(fistTimePanel, Vector3.zero, Vector3.one, 0.5f));
                return;
            }
            
            fistTimePanel.gameObject.SetActive(false);
            foreach(var panel in panels)
                StartCoroutine(LerpUISize(panel, Vector3.zero, Vector3.one, 0.5f));
        }

        private void CalculatePlayerScore()
        {
            var globalScore = 0;
            var globalMaxScore = 0;
            foreach (var animal in animals)
            {
                globalScore += animal.Animal.quiz.QuizQuestions.Count(q => q.HasBeenAnsweredCorrectly == true);
                globalMaxScore += animal.Animal.quiz.QuizQuestions.Count;
            }
            playerScoreTxt.text = $"{globalScore}/{globalMaxScore}";
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.RightArrow))
                NextAnimal();
            if(Input.GetKeyDown(KeyCode.LeftArrow))
                PreviousAnimal();
            
            UpdateCameraFocus();
        }

        private void ExitGame()
        {
            GameManager.Instance.SavePlayerData();
            Application.Quit();
        }

        private void NextAnimal()
        {
            _index = _index >= animals.Count - 1 ? 0 : _index + 1;
            SoundManager.Instance.PlaySfx(prevNextButtonSound, transform, 1f);
            UpdateAnimalUI();
        }

        private void PreviousAnimal()
        {
            _index = _index <= 0 ? animals.Count-1 : _index - 1;
            SoundManager.Instance.PlaySfx(prevNextButtonSound, transform, 1f);
            UpdateAnimalUI();
        }

        private void SubmitName()
        {
            SoundManager.Instance.PlaySfx(prevNextButtonSound, transform, 1f);
            player.ChangeName(playerNameInputField.text);
            player.SetIsFirstTime(false);
            playerNameTxt.text = player.PlayerName;
            StartCoroutine(LerpUISize(fistTimePanel, Vector3.one, Vector3.zero, 0.5f));
            foreach(var panel in panels)
                StartCoroutine(LerpUISize(panel, Vector3.zero, Vector3.one, 0.5f));
        }

        private void CheckIfAnyCharacterUnlocked()
        {
            for(int i = 0; i < animals.Count; i++)
            {
                if (i <= 0) continue;
                if (!animals[i].Animal.isLocked) continue;
                var correctAnswers = animals[i - 1].Animal.quiz.QuizQuestions.Count(
                    q => q.HasBeenAnsweredCorrectly == true);
                if(correctAnswers >= 5)
                    animals[i].Animal.isLocked = false;

            }
        }

        private void UpdateAnimalUI()
        {
            animalNameTxt.text = animals[_index].Animal.AnimalName;
            var score = animals[_index].Animal.quiz.QuizQuestions.Count(q => q.HasBeenAnsweredCorrectly == true);
            var maxScore = animals[_index].Animal.quiz.QuizQuestions.Count;
            animalScoreTxt.text = $"{score}/{maxScore}";
            
            var isLocked = animals[_index].Animal.isLocked;
            
            if(_index > 0)
                lockedPanelTxt.text =
                    $"Gather 5 Gems with {animals[_index - 1].Animal.AnimalName} to unlock {animals[_index].Animal.AnimalName}";
            
            lockedPanelAnimator.SetBool(IsLocked, isLocked);
            _playButtonColor = isLocked ? playButtonDisabledColor : playButtonEnabledColor;
        }

        private void UpdateCameraFocus()
        {
            menuCamera.transform.position = Vector3.Lerp(menuCamera.transform.position, animals[_index].transform.position, 5f * Time.deltaTime);
            _playButtonImage.color = Color.Lerp(_playButtonImage.color, _playButtonColor, 5f * Time.deltaTime);
        }

        IEnumerator LerpUISize(RectTransform panel, Vector3 startSize, Vector3 endSize, float duration)
        {
            float elapsedTime = 0f;
            
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                panel.localScale = Vector3.Lerp(startSize, endSize, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            panel.localScale = endSize;
        }

        private void OpenSettingsMenu()
        {
            settingsMenu.SetActive(true);
        }
    }
}
