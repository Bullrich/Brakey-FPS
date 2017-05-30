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

        [Client]
        private void Shoot()
        {
            RaycastHit _hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range,
                shootMask))
            {
                // We hit something
                if (_hit.collider.tag == player_tag)
                    CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
            }
        }

        [Command]
        void CmdPlayerShot(string _playerID, int _damage)
        {
            Debug.Log(_playerID + "has been shot.");

            Player _player = GameManager.GetPlayer(_playerID);
            _player.RpcTakeDamage(_damage);
        }
    }
}