using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    int currentWaveIndex;//l'indice della wave corrente
    int currentWaveSpawnCount;//indica il numero di nemici che sono stati spawnati

    public WaveData[] data;
    WaveData[] saveWave;
    public Camera cameraReference;

    //se ci sono più nemici di questo numero allora smetti si spawnare
    public int maximumEnemyCount = 300;
    float spawnTimer;//timer che serve per capire quando spawnare il prossimo numero di nemici
    float curretWaveDuration = 0f;

    public static SpawnManager instance;

    void Awake(){
        curretWaveDuration = 0f;
        currentWaveIndex = 0;
        spawnTimer = 0f;

    }

    void Start(){
        if(instance){
            Debug.LogWarning("There is more than 1 Spawn Manager in the Scene! Please remove the extras");
        }
        instance = this;
    }

    void Update(){
        //Aggiorna lo spawn timer
        spawnTimer -= Time.deltaTime;
        curretWaveDuration += Time.deltaTime;

        if(spawnTimer <= 0){
            //controlla se siamo pronti a passare alla prossima wave
            if(HasWaveEnded()){
                currentWaveIndex++;
                curretWaveDuration = currentWaveSpawnCount = 0;

                //se abbiamo finito l'ultima wave, disabilita questo componente
                if(currentWaveIndex >= data.Length){
                    Debug.Log("All waves hase been spawned! Shutting down");
                    enabled = false;
                    GameManager.instance.ChangeState(GameManager.GameState.GameOver);
                }

                return;
            }

            //non spawnare nemici se non abbiamo rispettato le condizioni
            if(!CanSpawn()){
                spawnTimer *= data[currentWaveIndex].GetSpawnInterval();
                return;
            }

            //prendi l'arrey dei nemici da spawnare
            GameObject[] spawns = data[currentWaveIndex].GetSpawns(EnemyStats.count);

            //Spawna tutti i nemici
            foreach(GameObject prefab in spawns){
                //non spawnare se abbaimo raggiunto il limite
                if(!CanSpawn()){
                    continue;
                }

                //spawna i nemici
                Instantiate(prefab, GeneratePosition(), Quaternion.identity);
                currentWaveSpawnCount++;
            }

            //resetta lo spawn timer
            spawnTimer += data[currentWaveIndex].GetSpawnInterval();
        }

    }

    //Questa funzione ci dice se possiamo spawnare nemici
    public bool CanSpawn(){
        //non spawnare se abbaimo superato il numero massimo di nemici
        if(hasExceededMaxiEnemies()){
            return false;
        }

        //non spawnare se abbiamo superato il numero massimo di nemici della wave
        if(instance.currentWaveSpawnCount > instance.data[currentWaveIndex].totalSpawns){
            return false;
        }

        //non spawnare se abbiamo superato la durate della wave
        if(instance.curretWaveDuration > instance.data[currentWaveIndex].duration){
            return false;
        }

        return true;
    }

    //permette ad altri script di veder se abbiamo superato il numero massimo di nemici
    public static bool hasExceededMaxiEnemies(){
        if(!instance){
            return false;
        }

        if(EnemyStats.count > instance.maximumEnemyCount){
            return true;
        }

        return false;
    }

    //indica se la wave è finita
    public bool HasWaveEnded(){
        WaveData currentWave = data[currentWaveIndex];

        //se waveDuration è una delle condizioni necessarie a far finire la wave, controlla da quanto la wave è in corso
        //se la durata corrente è più piccola della durata della wave, alla continua a Spawnare
        if((currentWave.exitCondition & WaveData.ExitCondition.waveDuration) > 0){
            if(curretWaveDuration < currentWave.duration){
                return false;
            }
        }

        //se raggingere il numero totale di nemici è la condizione che fa finire la wave, devi controllare quanti nemici ci sono
        //se il numero di nemici è minore del numero di nemici totale della wave allora continua
        if((currentWave.exitCondition & WaveData.ExitCondition.reachedTotalSpawns) > 0){
            if(currentWaveSpawnCount < currentWave.totalSpawns){
                return false;
            }
        }

        //altrimenti se Uccidere tutti i nemici è la condizione, controlla se ci sono nemici vivi
        //se il numero di nemici vivi e superiore a 0 allora continua
        if(currentWave.mustKillAll && EnemyStats.count > 0){
            return false;
        }

        return true;
    }

    void Reset(){
        cameraReference = Camera.main;
    }

    // crea un luogo dove far spawnare il nemico
    public static Vector3 GeneratePosition(){
        //se non ci sono refere alla camera, allora creane una
        if(!instance.cameraReference){
            instance.Reset();
        }

        //da un errore se la camera non è ortografica
        if(!instance.cameraReference.orthographic){
            Debug.LogWarning("The reference camera is not othographic! This will cause enemy to spawns whithin the camera boundaries!");
        }
        
        //Genera uno spawn point al di fuori della camera
        float x = Random.Range(0f, 1f), y = Random.Range(0f, 1f);

        //Poi ritorna randomicamente se arrotondare la x o la y
        switch(Random.Range(0,2)){
            case 0: default:
                return instance.cameraReference.ViewportToWorldPoint(new Vector3(Mathf.Round(x), y));
            case 1:
                return instance.cameraReference.ViewportToWorldPoint(new Vector3(x, Mathf.Round(y)));
        }
    }

    //cotrolla se il nemico è in camera
    public static bool IsWithinBoundaries(Transform checkedObject){
        //prendi la camera
        Camera c = instance && instance.cameraReference ? instance.cameraReference : Camera.main;

        Vector2 viewport = c.WorldToViewportPoint(checkedObject.position);
        if(viewport.x < 0f || viewport.x > 1f) return false;
        if(viewport.y < 0f || viewport.y > 1f) return false;
        return true;
    }
}
