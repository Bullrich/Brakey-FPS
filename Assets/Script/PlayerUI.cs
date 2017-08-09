using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

// by @Bullrich

namespace game
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private RectTransform thrusterFuelFill, lifeFill;
        private PlayerController controller;
        private Player player;

        [SerializeField] private GameObject pauseMenu;

        public Text scoreText, winnerText;
        string netId;

        public void SetController(PlayerController _controller, string _netId)
        {
            controller = _controller;
            netId = _netId;
            player = _controller.gameObject.GetComponent<Player>();
        }

        private void Start()
        {
            PauseMenu.IsPaused = false;
            hasWinner = false;
        }

        private void Update()
        {
            SetFuelAmount(controller.GetThrusterFuelAmount());
            SetLifeAmount(player.GetCurrentHealth() / player.GetMaxHealth());
            UpdateScore();
            if (GameManager.instance.GetWinner() != null && !hasWinner)
                SetWinner(GameManager.instance.GetWinner());

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
            }
        }

        public void UpdateScore()
        {
            string scoreString = "";
            foreach (KeyValuePair<string, int> score in GameManager.playerScore)
                scoreString += string.Format("{0}: {1}\n", score.Key, score.Value);
            scoreText.text = scoreString;
        }

        public void SetFuelAmount(float _amount)
        {
            thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
        }

        public void SetLifeAmount(float _amount)
        {
            lifeFill.localScale = new Vector3(1f, _amount, 1f);
        }

        bool hasWinner;

        public void SetWinner(string winnerName)
        {
            winnerText.transform.parent.gameObject.SetActive(true);
            winnerText.text = string.Format("{0} is the winner!", winnerName);
            Invoke("ExitGame", 3f);
            hasWinner = true;
            if (winnerName.Replace("Player ", "") == netId)
                AchievmentManager.instance.ShowAchievement("win");
            else
            AchievmentManager.instance.ShowAchievement("finish");
        }

        void ExitGame()
        {
            NetworkManager networkManager = NetworkManager.singleton;
            MatchInfo matchInfo = networkManager.matchInfo;
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();
        }

        public void TogglePauseMenu()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            PauseMenu.IsPaused = pauseMenu.activeSelf;
        }
    }
}