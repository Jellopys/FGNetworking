using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FiringAction : NetworkBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject clientSingleBulletPrefab;
    [SerializeField] GameObject serverSingleBulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] private float weaponCooldown = 0.5f;

    public NetworkVariable<int> currentAmmo = new NetworkVariable<int>(10);
    private int maxAmmo = 10;
    private bool isOnCooldown = false;


    public override void OnNetworkSpawn()
    {
        playerController.onFireEvent += Fire;
    }

    private void Fire(bool isShooting)
    {

        if (isShooting && currentAmmo.Value > 0 && !isOnCooldown)
        {
            ShootLocalBullet();
            StartCoroutine(StartWeaponDelay());
        }
    }

    IEnumerator StartWeaponDelay()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(weaponCooldown);
        isOnCooldown = false;
    }

    public void GainAmmo(int ammoToGain)
    {
        currentAmmo.Value = Mathf.Clamp(currentAmmo.Value += ammoToGain, 0, maxAmmo);
    }

    [ServerRpc]
    private void ShootBulletServerRpc()
    {
        GameObject bullet = Instantiate(serverSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        currentAmmo.Value--;
        ShootBulletClientRpc();
    }

    [ClientRpc]
    private void ShootBulletClientRpc()
    {
        if (IsOwner) return;
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

    }

    private void ShootLocalBullet()
    {
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        
        ShootBulletServerRpc();
    }
}
