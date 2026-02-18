using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : WeaponEffect
{
    Dictionary<EnemyStats, float> affectedTargets = new Dictionary<EnemyStats, float>();
    List<EnemyStats> targetsToUnaffect = new List<EnemyStats>(); 

    void Update(){
        Dictionary<EnemyStats, float> affectedTargetsCopy = new Dictionary<EnemyStats, float>(affectedTargets);

        //controlla tutti i nemici colpiti dall'arma e riduci il cooldown
        //se il cooldown si riduce a zero farà danno ai nemici
        foreach(KeyValuePair<EnemyStats, float> pair in affectedTargetsCopy){
            affectedTargets[pair.Key] -= Time.deltaTime;
            if(pair.Value <= 0){
                if(targetsToUnaffect.Contains(pair.Key)){
                    //se il nemico è già stato colpito non gli deve essere applicato il danno
                    affectedTargets.Remove(pair.Key);
                    targetsToUnaffect.Remove(pair.Key);
                }else{
                    //resetta il cooldown e applica il danno
                    Weapons.Stats stats = weapon.GetStats();
                    affectedTargets[pair.Key] = stats.cooldown;
                    pair.Key.TakeDamage(GetDamage(), transform.position, stats.knockback);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.TryGetComponent(out EnemyStats es)){
            //se il nemico non ha più l'invibiltà su quest'arma
            //aggiungilo ai target
            if(!affectedTargets.ContainsKey(es)){
                //parti sempre con un intervallo di 0 
                //così che il nemico prende danno al prossimo frame
                affectedTargets.Add(es, 0);
            }else{
                if(targetsToUnaffect.Contains(es)){
                    targetsToUnaffect.Remove(es);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D col){
        if(col.TryGetComponent(out EnemyStats es)){
            //non rimuovere il nemico nel momento stesso in cui esce dall'area dell'arma
            //perchè dobbiamo comunque tenere traccia del cooldown della sua invincibilità
            if(affectedTargets.ContainsKey(es)){
                targetsToUnaffect.Add(es);
            }
        }
    }
}
