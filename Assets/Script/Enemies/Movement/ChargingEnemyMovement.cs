using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemyMovement : EnemiesMovement
{
    Vector2 chargeDirection;

    //calcoliamo la direzione dove i nemici caricano
    protected override void Start(){
        base.Start();
        chargeDirection = (player.transform.position - transform.position).normalized;
    }

    //invece di muoversi verso il player, muoviti semplicemente in avanti
    public override void Move(){
        transform.position += (Vector3)chargeDirection * enemyData.currentSpeed * Time.deltaTime;
    }
}
