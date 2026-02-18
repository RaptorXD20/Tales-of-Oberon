using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    
    SaveData saveData;
    int totalKill;
    PlayerStats player;
    string playerName;

    void Start(){
        player = FindObjectOfType<PlayerStats>();
        totalKill = 0;
        if(player != null){
            playerName = CheckCharacter(player.GetCharacterData());
        }

        totalKill = 0;
        saveData = SaveSystem.LoadData();

        if(saveData == null){
            float[] record = new float[4];
            for(int i=0; i < 4; i++){
                record[i] = 0f;
            }

            int[] kill = new int[4];
            for(int j=0; j < 4; j++){
                kill[j] = 0;
            }
            saveData = new SaveData(0, record, kill);
            DoSave(0, record, kill);
        }
    }

    public void SaveAndGo(){
        float record = GameManager.instance.getStopwatchValue();
        if(record < saveData.recordPlayer){//se il player non ha superato il suo record, ritorna il vecchio record
            record = saveData.recordPlayer;
        }

        int[] kills = new int[4];
        int index = 0;
        switch (playerName){
            case "Korze":
                index = 0;
                kills = ReturnNewKill(index,saveData.kills, totalKill);
                break;
            case "Tamaki":
                index = 1;
                kills = ReturnNewKill(index,saveData.kills, totalKill);
                break;
            case "Silveras":
                index = 2;
                kills = ReturnNewKill(index,saveData.kills, totalKill);
                break;
            case "Rubinas":
                index = 3;
                kills = ReturnNewKill(index,saveData.kills, totalKill);
                break;
            default:
                for(int j=0; j < 4; j++){
                    kills[j] = 0;
                }
                break;
        }
        DoSave(record,ReturnNewRecords(index, saveData.records, record), kills);
    }
    
    public void DoSave(float recordPlayer, float[] record, int[] kill){
        SaveSystem.SaveRun(recordPlayer, record, kill);
    }

    public void SaveKill(){
        totalKill++;
    }

    string CheckCharacter(CharacterData data){
        string name = data.Name;
        string result = null;

        switch(name){
            case "Korze":
                result = "Korze";
                break;
            case "Tamaki":
                result = "Tamaki";
                break;
            case "Silveras":
                result = "Silveras";
                break;
            case "Rubinas":
                result = "Rubinas";
                break;
            default:
                break;
        }

        return result;
    }

    float[] ReturnNewRecords(int index, float[] records, float newRecord){
        float[] aux = records;
        switch(index){
            case 0:
                if(aux[index] < newRecord){
                    aux[index] = newRecord;
                }
                break;
            case 1:
                if(aux[index] < newRecord){
                    aux[index] = newRecord;
                }
                break;
            case 2:
                if(aux[index] < newRecord){
                    aux[index] = newRecord;
                }
                break;
            case 3:
                if(aux[index] < newRecord){
                    aux[index] = newRecord;
                }
                break;

            default:
                return aux;
        }

        return aux;
    }

    int[] ReturnNewKill(int index, int[] kills, int newKills){
        int[] aux =kills;
        switch(index){
            case 0:
                if(aux[index] < newKills){
                    aux[index] = newKills;
                }
                break;
            case 1:
                if(aux[index] < newKills){
                    aux[index] = newKills;
                }
                break;
            case 2:
                if(aux[index] < newKills){
                    aux[index] = newKills;
                }
                break;
            case 3:
                if(aux[index] < newKills){
                    aux[index] = newKills;
                }
                break;

            default:
                break;
        }

        return aux;
    }

    public void ResetSave(){
        SaveSystem.ResetSave();
    }
}
