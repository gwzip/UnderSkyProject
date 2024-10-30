using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RoguelikeTemplate
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;

        public Room currentRoom;

        public Text floorText;
        public GameObject treasurePrefab;
        public List<Entity> entities = new List<Entity>();

        void Awake()
        {
            instance = this;
            if (currentRoom != null) floorText.text = currentRoom.level.ToString();
        }

        public void RemoveEntity(Entity entity)
        {
            if (!entities.Contains(entity)) return;

            entities.Remove(entity);

            // Survival Room Check
            if (currentRoom.type == Room.Type.Survive && currentRoom.timeLeft > 1) currentRoom.SpawnSingle();

            if (entities.Count <= 0)
            {
                // Fight Room Check
                if (currentRoom.type == Room.Type.Fight && currentRoom.currentWave != currentRoom.waves.Count)
                {
                    currentRoom.SpawnWave(2);
                    return;
                }

                // Boss Room Check
                if (currentRoom.type == Room.Type.Boss && currentRoom.currentWave < currentRoom.waves.Count) currentRoom.SpawnWave(1);
                else
                {
                    // All doors should display their choices
                    currentRoom.UnlockDoors();
                    SpawnLoot();
                }
            }
        }

        public void SpawnLoot()
        {
            Instantiate(treasurePrefab, currentRoom.chestPos, Quaternion.identity);
        }
    }

}

