using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RoguelikeTemplate
{
    public class Room : MonoBehaviour
    {
        public enum Type { Fight, Survive, Shop, Boss }

        public Type type;

        public static Room instance;

        public int level;
        public Vector3 chestPos;
        public DungeonEntrance[] doors;
        public DungeonEntrance exit;

        [Header("Boundaries")]
        public Vector2 minMaxX;
        public Vector2 minMaxY;

        [Header("Survival Room Settings")]
        public bool timeUp;
        public int limitTreshold;
        public float timeLeft;
        public Text countdownText;

        [Header("Wave Management")]
        public GameObject spawnIndicatorPrefab;
        public int currentWave;
        [Tooltip("How many waves will be in this room ||| How many enemies to spawn in next wave")]
        public List<int> waves;
        public GameObject[] enemiesToSpawn;
        public Transform[] spawnPositions;

        // The index of what to spawn next
        private int spawnIndex;
        // The index of where to spawn next
        private int positionIndex;
        private float enemyDelay = .75f;
        private float indicatorDelay = .1f;

        private void Awake()
        {
            if (instance != null) Destroy(gameObject);
            else instance = this;
        }

        void Start()
        {
            LevelManager.instance.currentRoom = this;
            if (type != Type.Boss)
            {
                if (type == Type.Survive)
                {
                    SpawnContinous(2);
                    countdownText = GameManager.instance.countdownText;
                    countdownText.gameObject.SetActive(true);
                }
                else if (type == Type.Fight) SpawnWave(2);
            }

            if (type == Type.Shop) UnlockDoors();

            exit.Exit();

            GameManager.instance.PlayerUIPrompt(true);
        }

        void Update()
        {
            if (type != Type.Survive) return;

            if (timeLeft <= 0.04f)
            {
                if (!timeUp)
                {
                    if (LevelManager.instance.entities.Count > 0)
                    {
                        // Kill all enemies
                        LevelManager.instance.entities[0].swordInvincible = false;
                        LevelManager.instance.entities[0].TakeDamage(new DamageTaken(1000, 0, Vector2.zero, 1));
                        LevelManager.instance.RemoveEntity(LevelManager.instance.entities[0]);
                    }
                    else
                    {
                        countdownText.text = "00:00";
                        countdownText.gameObject.SetActive(false);
                        timeUp = true;
                    }
                }
            }
            else
            {
                timeLeft -= 1 * Time.deltaTime;

                float minutes = Mathf.FloorToInt(timeLeft / 60);
                float seconds = Mathf.FloorToInt(timeLeft % 60);

                countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }

        public void SpawnWave(int delay)
        {
            StartCoroutine(SpawnWaveCO(delay));
        }

        public void SpawnContinous(int delay)
        {
            StartCoroutine(SpawnContinousCO(delay));
        }

        public void SpawnSingle()
        {
            StartCoroutine(SpawnSingleCO());
        }

        private IEnumerator SpawnSingleCO()
        {
            // Don't reset indexes
            int startSpawnIndex = spawnIndex;
            int startPositionIndex = positionIndex;

            // Spawn indicators first
            GameObject indicator = Instantiate(spawnIndicatorPrefab, spawnPositions[positionIndex].position, Quaternion.identity);
            Destroy(indicator, 4);
            spawnIndex++;
            positionIndex++;

            if (positionIndex >= spawnPositions.Length) positionIndex = 0;

            yield return new WaitForSeconds(indicatorDelay);

            spawnIndex = startSpawnIndex;
            positionIndex = startPositionIndex;

            // Wait
            yield return new WaitForSeconds(enemyDelay);

            // If time ran out, don't bother spawning new enemy
            if (timeLeft <= 0) yield break;

            // Then spawn enemies
            Enemy enemy = Instantiate(enemiesToSpawn[UnityEngine.Random.Range(0, enemiesToSpawn.Length)], spawnPositions[positionIndex].position, Quaternion.identity).GetComponent<Enemy>();
            enemy.necromancerSummoned = true;
            LevelManager.instance.entities.Add(enemy);
            spawnIndex++;
            positionIndex++;

            if (spawnIndex >= enemiesToSpawn.Length) spawnIndex = 0;
            if (positionIndex >= spawnPositions.Length) positionIndex = 0;

            yield return new WaitForSeconds(indicatorDelay);

            currentWave++;
        }

        private IEnumerator SpawnContinousCO(int delay)
        {
            if (delay > 0) yield return new WaitForSeconds(delay);

            // Don't reset indexes
            int startSpawnIndex = spawnIndex;
            int startPositionIndex = positionIndex;

            while (spawnIndex < limitTreshold)
            {
                // Spawn indicators first
                GameObject indicator = Instantiate(spawnIndicatorPrefab, spawnPositions[positionIndex].position, Quaternion.identity);
                Destroy(indicator, 4);
                spawnIndex++;
                positionIndex++;

                if (positionIndex >= spawnPositions.Length) positionIndex = 0;

                yield return new WaitForSeconds(indicatorDelay);
            }

            spawnIndex = startSpawnIndex;
            positionIndex = startPositionIndex;

            // Wait
            yield return new WaitForSeconds(enemyDelay);

            // If time ran out, don't bother spawning new enemy
            if (timeLeft <= 0) yield break;

            while (LevelManager.instance.entities.Count < limitTreshold)
            {
                // Then spawn enemies
                Enemy enemy = Instantiate(enemiesToSpawn[UnityEngine.Random.Range(0, enemiesToSpawn.Length)], spawnPositions[positionIndex].position, Quaternion.identity).GetComponent<Enemy>();
                enemy.necromancerSummoned = true;
                LevelManager.instance.entities.Add(enemy);
                spawnIndex++;
                positionIndex++;

                if (spawnIndex >= enemiesToSpawn.Length) spawnIndex = 0;
                if (positionIndex >= spawnPositions.Length) positionIndex = 0;

                yield return new WaitForSeconds(indicatorDelay);
            }

            currentWave++;
        }

        public IEnumerator SpawnWaveCO(int delay)
        {
            if (delay > 0) yield return new WaitForSeconds(delay);

            // Don't reset indexes
            int startSpawnIndex = spawnIndex;
            int startPositionIndex = positionIndex;
            int nextBatch = spawnIndex + waves[currentWave];

            while(spawnIndex < nextBatch)
            {
                // Spawn indicators first
                GameObject indicator = Instantiate(spawnIndicatorPrefab, spawnPositions[positionIndex].position, Quaternion.identity);
                Destroy(indicator, 4);
                spawnIndex++;
                positionIndex++;

                if (positionIndex >= spawnPositions.Length) positionIndex = 0;

                yield return new WaitForSeconds(indicatorDelay);
            }

            spawnIndex = startSpawnIndex;
            positionIndex = startPositionIndex;

            // Wait
            yield return new WaitForSeconds(enemyDelay);

            while (spawnIndex < nextBatch)
            {
                // Then spawn enemies
                Enemy enemy = Instantiate(enemiesToSpawn[spawnIndex], spawnPositions[positionIndex].position, Quaternion.identity).GetComponent<Enemy>();
                if (type == Type.Survive || type == Type.Boss) enemy.necromancerSummoned = true;
                LevelManager.instance.entities.Add(enemy);
                spawnIndex++;
                positionIndex++;

                if (spawnIndex >= enemiesToSpawn.Length)
                {
                    nextBatch -= spawnIndex;
                    spawnIndex = 0;
                }

                if (positionIndex >= spawnPositions.Length) positionIndex = 0;

                yield return new WaitForSeconds(indicatorDelay);
            }

            currentWave++;
        }

        public void UnlockDoors()
        {
            for (int i = 0; i < doors.Length; i++) doors[i].Unlock();
        }
    }

    public class Wave
    {
        public int wave;
        public int batch;

        public Wave(int _wave, int _batch)
        {
            wave = _wave;
            batch = _batch;
        }
    }

}
