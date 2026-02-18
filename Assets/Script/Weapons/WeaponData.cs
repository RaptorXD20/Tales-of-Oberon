using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//questo codice è il sostituto di WeaponScriptableObject
[CreateAssetMenu(fileName = "Weapon Data", menuName = "ScriptableObject/Weapon Data ")]
public class WeaponData : ItemData
{
    [HideInInspector] public string behaviour;
    public Weapons.Stats baseStats;
    public Weapons.Stats[] linearGrowth;
    public Weapons.Stats[] randomGrowth;
    

    //Ci da le statistiche / descrizione del prossimo livello
    public Weapons.Stats GetLevelData(int level){
        //prendi le statistiche del prossimo livello
        if(level - 2 < linearGrowth.Length){
            return linearGrowth[level - 2];
        }

        //altrimenti prendi le statistiche del randomGrowth 
        if(randomGrowth.Length > 0){
            return randomGrowth[Random.Range(0, randomGrowth.Length)];
        }

        //se non è nessuna di questi ritorna un valore vuoto e manda un messaggio di errore
        Debug.LogWarning(string.Format("Weapon doesn't have its level up stats configured for Level{0}!", level));
        return new Weapons.Stats();
    }
}
