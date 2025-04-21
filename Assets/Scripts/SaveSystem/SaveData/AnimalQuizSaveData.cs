using System;
using System.Collections.Generic;

namespace SaveSystem.SaveData
{
    [Serializable]
    public class AnimalQuizSaveData
    {
        public string animalId;
        public List<QuizQuestionSaveData> questions;
    }
}
