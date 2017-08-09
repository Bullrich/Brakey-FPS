using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// By @Bullrich

namespace game
{

    public class CallShakeCamera : MonoBehaviour
    {
        public CameraShake cameraShake;
		public float cameraAmount = 1.5f, cameraDuration = .5f;

        public void Shake()
        {
            cameraShake.ShakeCamera(cameraAmount, cameraDuration);
        }

		public void CallNextScreen(){
			SceneManager.LoadScene(1);
		}
    }
}