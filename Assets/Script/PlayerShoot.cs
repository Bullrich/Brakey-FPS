using UnityEngine.Networking;
using UnityEngine;

// By @Bullrich

namespace game
{
    [RequireComponent(typeof(WeaponManager))]
    public class PlayerShoot : NetworkBehaviour
    {
        private const string
            player_tag = "Player";

        private PlayerWeapon currentWeapon;
        [SerializeField] private LayerMask shootMask;

        [SerializeField] private Camera cam;

        private WeaponManager weaponManager;

        private void Start()
        {
            if (cam == null)
            {
                Debug.LogError("PlayerShoot: No camera referenced");
                this.enabled = false;
            }

            weaponManager = GetComponent<WeaponManager>();
        }

        void Update()
        {
            currentWeapon = weaponManager.GetCurrentWeapon();

            if (currentWeapon.fireRate <= 0f)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    CancelInvoke("Shoot");
                }
            }
        }

        // Is called on the server when a player shoots
        [Command]
        private void CmdOnShoot()
        {
            RpcDoShootEffect();
        }

        // Is called on the server when we hit something.
        [Command]
        private void CmdOnHit(Vector3 _pos, Vector3 _normal)
        {
            RpcDoHitEffect(_pos, _normal);
        }

        // Spawn of hit effect
        [ClientRpc]
        private void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
        {
            GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, 
                _pos, Quaternion.LookRotation(_normal));
            //TODO: Create a object pooling
            Destroy(_hitEffect, 2f);
        }

        // Is called on all clients when we need to do a shoot effect
        [ClientRpc]
        private void RpcDoShootEffect()
        {
            weaponManager.GetCurrentGraphics().muzzleFlash.Play();
        }

        [Client]
        private void Shoot()
        {
            if(!isLocalPlayer)
            return;
            
            // We are shooting, call the OnShoot method on the server
            CmdOnShoot();
          
            RaycastHit _hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range,
                shootMask))
            {
                // We hit something
                if (_hit.collider.tag == player_tag)
                    CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
                
                // We hit something, call on hit method from the server
                CmdOnHit(_hit.point, _hit.normal);
            }
        }

        [Command]
        private void CmdPlayerShot(string _playerID, int _damage)
        {
            Debug.Log(_playerID + "has been shot.");

            Player _player = GameManager.GetPlayer(_playerID);
            _player.RpcTakeDamage(_damage);
        }
    }
}