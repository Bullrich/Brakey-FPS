using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// by @Bullrich

namespace game
{
    public class WeaponManager : NetworkBehaviour
    {
        [SerializeField] private PlayerWeapon primaryWeapon;
        private PlayerWeapon currentWeapon;

        private const string weapon_layer = "Weapon";

        [SerializeField] private Transform weaponHolder;

        public void Start()
        {
            EquipWeapon(primaryWeapon);
        }

        void EquipWeapon(PlayerWeapon _weapon)
        {
            currentWeapon = _weapon;

            GameObject _weaponIns = Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
            _weaponIns.transform.SetParent(weaponHolder);

            if (isLocalPlayer)
                _weaponIns.layer = LayerMask.NameToLayer(weapon_layer);
        }

        public PlayerWeapon GetCurrentWeapon()
        {
            return currentWeapon;
        }
    }
}