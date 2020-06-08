using BehaviourMachine;
using UnityEngine;

namespace EscapeFromGod.Functions
{
    class NamelessScreen : MonoBehaviour
    {
        public static float screenHeight = 1280f;
        public static float screenWidth = 720f;

        private void Awake()
        {
#if Pasted
            screenHeight = Screen.height;
            screenWidth = Screen.width;
#endif
        }
    }
}
