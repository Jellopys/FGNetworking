using Unity.Netcode;
using UnityEngine;

public class HealthKit : NetworkBehaviour
{
    [SerializeField] GameObject healthKitPrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsServer)
        {
            Health health = other.GetComponent<Health>();
            if (!health) return;
            health.GainHealth(25);

            int xPosition = Random.Range(-4, 4);
            int yPosition = Random.Range(-2, 2);

            transform.position = new Vector3(xPosition, yPosition);

            //NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
            //networkObject.Despawn();
        }
    }
}
