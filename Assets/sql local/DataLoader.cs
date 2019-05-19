using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    public string[] accounts;

    IEnumerator Start()
    {
        //string CreateUserURL = "http://localhost/balikaral/insertAccount.php";
        WWW conn = new WWW("http://localhost/balikaral/connection(accounts).php");
        yield return conn;
        string connString = conn.text;
        print(connString);
        accounts = connString.Split(';');
        print(GetDataValue(accounts[0], "score"));
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if(value.Contains("|"))value = value.Remove(value.IndexOf("|"));
        return value;
    }

}