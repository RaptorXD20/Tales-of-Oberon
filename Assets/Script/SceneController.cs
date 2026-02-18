using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;//è necessaria per gestire le scene

public class SceneController : MonoBehaviour
{

    public void SceneChange(string name){
        SceneManager.LoadScene(name);
    }

    public void Exit(){
        Application.Quit();
    }
}
