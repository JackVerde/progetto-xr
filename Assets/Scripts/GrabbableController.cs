using System;
using UnityEngine;

public class GrabbableController : MonoBehaviour
{
    private Transform table;
    private Vector3 initPosition;
    private Quaternion initRotation;

    private void Start()
    {
        table = GameObject.FindWithTag("Table")?.transform;
        var t = transform;
        initPosition = t.position;
        initRotation = t.rotation;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("ENTER " + other.gameObject.name);
        if (!other.gameObject.CompareTag("Table")) return;
        if (table != null && transform.parent == null)
        {
            transform.parent = table;
            transform.GetComponentInParent<Rigidbody>().isKinematic = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Debug.Log("EXIT " + other.gameObject.name);
        if (!other.gameObject.CompareTag("Table")) return;
        if (transform.parent != null && transform.parent.Equals(other.gameObject.transform))
        {
            transform.parent = null;
            if (!transform.GetComponentInParent<OVRGrabbable>().isGrabbed)
                transform.GetComponentInParent<Rigidbody>().isKinematic = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("EXIT " + other.gameObject.name);
        if (other.gameObject.CompareTag("Table"))
        {
            if (transform.parent != null && transform.parent.Equals(other.gameObject.transform))
            {
                transform.parent = null;
                if (!transform.GetComponentInParent<OVRGrabbable>().isGrabbed)
                    transform.GetComponentInParent<Rigidbody>().isKinematic = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(!other.gameObject.CompareTag("Table")) return;
        
        transform.GetComponentInParent<Rigidbody>().isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTER " + other.gameObject.name);
        if (other.gameObject.CompareTag("Table"))
        {
            if (table != null && transform.parent == null)
            {
                transform.parent = table;
                transform.GetComponentInParent<Rigidbody>().isKinematic = true;
            }
        }

        if (other.CompareTag("Respawn"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            var t = transform;
            t.position = initPosition;
            t.rotation = initRotation;
        }
    }
}