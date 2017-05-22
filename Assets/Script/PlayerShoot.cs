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
                    CmdPlayerShot(_hit.collider.name);
            }
        }

        [Command]
        void CmdPlayerShot(string _id)
        {
            Debug.Log(_id + "has been shot.");
        }

    }
}