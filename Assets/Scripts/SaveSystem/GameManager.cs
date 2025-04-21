using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models;
using QuizSystem;
using SaveSystem.SaveData;
using UnityEngine;

namespace SaveSystem
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private List<Animal> animals = new List<Animal>();
    
        public static GameManager Instance;
        
        public Player Player => player;
        public List<Animal> Animals => animals;
        
        // Save Models
        private PlayerSaveData _playerData;
        private QuizData _quizData;
        // File paths
        private string saveFilePath => Application.persistentDataPath + "/save.json";
        private TextAsset quizFile => Resources.Load<TextAsset>("quiz");
    
    
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
        
            DontDestroyOnLoad(gameObject);
            Instance = this;
            Application.targetFrameRate = 60;
            LoadQuizData();
            LoadPlayerData();
        }

        public void LoadPlayerData()
        {
            if (File.Exists(saveFilePath))
            {
                _playerData = LoadJson<PlayerSaveData>(saveFilePath);
            }
            else
            {
                _playerData = ConstructInitialSaveData();
                SaveJson(_playerData, saveFilePath);
            }
            
            LoadSaveDataToModels();
        }

        public void SavePlayerData()
        {
            List<AnimalSaveData> animalSaveDatas = new List<AnimalSaveData>();
            List<AnimalQuizSaveData> animalQuizDatas = new List<AnimalQuizSaveData>();
            
            foreach (var animal in animals)
            {
                var animalQuizSaveData = new AnimalQuizSaveData();
                var animalData = new AnimalSaveData
                {
                    id = animal.AnimalId,
                    isLocked = animal.isLocked,
                };
                animalSaveDatas.Add(animalData);

                animalQuizSaveData.animalId = animal.AnimalId;
                
                var animalQuestions = new List<QuizQuestionSaveData>();
                foreach (var question in animal.quiz.QuizQuestions)
                {
                    var questionData = new QuizQuestionSaveData();
                    questionData.questionId = question.QuestionId;
                    questionData.hasBeenAnswered = question.HasBeenAnswered;
                    questionData.hasBeenAnsweredCorrectly = question.HasBeenAnsweredCorrectly;
                    animalQuestions.Add(questionData);
                }
                animalQuizSaveData.questions = animalQuestions;
                animalQuizDatas.Add(animalQuizSaveData);
            }
            
            _playerData = new PlayerSaveData
            {
                playerName = player.PlayerName,
                isFirstTime = player.IsFirstTime,
                lastSelectedAnimalId = player.LastSelectedAnimalId,
                animalsData = animalSaveDatas,
                questionsData = animalQuizDatas,
                saveDate = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss")
            };
            
            SaveJson(_playerData, saveFilePath);
        }

        private void LoadQuizData()
        {
            _quizData = JsonUtility.FromJson<QuizData>(quizFile.text);
        }

        private void LoadSaveDataToModels()
        {
            foreach (var animal in animals)
            {
                var savedAnimal = _playerData.animalsData.First(animalData => animalData.id == animal.AnimalId);
                var savedQuestions = _playerData.questionsData.First(qData => qData.animalId == animal.AnimalId);
                animal.isLocked = savedAnimal.isLocked;
                
                var quiz = _quizData.animalQuiz.First(animalQuiz => animalQuiz.animalId == animal.AnimalId);

                List<QuizQuestion> quizQuestions = new List<QuizQuestion>();
                foreach (var question in quiz.questions)
                {
                    QuizQuestion quizQuestion =
                        new QuizQuestion(question.questionId, question.question, question.answers);
                    quizQuestions.Add(quizQuestion);
                }

                foreach (var question in quizQuestions)
                {
                    var questionData = savedQuestions.questions.First(q => q.questionId == question.QuestionId);
                    question.FlagQuizQuestion(questionData.hasBeenAnswered, questionData.hasBeenAnsweredCorrectly);
                }
                
                var animalQuiz = new AnimalQuiz(quizQuestions);
                animal.quiz = animalQuiz;
            }
            
            player.ChangeName(_playerData.playerName);
            player.SetIsFirstTime(_playerData.isFirstTime);
            player.SetLastSelectedAnimalId(_playerData.lastSelectedAnimalId);
        }

        private PlayerSaveData ConstructInitialSaveData()
        {
            List<AnimalSaveData> animalSaveDatas = new List<AnimalSaveData>();
            foreach (var animal in animals)
            {
                var animalData = new AnimalSaveData
                {
                    id = animal.AnimalId,
                    isLocked = animal.isLocked,
                };
                animalSaveDatas.Add(animalData);
            }
        

            var animalQuizSaveData = new List<AnimalQuizSaveData>();
            foreach (var animalQuiz in _quizData.animalQuiz)
            {
                var animalQuestions = new List<QuizQuestionSaveData>();
                foreach (var question in animalQuiz.questions)
                {
                    var animalQuestion = new QuizQuestionSaveData
                    {
                        questionId = question.questionId,
                        hasBeenAnswered = false,
                        hasBeenAnsweredCorrectly = false
                    };
                    animalQuestions.Add(animalQuestion);
                }
                var quizSaveData = new AnimalQuizSaveData
                {
                    animalId = animalQuiz.animalId,
                    questions = animalQuestions
                };
                animalQuizSaveData.Add(quizSaveData);
            }
        
            var playerData = new PlayerSaveData
            {
                playerName = "Unnamed Player",
                isFirstTime = true,
                lastSelectedAnimalId = animals[0].AnimalId,
                animalsData = animalSaveDatas,
                questionsData = animalQuizSaveData,
                saveDate = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss"),
            };
        
            return playerData;
        }

        private T LoadJson<T>(string path)
        {
            return JsonUtility.FromJson<T>(File.ReadAllText(path));
        }

        private void SaveJson<T>(T data, string path)
        {
            Debug.Log("Saving Data");
            File.WriteAllText(path, JsonUtility.ToJson(data, true));
        }
    }
}
