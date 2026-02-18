using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    //Variables
    [SerializeField] List<GameObject> terrainChunk;
    [SerializeField] GameObject player;
    [SerializeField] float checkRadius;
    //GameObject player;
    Vector3 noTerrainPosition;//serve per vedere dove non ci sono i chunk
    [SerializeField] LayerMask terrainMask;//serve per capire inche layer è il terreno
    public GameObject currentChunk;
    Vector3 playerLastPosition;

    [Header("Optimization")]
    [SerializeField] List<GameObject> spawnedChunks;//lista dei chunk già spawnati
    [SerializeField] GameObject lastChunk;//l'ultimo chunk spawnato
    [SerializeField] float maxOfDistance;//la distanza massima dopo la quale i chunk non vengono più caricati
    float opDistance;
    float optimazerCooldown;
    [SerializeField] float optimazerCooldownDuration;
    

    // Start is called before the first frame update
    void Start()
    {
        playerLastPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
        ChunkOptimazer();
    }

    void ChunkChecker(){
        
        if(!currentChunk){
            return;
        }

        Vector3 moveDir = player.transform.position - playerLastPosition; //questo indica la direzione verso cui il player sta andando
        playerLastPosition = player.transform.position;//successivamente dobbiamo aggiornale l'ultima posizione

        string directionName = GetDirectionName(moveDir);//otteniamo la direzione del giocatore

        //controlliamo che non ci sia già un chunk nella direzione
        if(directionName.Contains("Up")){
            CheckAndSpawnChunks("Up");
        }
        if(directionName.Contains("Down")){
            CheckAndSpawnChunks("Down");
        }
        if(directionName.Contains("Left")){
            CheckAndSpawnChunks("Left");
        }
        if(directionName.Contains("Right")){
            CheckAndSpawnChunks("Right");
        }
        if(currentChunk){
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find(directionName).position, checkRadius, terrainMask)){
                SpawnChunk(currentChunk.transform.Find(directionName).position);
                //se il player si muove diagonalmente dobbiamo anche spawnare i chunk 
                //nella direzione primaria e secondaria del movimento diagonale
                if(directionName.Contains("Up") && directionName.Contains("Right")){//in alto e destra
                    ChunkDiagonalSpawn("Up", "Right");
                }else if(directionName.Contains("Down") && directionName.Contains("Right")){//in basso e destra
                    ChunkDiagonalSpawn("Down", "Right");
                }else if(directionName.Contains("Up") && directionName.Contains("Left")){//in alto e sinistra
                    ChunkDiagonalSpawn("Up", "Left");
                }else if(directionName.Contains("Down") && directionName.Contains("Left")){//in basso e sinistra
                    ChunkDiagonalSpawn("Down", "Left");
                }
            }
        }
    }

    string GetDirectionName(Vector3 direction){
        direction = direction.normalized;//normalizzando la direzione rende più pulito e preciso il calcolo
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){//qui controlliamo se il movimento è prettamente orizontale o verticale
            //ora dobbiamo controllare se il movimento ha una componente secondaria 
            //in questo caso controlliamo se va verso sopra o verso sotto
            if(direction.y > 0.5f){
                //si muove anche verso sopra
                return direction.x > 0 ? "Right Up" : "Up Left";
            }else if(direction.y < -0.5f){
                //si muove anche verso sotto
                return direction.x > 0 ? "Right Down" : "Left Down";
            }else{
                //si muove solo orizzontalmente
                return direction.x > 0 ? "Right" : "Left";
            }
        }else{
            // invece qui controlliamo se va verso destra o sinistra
            if(direction.x > 0.5f){
                //Si muove anche verso destra
                return direction.y > 0 ? "Right Up" : "Right Down";
            }else if(direction.x < -0.5f){
                //Si muove anche verso sinistra
                return direction.y > 0 ? "Up Left" : "Left Down";
            }else{
                //si muove solo verticalmente
                return direction.y > 0 ? "Up" : "Down";
            }
        }
    }

    void SpawnChunk(Vector3 spawnPosition){
        int rand = Random.Range(0, terrainChunk.Count);
        lastChunk = Instantiate(terrainChunk[rand], spawnPosition, Quaternion.identity);
        spawnedChunks.Add(lastChunk);
    }

    void ChunkOptimazer(){

        optimazerCooldown -= Time.deltaTime;

        if(optimazerCooldown <= 0f){
            optimazerCooldown = optimazerCooldownDuration;
        }else{
            return;
        }

        foreach(var chunk in spawnedChunks){
            opDistance = Vector3.Distance(player.transform.position, chunk.transform.position); 
            if(opDistance > maxOfDistance){
                chunk.SetActive(false);
            }else{
                chunk.SetActive(true);
            }
        }
    }

    void ChunkDiagonalSpawn(string dir_1, string dir_2){//questa funziona spawna i chunk che affiancano il chunk diagonale
        if(!Physics2D.OverlapCircle(currentChunk.transform.Find(dir_1).position, checkRadius, terrainMask)){
            SpawnChunk(currentChunk.transform.Find(dir_1).position);
        }else if(!Physics2D.OverlapCircle(currentChunk.transform.Find(dir_2).position, checkRadius, terrainMask)){
            SpawnChunk(currentChunk.transform.Find(dir_2).position);
        }
    }

    void CheckAndSpawnChunks(string dir){
        if(!Physics2D.OverlapCircle(currentChunk.transform.Find(dir).position, checkRadius, terrainMask)){
            SpawnChunk(currentChunk.transform.Find(dir).position);
        }
    }
}
