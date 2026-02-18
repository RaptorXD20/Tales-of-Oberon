using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraWeapon : Weapons
{
    protected Aura currentAura;

    protected override void Update(){}

    public override void OnEquip(){
        //prova a rimpiazzare l'area dell'arma con una nuova
        if(currentStats.auraPrefab){
            if(currentAura){
                Destroy(currentAura);
            }
            currentAura = Instantiate(currentStats.auraPrefab, transform);
            currentAura.weapon = this;
            currentAura.player = player;
            currentAura.transform.localScale = new Vector3(currentStats.area, currentStats.area, currentStats.area);
        }
    }

    public override void OnUnequip(){
        if(currentAura){
            Destroy(currentAura);
        }
    }

    public override bool DoLevelUp(){
        if(!base.DoLevelUp()){
            return false;
        }

        if(currentAura){
            currentAura.transform.localScale = new Vector3(currentStats.area, currentStats.area, currentStats.area);
        }

        return true;
    }
}
