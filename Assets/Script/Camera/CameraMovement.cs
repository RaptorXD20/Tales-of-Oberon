using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset; //questo va settato nell'inspector con la z a -10, quest'ultima è l'altezza stardard della telecamere 2D 

    // Update is called once per frame


    void Update()
    {
        transform.position = target.position + offset;
    }
}
