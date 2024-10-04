using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
    public class SimpleActivatorMenu : MonoBehaviour
    {
        // A simple menu which, when given references
        // to GameObjects in the scene
        public Text camSwitchButton; // Use Text instead of Image
        public GameObject[] objects;

        private int m_CurrentActiveObject;

        private void OnEnable()
        {
            // Active object starts from first in array
            m_CurrentActiveObject = 0;
            camSwitchButton.text = objects[m_CurrentActiveObject].name; // Set the button text to the name of the first object
        }

        public void NextCamera()
        {
            int nextActiveObject = m_CurrentActiveObject + 1 >= objects.Length ? 0 : m_CurrentActiveObject + 1;

            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(i == nextActiveObject); // Activate the next object
            }

            m_CurrentActiveObject = nextActiveObject; // Update the current active object index
            camSwitchButton.text = objects[m_CurrentActiveObject].name; // Update the button text
        }
    }
}
