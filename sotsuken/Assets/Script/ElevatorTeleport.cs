using UnityEngine;

public class ElevatorTeleport : MonoBehaviour
{
    public Transform teleportTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CharacterController cc = other.GetComponent<CharacterController>();

        if (cc != null)
            cc.enabled = false;

        other.transform.position = teleportTarget.position;

        if (cc != null)
            cc.enabled = true;
    }
}
