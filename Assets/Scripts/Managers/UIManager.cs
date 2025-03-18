using System.Collections.Generic;
using _23DaysLeft.Monsters;
using _23DaysLeft.UI;
using System;
using UnityEngine;

namespace _23DaysLeft.Managers
{
    public class UIManager : MonoBehaviour
    {
        private MainSceneUIList uiList;
        public Action<float> OnChangeLoadingProgress;
        
        private Dictionary<CreatureController, CreatureHealthBar> creatureHealthBars = new();
        private const string creatureHpBar = "CreatureHealthBar";

        public void Init(MainSceneUIList ui)
        {
            uiList = ui;
            uiList.PlayerConditionPanel.Init();
        }
        
        public void ActiveCreatureHpBar(CreatureController creature, Transform target, float maxHp)
        {
            var hpBar = Global.Instance.PoolManager.Spawn<CreatureHealthBar>(creatureHpBar);
            hpBar.transform.SetParent(uiList.WorldCanvas.transform);
            hpBar.SetInfo(target, maxHp);
            creatureHealthBars.Add(creature, hpBar);
        }
        
        public void UpdateCreatureHpBar(CreatureController creature, float currenHp)
        {
            if (creatureHealthBars.ContainsKey(creature))
            {
                creatureHealthBars[creature].ChangeHealth(currenHp);
            }
        }
        
        public void InactiveCreatureHpBar(CreatureController creature)
        {
            if (!creatureHealthBars.ContainsKey(creature))
            {
                Debug.LogWarning("UIManager: Not exist creature health bar.");
                return;
            }
            
            creatureHealthBars[creature].Hide();
            creatureHealthBars.Remove(creature);
        }
        
        public void ActiveBossInfoPanel(string name, float maxHp)
        {
            uiList.BossInfoPanel.SetInfo(name, maxHp);
            uiList.BossInfoPanel.Show();
        }
        
        public void UpdateBossInfoPanel(float currentHp)
        {
            uiList.BossInfoPanel.ChangeHealth(currentHp);
        }
        
        public void InactiveBossInfoPanel()
        {
            uiList.BossInfoPanel.Hide();
        }

        public void OnChangePlayerHealth(float value)
        {
            uiList.PlayerConditionPanel.OnChangeHealth?.Invoke(value);
        }
        
        public void OnChangePlayerStamina(float value)
        {
            uiList.PlayerConditionPanel.OnChangeHunger?.Invoke(value);
        }
    }
}