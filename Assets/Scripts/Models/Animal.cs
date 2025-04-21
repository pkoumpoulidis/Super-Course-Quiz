using QuizSystem;
using Unity.Cinemachine;
using UnityEngine;

namespace Models
{
    [CreateAssetMenu(fileName = "New Animal", menuName = "Animal")]
    public class Animal : ScriptableObject
    {
        [SerializeField] private string animalId;
        [SerializeField] private string animalName;
        [SerializeField] private GameObject prefab;
        [SerializeField] private CinemachineCamera camera;
        public bool isLocked;
        public AnimalQuiz quiz;
        public string AnimalId => animalId;
        public string AnimalName => animalName;
        public GameObject Prefab => prefab;
        public CinemachineCamera Camera => camera;

    }
}
