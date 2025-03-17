using System.Collections.Generic;
using _23DaysLeft.Managers;
using _23DaysLeft.Monsters;
using UnityEngine;

namespace _23DaysLeft.Utils
{
    public class OnGui : MonoBehaviour
    {
        public bool isDebug = true;
        private List<GameObject> spawnObjects = new();
        [SerializeField] private CreatureSpawner spawner;
        [SerializeField] private CreatureController creature;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        public void OnGUI()
        {
            if (!isDebug) return;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 25;
        }
    }
}