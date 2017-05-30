// By @Bullrich

using UnityEngine;

namespace game
{
    [System.Serializable]
    public class PlayerWeapon
    {
        public string name = "Weapon";

        public float range = 200f;
        public int damage = 10;

        public float fireRate = 0f;

        public GameObject graphics;
    }
}