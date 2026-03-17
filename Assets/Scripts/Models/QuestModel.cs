using Assets.Scripts.Components.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class QuestModel
    {
        [SerializeField] private string _questText;
        [SerializeField] private IEnumerable<QuestObjectiveModel> _objectives;
        [SerializeField] private ItemComponent _reward;

        public string QuestText { get => _questText; set => _questText = value; }
        public IEnumerable<QuestObjectiveModel> Objectives { get => _objectives; set => _objectives = value; }
        public ItemComponent Reward { get => _reward; set => _reward = value; }
    }
}
