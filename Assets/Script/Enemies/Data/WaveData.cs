using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Wave Data", menuName = "ScriptableObject/Wave Data")]
public class WaveData : SpawnData
{
    [Header("Wave Data")]
    //se ci sono meno nemici di questo valore, continuamo a generare nemici
    [Min(0)] public int startingCount = 0;
    //quanti nemici posso spawnare al massimo
    [Min(1)] public uint totalSpawns = uint.MaxValue;

    [System.Flags] public enum ExitCondition{waveDuration = 1, reachedTotalSpawns = 2}
    //setta tutto ciò che succede quando finisce una wave
    public ExitCondition exitCondition = (ExitCondition)1;

    //tutti i nemici devono essere morti prima che la wave finisca
    public bool mustKillAll = false;

    //il numero di nemici già spawnati nella wave
    [HideInInspector] public uint spawnCount;

    //ritorna un array di prefab che quasta wave può spawnare
    //prende una variabile opzionale che indica il numero di nemici a schermo
    public override GameObject[] GetSpawns(int totalEnemis = 0){
        //determina quanti nemici spawnano
        int count = Random.Range(spawnsPerTick.x, spawnsPerTick.y);
        //setta il count al numero di nemici
        //popola lo schermo finche non raggiungi il minimo di nemici da spawnare
        if(totalEnemis + count < startingCount){
            count = startingCount - totalEnemis;
        }

        //genera il risultato
        GameObject[] result = new GameObject[count];
        for(int i = 0; i < count; i++){
            //Scegli uno dei possibili spawn e aggiungilo al risultato
            result[i] = possibleSpawnPrefabs[Random.Range(0, possibleSpawnPrefabs.Length)];
        }

        return result;
    }

}
