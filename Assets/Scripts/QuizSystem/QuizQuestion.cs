using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuizSystem
{
    [Serializable]
    public class QuizQuestion
    {
        // Quiz Related Data
        public string Question { get; private set; }
        public List<string> Answers { get; private set; }
        // Progress Tracking Data
        public string QuestionId { get; private set; }
        public bool HasBeenAnswered { get; private set; }
        public bool HasBeenAnsweredCorrectly { get; private set; }

        public QuizQuestion(string questionId, string question, List<string> answers)
        {
            Question = question;
            Answers = answers;
            QuestionId = questionId;
        }

        public void FlagQuizQuestion(bool hasBeenAnswered, bool hasBeenAnsweredCorrectly)
        {
            HasBeenAnswered = hasBeenAnswered;
            HasBeenAnsweredCorrectly = hasBeenAnsweredCorrectly;
        }
        
        public bool Answer(string answer)
        {
            HasBeenAnswered = true;
            // First answer is always the correct one
            HasBeenAnsweredCorrectly = Answers[0] == answer ? true : false;
            return HasBeenAnsweredCorrectly;
        }
    }
}
