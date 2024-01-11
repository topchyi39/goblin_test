using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemie : MonoBehaviour
{
    public float Hp;
    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 2;
    public bool IsDied => Hp <= 0;

    public Animator AnimatorController;
    public NavMeshAgent Agent;

    private float lastAttackTime = 0;

    private Player Player;
    private Vector3 PlayerPosition => Player.transform.position;

    private void Start()
    {
        SceneManager.Instance.AddEnemie(this);
        Player = SceneManager.Instance.Player;
        Agent.SetDestination(PlayerPosition);
    }
    
    public void Hit(float value)
    {
        Hp -= value;
        if (IsDied)
        {
            Die();
            Agent.isStopped = true;
        }
    }

    private void AttackStarted() { }
    private void AttackPerformed() { }
    private void AttackEnded() { }

    private void Update()
    {
        if(IsDied)
        {
            return;
        }

        var distance = Vector3.Distance(transform.position, PlayerPosition);
        var inRange = distance <= AttackRange;
        Agent.isStopped = inRange;
        
        if (inRange)
        {
            if (Time.time - lastAttackTime > AtackSpeed)
            {
                lastAttackTime = Time.time;
                Player.Hp -= Damage;
                AnimatorController.SetTrigger("Attack");
            }
        }
        else
        {
            Agent.SetDestination(PlayerPosition);
        }
        AnimatorController.SetFloat("Speed", Agent.speed); 
    }



    protected virtual void Die()
    {
        SceneManager.Instance.RemoveEnemie(this);
        Agent.enabled = false;
        AnimatorController.SetTrigger("Die");
    }
}
