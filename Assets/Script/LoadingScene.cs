using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] GameObject loadScreen;
    [SerializeField] Image loadBar;
    [SerializeField] int currentScene;
    PlayerStats player;
    PlayerMovement playerMove;

    void Awake(){
        loadBar.fillAmount = 0;
        player = FindObjectOfType<PlayerStats>();
        if(player){
            playerMove = FindObjectOfType<PlayerStats>().GetComponent<PlayerMovement>();
        }
    }

    public void LoadScene(int sceneIndex){
        if(currentScene == 0){//se ci troviamo nella scena di gioco
            if(player){
                player.gameEnding = true;
                playerMove.enabled = false;
            }
        }
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    IEnumerator LoadSceneAsync(int sceneId){

        float duration = 0;

        loadScreen.SetActive(true);

        if(Time.timeScale < 1f){
            Time.timeScale = 1f;
        }
        while(duration < 2.1f){
            duration += Time.deltaTime;

            loadBar.fillAmount = duration;

            yield return null;
        }

        SceneManager.LoadSceneAsync(sceneId);
    }
}
