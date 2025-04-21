using System;
using System.Collections.Generic;

namespace QuizSystem
{
    [Serializable]
    public class QuizData
    {
        public List<AnimalQuizData> animalQuiz;
    }

    [Serializable]
    public class AnimalQuizData
    {
        public string animalId;
        public List<QuizQuestionData> questions;
    }


    [Serializable]
    public class QuizQuestionData
    {
        public string questionId;
        public string question;
        public List<string> answers;
    }
}