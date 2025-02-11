using DG.Tweening;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private GameObject iconPrefab; // Sahnedeki ikon nesnesi (önceden aktif değil olmalı)
        [SerializeField] private LayerMask hitLayers;   // Hangi katmanlara tıklanabileceğini belirler
        [SerializeField] private float moveDuration = 0.3f; // Hareket süresi
        [SerializeField] private float scaleAmount = 1.5f; // Büyüme miktarı
        [SerializeField] private float activeTime = 0.5f; // İkonun görünme süresi
        [SerializeField] private Collider _ticCollider;
        
        private Vector3 originalScale;
        
        #endregion

        #endregion
        
        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onInputParams += MoveIconToMousePosition;
        }

        private void UnsubscribeEvents()
        {
            InputSignals.Instance.onInputParams -= MoveIconToMousePosition;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        #endregion
        
        void Start()
        {
            if (iconPrefab != null)
            {
                iconPrefab.SetActive(false); // Başlangıçta kapalı
                originalScale = iconPrefab.transform.localScale;
            }
        }
        // basıldığı noktada ikon oluşturur ve gerekli bölgeye yönlendirme sağlar.
        void MoveIconToMousePosition(InputParams ray)
        {
            
            RaycastHit hit;

            if (Physics.Raycast(ray.HitPosition, out hit, Mathf.Infinity, hitLayers)) // Yüzeye çarptıysa
            {
                if (iconPrefab != null)
                {
                    _ticCollider.transform.position = hit.point;
                    
                    // Belirli bir süre sonra kapat
                    Invoke(nameof(HideIcon), activeTime);
                    TowerSignals.Instance.onHitTarget?.Invoke(hit.point);
                }
            }
        }

        void HideIcon()
        {
            iconPrefab.SetActive(false);
        }
    }
}