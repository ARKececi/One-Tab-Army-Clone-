using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables
        
        private Ray _hitPosition;
        private bool _click;

        #endregion

        #endregion
        
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                MousePosition(ray);
            }
        }

        private void MousePosition(Ray ray)
        {
            _hitPosition = ray;
            InputSignals.Instance.onInputParams?.Invoke(new InputParams()
            {
                HitPosition = _hitPosition,
            });
        }
    }
}