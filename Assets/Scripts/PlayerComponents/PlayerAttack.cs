using System;
using System.Linq;
using UniRx;
using UnityEngine;


namespace PlayerComponents
{
    [Serializable]
    public class AttackData
    {
        public float Damage;
        public float DoubleAttackCooldown;
        public float DoubleAttackMultiplier;
        public int DoubleAttackPerformedCount;
        public double AttackRange;
        
    }
    
    
    public class PlayerAttack
    {

        public ReactiveProperty<bool> ReadyToDoubleAttack { get; private set; } = new ();
        public ReactiveProperty<float> CurrentDoubleAttackCooldown { get; private set; } = new ();
        
        private bool _isDoubleAttack;
        private int _currentAttackCount;
        

        private PlayerAttackViewModel _viewModel;
        private EnemiesManager _enemiesManager;
            
        private readonly AttackData _data;
        private readonly Transform _characterTransform;
        private readonly Animator _characterAnimator;
        private readonly PlayerMovement _movement;
        
        private readonly int AnimatorAttackKey = Animator.StringToHash("Attack");
        private readonly int AnimatorDoubleAttackKey = Animator.StringToHash("DoubleAttack");

        public PlayerAttack(AttackData data, Transform characterTransform, Animator characterAnimator, PlayerMovement movement)
        {
            _data = data;
            _characterTransform = characterTransform;
            _characterAnimator = characterAnimator;
            _movement = movement;
            _enemiesManager = SceneManager.Instance.EnemiesManager;
            
            CurrentDoubleAttackCooldown.Value = _data.DoubleAttackCooldown;
            
            _viewModel = new PlayerAttackViewModel(this);
        }
        
        public void Attack()
        {
            _characterAnimator.SetTrigger(AnimatorAttackKey);
        }

        public void DoubleAttack()
        {
            if (!ReadyToDoubleAttack.Value) return;

            ReadyToDoubleAttack.Value = false;
            CurrentDoubleAttackCooldown.Value = _data.DoubleAttackCooldown;
            _currentAttackCount = 0;
            _isDoubleAttack = true;
            
            _characterAnimator.SetTrigger(AnimatorDoubleAttackKey);
        }

        public void Tick()
        {
            PerformCooldown();
            
            if (CurrentDoubleAttackCooldown.Value <= 0)
            {
                var closesEnemy = _enemiesManager.GetClosestEnemyAtPoint(_characterTransform.position);
                ReadyToDoubleAttack.Value = closesEnemy && InRange(closesEnemy.transform.position);
            }
        }
        
        private void PerformCooldown()
        {
            if (!(CurrentDoubleAttackCooldown.Value > 0)) return;

            CurrentDoubleAttackCooldown.Value -= Time.deltaTime;
        }

        public void AttackPerformed(out bool killed)
        {
            killed = false;
            var closesEnemy = _enemiesManager.GetClosestEnemyAtPoint(_characterTransform.position);
            
            var damage = GetDamage();
            
            if (closesEnemy && InRange(closesEnemy.transform.position))
            {
                _movement.LookAtPoint = closesEnemy.transform.position;
               closesEnemy.Hit(damage);
                killed = closesEnemy.IsDied;
            }

            if (!_isDoubleAttack) return;
            
            _currentAttackCount++;
            if (_currentAttackCount >= _data.DoubleAttackPerformedCount) _isDoubleAttack = false;
        }

        private float GetDamage()
        {
            return _data.Damage * (_isDoubleAttack ? _data.DoubleAttackMultiplier : 1f);
        }

        private bool InRange(Vector3 point)
        {
            var distance = Vector3.Distance(point, _characterTransform.position);

            return distance <= _data.AttackRange;
        }

        public bool EnemiesInRange(out Vector3 point)
        {
            var closesEnemy = _enemiesManager.GetClosestEnemyAtPoint(_characterTransform.position);
            point = closesEnemy == null ? Vector3.zero : closesEnemy.transform.position;
            
            return InRange(point);
        }
    }
}