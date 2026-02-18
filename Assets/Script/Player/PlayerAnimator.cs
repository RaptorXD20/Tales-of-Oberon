using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    //Reference
    Animator animator;
    PlayerMovement playerMove; 
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMovement>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMove.moveDir.x != 0 || playerMove.moveDir.y != 0){
            animator.SetBool("IsMoving", true);
            SpriteDirectionCheck();
        }else{
            animator.SetBool("IsMoving", false);
        }
    }

    void SpriteDirectionCheck(){
        if(playerMove.lastHorizontalVector <0){//questo codice farà girare lo sprite se gli diciamo di andare indietro
            sprite.flipX = true;
        }else{
            sprite.flipX = false;
        }
    }

    public void SetAnimatorController(RuntimeAnimatorController controller){
        //controlla se c'è l'animetor
        if(!animator){
            animator = GetComponent<Animator>();
        }
        //poi aggiorna l'animator controller
        animator.runtimeAnimatorController = controller;
    }
}
