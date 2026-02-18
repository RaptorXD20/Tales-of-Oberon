using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour
{
    
    [SerializeField] EnemyScriptableObject enemyData;

    //Currentstats
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float currentDamage;
    [HideInInspector] public float currentHP;

    Transform player;

    [Header("Damage Feedback")]
    [SerializeField] Color damageColor = new Color(1,0,0,1);//il colore che prende lo sprite quando viene colpito
    [SerializeField] float damageFlashDuration = 0.2f;//la durata del damage feedback
    [SerializeField] float deathFadeTime = 0.6f;//quanto tempo impiega per fare l'animazione di morte
    Color originalColor;
    SpriteRenderer sr;
    EnemiesMovement movement;
    DropRateManager drm;
    SaveManager sm;

    public static int count;//questa variabile statica mi serve per contare tutti i nemici

    void Awake(){
        count++;

        currentDamage = enemyData.Damage;
        currentHP = enemyData.MaxHP;
        currentSpeed = enemyData.Speed;
    }

    void Start(){
        player = FindObjectOfType<PlayerStats>().transform;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        movement = GetComponent<EnemiesMovement>();
        drm = GetComponent<DropRateManager>();
        sm = FindObjectOfType<SaveManager>();
    }

    //questa coroutine gestisce il flash che il nemico fa quando viene colpito
    IEnumerator DamageFlash(){
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originalColor;
    }

    public void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 5f, float knockbackDuration = 0.2f){
        currentHP -= dmg;
        StartCoroutine(DamageFlash());//attiva la coroutine che cambia il colore del nemico

        if(dmg > 0){
            GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform);
        }

        //poi applica un effetto di knockback
        if(knockbackForce > 0){
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            if(movement != null){
                movement.Knockback(dir.normalized * knockbackForce, knockbackDuration);
            }
        }

        if(currentHP <= 0){
            Kill();
        }
    }

    //coroutine che gestisce l'animazione di dissolvenza quando il nemico muore
    IEnumerator KillFade(){
        //asppetta un frame
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0;
        float orignAlfa = sr.color.a;

        //questo loop crea l'effetto dissolvenza
        while(t < deathFadeTime){
            yield return w;
            t += Time.deltaTime;

            //setta il colore con una nuova opacità
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (1 - t/deathFadeTime) * orignAlfa);
        }

        //alla fine della dissolvenza distruggi l'oggetto
        Destroy(gameObject);
    }

    public void Kill(){
        drm.IsKilled();
        sm.SaveKill();
        GameManager.instance.AddKill();
        StartCoroutine(KillFade());
    }

    private void OnCollisionStay2D(Collision2D col){
        if(col.gameObject.CompareTag("Player")){
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage, transform.position);
        }
    }

    private void OnDestroy(){
        if(!gameObject.scene.isLoaded){
            return;
        }
        
        count--;
    }
}
