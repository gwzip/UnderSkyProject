using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;


namespace CombatSystem2D
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Header("Pools and damage")]
        public GameObject hitNumberPrefab;
        public List<Projectile> arrowPool = new List<Projectile>();
        public List<Projectile> magicPool = new List<Projectile>();

        private void Awake()
        {
            if (instance != null) Destroy(gameObject);
        }

        void Start()
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        public float AngleBetweenTwoPoints(Vector2 from, Vector2 to)
        {
            Vector2 direction = to - from;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }
}