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

    public int questionScoreValue = 1;

    public Boolean isAnswered = false;
    public Boolean isAnsweredCorrectly = false;
}

public class Question : MonoBehaviour
{
    
    public string fact;
    public bool isTrue;

    public Image summaryPanel;
    public Text summaryPanelTitleText;
    public Text summaryPanelScoreText;
    public Button summaryPanelTryAgainBtn;
    public Button summaryPanelReturnToMMButton;


    public Text questionBox;
    public Button tamaButton;
    public Button maliButton;
        
    GameObject questionPanel;
    
    private QuestionDTO[] questionDtoArr;

    private int currentQuestionNumber = 0;

    private string tamaOMaliURL = "http://localhost/balikaral/connection_true_or_false.php";

    private WebRequestHelper wrh;

    private void Start()
    {
        Debug.Log("start called");
        tamaButton.onClick.AddListener(trueBtnOnClick);
        maliButton.onClick.AddListener(falseBtnOnClick);
        summaryPanelTryAgainBtn.onClick.AddListener(summaryPanelTryAgainBtnOnClick);
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
        fetchAndResetQuestions();
    }

    public void fetchAndResetQuestions()
    {
        currentQuestionNumber = 0;
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
            summarize();
            displaySummaryPanel(true);
        }
    }

    private void trueBtnOnClick()
    {
        scoreAnswer(true);
    }

    private void falseBtnOnClick()
    {
        scoreAnswer(false);
    }

    private void scoreAnswer(bool trueOrFalse)
    {
        int arrLength = questionDtoArr.Length;
        // Subtract 1 because the nextQuestion is called advanced
        int currentQuestion = currentQuestionNumber - 1;
        if (arrLength > 0 && currentQuestion < arrLength)
        {
            QuestionDTO questionDto = questionDtoArr[currentQuestion];
            String correctAnswer = questionDto.correctAns;
            questionDtoArr[currentQuestion].isAnswered = true;
            questionDtoArr[currentQuestion].isAnsweredCorrectly = (
                    (trueOrFalse && correctAnswer.Equals("1")) || (!trueOrFalse && correctAnswer.Equals("0"))
                ) ? true : false;

            Debug.Log(String.Format("Question #{0} has been aswered {1}", (currentQuestion), (questionDtoArr[currentQuestion].isAnsweredCorrectly) ? "correctly" : "incorrectly"));

            // End the game if a wrong answer has been selected
            if (!questionDtoArr[currentQuestion].isAnsweredCorrectly)
            {
                summarize();
                summaryPanelTitleText.text = "Game Over";
                displaySummaryPanel(true);
            }
            else
            {
                nextQuestion();
            }
        }
    }

    private void summarize()
    {
        int totalScore = 0;
        foreach (QuestionDTO qdto in questionDtoArr)
        {
            if (qdto.isAnswered && qdto.isAnsweredCorrectly)
            {
                totalScore += qdto.questionScoreValue;
            } 
        }

        summaryPanelScoreText.text = totalScore.ToString();
    }

    private void displaySummaryPanel(bool show)
    {
        if (show)
        {
            summaryPanel.gameObject.SetActive(true);
        }
        else
        {
            summaryPanel.gameObject.SetActive(false);
        }
    }

    private void summaryPanelTryAgainBtnOnClick()
    {
        displaySummaryPanel(false);
        fetchAndResetQuestions();
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


