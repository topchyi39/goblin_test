using UI;
using UniRx;
using UnityEngine;

namespace PlayerComponents
{
    public class PlayerAttackViewModel : IViewModel
    {
        private float doubleAttackCooldown;
        
        private PlayerAttack _model;
        private PlayerAttackView _view;
        
        public PlayerAttackViewModel(PlayerAttack attackModel)
        {
            _view = UIManager.Instance.GetScreen<AttackScreen>().Bind<PlayerAttackView>(this);

            _model = attackModel;
            doubleAttackCooldown = _model.CurrentDoubleAttackCooldown.Value;
            _model.CurrentDoubleAttackCooldown.Subscribe(value=> _view.UpdateCooldown(value/doubleAttackCooldown));
            _model.ReadyToDoubleAttack.Subscribe(_view.UpdateInteractable);
        }

        public void Attack()
        {
            _model.Attack();
        }

        public void DoubleAttack()
        {
            _model.DoubleAttack();
        }
    }
}