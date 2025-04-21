using Models;
using UnityEngine;

namespace MainMenu
{
    public class AnimalViewPoint : MonoBehaviour
    {
        [SerializeField] private Animal animal;
        
        public Animal Animal => animal;
    }
}
