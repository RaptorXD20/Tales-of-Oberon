using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapons
{
    protected float currentAttackInterval;
    protected int currentAttackCount;

    protected override void Update(){
        base.Update();

        //se currentAttackInterval è minore di 0 zero, attachiamo
        if(currentAttackInterval > 0){
            currentAttackInterval -= Time.deltaTime;
            if(currentAttackInterval <= 0){
                Attack(currentAttackCount);
            }
        }
    }

    public override bool CanAttack(){
        if(currentAttackCount > 0){
            return true;
        }else{
            return base.CanAttack();
        }
    }

    protected override void Attack(int attackCount){
        //se non è stato inserito nessun prefab per il proiettile
        if(!currentStats.projectilePrefab){
            Debug.LogWarning(string.Format("Il prefab del proiettile non è stato inserite per {0}", name));
            currentCooldown = weaponData.baseStats.cooldown;
            return;
        }

        //controlliamo se possiamo attaccare
        if(!CanAttack()){
            return;
        }

        //se puoi attaccare calcola l'angolo e l'offset del proiettile
        float spawnAngle = GetSpawnAngle();

        //Spawna il proiettile
        Projectile prefab = Instantiate(currentStats.projectilePrefab, 
            player.transform.position + (Vector3)GetSpawnOffset(spawnAngle), 
            Quaternion.Euler(0,0,spawnAngle));
        
        prefab.weapon = this;
        prefab.player = player;
        //prefab.transform.Rotate(0,0,-45);

        //resetta il cooldown solo se è l'attacco avviene perchè è già scaduto 
        if(currentCooldown <= 0){
            currentCooldown +=  currentStats.cooldown;
        }

        attackCount--;

        //faccaimo per caso un'altro attacco
        if(attackCount > 0){
            currentAttackCount = attackCount;
            currentAttackInterval = weaponData.baseStats.projectileInterval;
        }
    }

    //prendi la direzione nella cuale deve dirigersi il proiettile
    protected virtual float GetSpawnAngle(){
        return Mathf.Atan2(movement.lastMovedVector.y, movement.lastMovedVector.x) * Mathf.Rad2Deg;
    }

    //scegli un punto random dove far spawnare il proiettile
    //fallo girare verso la direzione del nemico
    protected virtual Vector2 GetSpawnOffset(float angle){
        return Quaternion.Euler(0,0,angle) * new Vector2(
            Random.Range(currentStats.spawnVariance.xMin, currentStats.spawnVariance.xMax),
            Random.Range(currentStats.spawnVariance.yMin, currentStats.spawnVariance.yMax)
        );
    }
}
