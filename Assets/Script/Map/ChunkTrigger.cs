using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    //variables 
    MapController mapController;
    [SerializeField] GameObject targetMap;

    // Start is called before the first frame update
    void Start()
    {
        mapController = FindObjectOfType<MapController>();
    }

    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D col){
        if(col.CompareTag("Player")){
            mapController.currentChunk = targetMap;
        }
    }

    private void OnTriggerExit2D(Collider2D col){
        if(col.CompareTag("Player")){
            if(mapController.currentChunk == targetMap){
                mapController.currentChunk = null;
            }
        }
    }
}
