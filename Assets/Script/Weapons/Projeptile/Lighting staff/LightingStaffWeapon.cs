using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingStaffWeapon : ProjectileWeapon
{
    List<EnemyStats> allSelectedEnemies = new List<EnemyStats>();

    protected override void Attack(int attackCount = 1){
        //se non c'è alcun prefab assegnato, manda un messaggio di errore
        if(!currentStats.hitEffect){
            Debug.LogWarning(string.Format("Hit effect prefab has not been set for {0}", name));
            currentCooldown = currentStats.cooldown;
            return;
        }

        if(!CanAttack()){
            return;
        }

        //se il cooldown arriva a 0, questo è il primo colpo dell'arma
        //ricalcola l'array dei nemici
        if(currentCooldown <= 0){
            allSelectedEnemies = new List<EnemyStats>(FindObjectsOfType<EnemyStats>());
            currentCooldown += currentStats.cooldown;
            currentAttackCount = attackCount;
        }

        //trova un nemico da colpire
        EnemyStats target = PickEnemy();
        if(target){
            DamageArea(target.transform.position, currentStats.area, GetDamage());
            GameObject hit = Instantiate(currentStats.hitEffect, target.transform.position, Quaternion.identity);
            Destroy(hit, 0.5f);
        }

        //se abbaimo più di un attacco da fare
        if(attackCount > 0){
            currentAttackCount = attackCount - 1;
            currentAttackInterval = currentStats.projectileInterval;
        }
    }

    //controlla se ci sono nemici a scherma da colpire
    EnemyStats PickEnemy(){
        EnemyStats target = null;
        while(!target && allSelectedEnemies.Count > 0){
            int idx = Random.Range(0, allSelectedEnemies.Count);
            target = allSelectedEnemies[idx];

            //se il target è già morto, rimuovilo e skippa
            if(!target){
                allSelectedEnemies.Remove(target);
                continue;
            }

            //controlla se il nemico è sullo schermo
            Renderer r = target.GetComponent<Renderer>();
            if(!r || !r.isVisible){
                allSelectedEnemies.Remove(target);
                target = null;
                continue;
            }
        }

        allSelectedEnemies.Remove(target);
        return target;
    }

    //fa danni ad area
    void DamageArea(Vector2 position, float radius, float damage){
        Collider2D[] targets = Physics2D.OverlapCircleAll(position, radius);
        foreach(Collider2D t in targets){
            EnemyStats es = t.GetComponent<EnemyStats>();
            if(es){
                es.TakeDamage(damage, transform.position);
            }
        }
    }
}
