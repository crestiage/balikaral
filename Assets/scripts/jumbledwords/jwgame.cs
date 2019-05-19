using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class jwgame : MonoBehaviour
{
    
    private JumbledwordsDTO[] jws;
    private WebRequestHelper wrh;

    GameObject questionPanel;

    private int currentQuestionNumber = 0;

    public Text questionBox;
    public Button letterA;
    public Button letterB;
    public Button letterC;
    public Button letterD;
    public Button letterE;
    public Button letterF;
    public Button letterG;
    public Button letterH;
    public Button letterI;
    public Button letterJ;



    private string JumbledWordsURL = "http://localhost/balikaral/connection_jumbled_words.php";
    // Start is called before the first frame update
    void Start()
    {

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fetchQuestions()
    {
        wrh.fetchWebDataString(JumbledWordsURL, processQuestionDTO);
    }

    public void processQuestionDTO(string result)
    {
        // Debug.Log("Called! TestCallback");
        string jsonString = result;
        jsonString = CommonHelper.JsonHelper.fixJson(jsonString);
        jws = CommonHelper.JsonHelper.FromJson<JumbledwordsDTO>(jsonString);
        Debug.Log(jws);
        // Debug.Log(questionDtoArr[0].isAnswered);
        nextQuestion();
    }

    public void nextQuestion()
    {
        // Debug.Log(currentQuestionNumber);
        int arrLength = jws.Length;
        if (arrLength > 0 && currentQuestionNumber < arrLength)
        {
            JumbledwordsDTO questionDto = jws[currentQuestionNumber];
            questionBox.text = questionDto.question;
            currentQuestionNumber += 1;
            Debug.Log(questionDto.question);
        }
        else if (currentQuestionNumber >= arrLength)
        {
            // Summary page
        }
    }

}
