using SaveSystem;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var question = GameManager.Instance.Animals[0].quiz.GetRandomQuestion();
        Debug.Log(question.Question);
        Debug.Log(question.Answer(question.Answers[0]));
        
        var question2 = GameManager.Instance.Animals[1].quiz.GetRandomQuestion();
        Debug.Log(question2.Question);
        Debug.Log(question2.Answer(question.Answers[1]));

        
        GameManager.Instance.SavePlayerData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
