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
        build.canBuild = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Ground" ||
            LayerMask.LayerToName(other.gameObject.layer) == "Player")
            return;
        build.canBuild = false;
    }
    private void OnTriggerExit(Collider other)
    {
        build.canBuild = true;
    }
}
