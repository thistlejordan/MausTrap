using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class DirectionExtensions
    {
        public static Vector3 ToVector3(this DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.UP:
                    return Vector3.up;
                case DirectionEnum.DOWN:
                    return Vector3.down;
                case DirectionEnum.LEFT:
                    return Vector3.left;
                case DirectionEnum.RIGHT:
                    return Vector3.right;
            }

            return Vector3.zero;
        }
    }
}
