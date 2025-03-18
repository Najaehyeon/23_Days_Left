using System.Collections.Generic;
using _23DaysLeft.Monsters;
using _23DaysLeft.UI;
using System;
using UnityEngine;

namespace _23DaysLeft.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        private MainSceneUIList uiList;
        public Action<float> OnChangeLoadingProgress;
        
        private Dictionary<CreatureController, CreatureHealthBar> creatureHealthBars = new();
        private const string creatureHpBar = "CreatureHealthBar";

        public void Init(MainSceneUIList ui)
        {
            uiList = ui;
        }
        
        public void ActiveCreatureHpBar(CreatureController creature, Transform target, float maxHp)
        {
            var hpBar = PoolManager.Instance.Spawn<CreatureHealthBar>(creatureHpBar);
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
            else
            {
                Debug.LogWarning("UIManager: Not exist creature health bar.");
            }
        }
        
        public void InactiveCreatureHpBar(CreatureController creature)
        {
            if (!creatureHealthBars.ContainsKey(creature))
            {
                Debug.LogWarning("UIManager: Not exist creature health bar.");
                return;
            }
            
            creatureHealthBars[creature].Show();
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
    }
}