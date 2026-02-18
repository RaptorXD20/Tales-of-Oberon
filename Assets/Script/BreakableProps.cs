using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    [SerializeField] float currentHP;
    public void TakeDamage(float dmg){
        currentHP -= dmg;
        if(currentHP <= 0){
            Kill();
        }
    }

    void Kill(){
        Destroy(gameObject);
    }
}
