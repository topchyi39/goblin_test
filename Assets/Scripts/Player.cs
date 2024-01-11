using System;
using PlayerComponents;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public float Hp;

    [SerializeField] private float restoreHpByKill = 2;
    [SerializeField] private AttackData attackData;
    [SerializeField] private MovementData movementData;
    
    
    private CharacterController controller;
    
    private PlayerInput input;
    private PlayerMovement movement;
    private PlayerAttack attack;
    
    private float lastAttackTime = 0;
    private bool isDead = false;
    public Animator AnimatorController;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        
        input = new PlayerInput();
        movement = new PlayerMovement(movementData, input, controller, AnimatorController);
        attack = new PlayerAttack(attackData, transform, AnimatorController, movement);
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }
        
        if (Hp <= 0)
        {
            Die();
            return;
        }
        
        TickComponents();
    }

    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");

        SceneManager.Instance.GameOver();
    }

    private void TickComponents()
    {
        input.Tick();
        attack.Tick();
        movement.Tick();
    }

    private void AttackStarted()
    {
        if (attack.EnemiesInRange(out var point))
        {
            movement.CanPerformed = false;
            movement.LookAtPoint = point;
        }
    }

    private void AttackPerformed()
    {
        attack.AttackPerformed(out var killed);
        
        if (killed)
            Hp += restoreHpByKill;
    }

    private void AttackEnded()
    {
        movement.CanPerformed = true;
    }
}
