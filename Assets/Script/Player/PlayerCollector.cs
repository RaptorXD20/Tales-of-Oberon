using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D detector;
    [SerializeField] float pullSpeed;

    void Start(){
        player = GetComponentInParent<PlayerStats>();
        //siamo questa funzione così per essere sicuri da prendere le statistiche del player che ha questo oggetto come oggetto figlio
    }

    public void SetRadius(float r){
        if(!detector){
            detector = GetComponent<CircleCollider2D>();
            detector.radius = r;
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        //questa funzione ci serve per vedere se stiamo raccogliendo un oggetto che è un Pickup
        if(col.TryGetComponent(out Pickup collectable)){
            collectable.Collect(player, pullSpeed);//gli facciamo usare la funzione Collect()
        }
    }
}
