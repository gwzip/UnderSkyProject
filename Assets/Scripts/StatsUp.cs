using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoguelikeTemplate
{


    [CreateAssetMenu(fileName ="New StatsUp", menuName ="New StatsUp")]
    public class StatsUp : ScriptableObject
    {
        public int health;
        public int damage;
        public float attackSpeed;
        public float speed;
        public int criticalChance;
        public int dashRange;
        public float dashCD;
        public float knockbackMultiplier;

        // Constructor
        public StatsUp(int _health, int _damage, float _attackSpeed, float _speed, int _criticalChance, float _knockback)
        {
            health = _health;
            damage = _damage;
            attackSpeed = _attackSpeed;
            speed = _speed;
            criticalChance = _criticalChance;
            knockbackMultiplier = _knockback;
        }

        // Apply stats
        public void SetStats(int _health, int _damage, float _attackSpeed, float _speed, int _criticalChance, float _knockback)
        {
            health = _health;
            damage = _damage;
            attackSpeed = _attackSpeed;
            speed = _speed;
            criticalChance = _criticalChance;
            knockbackMultiplier = _knockback;
        }

        // Getters and setters
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public float AttackSpeed
        {
            get { return attackSpeed; }
            set { attackSpeed = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public int CriticalChance
        {
            get { return criticalChance; }
            set { criticalChance = value; }
        }

        public int DashRange
        {
            get { return dashRange; }
            set { dashRange = value; }
        }

        public float DashCD
        {
            get { return dashCD; }
            set { DashCD = value; }
        }

        public float KnockbackMultiplier
        {
            get { return knockbackMultiplier; }
            set { KnockbackMultiplier = value; }
        }
    }
}
