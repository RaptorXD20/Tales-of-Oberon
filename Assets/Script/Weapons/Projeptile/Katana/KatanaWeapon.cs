using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaWeapon : ProjectileWeapon
{
    int currentSpawnCount;//quante volte l'arma colpisce durante la sua iterazione
    float currentSpawnYOffeset;//se ci sono più istanze le vedremo leggermente spostate nell'asse delle Y

    protected override void Attack(int attackCount = 1){
        //Se non è stato assegnato nessun proiettile, ritorna un messaggio di errore
        if(!currentStats.projectilePrefab){
            Debug.LogWarning(string.Format("Projectile prefab has not been set for {0}", name));
            currentCooldown = weaponData.baseStats.cooldown;
            return;
        }

        //se non c'è alcun proiettile assegnato, setta l'arma come in cooldown
        if(!CanAttack()){
            return;
        }

        //se questo è il primo attacco fatto resetta il currentSpawnCount
        if(currentCooldown <= 0){
            currentSpawnCount = 0;
            currentSpawnYOffeset = 0f;
        }

        //altrimenti calcola l'angolo e l'offset del nostro proiettile spawnato
        //se ci sono più iterazioni del proiettile inverti la direzione in cui spawna
        float spawnDir = Mathf.Sign(movement.lastMovedVector.x) * (currentSpawnCount % 2 != 0 ? -1 : 1);
        Vector2 spawnOffset = new Vector2(
            spawnDir * Random.Range(currentStats.spawnVariance.xMin, currentStats.spawnVariance.xMax), 
            currentSpawnYOffeset
        );

        //aggiungi e spawna il proiettile
        Projectile prefab = Instantiate(currentStats.projectilePrefab, 
            player.transform.position + (Vector3)spawnOffset, 
            Quaternion.identity
        );

        prefab.player = player;//setta il proprietario del'arma

        //inverti lo sprite del proiettile
        if(spawnDir < 0){
            prefab.transform.localScale = new Vector3(
                -Mathf.Abs(prefab.transform.localScale.x),
                prefab.transform.localScale.y,
                prefab.transform.localScale.z
            );
        }

        //assegna le statiche
        prefab.weapon = this;
        currentCooldown = weaponData.baseStats.cooldown;
        attackCount --;

        //determina se c'è un secondo proiettile da spawnare
        currentSpawnCount ++;
        if(currentSpawnCount > 1 && currentSpawnCount % 2 == 0){
            currentSpawnYOffeset += 1;
        }

        //dobbiamo fare un'altro attacco?
        if(attackCount > 0){
            currentAttackCount = attackCount;
            currentAttackInterval = weaponData.baseStats.projectileInterval; 
        }

        return;
    }
}
