using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMediator : MonoBehaviour
{
    public string startSceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void StartButton()
    {
        SceneManager.LoadScene(startSceneName);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
