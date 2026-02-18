using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops{
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }

    public List<Drops> drops;

    bool hasBeenKilled = false; //questo booleano mi serve per distinguere se il nemico è morto per via del player oppure no

    public void IsKilled(){
        hasBeenKilled = true;
    }

    void OnDestroy(){//la funzione OnDestroy si attiva prima che l'oggetto venga distrutto

        if(!gameObject.scene.isLoaded || hasBeenKilled == false){//questo codice impedisce che i drop rimangano quando chiudi il gioco
            return;//controlla se la scena corrente è caricata, e se non lo è esce dalla funzione
        }

        float randmNum = UnityEngine.Random.Range(0f, 100f);
        List<Drops> possibleDrops = new List<Drops>();
        //se un oggetto ha più drop dobbiamo fare una lista dei possibili drop che può spawnare

        foreach(Drops rate in drops){
            //controlliamo quale dei drop ha la possibilità di uscira confrontando il dropRate con randomNum
            if(randmNum <= rate.dropRate){
                //Se il dropRate è stato verificato allora aggiungiamo l'oggetto alla lista
                possibleDrops.Add(rate); 
            }
        }
        if(possibleDrops.Count > 0){
            //poi controlliamo se la lista non e vuota, e se non lo è allora estraiamo un drop casuale dalla lista 
            Drops drop = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            GameObject dropInstance = Instantiate(drop.itemPrefab, transform.position, Quaternion.identity);
        }
    }
}
