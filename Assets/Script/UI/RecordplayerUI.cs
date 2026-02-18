using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecordplayerUI : MonoBehaviour
{
    [Header("Character Data")]
    [SerializeField] TMP_Text playerRecord;

    SaveData save;

    void Start(){
        save = SaveSystem.LoadData();

        if(save != null){
            playerRecord.text = FormatRecord(save.recordPlayer);
        }
    }

    string FormatRecord(float time){
        int minutes = Mathf.FloorToInt(time / 60);//Calcola i minuti
        int seconds = Mathf.FloorToInt(time % 60);//Calcola i secondi
        return string.Format("{0:00}:{1:00}", minutes, seconds);//formattiamo l'orario 
    }
}
