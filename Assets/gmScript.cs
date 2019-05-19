using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gmScript : MonoBehaviour
{
    public static string currentWord;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        gmScript.currentWord += GetComponent<TextMesh>().text;
        Debug.Log(gmScript.currentWord);
    }
}
