using _23DaysLeft.Managers;
using System.Collections.Generic;
using _23DaysLeft.Monsters;
using UnityEngine;

namespace _23DaysLeft.Player
{
    public class PlayerSensor : MonoBehaviour
    {
        [SerializeField] private float detectRadius;
        [SerializeField] private float detectInterval;
        [SerializeField] private LayerMask detectLayer;

        private readonly HashSet<IDetectable> currentDetectables = new();
        private readonly HashSet<IDetectable> prevDetectables = new();

        private Collider[] buffer;
        private float lastDetectTime;
        private bool isDead;

        private void Start()
        {
            // TODO: 최대 몬스터 수 정해지면 변경
            buffer = new Collider[10];
            lastDetectTime = detectInterval;
            CharacterManager.Instance._player.controller.OnPlayerDead += PlayerDead;
        }

        private void Update()
        {
            if (isDead) return;
            if (Time.time - lastDetectTime < detectInterval) return;
            lastDetectTime = Time.time;

            prevDetectables.Clear();
            foreach (var detectable in currentDetectables)
                prevDetectables.Add(detectable);

            currentDetectables.Clear();

            int count = Physics.OverlapSphereNonAlloc(transform.position, detectRadius, buffer, detectLayer);
            for (int i = 0; i < count; i++)
            {
                IDetectable detectable = buffer[i].GetComponent<IDetectable>();
                if (detectable != null)
                {
                    currentDetectables.Add(detectable);
                    detectable.OnPlayerDetected(transform);
                }
            }

            foreach (IDetectable detectable in prevDetectables)
            {
                if (!currentDetectables.Contains(detectable))
                {
                    detectable.OnPlayerFaraway();
                }
            }
        }
        
        private void PlayerDead()
        {
            isDead = true;
            currentDetectables.Clear();
            prevDetectables.Clear();
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawWireSphere(transform.position, detectRadius);
        // }
    }
}
