using System;
using UnityEngine;

namespace PlayerComponents
{
    public class PlayerInput
    {
        public float HorizontalAxis;
        public float VerticalAxis;

        public void Tick()
        {
            HorizontalAxis = Input.GetAxis("Horizontal");
            VerticalAxis = Input.GetAxis("Vertical");
        }
    }

    [Serializable]
    public class MovementData
    {
        public float speed;
        public float rotationSpeed;
    }
    
    public class PlayerMovement
    {
        private Transform CharacterTransform => _characterController.transform;
        public bool CanPerformed { get; set; }
        public Vector3 LookAtPoint { get; set; }
        
        private readonly MovementData _data;
        private readonly PlayerInput _input;
        private readonly CharacterController _characterController;
        private readonly Animator _characterAnimator;

        private readonly int AnimationSpeedKey = Animator.StringToHash("Speed");
        
        
        private Vector2 _axis;
        
        public PlayerMovement(MovementData movementData, PlayerInput input, CharacterController characterController, Animator characterAnimator)
        {
            _data = movementData;
            _input = input;
            _characterController = characterController;
            _characterAnimator = characterAnimator;
            CanPerformed = true;
        }

        public void Tick()
        {
            _axis.x = _input.HorizontalAxis;
            _axis.y = _input.VerticalAxis;
            _axis.Normalize();

            if (!CanPerformed)
            {
                var direction = (LookAtPoint - CharacterTransform.position).normalized;
                
                var rotation = CharacterTransform.rotation;
                var targetRotation = Quaternion.LookRotation(direction);
                rotation = Quaternion.RotateTowards(rotation, targetRotation, _data.rotationSpeed);
                CharacterTransform.rotation = rotation;
                return;
            }
            
            Move();
            Rotate();
        }

        private void Move()
        {
            var velocity = new Vector3(_axis.x, 0, _axis.y);
            
            _characterController.Move(velocity * (_data.speed * Time.deltaTime));
            _characterAnimator.SetFloat(AnimationSpeedKey, velocity.magnitude);
        }

        private void Rotate()
        {
            if (_axis.magnitude <= 0) return;
            
            var velocity = new Vector3(_axis.x, 0, _axis.y);
            
            var rotation = CharacterTransform.rotation;

            var targetRotation = Quaternion.LookRotation(velocity);
            rotation = Quaternion.RotateTowards(rotation, targetRotation, _data.rotationSpeed);
            CharacterTransform.rotation = rotation;
        }
    }
}