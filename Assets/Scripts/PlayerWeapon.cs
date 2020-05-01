using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerWeapon : MonoBehaviourPun
{
    [Header("Stats")]
    public int damage;
    public int curAmmo;
    public int maxAmmo;
    public float bulletSpeed;
    public float shootRate;

    private float lastShootTime;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPos;

    private PlayerController player;

    void Awake() {
        player = GetComponent<PlayerController>();
    }

    public void TryShoot() {
        // can we shoot ?
        if (curAmmo <= 0 || Time.time - lastShootTime < shootRate) {
            return;
        }

        curAmmo--;
        lastShootTime = Time.time;

        // Update the ammo UI
        GameUI.instance.UpdateAmmoText();

        // Spawn the bullet
        photonView.RPC("SpawnBullet", RpcTarget.All, bulletSpawnPos.position, Camera.main.transform.forward);
    }

    [PunRPC]
    void SpawnBullet(Vector3 pos, Vector3 dir) {
        // spawn and orientate it
        GameObject bulletObj = Instantiate(bulletPrefab, pos, Quaternion.identity);
        bulletObj.transform.forward = dir;

        // get the bullet script
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();

        // initialize it and set the velocity
        bulletScript.Initialize(damage, player.id, player.photonView.IsMine);
        bulletScript.rig.velocity = dir * bulletSpeed;
    }

    [PunRPC]
    public void GiveAmmo(int ammoToGive) {
        curAmmo = Mathf.Clamp(curAmmo + ammoToGive, 0, maxAmmo);

        // update the ammo text
        GameUI.instance.UpdateAmmoText();
    }
}
