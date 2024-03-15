using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    private int currentLives = 2;


    public override void OnNetworkSpawn()
    {
        currentHealth.OnValueChanged += CheckHealth;
        if(!IsServer) return;
        currentHealth.Value = 100;
    }


    public void TakeDamage(int damage)
    {
        damage = damage<0? damage:-damage;
        currentHealth.Value += damage;
    }

    private void CheckHealth(int oldValue, int newValue)
    {
        if (newValue <= 0)
        {
            TriggerDeath();
        }
    }

    public void GainHealth(int health)
    {
        currentHealth.Value = Mathf.Clamp(currentHealth.Value += health, 0, 100);
    }

    private void TriggerDeath()
    {
        Debug.Log("Trigger Death");
        transform.position = new Vector3(Random.Range(-4, 4), Random.Range(-3, 3));
        currentHealth.Value = 10;
        
        currentLives--;
        if (currentLives <= 0)
        {
            Debug.Log("Die permanently");

            NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
            networkObject.Despawn();
        }
    }

}
