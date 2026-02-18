using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    //Reference
    SpriteRenderer sprite;
    Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        playerTransform = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > playerTransform.position.x){
            sprite.flipX = true;
        }else{
            sprite.flipX = false;
        }
    }

}
