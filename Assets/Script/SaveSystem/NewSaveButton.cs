using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSaveButton : MonoBehaviour
{
    [SerializeField] GameObject continueButton;
    SaveManager sm;

    SaveData save;

    void Start(){
        continueButton = GameObject.Find("Continua");
        sm = FindObjectOfType<SaveManager>();
        save = SaveSystem.LoadData();

        if(save == null){
            float[] record = new float[4];
            for(int i=0; i < 4; i++){
                record[i] = 0f;
            }

            int[] kill = new int[4];
            for(int j=0; j < 4; j++){
                kill[j] = 0;
            }
            save = new SaveData(0, record, kill);
            sm.DoSave(0, record, kill);
        }
    }

    void Update(){
        if(save == null || save.recordPlayer == 0f){
            continueButton.SetActive(false);
        }else{
            continueButton.SetActive(true);
        }
    }
}
