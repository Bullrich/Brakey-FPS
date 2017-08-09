using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// By @Bullrich

namespace game
{
[RequireComponent(typeof(AudioSource))]
    public class CallShakeCamera : MonoBehaviour
    {
        public AudioClip[] blip;
        public CameraShake cameraShake;
        AudioSource audio;
		public float cameraAmount = 1.5f, cameraDuration = .5f;

        void Start(){
            audio = GetComponent<AudioSource>();
        }

        public void Shake()
        {
            cameraShake.ShakeCamera(cameraAmount, cameraDuration);
        }

        public void Blip(){
            audio.clip = blip[Random.Range(0, blip.Length)];
            audio.Play();
        }

		public void CallNextScreen(){
			SceneManager.LoadScene(1);
		}
    }
}