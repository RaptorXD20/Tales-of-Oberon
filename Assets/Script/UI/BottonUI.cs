using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BottonUI : MonoBehaviour
{
    [SerializeField] CharacterData playerData;

    [Header("Display")]
    [SerializeField] TMP_Text displayHP;
    [SerializeField] TMP_Text displayMoveSpeed;
    [SerializeField] TMP_Text displayMight;
    [SerializeField] TMP_Text displayRecovery;
    [SerializeField] TMP_Text displayArmor;
    [SerializeField] TMP_Text displayLuck;
    [SerializeField] TMP_Text displayMagnet;
    [SerializeField] TMP_Text displaySpeed;

    [Header("Charcter info")]
    [SerializeField] Image displayIcon;
    [SerializeField] Image displayPoltrait;
    [SerializeField] Image displayStartingWeapon;

    [Header("Character Data")]
    [SerializeField] TMP_Text Kill;
    [SerializeField] TMP_Text Record;


    SaveData save;

    void Start(){
        save = SaveSystem.LoadData();

        if(save != null){
            Record.text = FormatRecord(save.records[FindIndex(playerData)]);
            Kill.text = (save.kills[FindIndex(playerData)]).ToString();
        }

        if(playerData){
            displayHP.text = playerData.stats.maxHP.ToString();
            displayMoveSpeed.text = playerData.stats.moveSpeed.ToString();
            displayMight.text = playerData.stats.might.ToString();
            displayRecovery.text = playerData.stats.recovery.ToString();
            displayArmor.text = playerData.stats.armor.ToString();
            displayLuck.text = playerData.stats.luck.ToString();
            displayMagnet.text = playerData.stats.magnet.ToString();
            displaySpeed.text = playerData.stats.speed.ToString();

            displayIcon.sprite = playerData.Icon;
            displayStartingWeapon.sprite = playerData.StartingWeapon.icon;
            displayPoltrait.sprite = playerData.Poltrait;
        }
    }

    string FormatRecord(float time){
        int minutes = Mathf.FloorToInt(time / 60);//Calcola i minuti
        int seconds = Mathf.FloorToInt(time % 60);//Calcola i secondi
        return string.Format("{0:00}:{1:00}", minutes, seconds);//formattiamo l'orario 
    }

    int FindIndex(CharacterData data){
        string name = data.Name;
        int result = 0;

        switch(name){
            case "Korze":
                result = 0;
                break;
            case "Tamaki":
                result = 1;
                break;
            case "Silveras":
                result = 2;
                break;
            case "Rubinas":
                result = 3;
                break;
            default:
                break;
        }

        return result;
    }
}
