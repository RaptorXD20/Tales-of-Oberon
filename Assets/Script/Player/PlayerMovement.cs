using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    [HideInInspector] public Vector2 moveDir;
    //questi due float mi servono per evitare eventuali glich grafici quando il personaggi si sposta sia sulla X che sulla Y
    [HideInInspector] public float lastHorizontalVector;
    [HideInInspector] public float lastVerticalVector;
    [HideInInspector] public Vector2 lastMovedVector;

    //reference
    Rigidbody2D rigidBody;
    PlayerStats player;

    Vector2 knockbackVelocity;//la velocità con cui ci si sposta durante il knockback
    float knockbackDuration;//la durata dello spostamento del knockback

    void Start()
    {
        player = GetComponent<PlayerStats>();
        rigidBody = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1, 0f);//questo ci serve per settare una direzione standard
        //qeusto perchè quando usaremo lastMovedVector il proiettile partirà indipendentemente dal movimento o meno del player
    }

    
    void Update()
    {
        inputManagement();
    }

    void FixedUpdate(){//usiamo FixedUpdate perchè ci pemette di gestire la fisica indipendentemente dai frame
        if(knockbackDuration > 0){
            //se si, continuiamo il knokback
            //transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            rigidBody.AddForce(knockbackVelocity, ForceMode2D.Impulse);
            knockbackDuration -= Time.deltaTime;
        }else{//altrimenti, si muoverà sempre verso il player
            Move(); 
        }
    }

    void inputManagement(){
        //questo if serve a disabilitare gli input quando siamo in Game Over
        if(GameManager.instance.isLevelUp){
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        if(moveDir.x != 0){
            lastHorizontalVector = moveDir.x;
            lastMovedVector = new Vector2(lastHorizontalVector, 0f);//l'ultimo movimento nell'asse x
        }
        if(moveDir.y != 0){
            lastVerticalVector = moveDir.y;
            lastMovedVector = new Vector2(0f, lastVerticalVector);//l'ultimo movimento nell'asse y
        }

        if(moveDir.x != 0 & moveDir.y != 0){//questo ci serve nel caso ci muoviamo in diagonale
            lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector);//l'ultima posizione in diagonale
        }
    }

    void Move(){
        //questo if serve a disabilitare gli input quando siamo in Game Over
        if(GameManager.instance.isLevelUp){
            return;
        }

        rigidBody.velocity = new Vector2(moveDir.x * player.CurrentSpeed, moveDir.y * player.CurrentSpeed);
    }

    public void Knockback(Vector2 velocity, float duration){
        //prima di tutto contolliamo se siamo già dentro un knockback
        //lo facciamo controllando la duratta attuale
        if(knockbackDuration > 0){
            return;
        }

        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
}
