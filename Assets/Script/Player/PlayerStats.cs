using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //Reference
    CharacterData playerData;

    //Stats
    public CharacterData.Stats baseStats;
    [SerializeField] CharacterData.Stats actualStats;
    float health;

    //Stats da far vedere nel menù di pausa
    //una region serve ad organizzare il codice in un unica sezione, migliorando la navigazione nel codice

    #region Current Stats Properties
    public float CurrentHP{
        get{
            return health;
        }
        set{
            //controllo se il valore è cambiato
            if(health != value){
                health = value;
                if(GameManager.instance != null){
                    GameManager.instance.currentHPDisplay.text = string.Format("Vita: {0}/{1}", health, actualStats.maxHP);
                }
            }
        }
    }

    public float CurrentMaxHP{
        get{
            return actualStats.maxHP;
        }
        set{
            //controllo se il valore è cambiato
            if(actualStats.maxHP != value){
                actualStats.maxHP = value;
                if(GameManager.instance != null){
                    GameManager.instance.currentHPDisplay.text = string.Format("Vita: {0}/{1}", health, actualStats.maxHP);
                }
            }
        }
    }

    public float CurrentArmor{
        get{
            return Armor;
        }
        set{
            Armor = value;
        }
    }

    public float Armor{
        get{
            return actualStats.armor;
        }
        set{
            //controllo se il valore è cambiato
            if(actualStats.armor != value){
                actualStats.armor = value;
                if(GameManager.instance != null){
                    GameManager.instance.currentArmorDisplay.text = string.Format("Armatura: ", actualStats.armor);
                }
            }
        }
    }

    public float CurrentSpeed{
        get{
            return MoveSpeed;
        }
        set{
            MoveSpeed = value;
        }
    }

    public float MoveSpeed{
        get{
            return actualStats.moveSpeed;
        }
        set{
            if(actualStats.moveSpeed != value){
                actualStats.moveSpeed = value;
                if(GameManager.instance != null){
                    GameManager.instance.currentHPDisplay.text = "Velocita': " + actualStats.moveSpeed;
                }
            }
        }
    }

    public float CurrentProjectileSpeed{
        get{
            return Speed;
        }
        set{
            Speed = value;
        }
    }

    public float Speed{
        get{
            return actualStats.speed;
        }
        set{
            //controllo se il valore è cambiato
            if(actualStats.speed != value){
                actualStats.speed = value;
                if(GameManager.instance != null){
                    GameManager.instance.currentProjectileSpeedDisplay.text ="Rapidita': " + actualStats.speed;
                }
            }
        }
    }

    public float CurrentRecovery{
        get{
            return Recovery;
        }
        set{
            Recovery = value;
        }
    }

    public float Recovery{
        get{
            return actualStats.recovery;
        }
        set{
            //controllo se il valore è cambiato
            if(actualStats.recovery != value){
                actualStats.recovery = value;
                if(GameManager.instance != null){
                    GameManager.instance.currentRecoveryDisplay.text ="Recupero: " + actualStats.recovery;
                }
            }
        }
    }

    public float CurrentMight{
        get{
            return Might;
        }
        set{
            Might = value;
        }
    }

    public float Might{
        get{
            return actualStats.might;
        }
        set{
            //controllo se il valore è cambiato
            if(actualStats.might != value){
                actualStats.might = value;
                if(GameManager.instance != null){
                    GameManager.instance.currentMightDisplay.text ="Forza: " + actualStats.might;
                }
            }
        }
    }

    public float CurrentMagnet{
        get{
            return Magnet;
        }
        set{
            Magnet = value;
        }
    }

    public float Magnet{
        get{
            return actualStats.magnet;
        }
        set{
            //controllo se il valore è cambiato
            if(actualStats.magnet != value){
                actualStats.magnet = value;
                if(GameManager.instance != null){
                    GameManager.instance.currentMagnetDipslay.text ="Raccolta: " + actualStats.magnet;
                }
            }
        }
    }

    public float Luck{
        get{
            return actualStats.luck;
        }
        set{
            //controllo se il valore è cambiato
            if(actualStats.luck != value){
                actualStats.luck = value;
                if(GameManager.instance != null){
                    GameManager.instance.currentLuckDisplay.text = "Fortuna: " + actualStats.luck;
                }
            }
        }
    }

    public float CurrentLuck{
        get{
            return Luck;
        }
        set{
            Luck = value;
        }
    }
    #endregion

    [SerializeField] GameObject damageEffect;

    //Inventory
    PlayerInventory inventory;
    //queste due variabili tengono nota degli indici degli arrey nell'inventario
    [SerializeField] int weaponIndex;
    [SerializeField] int passiveItemIndex;

    //level up system
    [Header("Experience/Level")]
    [SerializeField] int expericence = 0;
    [SerializeField] int level = 1;
    [SerializeField] int expericenceCap;

    public bool gameEnding = false;
    
    [System.Serializable]//questo tag pemette ad una classe di essere serializzata da unity
    public class LevelRange{
        public int startLevel;
        public int endLevel;
        public int expericenceCapIncrease;
    }

    //I-frames
    [Header("I_frames")]
    [SerializeField] float invicibilityDuration;//durate massa degli I-frames
    float invicibilityTimer;//conto alla rovescia degli I-frame
    bool isInvincible;
    
    public List<LevelRange> levelRanges;

    PlayerMovement movement;
    PlayerAnimator animator;
    PlayerCollector collector;

    void Awake(){
        //preleva il personaggio
        playerData = CharacterSelector.GetData();
        if(CharacterSelector.instance){
            CharacterSelector.instance.DestroySigleton();
        }

        inventory=GetComponent<PlayerInventory>();
        movement = GetComponent<PlayerMovement>();
        collector = GetComponentInChildren<PlayerCollector>();

        baseStats = actualStats = playerData.stats;
        collector.SetRadius(actualStats.magnet);
        health = actualStats.maxHP;

        //cambia l'animazione del personaggio con quelle del personaggio scelto
        animator = GetComponent<PlayerAnimator>();
        if(playerData.controller){
            animator.SetAnimatorController(playerData.controller);
        }
    }

    void Start(){
        //Spawn dell'arma iniziale
        inventory.Add(playerData.StartingWeapon);

        //inizialize the exprerience cap
        expericenceCap = levelRanges[0].expericenceCapIncrease;

        GameManager.instance.currentHPDisplay.text ="Vita: " + CurrentHP;
        GameManager.instance.currentSpeedDisplay.text ="Veclocita': " + CurrentSpeed;
        GameManager.instance.currentProjectileSpeedDisplay.text ="Rapidita': " + CurrentProjectileSpeed;
        GameManager.instance.currentRecoveryDisplay.text ="Recupero: " + CurrentRecovery;
        GameManager.instance.currentMightDisplay.text ="Forza: " + CurrentMight;
        GameManager.instance.currentMagnetDipslay.text ="Raccolta: " + CurrentMagnet;
        GameManager.instance.currentArmorDisplay.text = "Armatura:" + CurrentArmor;
        GameManager.instance.currentLuckDisplay.text = "Fortuna:" + CurrentLuck;

        GameManager.instance.AssaignCharacterUI(playerData);
        GameManager.instance.UpdateLeveltext(level);
        GameManager.instance.UpdateHealthbar(CurrentHP , actualStats.maxHP);
        GameManager.instance.UpdateExpBar(expericence, expericenceCap);

    }

    void Update(){
        if(invicibilityTimer > 0){//qui stiamo gestendo la durata degli I-frames e quando scade il timer vengono disabilitati
            invicibilityTimer -= Time.deltaTime;
        }else if(isInvincible){
            isInvincible = false;
        }

        Recover(); 
    }

    public void RecalculateStats(){
        actualStats = baseStats;
        foreach(PlayerInventory.Slot s in inventory.passiveItemSlots){
            Passive p = s.item as Passive;
            if(p){
                actualStats += p.GetBoosts();
            }
        }
        collector.SetRadius(actualStats.magnet);
    }

    public void IncreaseExpericence(int exp){
        expericence += exp;
        LevelUpCheck();
        GameManager.instance.UpdateExpBar(expericence, expericenceCap);
    }

    void LevelUpCheck(){
        if(expericence >= expericenceCap){//se hai abbastanza esperienza alza il livello e aumenta il cap massimo dell exp
            level++;
            GameManager.instance.UpdateLeveltext(level);
            expericence -= expericenceCap;
            
            int increase = 0;
            foreach(LevelRange range in levelRanges){
                if(level >= range.startLevel && level <= range.endLevel){
                    increase = range.expericenceCapIncrease;
                }
            }

            expericenceCap += increase;
            GameManager.instance.UpdateExpBar(expericence, expericenceCap);
            GameManager.instance.StartLevelUp();
        }
    }

    public void RestoreHP(float cure){
        if(CurrentHP < actualStats.maxHP){
            CurrentHP += cure;
            if(CurrentHP > actualStats.maxHP){
                CurrentHP = actualStats.maxHP;
            }
            GameManager.instance.UpdateHealthbar(CurrentHP , actualStats.maxHP);
        }
    }

    public void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 1f, float knockbackDuration = 0.2f){
        //se il giocatore non ha I-frame, allora prenderà danno
        if(!isInvincible){
            CurrentHP -= dmg - (dmg * (actualStats.armor / 100f));//dimuniamo il danno tramite l'armatura

            //avvia l'effetto del danno ricevuto
            if(damageEffect){
                GameObject effect = Instantiate(damageEffect, transform.position, Quaternion.identity);
                Destroy(effect, 0.5f);//distrugge l'effetto del danno
            }

            //poi applica un effetto di knockback
            /*if(gameEnding == false){
                if(knockbackForce > 0){
                    Vector2 dir = (Vector2)transform.position - sourcePosition;
                    movement.Knockback(dir.normalized * knockbackForce, knockbackDuration);
                }
            }*/

            //dopo che il player prende danno, gli diamo qualche secondo di I-frames
            invicibilityTimer = invicibilityDuration;
            isInvincible = true;

            if(CurrentHP <= 0){
                Kill();
            }

            GameManager.instance.UpdateHealthbar(CurrentHP , actualStats.maxHP);
        }
    }

    void Kill(){
        GameManager.instance.AssaignLevelUI(level);
        GameManager.instance.AssaignTimeOfRun();
        GameManager.instance.AssaignWeaponsAndPassiveItemsUI(inventory.ReturnImage(inventory.weaponSlots), inventory.ReturnImage(inventory.passiveItemSlots));
        GameManager.instance.GameOver();//se si alora chiama la funzione di Game Over
    }
    
    private void Recover(){
        if(CurrentHP < actualStats.maxHP){
            CurrentHP += CurrentRecovery * Time.deltaTime;
            CurrentHP += Recovery * Time.deltaTime;

            //questo assicura che la vita del giocatore non eccede il suo massimo
            if(CurrentHP > actualStats.maxHP){
                CurrentHP = actualStats.maxHP;
            }
        }
        GameManager.instance.UpdateHealthbar(CurrentHP , actualStats.maxHP);
    }

    public CharacterData GetCharacterData(){
        CharacterData result = playerData;
        return result;
    }
}
