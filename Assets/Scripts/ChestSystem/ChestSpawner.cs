using System.Collections.Generic;
using System.Linq;
using SaveSystem;
using UnityEngine;

namespace ChestSystem
{
    public class ChestSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject chestPrefab;
        [SerializeField] private List<Transform> spawnPoints;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            int count = GameManager.Instance.Animals.First(
                animal => animal.AnimalId == GameManager.Instance.Player.LastSelectedAnimalId).quiz.QuizQuestions.Count;
            ShuffleList(spawnPoints);
        
            for (int i = 0; i < count; i++)
            {
                Instantiate(chestPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            }
        }
    
        private void ShuffleList(List<Transform> points)
        {
            for (int i = points.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                // Swap elements
                (points[i], points[randomIndex]) = (points[randomIndex], points[i]);
            }
        }
    }
}
