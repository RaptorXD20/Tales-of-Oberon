using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TresureChest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col){
        PlayerInventory p = col.GetComponent<PlayerInventory>();
        if(p){
            bool randomBool = Random.Range(0,2) == 0;

            OpenTresureChest(p, randomBool);

            Destroy(gameObject);
        }
    }

    private void OpenTresureChest(PlayerInventory inventory, bool isHigherTier){
        //controlla ogni arma e vedi se può evolversi
        foreach(PlayerInventory.Slot s in inventory.weaponSlots){
            Weapons w = s.item as Weapons;
            if(w.weaponData.evolutionData == null){//ignora le armi che non si evolvono
                continue;
            }

            //controlla ogni possibile evoluzione dell'arma
            foreach(ItemData.Evolution e in w.weaponData.evolutionData){
                //prova a fare l'evoluzione tramite la chest 
                if(e.condition == ItemData.Evolution.Condition.tresureChest){
                    bool attempt = w.AttemptEvolution(e, 0);
                    if(attempt){
                        return;
                    }
                }
            }
        }
    }

}
