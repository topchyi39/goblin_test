using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemiesManager
{
    public IReadOnlyList<Enemie> Enemies => _enemies;
        
    private int currWave = 0;
    private List<Enemie> _enemies = new (100);
        
        
    public void AddEnemy(Enemie enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemie enemy)
    {
        _enemies.Remove(enemy);
    }

    public Enemie GetClosestEnemyAtPoint(Vector3 position)
    {
        Enemie closestEnemie = null;
        for (int i = 0; i < _enemies.Count; i++)
        {
            var enemie = _enemies[i];
            if (enemie == null || enemie.IsDied)
            {
                continue;
            }
        
            if (closestEnemie == null)
            {
                closestEnemie = enemie;
                continue;
            }
        
            var distance = Vector3.Distance(position, enemie.transform.position);
            var closestDistance = Vector3.Distance(position, closestEnemie.transform.position);
        
            if (distance < closestDistance)
            {
                closestEnemie = enemie;
            }
        
        }

        return closestEnemie;
    }
}