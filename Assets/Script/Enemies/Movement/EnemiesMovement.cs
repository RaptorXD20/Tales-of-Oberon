using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesMovement : MonoBehaviour
{
    //variables
    protected EnemyStats enemyData;
    protected Transform player;//questa è una referance alla posizione del player
    protected Rigidbody2D rb;//per vedere se ha un rigidbody

    protected Vector2 knockbackVelocity;//la velocità con cui ci si sposta durante il knockback
    protected float knockbackDuration;//la durata dello spostamento del knockback

    public enum OutOfFrameAction{none, respawnAtEdge, despawn}
    public OutOfFrameAction outOfFrameAction = OutOfFrameAction.respawnAtEdge;

    [System.Flags]
    public enum KnockbackVariance{ duration = 1, velocity = 2}
    public KnockbackVariance knockbackVariance = KnockbackVariance.velocity;

    protected bool spawnedOutOfFrame = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        spawnedOutOfFrame = !SpawnManager.IsWithinBoundaries(transform);
        enemyData = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();

        //scegli un giocatore random sull schermo, invece di prendere sempre il primo
        PlayerMovement[] allPlayers = FindObjectsOfType<PlayerMovement>();
        player = allPlayers[Random.Range(0, allPlayers.Length)].transform;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //controlliamo se siamo sotto l'effeto di un knockback
        if(knockbackDuration > 0){
            //se si, continuiamo il knokback
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }else{//altrimenti, si muoverà sempre verso il player
            Move();
            HandleOutOfFrameAction();
        }
    }

    //se i nemici escono dal frame, gestiscili
    protected virtual void HandleOutOfFrameAction(){
        if(!SpawnManager.IsWithinBoundaries(transform)){
            switch(outOfFrameAction){
                case OutOfFrameAction.none: default:
                    break;
                case OutOfFrameAction.respawnAtEdge:
                    //se il nemico è fuori dalla telecamera, teletrasportalo vicono al bordo della camera
                    transform.position = SpawnManager.GeneratePosition();
                    break;
                case OutOfFrameAction.despawn:
                    //se esce dalla telecamera distruggilo
                    //ma non lo fare se spawna fuori dal frame
                    if(!spawnedOutOfFrame){
                        Destroy(gameObject);
                    }
                    break;
            }
        }else{
            spawnedOutOfFrame = false;
        }
    }

    public virtual void Move(){
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyData.currentSpeed * Time.deltaTime);
    }

    public virtual void Knockback(Vector2 velocity, float duration){
        //prima di tutto contolliamo se siamo già dentro un knockback
        //lo facciamo controllando la duratta attuale
        if(knockbackDuration > 0){
            return;
        }

        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
}
