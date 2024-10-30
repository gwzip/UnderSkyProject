using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RoguelikeTemplate
{ 

    public class XPManager : MonoBehaviour
    {
        public static XPManager instance;

        public int currentLevel;
        public int currentXP, requiredXP;
        public ParticleSystem levelUpEffect;
        public Text xpText;

        public GameObject[] statsUp;

        public Text levelText;
        public Slider levelSlider;

        private MySlider smartSlider;
        private AudioSource audioSource;

        void Start()
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            levelSlider.maxValue = requiredXP;
            levelSlider.value = currentXP;
            smartSlider = levelSlider.GetComponent<MySlider>();
            levelText.text = currentLevel.ToString();
            xpText.text = currentXP + "/" + requiredXP;
        }

        public void AddXP(int amount)
        {
            currentXP += amount;
            xpText.text = currentXP + "/" + requiredXP;
            smartSlider.AddXP(currentXP, .1f);

            // Handle level up
            if (currentXP >= requiredXP)
            {
                currentXP -= requiredXP;
                LevelUp();
            }
        }

        public void LevelUp()
        {
            // Next level
            audioSource.Play();
            currentLevel++;
            levelText.text = currentLevel.ToString();
            requiredXP += 3;
            xpText.text = currentXP + "/" + requiredXP;
            levelUpEffect.Play();

            // Choose on stats preset depending on current level
            StatsUp statsToAdd = ScriptableObject.CreateInstance<StatsUp>();

            if (currentLevel %3 == 0) // Every third level do a medium boost
            {
                statsToAdd.SetStats(2, 2, 0, 0, 0, 0);
            }
            else if (currentLevel %5 == 0) // Every fifth level do a large boost
            {
                statsToAdd.SetStats(3, 2, 1, 20, 5, 0);
            }
            else // Every other level do a small boost
            {
                statsToAdd.SetStats(1, 1, 0, 10, 0, 0);
            }

            // Give stats

            switch (Player.instance.gameClass) // Depending on the class
            {
                // Add stats based on class and on level
                case Player.GameClass.Knight: break;
                case Player.GameClass.Archer: break;
                case Player.GameClass.Wizard: break;
            }

            DisplayStats(statsToAdd);
        }

        public void DisplayStats(StatsUp statsToAdd)
        {
            // Display giving stats

            // Color Codes:
            // Health, Attack, Attack Speed, Speed, Critical Chance
            //#0FE72B  #E71011 #E7A510      #10A3E7 #8E10E7

            List<Transform> realignStats = new List<Transform>();

            // Align each and every stat that has increased
            for (int i = 0; i < statsUp.Length; i++)
            {
                string prefix = statsUp[i].GetComponentInChildren<Text>().text;

                prefix = prefix.Substring(prefix.LastIndexOf(' ') + 1);

                // If you can make this work then great, but otherwise I'll do it the dumb way

                // Get each property of container and out the value
                float.TryParse(statsToAdd.GetType().GetProperties()[i].GetValue(statsToAdd, null).ToString(), out float result);

                if (result > 0) // If there is anything to add, update UI Stat
                {
                    statsUp[i].GetComponentInChildren<Text>().text =
                        "+" + statsToAdd.GetType().GetProperties()[i].GetValue(statsToAdd, null).ToString() + " " + prefix;
                    statsUp[i].SetActive(true);
                    realignStats.Add(statsUp[i].transform);
                }
            }

            // Actually give stats to player
            Player.instance.AddStats(statsToAdd);

            // Realign stats
            realignStats.Reverse();
            int offset = 0;
            if (realignStats.Count > 3) offset = 250;

            for (int i = realignStats.Count - 1; i >= 0; i--)
                realignStats[i].transform.localPosition = new Vector3(realignStats[i].transform.localPosition.x, 125 * i - offset, 0);

            // Play animation of stat showing up
            StartCoroutine(ShowStats(realignStats));

            // Hide them after 3 seconds
            Invoke("HideStats", 3);
        }

        IEnumerator ShowStats(List<Transform> realignStats)
        {
            realignStats.Reverse();
            for (int i = 0; i < realignStats.Count; i++)
            {
                realignStats[i].GetComponent<Animator>().Play("StatsPanelShow");
                yield return new WaitForSeconds(.15f);
            }
        }

        private void HideStats() // Invoked
        {
            for (int i = 0; i < statsUp.Length; i++) statsUp[i].SetActive(false);
        }
    }

}