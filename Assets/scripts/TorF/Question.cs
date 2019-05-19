using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

[System.Serializable]
public class QuestionDTO
{
    public string question_id;
    public string subject;
    public string question;
    public string correctAns;

    public Boolean isAnswered = false;
    public Boolean isAnsweredCorrectly = false;
}

public class Question : MonoBehaviour
{
    
    public string fact;
    public bool isTrue;

    public Text questionBox;
    public Button tamaButton;
    public Button maliButton;

    GameObject questionPanel;
    
    public QuestionDTO[] questionDtoArr;

    private int currentQuestionNumber = 0;

    private string tamaOMaliURL = "http://localhost/balikaral/connection_true_or_false.php";

    private WebRequestHelper wrh;

    private void Start()
    {
        Debug.Log("start called");
        tamaButton.onClick.AddListener(trueBtnOnClick);
        maliButton.onClick.AddListener(falseBtnOnClick);
    }

    void OnEnable()
    {
        // Debug.Log("OnEnable called");
        wrh = new WebRequestHelper(this);
        SceneManager.sceneLoaded += OnSceneLoaded;  
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // string requestResult;
        // requestResult = wrh.fetchWebDataString(tamaOMaliURL);
        fetchQuestions();
    }

    public void fetchQuestions()
    {
        wrh.fetchWebDataString(tamaOMaliURL, processQuestionDTO);
    }

    public void processQuestionDTO(string result)
    {
        // Debug.Log("Called! TestCallback");
        String jsonString = result;
        jsonString = CommonHelper.JsonHelper.fixJson(jsonString);
        questionDtoArr = CommonHelper.JsonHelper.FromJson<QuestionDTO>(jsonString);
        // Debug.Log(questionDtoArr[0].isAnswered);
        nextQuestion();
    }

    public void nextQuestion()
    {
        // Debug.Log(currentQuestionNumber);
        int arrLength = questionDtoArr.Length;
        if (arrLength > 0 && currentQuestionNumber < arrLength)
        {
            QuestionDTO questionDto = questionDtoArr[currentQuestionNumber];
            questionBox.text = questionDto.question;
            currentQuestionNumber += 1;
        }else if (currentQuestionNumber >= arrLength)
        {
            // Summary page
        }
    }

    private void trueBtnOnClick()
    {
        scoreAnswer(true);
        nextQuestion();
    }

    private void falseBtnOnClick()
    {
        scoreAnswer(false);
        nextQuestion();
    }

    private void scoreAnswer(bool trueOrFalse)
    {
        int arrLength = questionDtoArr.Length;
        if (arrLength > 0 && currentQuestionNumber < arrLength)
        {
            QuestionDTO questionDto = questionDtoArr[currentQuestionNumber];
            String correctAnswer = questionDto.correctAns;
            questionDtoArr[currentQuestionNumber].isAnsweredCorrectly = (
                    (trueOrFalse && correctAnswer.Equals("1")) || (!trueOrFalse && correctAnswer.Equals("0"))
                ) ? true : false;

            Debug.Log(String.Format("Question #{0} has been aswered {1}", (currentQuestionNumber), (questionDtoArr[currentQuestionNumber].isAnsweredCorrectly) ? "correctly" : "incorrectly"));
        }
    }

    /*private IEnumerator fetchQuestions(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            string jsonString = uwr.downloadHandler.text;
            jsonString = CommonHelper.JsonHelper.fixJson(jsonString);
            Debug.Log("Received: " + jsonString);
            
            questionDto = CommonHelper.JsonHelper.FromJson<QuestionDTO>(jsonString);

            Debug.Log(questionDto[0].ToString());
        }
    }*/

}


