using System;
using PlayerComponents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerAttackView : MonoBehaviour, IView
    {
        public Type ViewModelType => typeof(PlayerAttackViewModel);
        
        [SerializeField] private Button attack;

        [SerializeField] private AttackButton doubleAttackButton;
        
        private PlayerAttackViewModel _viewModel;

        private void Awake()
        {
            attack.onClick.AddListener(AttackClicked);
            doubleAttackButton.SubscribeOnClick(DoubleAttackClicked);
        }

        public void Bind(IViewModel viewModel)
        {
            _viewModel = viewModel as PlayerAttackViewModel;
        }

        public void UpdateInteractable(bool value) => doubleAttackButton.UpdateInteractable(value);
        
        public void UpdateCooldown(float value) => doubleAttackButton.UpdateCooldown(value);

        private void AttackClicked()
        {
            _viewModel?.Attack();
        }

        private void DoubleAttackClicked()
        {
            _viewModel?.DoubleAttack();
        }
    }
}