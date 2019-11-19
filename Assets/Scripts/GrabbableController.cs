using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Throwable))]
public class GrabbableController : MonoBehaviour
{
    private Throwable throwable;
    private Transform table;
    private Vector3 initPosition;
    private Quaternion initRotation;

    private void Start()
    {
        throwable = GetComponent<Throwable>();
        table = GameObject.FindWithTag("Table")?.transform;
        var t = transform;
        initPosition = t.position;
        initRotation = t.rotation;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("ENTER " + other.gameObject.name);
        if (!other.gameObject.CompareTag("Table")) return;
        if (table != null && transform.parent==null)
            transform.parent = table;
    }

    private void OnCollisionExit(Collision other)
    {
        Debug.Log("EXIT " + other.gameObject.name);
        if (!other.gameObject.CompareTag("Table")) return;
        if (transform.parent!=null && transform.parent.Equals(other.gameObject.transform))
            transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Respawn")) return;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        var t = transform;
        t.position = initPosition;
        t.rotation = initRotation;
    }
}