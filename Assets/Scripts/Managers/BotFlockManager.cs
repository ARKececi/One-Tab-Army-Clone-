using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class BotFlockManager : MonoBehaviour
    {
        public List<BotFlock> allBotsFlocks = new List<BotFlock>();

        // Flocking hesaplamalarını bu metodda yapacağız
        private void Update()
        {
            CalculateFlocking();
        }

        // Flocking davranışlarını hesapla
        private void CalculateFlocking()
        {
            for (int i = 0; i < allBotsFlocks.Count; i++)
            {
                // Her botun flocking davranışını hesaplayacak
                Vector3 cohesion = CalculateCohesion(i);
                Vector3 separation = CalculateSeparation(i);
                Vector3 alignment = CalculateAlignment(i);

                // Hesaplanan değerleri botlara ilet
                allBotsFlocks[i].SetFlockingValues(cohesion, separation, alignment);
            }
        }

        // Cohesion (birleşme) hesaplama
        private Vector3 CalculateCohesion(int botIndex)
        {
            Vector3 centerOfMass = Vector3.zero;

            for (int i = 0; i < allBotsFlocks.Count; i++)
            {
                if (i != botIndex) // Kendini dahil etme
                {
                    centerOfMass += allBotsFlocks[i].transform.position;
                }
            }

            centerOfMass /= allBotsFlocks.Count - 1;
            Debug.Log(allBotsFlocks[botIndex].transform.position);
            return (centerOfMass - allBotsFlocks[botIndex].transform.position);
        }

        // Separation (ayrılma) hesaplama
        private Vector3 CalculateSeparation(int botIndex)
        {
            Vector3 separationForce = Vector3.zero;
            for (int i = 0; i < allBotsFlocks.Count; i++)
            {
                if (i != botIndex && Vector3.Distance(allBotsFlocks[botIndex].transform.position, allBotsFlocks[i].transform.position) < 2f)
                {
                    separationForce += allBotsFlocks[botIndex].transform.position - allBotsFlocks[i].transform.position;
                }
            }

            return separationForce;
        }

        // Alignment (hizalama) hesaplama
        private Vector3 CalculateAlignment(int botIndex)
        {
            Vector3 averageVelocity = Vector3.zero;

            for (int i = 0; i < allBotsFlocks.Count; i++)
            {
                if (i != botIndex)
                {
                    averageVelocity += allBotsFlocks[i]._agent.velocity;
                }
            }

            averageVelocity /= allBotsFlocks.Count - 1;
            return averageVelocity - allBotsFlocks[botIndex]._agent.velocity;
        }

        // Botu listene ekle
        public void RegisterBot(BotFlock botFlock)
        {
            if (!allBotsFlocks.Contains(botFlock))
            {
                allBotsFlocks.Add(botFlock);
            }
        }

        // Botu listeden çıkar
        public void DeregisterBot(BotFlock botFlock)
        {
            if (allBotsFlocks.Contains(botFlock))
            {
                allBotsFlocks.Remove(botFlock);
            }
        }
    }
}
