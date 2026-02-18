using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapons : Item
{
    [System.Serializable]
    public struct Stats{
        public string name, description;

        [Header("Visulals")]
        public Projectile projectilePrefab;//Lo utilizzi se la tua arma spara in proiettile
        public Aura auraPrefab;//Lo utilizzi se la tua arma è un effetto ad area
        public GameObject hitEffect;
        public Rect spawnVariance;

        [Header("Values")]
        public float lifespan;//Se è a 0 l'oggetto resta vivio per sempre
        public float damage, damageVariance, area, speed, cooldown, projectileInterval, knockback;
        public int number, piercing, maxInstances;

        //permetti di usare l'operatore + per sommare due statistiche tra loro
        //diventerà molto uite più tardi quando dobbiamo aumentare le statistiche delle armi
        public static Stats operator +(Stats s1, Stats s2){
            Stats result = new Stats();
            result.name = s2.name ?? s1.name;
            result.description = s2.description ?? s1.description;
            result.projectilePrefab = s2.projectilePrefab ?? s1.projectilePrefab;//lo utilizzi solo se l'arma spara un proietile
            result.auraPrefab = s2.auraPrefab ?? s1.auraPrefab;//lo utilizzi solo se l'arma ha un effeto ad area
            result.hitEffect = s2.hitEffect == null ? s1.hitEffect : s2.hitEffect;
            result.spawnVariance = s2.spawnVariance;
            result.lifespan = s1.lifespan + s2.lifespan;
            result.damage = s1.damage + s2.damage;
            result.damageVariance = s1.damageVariance + s2.damageVariance;
            result.area = s1.area + s2.area;
            result.speed = s1.speed + s2.speed;
            result.number = s1.number + s2.number;
            result.cooldown = s1.cooldown + s2.cooldown;
            result.piercing = s1.piercing + s2.piercing;
            result.projectileInterval = s1.projectileInterval + s2.projectileInterval;
            result.knockback = s1.knockback + s2.knockback;
            return result;
        }

        public float GetDamage(){
            return damage + Random.Range(0, damageVariance);
        }
    }

    protected Stats currentStats;
    public WeaponData weaponData;//referenze allo scriptable Object
    protected float currentCooldown;

    protected PlayerMovement movement;//reference al playermovent

    //per creare l'arma in maniera dinamica chiama la funzione initialise per settare tutto
    public virtual void Initialise(WeaponData data){
        base.Initialise(data);
        maxLevel = data.maxLevel;
        player = FindObjectOfType<PlayerStats>();
        movement = FindObjectOfType<PlayerMovement>();

        this.weaponData = data;
        currentStats = data.baseStats;
        currentCooldown  = currentStats.cooldown;
    }

    protected virtual void Awake(){
        if(weaponData){
            currentStats = weaponData.baseStats;
        }
    }

    protected virtual void Start(){
        //inizializza l'arma solo se weaponData non è null
        if(weaponData){
            Initialise(weaponData);
        }
    }

    protected virtual void Update(){
        currentCooldown -= Time.deltaTime;
        if(currentCooldown <= 0){
            Attack(currentStats.number);
        }
    }

    //fai il level up al livello successivo e modifica le statistiche
    public override bool DoLevelUp(){
        base.DoLevelUp();
        //non fare il level up se sei maxLevel
        if(!CanLevelUp()){
            Debug.LogWarning(string.Format("Cannot level up {0} to Level {1}, max Level of {2} already reached", name , currentLevel, maxLevel));
            return false;
        }

        //altrimenti, aggiungi le statistiche del prossimo livello
        currentStats += weaponData.GetLevelData(++currentLevel);
        return true;
    }

    //controlla se l'arma può attacare in questo momento
    public virtual bool CanAttack(){
        return currentCooldown <= 0;
    }

    //fai un attacco con l'arma 
    //ritorna true se l'attacco è andato a segno
    //ovviamente questa funzione ora come ora non fa niente, bisogno fare l'override
    protected virtual void Attack(int attackCount){
        if(CanAttack()){
            currentCooldown += currentStats.cooldown;
        }
    }

    //prendi il danno che l'arma dovrebbe fare 
    //aggiungi la caratteristica Might del player
    public virtual float GetDamage(){
        return currentStats.GetDamage() * player.CurrentMight;
    }

    //funzione per prendere le statistiche dell'arma
    public virtual Stats GetStats(){
        return currentStats;
    }
}
