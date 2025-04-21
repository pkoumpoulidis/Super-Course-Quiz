using UnityEngine;

namespace Models
{
    [CreateAssetMenu(fileName = "Player", menuName = "Scriptable Objects/Player")]
    public class Player : ScriptableObject
    {
        public string PlayerName { get; private set; }
        public bool IsFirstTime { get; private set; }
        public string LastSelectedAnimalId { get; private set; }

        public void ChangeName(string playerName) => PlayerName = playerName;
        public void SetLastSelectedAnimalId(string lastSelectedAnimalId) => LastSelectedAnimalId = lastSelectedAnimalId;
        public void SetIsFirstTime(bool isFirstTime) => IsFirstTime = isFirstTime;
    }
}
