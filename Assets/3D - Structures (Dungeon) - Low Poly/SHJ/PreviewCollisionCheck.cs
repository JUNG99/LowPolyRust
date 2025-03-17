using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewCollisionCheck : MonoBehaviour
{
    public Build build;

    private void Start()
    {
        build = FindObjectOfType<Build>();
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        build.onCollision = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        build.onCollision = false;
    }

    private void OnTriggerExit(Collider other)
    {
        build.onCollision = true;
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    Debug.Log(other.name);
    //    if (!build.canBuild)
    //        build.canBuild = true;
    //    if (LayerMask.LayerToName(other.gameObject.layer) == "Ground" ||
    //        LayerMask.LayerToName(other.gameObject.layer) == "Player")
    //        return;

    //    build.canBuild = false;

    //}
}
