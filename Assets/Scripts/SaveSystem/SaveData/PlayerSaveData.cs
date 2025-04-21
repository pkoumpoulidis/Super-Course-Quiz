using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace SaveSystem.SaveData
{
    [Serializable]
    public class PlayerSaveData
    {
        public string playerName;
        public bool isFirstTime;
        public string lastSelectedAnimalId;
        public List<AnimalSaveData> animalsData;
        public List<AnimalQuizSaveData> questionsData;
        public string saveDate;
    }
}
