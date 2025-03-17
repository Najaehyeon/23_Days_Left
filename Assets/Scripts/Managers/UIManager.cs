using System;
using System.Collections.Generic;
using _23DaysLeft.Monsters;
using UnityEngine;

namespace _23DaysLeft.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Canvas")]
        [SerializeField] private Canvas screenCanvas;
        [SerializeField] private Canvas worldCanvas;
        
        [Header("UI")]
        [SerializeField] private BossInfoPanel bossInfoPanel;

        private Dictionary<CreatureController, CreatureHealthBar> creatureHealthBars = new();
        private const string creatureHpBar = "CreatureHealthBar";
        
        public void ActiveCreatureHpBar(CreatureController creature, Transform target, float maxHp)
        {
            var hpBar = PoolManager.Instance.Spawn<CreatureHealthBar>(creatureHpBar);
            hpBar.transform.SetParent(worldCanvas.transform);
            hpBar.Init(target, maxHp);
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
            
            creatureHealthBars[creature].Inactive();
            creatureHealthBars.Remove(creature);
        }
        
        public void ActiveBossInfoPanel(string name, float maxHp)
        {
            bossInfoPanel.Init(name, maxHp);
        }
        
        public void UpdateBossInfoPanel(float currentHp)
        {
            bossInfoPanel.ChangeHealth(currentHp);
        }
        
        public void InactiveBossInfoPanel()
        {
            bossInfoPanel.Inactive();
        }
    }
}