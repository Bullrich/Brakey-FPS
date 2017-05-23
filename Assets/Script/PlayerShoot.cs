using UnityEngine.Networking;
using UnityEngine;

// By @Bullrich

namespace game
{
    public class PlayerShoot : NetworkBehaviour
    {
        private const string player_tag = "Player";

        public PlayerWeapon weapon;
        [SerializeField]
        private LayerMask shootMask;

        [SerializeField]
        private Camera cam;

        private void Start()
        {
            if (cam == null)
            {
                Debug.LogError("PlayerShoot: No camera referenced");
                this.enabled = false;
            }
        }

        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }

        [Client]
        private void Shoot()
        {
            RaycastHit _hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, shootMask))
            {
                // We hit something
                if (_hit.collider.tag == player_tag)
                    CmdPlayerShot(_hit.collider.name, weapon.damage);
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