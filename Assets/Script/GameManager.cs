using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //rendiamo il GameManager un Singleton
    public static GameManager instance;

    //qui stiamo definendo i diversi stati del gioco
    public enum GameState{
        Gameplay,
        Paused,
        LevelUp,
        GameOver
    }

    public GameState currentState;
    public GameState previousState;

    //Pause Screen
    [Header("Screens")]
    [SerializeField] GameObject pausedScreen;
    [SerializeField] GameObject resultScreen;
    [SerializeField] GameObject levelUpScreen;

    //Current Stats Display
    [Header("Current Stats Displays")]
    public TMP_Text currentHPDisplay;
    public TMP_Text currentSpeedDisplay;
    public TMP_Text currentProjectileSpeedDisplay;
    public TMP_Text currentMightDisplay;
    public TMP_Text currentRecoveryDisplay;
    public TMP_Text currentMagnetDipslay;
    public TMP_Text currentArmorDisplay;
    public TMP_Text currentLuckDisplay;

    [Header("Results Displays")]
    public Image characterImage;
    public TMP_Text characterName;
    public TMP_Text levelReachedDisplay;
    public TMP_Text timeRunDisplay;
    public TMP_Text killDisplay;
    public List<Image> inventoryWeaponsUI = new List<Image>(6);
    public List<Image> inventoryPassiveItemsUI = new List<Image>(6);


    [Header("Player HUD")]
    public Image playerPoltrait;
    public Image playerHealthBar;
    public Image playerExpBar;
    public TMP_Text playerName;
    public TMP_Text playerLevel;

    [Header("Stopwatch")]
    public float timeLimit; //il limite di tempo dello Stopwatch in secondi
    float stopwatchTime; //il tempo che è passato dal inizio dello stopwatch
    public TMP_Text stopwatchDisplay;

    [Header("Damage Text Settings")]
    [SerializeField] Canvas damageTextCanvas;
    [SerializeField] float textFontSize;
    [SerializeField] TMP_FontAsset textFont;
    [SerializeField] Camera referenceCamera;

    public bool isGameOver = false;//flag di controllo per il Game over
    public bool isLevelUp = false;//flag di controllo del level up

    public GameObject playerObject;//riferimento al player
    PlayerInventory playerInventory;
    GamerOverStats gos;
    int kill;

    void Awake(){
        if(instance == null){
            instance = this;
            playerInventory = playerObject.GetComponent<PlayerInventory>();
            gos = GetComponent<GamerOverStats>();
        }else{
            Debug.LogWarning("Extra " + this + "Deleted!");
            Destroy(gameObject);
        }
        DisableScreen(); 
    }

    void Start(){
        kill = 0;
        stopwatchTime = 0f;
    }

    void Update(){ 

        //usiamo un switch per definere lo stato corrente del gioco
        switch(currentState){
            case GameState.Gameplay:
                CheckPauseAndResume();
                UpdateStopwatch();
                break;

            case GameState.Paused:
                CheckPauseAndResume();
                break;

            case GameState.GameOver:
                if(!isGameOver){//isGameOver deve essere falso, perchè sennò il gioco continuerà a dare Game Over senza fermarsi
                    Time.timeScale = 0f;//ferma il gioco
                    DisplayResults();
                }
                break;
            
            case GameState.LevelUp:
                if(!isLevelUp){
                    isLevelUp = true;
                    Time.timeScale = 0f;
                    levelUpScreen.SetActive(true);
                }
                break;

            default:
            Debug.LogError("The State does not exist!");
                break;
        }
    }

    public float GetElapsedTime(){
        return stopwatchTime;
    }

    //una funzione che incapsula il cambio di stato
    public void ChangeState(GameState newState){
        currentState = newState;
    }

    //Paused state
    public void PauseGame(){
        if(currentState != GameState.Paused){
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f; //Ferma il gioco
            pausedScreen.SetActive(true);
            Debug.Log("The Game is Paused");
        }
    }

    public void ResumeGame(){
        if(currentState == GameState.Paused){
            ChangeState(previousState);
            Time.timeScale = 1f; //Fa ripartire il gioco da dove si era fermato
            pausedScreen.SetActive(false);
            Debug.Log("The Game is Unpaused");
        }
    }
    
    //Questa funzione controlla se viene premuto il tasto per mettere in pausa
    public void CheckPauseAndResume(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(currentState == GameState.Paused){
                ResumeGame();
            }else{
                PauseGame();
            }
        }
    }

    void DisableScreen(){
        pausedScreen.SetActive(false);
        resultScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    //GameOver state
    public void GameOver(){
        isGameOver = false;
        ChangeState(GameState.GameOver);
    }

    void DisplayResults(){
        Debug.Log("Game Over");
        AssaignKillUI();
        gos.SetStatsDisplay();
        resultScreen.SetActive(true);
    }

    public void AssaignCharacterUI(CharacterData characterData){
        characterImage.sprite = characterData.Icon;
        characterName.text = characterData.Name;
        playerName.text = characterData.Name;
        playerPoltrait.sprite = characterData.Poltrait;
    }

    public void AssaignLevelUI(int levelReached){
        levelReachedDisplay.text = levelReached.ToString();
    }

    public void AssaignKillUI(){
        killDisplay.text = kill.ToString();
    }

    public void AssaignTimeOfRun(){
        float time = Time.realtimeSinceStartup;
        int minutes = Mathf.FloorToInt(time) / 60;//Calcola i minuti
        int seconds = Mathf.FloorToInt(time) % 60;//Calcola i secondi
        timeRunDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);//formattiamo l'orario e mandiamolo a schermo
    }

    public void AssaignWeaponsAndPassiveItemsUI(List<Image> weapons, List<Image> passiveItems){
        //controllo se non ci sono priblemi con le liste
        if(inventoryWeaponsUI.Count != weapons.Count || inventoryPassiveItemsUI.Count != passiveItems.Count){
            Debug.Log("Error: chosen weapon and passive item data lists have different lenghts");
            return;
        }

        //assegna le armi dell'inventario alla schermata di Game over
        for(int i=0; i<inventoryWeaponsUI.Count; i++){
            if(weapons[i].sprite){
                inventoryWeaponsUI[i].enabled = true;
                inventoryWeaponsUI[i].sprite = weapons[i].sprite;
            }else{
                inventoryWeaponsUI[i].enabled = false;
            }
        }

        //assegna gli oggetti passivi dell'inventario alla schermata di Game over
        for(int j=0; j<inventoryPassiveItemsUI.Count; j++){
            if(passiveItems[j].sprite){
                inventoryPassiveItemsUI[j].enabled = true;
                inventoryPassiveItemsUI[j].sprite = passiveItems[j].sprite;
            }else{
                inventoryPassiveItemsUI[j].enabled = false;
            }
        }
    }

    public void UpdateStopwatch(){
        stopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();

        if(stopwatchTime >= timeLimit){
            Debug.Log("Timer scaduto");
            ChangeState(GameState.GameOver);
        }
    }

    void UpdateStopwatchDisplay(){
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);//Calcola i minuti
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);//Calcola i secondi
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);//formattiamo l'orario e mandiamolo a schermo
    }

    public float getStopwatchValue(){
        return stopwatchTime;
    }

    public void StartLevelUp(){
        ChangeState(GameState.LevelUp);
        playerInventory.RemoveAndApplyUpgrades();
    }

    public void EndLevelUp(){
        isLevelUp = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }

    public void UpdateHealthbar(float hp, float maxHP){
        playerHealthBar.fillAmount = hp / maxHP;
    }

    public void UpdateExpBar(float exp, float expMax){
        playerExpBar.fillAmount = exp / expMax;
    }

    public void UpdateLeveltext(int level){
        playerLevel.text = "Lv "+ level;
    }

    IEnumerator GenerateFloatingTextCoroutine(string text, Transform target, float duration = 1f, float speed = 1f){
        //inizia a generare il testo
        GameObject textObj = new GameObject();
        RectTransform rect = textObj.AddComponent<RectTransform>();
        TextMeshProUGUI tmPro = textObj.AddComponent<TextMeshProUGUI>();
        tmPro.text = text;
        tmPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
        tmPro.verticalAlignment = VerticalAlignmentOptions.Middle;
        tmPro.fontSize = textFontSize;
        if(textFont){
            tmPro.font = textFont;
        }
        rect.position = referenceCamera.WorldToScreenPoint(target.position);

        //distruggi dopo che il tempo è passato
        Destroy(textObj, duration);

        //rendi l'oggetto un figlio della canvas
        textObj.transform.SetParent(instance.damageTextCanvas.transform);
        textObj.transform.SetSiblingIndex(0);

        //muovi il testo verso l'alto 
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0;
        //float offset = 0;//questo è l'offset dell asse delle y
        Vector3 lastKnownPosition = target.position;
        while(t < duration){

            //dissolvenza del testo
            tmPro.color = new Color(tmPro.color.r, tmPro.color.g, 0, 1 - t/duration);

            //se il target esiste allora setta la posizione del pop up
            if(target){
                lastKnownPosition = target.position;
            }

            //aspetta il frame
            yield return w;
            t += Time.deltaTime;
        } 
    }

    public static void GenerateFloatingText(string text, Transform target, float duration = 1f, float speed = 1f){
        if(!instance.damageTextCanvas){//se non esiste la canvas non fare niente
            return;
        }

        //se non hai la reference alla camera creala
        if(!instance.referenceCamera){
            instance.referenceCamera = Camera.main;
        }

        instance.StartCoroutine(instance.GenerateFloatingTextCoroutine(text, target, duration, speed));
    }

    public void AddKill(){
        kill++;
    }

    public void TimeScale(){
        Time.timeScale = 1f;
    }
}
