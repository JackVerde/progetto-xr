
using UnityEngine;

public class OculusGrabPoint : MonoBehaviour
{
    public GameObject grab;

    private bool grabbed;

    // Update is called once per frame
    void Update()
    {
        if (grab == null && !grabbed)
            return;

        var gripPressed = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);

        if (gripPressed && !grabbed)
        {
            Debug.Log("Pressed!");
            var joint = GetComponent<SpringJoint>();
            joint.connectedBody = grab.GetComponentInParent<Rigidbody>();
            grabbed = true;
        }
        else if (!gripPressed && grabbed)
        {
            var joint = GetComponent<SpringJoint>();
            joint.connectedBody = null;
            grabbed = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision with:" + other.name);
        grab = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (grab != null && grab.Equals(other.gameObject))
            grab = null;
    }
}