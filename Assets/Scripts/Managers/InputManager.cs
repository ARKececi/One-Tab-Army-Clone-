using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables
        
        private Vector3 _hitPosition;
        private bool _click;

        #endregion

        #endregion
        
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    MousePosition(Input.mousePosition,hit.point);
                }
            }
        }

        private void MousePosition(Vector3 mousePosition , Vector3 hitPosition)
        {
            _hitPosition = hitPosition;
            InputSignals.Instance.onInputParams?.Invoke(new InputParams()
            {
                HitPosition = _hitPosition,
            });
        }
    }
}