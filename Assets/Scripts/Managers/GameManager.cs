using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        protected GameManager() { }

        //Global Fields

        public PlayerCharacterComponent _playerCharacter; //The player character (for initial single-player development and debugging)
        public int minimumDamage = 0; //Any damage dealt that is less than minimum will be set to 0
        public float iFramesDuration = 0.1f; //Invincibility duration after a player is hit
        [Range(1, 10)] public int m_IFrameFlickerRate = 4; //Invincibility frame flicker rate

        //Temporary (Equipped item image will eventually adjust with player selection)
        public Image m_EquippedItemImage;
        //End temporary

        public List<GameObject> m_ActiveProjectiles;

        public GameObject m_AggroRangeObject; //Use this prefab for building an aggro object for enemies that don't have one yet

        public bool debugMode = false;
        public Image m_DebugWindow;

        [Range(1, 10)] public float m_TextSpeed = 5f; //How fast does text render on the UI? Higher is faster

        public int MinimumDamage { get => minimumDamage; set => minimumDamage = value; }
        public float IFramesDuration { get => iFramesDuration; set => iFramesDuration = value; }
        public bool DebugMode { get => debugMode; set => debugMode = value; }

        void Awake()
        {
            DontDestroyOnLoad(gameObject); //Don't destroy our Game Manager object between scene transitions
        }
    }
}