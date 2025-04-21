using System;

namespace SaveSystem.SaveData
{
    [Serializable]
    public class QuizQuestionSaveData
    {
        public string questionId;
        public bool hasBeenAnswered;
        public bool hasBeenAnsweredCorrectly;
    }
}
