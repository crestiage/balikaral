using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestHelper
{
    // Start is called before the first frame update

    private MonoBehaviour mono;

    public bool showDebugMsg = false;

    public WebRequestHelper(MonoBehaviour classInstance)
    {
        mono = classInstance;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Sample Call for requestWebDataString
    public void fetchWebDataString(string uri, System.Action<string> callback)
    {
        // assigning example
        // string result
        // mono.StartCoroutine(requestWebDataString(uri, value => result = value));
        mono.StartCoroutine(requestWebDataString(uri, callback));
    }

    private IEnumerator requestWebDataString (string uri, System.Action<string> result)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError)
        {
            logMsg("Error While Sending: " + uwr.error);
        }
        else
        {
            string recvdString = uwr.downloadHandler.text;
            logMsg("Received: " + recvdString);
            result(recvdString);
        }
    }

    private void logMsg(string msg)
    {

        string debugMsg = string.Format("[{0}] {1}", this.GetType().Name, msg);
        if (showDebugMsg)
        {
            Debug.Log(debugMsg);
        }
        
    }

}
