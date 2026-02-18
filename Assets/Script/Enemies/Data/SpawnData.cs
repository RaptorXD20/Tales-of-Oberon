using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnData : ScriptableObject
{
    //Una lista di possibili GameObject che puoi spawnare
    public GameObject[] possibleSpawnPrefabs = new GameObject[1];

    //Tempo tra ogni spawn (in secondi). Prenderà un numero randomico tra X e Y
    public Vector2 spawnInterval = new Vector2(2,3);

    //Quanti nemici spawnao durante l'intervallo
    public Vector2Int spawnsPerTick = new Vector2Int(1,1);

    //in quanto tempo (in secondi) spawna i nemici
    [Min(0.1f)] public float duration = 60;//il flag all'inizio serve per imoostare un valore minimo

    //ritorna un array di nemici da Spawnare
    //prende un paramentro opzionale che indica quanti nemici a schermo ci sono
    public virtual GameObject[] GetSpawns(int totalEnemis = 0){
        //determina quanti nemici spawnano
        int count = Random.Range(spawnsPerTick.x, spawnsPerTick.y);

        //genera il risultato
        GameObject[] result = new GameObject[count];
        for(int i = 0; i < count; i++){
            //scegli randomicamente uno dei possibili nemici da spawnare
            result[i] = possibleSpawnPrefabs[Random.Range(0, possibleSpawnPrefabs.Length)];
        }

        return result;
    }

    //prendi uno intervallo si spawn randomico tra il valore minimo e quello massimo
    public virtual float GetSpawnInterval(){
        return Random.Range(spawnInterval.x, spawnInterval.y);
    }
}
