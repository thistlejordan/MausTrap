using UnityEngine;

namespace Assets.Scripts.Models
{
    public class KillQuestObjectiveModel : QuestObjectiveModel
    {
        [SerializeField] private CharacterComponent _killTarget;

        public CharacterComponent KillTarget { get => _killTarget; set => _killTarget = value; }
    }
}
