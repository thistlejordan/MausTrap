using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components.Shared
{
    public class AxisInput : MonoBehaviour
    {
        private bool usingInput;
        private Dictionary<string, bool> axesInUse = new Dictionary<string, bool>
        {
            { "Horizontal", false },
            { "Vertical", false }
        };

        public bool GetAxisDown(string axisName)
        {
            bool axisInUse = axesInUse[axisName];
            int axisValue = (int)Input.GetAxisRaw(axisName);

            if (!axisInUse && axisValue != 0)
            {
                axisInUse = true;
                return true;
            }
            return false;
        }

        public bool GetAxis(string axisName)
        {
            bool axisInUse = axesInUse[axisName];
            int axisValue = (int)Input.GetAxisRaw(axisName);

            return (axisInUse && axisValue != 0);
        }

        public bool GetAxisUp(string axisName)
        {
            bool axisInUse = axesInUse[axisName];
            int axisValue = (int)Input.GetAxisRaw(axisName);

            if (axisInUse && axisValue == 0)
            {
                axisInUse = false;
                return true;
            }
            return false;
        }
    }
}
