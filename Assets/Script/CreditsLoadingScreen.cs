using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsLoadingScreen : MonoBehaviour
{
    float time = 0;

    [SerializeField] GameObject loadScreen;
    [SerializeField] Image loadBar;

    void Update()
    {
        time += Time.deltaTime;
        if(time >= 45f){
            StartCoroutine(LoadSceneAsync(0));
        }else if(Input.GetKeyDown("space")){
            StartCoroutine(LoadSceneAsync(0));
        }
    }

    IEnumerator LoadSceneAsync(int sceneId){
        float duration = 0;

        loadScreen.SetActive(true);

        while(duration < 2.1f){
            duration += Time.deltaTime;

            loadBar.fillAmount = duration;

            yield return null;
        }

        SceneManager.LoadSceneAsync(sceneId);
    }
}
