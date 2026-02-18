using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //record personale
    public float recordPlayer;

    //recod dei personaggio;
    public float[] records;

    //kill
    public int[] kills;

    public SaveData(float recordPlayer, float[] record, int[] kill){
        this.recordPlayer = recordPlayer;

        records = new float[4];
        for(int i=0; i<4; i++){
            records[i] = record[i];
        } 

        kills = new int[4];
        for(int j=0; j<4; j++){
            kills[j] = kill[j];
        }
    }
}
