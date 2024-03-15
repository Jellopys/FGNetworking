using Unity.Netcode;
using UnityEngine;

public class AmmoPickup : NetworkBehaviour
{
    [SerializeField] GameObject ammoPickupPrefab;
    private int ammoToGive = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsServer)
        {
            FiringAction weapon = other.GetComponent<FiringAction>();
            if (!weapon) return;
            weapon.GainAmmo(ammoToGive);

            int xPosition = Random.Range(-4, 4);
            int yPosition = Random.Range(-2, 2);

            transform.position = new Vector3(xPosition, yPosition);
        }
    }
}
