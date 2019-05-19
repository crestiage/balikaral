using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class register : MonoBehaviour
{
    public InputField Username;
    public InputField Email;
    public InputField test;
    public Dropdown Sex;
    public Dropdown Country;

    string CreateUserURL = "http://localhost/balikaral/insertAccount.php";

    public void CreateUser()
    {
        UnityEngine.Debug.Log("hello");
        var userName = Username.text;
        var email = Email.text;
        var sex = Sex.options[Sex.value].text;
        var country = Country.options[Country.value].text;
        
        UnityEngine.Debug.Log("Username: " + userName);
        UnityEngine.Debug.Log("Country: " + country);
        StartCoroutine(CreateUserRequest(userName, email, sex, country));
    }

    private IEnumerator CreateUserRequest(string userName, string email, string sex, string country)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", userName);
        form.AddField("emailPost", email);
        form.AddField("sexPost", sex);
        form.AddField("countryPost", country);

        using (UnityWebRequest www = UnityWebRequest.Post(CreateUserURL, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                SceneManager.LoadScene("verification");
            }
        }
    }
}
