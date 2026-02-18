using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventData : SpawnData
{
    [Header("Event Data")]
    [Range(0f, 1f)] public float probability = 1f;//la probabilità con cui l'evento avviene
    [Range(0f, 1f)] public float luckfactor = 1f;

    //se questo valore è maggiore di 0 l'evento spunterà solo dopo un certo livello
    public float activeAfter = 0;

    public abstract bool Activate(PlayerStats player = null);

    //controlla se l'evento corrente è ancora in corso
    public bool IsActive(){
        if(!GameManager.instance){
            return false;
        }
        if(GameManager.instance.GetElapsedTime() > activeAfter){
            return true;
        }
        return false;
    }

    //clacola un probabilità randomica per far avvenire l'evento
    public bool CheckIfWillHappen(PlayerStats s){
        //se la probabilità è uno significa che capita sempre
        if(probability >= 1){
            return true;
        }

        //Altrimenti, prendi un numero casuale e vedi se rispetta la probabilità
        if(probability / Mathf.Max(1, (s.baseStats.luck * luckfactor)) >= Random.Range(0f, 1f)){
            return true;
        }

        return false;
    }
}
