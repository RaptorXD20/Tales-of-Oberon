using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//questo codice sostituisce PassiveItemScriptableObject
//l'idea è che volgiamo raccogliere tutti i dati di un oggetto passivo in un singolo oggetto 
//invece di avere più oggetti che gestiscono cose diverse
//cosa che non sabbe possibile se continuiamo ad usare PassiveItemScriptableObject
[CreateAssetMenu(fileName = "Passive Item Data", menuName = "ScriptableObject/Passive Item Data ")]
public class PassiveData : ItemData
{
    public Passive.Modifier baseStats;
    public Passive.Modifier[] growth;

    public Passive.Modifier GetLevelData(int level){
        //prendi le statistiche dal prossimo livello
        if(level - 2 < growth.Length){
            return growth[level - 2];
        }

        //altrimenti ritorna un oggetto vuoto e un warning
        Debug.LogWarning(string.Format("Passive Item doesn't have its Level up stats configured for level {0}", level));
        return new Passive.Modifier();
    }
}
