using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ring Event Data", menuName = "ScriptableObject/Ring Event Data ")]
public class RingEventData : EventData
{
    [Header("Mob Data")]
    public GameObject spawnEffectPrefab;
    public Vector2 scale = new Vector2(1, 1);
    [Min(0)] public float spawnRadius = 10f, lifespan = 15f;

    public override bool Activate(PlayerStats player = null){
        //si attiva solo se il player è presente
        if(player){
            GameObject[] spawns = GetSpawns();
            float angleOffset = 2 * Mathf.PI /Mathf.Max(1, spawns.Length);
            float currentAngle = 0;
            foreach(GameObject g in spawns){
                //Calcola la posizione da spawnare
                Vector3 spawnPosition = player.transform.position + new Vector3(
                    spawnRadius * Mathf.Cos(currentAngle) * scale.x,
                    spawnRadius * Mathf.Sin(currentAngle) * scale.y
                );

                //se l'effetto dello spawn è stato inserito, instazialo
                if(spawnEffectPrefab){
                    Instantiate(spawnEffectPrefab, spawnPosition, Quaternion.identity);
                }

                //spawna il nemico
                GameObject s = Instantiate(g, spawnPosition, Quaternion.identity);

                //se il mob ha un lifespan allora distruggilo dopo che il suo tempo è finito
                if(lifespan > 0){
                    Destroy(s, lifespan);
                }

                currentAngle += angleOffset;
            }
        }

        return false;
    }
}
