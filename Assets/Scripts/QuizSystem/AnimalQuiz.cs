using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace QuizSystem
{
    [Serializable]
    public class AnimalQuiz
    {
        public List<QuizQuestion> QuizQuestions { get; private set; }

        public AnimalQuiz(List<QuizQuestion> quizQuestions)
        {
            QuizQuestions = quizQuestions;
        }

        public QuizQuestion GetRandomQuestion()
        {
            // Get a question that has not been answered yet.
            var unansweredQuestion = QuizQuestions.FirstOrDefault(question => question.HasBeenAnswered == false);
            // If player has answered all questions, return one that he didn't answer correctly.
            if(unansweredQuestion == null)
                unansweredQuestion = QuizQuestions.FirstOrDefault(question => question.HasBeenAnswered == false);
            // Else return a random one.
            if(unansweredQuestion == null)
                unansweredQuestion = QuizQuestions[Random.Range(0, QuizQuestions.Count)];
        
            return unansweredQuestion;
        }
    }
}
