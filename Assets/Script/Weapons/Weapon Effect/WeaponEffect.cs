using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Un GameObject che spawna come effetto di un'arma, es: i proiettili di un arma
public abstract class WeaponEffect : MonoBehaviour
{
    [HideInInspector] public PlayerStats player;
    [HideInInspector] public Weapons weapon;

    public float GetDamage(){
        return weapon.GetDamage();
    }
}
