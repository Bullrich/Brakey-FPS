using UnityEngine;

// by @Bullrich

namespace game
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private RectTransform thrusterFuelFill;
        private PlayerController controller;

        [SerializeField] private GameObject pauseMenu;

        public void SetController(PlayerController _controller)
        {
            controller = _controller;
        }

        private void Start()
        {
            PauseMenu.IsPaused = false;
        }

        private void Update()
        {
            SetFuelAmount(controller.GetThrusterFuelAmount());

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
            }
        }

        public void SetFuelAmount(float _amount)
        {
            thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
        }

        public void TogglePauseMenu()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            PauseMenu.IsPaused = pauseMenu.activeSelf;
        }
    }
}