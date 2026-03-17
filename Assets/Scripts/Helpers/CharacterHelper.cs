using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class CharacterHelper
    {
        public static Vector2 GetFacing(DirectionEnum direction)
        {
            var facing = new Vector2();

            switch (direction)
            {
                case DirectionEnum.DOWN: { facing = Vector2.down; } break;
                case DirectionEnum.UP: { facing = Vector2.up; } break;
                case DirectionEnum.LEFT: { facing = Vector2.left; } break;
                case DirectionEnum.RIGHT: { facing = Vector2.right; } break;
                default: { Debug.Log("Could not update Facing in CharacterHelper.GetFacing()"); } break;
            }

            return facing;
        }

        public static float GetRotation(DirectionEnum direction)
        {
            float rotation = 0f;

            switch (direction)
            {
                case DirectionEnum.DOWN: { rotation = 180f; } break;
                case DirectionEnum.UP: { rotation = 0f; } break;
                case DirectionEnum.LEFT: { rotation = 90f; } break;
                case DirectionEnum.RIGHT: { rotation = 270f; } break;
                default: { Debug.Log("Could not update Facing in CharacterHelper.GetFacing()"); } break;
            }

            return rotation;
        }
    }
}
