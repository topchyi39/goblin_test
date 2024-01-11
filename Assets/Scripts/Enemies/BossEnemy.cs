using UnityEngine;

namespace Enemies
{
    public class BossEnemy : Enemie
    {
        [SerializeField] private Enemie[] childEnemies;
        
        protected override void Die()
        {
            base.Die();

            foreach (var enemy in childEnemies)
            {
                enemy.transform.SetParent(null);
                enemy.gameObject.SetActive(true);
            }
        }
    }
}