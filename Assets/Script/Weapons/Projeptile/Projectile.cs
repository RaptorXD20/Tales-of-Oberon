using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//questo codice va messo in ogni prefab delle armi che vengono lanciate
public class Projectile : WeaponEffect
{
    [SerializeField] enum DamageSource{ projectile, player};//questa variambile serve per aplicare il knockback
    //se si sceglie come fonte il player, il nemico verrà spinto seguendo un vettore che parte dal player
    //mentre se si sceglie il projectile, l'oggetto si sposterà dal proiettile
    [SerializeField] DamageSource damageSource = DamageSource.projectile;
    public bool hasAutoAim = false;
    public Vector3 rotationSpeed = new Vector3(0,0,0);

    protected Rigidbody2D rb;
    protected int piercing;
    protected float range;

    protected virtual void Start(){
        rb = GetComponent<Rigidbody2D>();
        Weapons.Stats stats = weapon.GetStats();
        if(rb.bodyType == RigidbodyType2D.Dynamic){
            rb.angularVelocity = rotationSpeed.z;
            rb.velocity = transform.right * stats.speed;
        }

        //inpedisci all'area di essere 0, perchè altrimenti nasconderebbe il proiettile
        float area = stats.area == 0 ? 1 : stats.area;
        transform.localScale = new Vector3(area * Mathf.Sign(transform.localScale.x), area * Mathf.Sign(transform.localScale.y), -45);

        //setta la perforazione del proiettile
        piercing  = stats.piercing;

        //distruggi il proiettile quando finisce il suo life span
        if(stats.lifespan > 0){
            Destroy(gameObject, stats.lifespan);
        }

        //se il proiettile ha auto-aiming, allora targhetta automaticamente il nemico
        if(hasAutoAim == true){
            AcquireAutoAimFacing();
        }
    }

    //questa funzione permette il funzionamento dell'auto-aming
    public virtual void AcquireAutoAimFacing(){
        float aimAngle = 0f; //serve a determinare dove mirare
        List<EnemyStats> targets = new List<EnemyStats>(FindObjectsOfType<EnemyStats>());//lista di tutti i nemici nell'area

        //seleciona un nemico randomico, se c'è nè
        //altrimenti, scegli un angolo randomico
        if(targets.Count > 0){
            EnemyStats selectTarget = PickEnemy(targets);
            Vector2 difference = selectTarget.transform.position - transform.position;
            aimAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }else{
            aimAngle = Random.Range(0, 360f);  
        }

        //punta il proiettile contro il nemico
        transform.rotation = Quaternion.Euler(0,0,aimAngle);
    }

    EnemyStats PickEnemy(List<EnemyStats> allSelectedEnemies){
        EnemyStats target = null;
        while(!target && allSelectedEnemies.Count > 0){
            int idx = Random.Range(0, allSelectedEnemies.Count);
            target = allSelectedEnemies[idx];

            //se il target è già morto, rimuovilo e skippa
            if(!target){
                allSelectedEnemies.Remove(target);
                continue;
            }

            //controlla se il nemico è sullo schermo
            Renderer r = target.GetComponent<Renderer>();
            if(!r || !r.isVisible){
                allSelectedEnemies.Remove(target);
                target = null;
                continue;
            }
        }

        allSelectedEnemies.Remove(target);
        return target;
    }

    protected virtual void FixedUpdate(){
        if(rb.bodyType == RigidbodyType2D.Kinematic){
            Weapons.Stats stats = weapon.GetStats();
            transform.position += transform.right * stats.speed * Time.fixedDeltaTime;
            rb.MovePosition(transform.position);
            transform.Rotate(rotationSpeed * Time.fixedDeltaTime);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D col){
        EnemyStats es = col.GetComponent<EnemyStats>();//sarà diversa da null se colpisco un nemico
        BreakableProps bp = col.GetComponent<BreakableProps>();//sarà diversa da null se colpisco un oggetto

        //controlla se colpisce un nemico o un oggetto
        if(es){
            //prendi il danno dell'arma
            Vector3 source = damageSource == DamageSource.player && player ? player.transform.position : transform.position;

            //fai danno al nemico
            es.TakeDamage(GetDamage(), source);
            Weapons.Stats stats = weapon.GetStats();
            piercing--;

            if(stats.hitEffect){
                Destroy(Instantiate(stats.hitEffect, transform.position, Quaternion.identity), 0.4f);
            }

        }else if(bp){

            bp.TakeDamage(GetDamage());
            piercing--;

            Weapons.Stats stats = weapon.GetStats();
        }

        if(piercing <= 0){
            Destroy(gameObject);
        }
    }
}
