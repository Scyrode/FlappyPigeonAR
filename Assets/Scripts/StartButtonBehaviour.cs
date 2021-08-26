using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scyout
{
    public class StartButtonBehaviour : MonoBehaviour
    {
        public delegate void StartButtonPressAction();
        public static event StartButtonPressAction onStartButtonPress;

        private void OnMouseDown()
        {
            onStartButtonPress();
        }
    }
}
