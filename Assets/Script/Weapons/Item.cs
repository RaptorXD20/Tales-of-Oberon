using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//classe di base per Armi e Oggetti Passivi
//Il suo scopo principale è quello di gestire le evoluzioni, 
//perchè volgiamo sia le armi che gli oggetti passivi possano evolversi

public abstract class Item : MonoBehaviour
{
    public int currentLevel = 1, maxLevel = 3;

    //Evolution reference
    protected ItemData.Evolution[] evolutionData;
    protected PlayerInventory inventory;

    protected PlayerStats player;

    public virtual  void Initialise(ItemData data){
        maxLevel = data.maxLevel;

        //Prendi i dati dell'evoluzioni
        evolutionData = data.evolutionData;
        //prendi la reference all'invetario
        inventory = FindObjectOfType<PlayerInventory>();

        player = FindObjectOfType<PlayerStats>();
    }

    //chiama questa funzione per prendere ogni evoluzione che l'oggetto ha 
    public virtual ItemData.Evolution[] CanEvolve(){
        List<ItemData.Evolution> possibleEvolution = new List<ItemData.Evolution>();

        //controlla tutte le evoluzioni e aggiugile alla lista
        foreach(ItemData.Evolution evolution in evolutionData){
            if(CanEvolveCheck(evolution)){
                possibleEvolution.Add(evolution);
            }
        }

        return possibleEvolution.ToArray();
    }

    //verifica se una singola evoluzione è possibile
    public virtual bool CanEvolveCheck(ItemData.Evolution evo, int levelUpAmount = 1){
        //non può evolversi se l'oggetto non ha raggiunto il livello per evolversi
        if(evo.evolutionLevel > currentLevel + levelUpAmount){
            Debug.LogWarning(string.Format("Evolution failed. Current Level {0}, evolution level {1}", currentLevel, evo.evolutionLevel));
            return false;
        }

        //controlla se tutti i catalizzatori sono nell'invetario
        foreach(ItemData.Evolution.Config c in evo.catalyst){
            Item item = inventory.Get(c.itemType);
            if(!item || item.currentLevel < c.level){
                Debug.LogWarning(string.Format("Evolution failed. Missing {0}", c.itemType.name));
                return false;
            }
        }

        //altrimenti permetti l'evoluzione
        return true;
    }

    //questa funzione crea una nuova arma da aggiungere all'inventario e rimuove le armi che dovrebbero essere consumate
    public virtual bool AttemptEvolution(ItemData.Evolution evolutionData, int levelUpAmount = 1){
        if(!CanEvolveCheck(evolutionData, levelUpAmount)){
            return false;
        }

        bool consumePassives = (evolutionData.consumes & ItemData.Evolution.Consumption.passives) > 0? true : false;
        bool consumeWeapons = (evolutionData.consumes & ItemData.Evolution.Consumption.weapons) > 0? true : false;

        //cicla tra tutti i catalizzatori e controlla se dobbiamo consumarli
        foreach(ItemData.Evolution.Config c in evolutionData.catalyst){
            if(c.itemType is PassiveData && consumePassives){
                inventory.Remove(c.itemType, true);
            }
            if(c.itemType is WeaponData && consumeWeapons){
                inventory.Remove(c.itemType, true);
            }
        }

        //bisogna consumare anche l'oggetto che si sta evolvendo?
        if(this is Passive && consumePassives){
            inventory.Remove((this as Passive).data, true);
        }
        if(this is Weapons && consumeWeapons){
            inventory.Remove((this as Weapons).weaponData, true);
        }

        //inserisci la nuova arma nell'invetario
        inventory.Add(evolutionData.outcome.itemType);
        return true;
    }

    public virtual bool CanLevelUp(){
        return currentLevel <= maxLevel;
    }

    //quando un arma fa level up , vedi se può evolvere
    public virtual bool DoLevelUp(){
        if(evolutionData == null) return true;
        //prova ad evolversi in ogni evoluzione disponibile di quest'arma
        foreach(ItemData.Evolution e in evolutionData){
            if(e.condition == ItemData.Evolution.Condition.auto){
                AttemptEvolution(e);
            }
        }
        return true;
    }

    //quale effetto ricevi quando equipaggi l'arma
    public virtual void OnEquip(){}

    //quali effetti perdi di'equipagiando l'arma
    public virtual void OnUnequip(){}


}
