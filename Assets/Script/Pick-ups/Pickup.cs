using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{   
    public float lifespan = 0.5f;
    protected PlayerStats target;// la reference al target serve per far viaggiare l'oggetto verso il nemico
    protected float speed;// la velocità con cui l'oggetto viaggia verso il player
    Vector2 initialPosition;//posizione inziale del pickup
    float initialOffset;//serve per evitare che tuti gli oggetti gallaggiano allo stesso tempo
    //non è un miglioramente meccanico ma semplicemente un miglioramento estetico

    [Header("Bonuses")]
    public int expericence;
    public int health;

    [System.Serializable]
    public struct BobbingAnimation{
        public float frequency;
        public Vector2 direction;
    }

    public BobbingAnimation bobbingAnimation = new BobbingAnimation{frequency = 2f, direction = new Vector2(0,0.3f)};

    protected virtual void Start(){
        initialPosition = transform.position;
        initialOffset = Random.Range(0, bobbingAnimation.frequency);
    }

    protected virtual void Update(){
        if(target){
            //Muoviti verso il personaggio
            Vector2 distance = target.transform.position - transform.position;
            //questo serve per calcolare il movimente verso il player
            //e se il pickup arriva al player allora si auto distrugge
            if(distance.sqrMagnitude > speed * speed * Time.deltaTime){
                transform.position += (Vector3)distance.normalized * speed * Time.deltaTime;
            }else{
                Destroy(gameObject);
            }
        }else{
            transform.position = initialPosition + bobbingAnimation.direction * Mathf.Sin((Time.time + initialOffset) * bobbingAnimation.frequency);
        }
    }

    public virtual bool Collect(PlayerStats target, float speed, float lifespan = 0f){
        if(!this.target){
            this.target = target;
            this.speed = speed;
            if(lifespan > 0){
                this.lifespan = lifespan;
            }else{
                this.lifespan = 0.01f;
            }
            Destroy(gameObject, lifespan);
            return true;
        }

        return false;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("Player")){
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy(){
        if(!target){
            return;
        }

        if(expericence != 0){
            target.IncreaseExpericence(expericence);
        }

        if(health != 0){
            target.RestoreHP(health);
        }
    }
}