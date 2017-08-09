using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// by @Bullrich

namespace game
{
    [RequireComponent(typeof(AudioSource))]
    public class WeaponManager : NetworkBehaviour
    {
        [SerializeField] private PlayerWeapon primaryWeapon;
        private PlayerWeapon currentWeapon;
        private WeaponGraphics currentGraphics;

        private const string weapon_layer = "Weapon";

        [SerializeField] private Transform weaponHolder;

        public void Start()
        {
            EquipWeapon(primaryWeapon);
        }

        private void EquipWeapon(PlayerWeapon _weapon)
        {
            currentWeapon = _weapon;

            GameObject _weaponIns = Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
            _weaponIns.transform.SetParent(weaponHolder);

            currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
            if (currentGraphics == null)
                Debug.LogError("No WeaponGraphics component on the weapon object: " + _weaponIns.name);

            if (isLocalPlayer)
                Util.SetLayerRecursively(_weaponIns, LayerMask.NameToLayer(weapon_layer));
        }

        public PlayerWeapon GetCurrentWeapon()
        {
            return currentWeapon;
        }

        public WeaponGraphics GetCurrentGraphics()
        {
            return currentGraphics;
        }
    }
}