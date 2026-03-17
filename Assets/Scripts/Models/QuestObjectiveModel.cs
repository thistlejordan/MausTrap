using UnityEngine;

namespace Assets.Scripts.Models
{
    public class QuestObjectiveModel
    {
        [SerializeField] bool _completed;

        public bool Completed { get => _completed; set => _completed = value; }
    }
}
