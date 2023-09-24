#region Includes
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
#endregion

namespace TS.PageSlider
{
    public class PageDot : MonoBehaviour
    {
        #region Variables


        [SerializeField] private Color _colorDefault;
        [SerializeField] private Color _colorSelected;
        [Header("Events")]
        public UnityEvent<bool> OnActiveStateChanged;
        public UnityEvent<int> OnPressed;

        public bool IsActive { get; private set; }
        public int Index { get; set; }

        private Image image;

        #endregion

        private void Start()
        {
            image = GetComponent<Image>();
            OnActiveStateChanged.AddListener(ActiveStateChanged);
            ChangeActiveState(Index == 0);
        }

        public virtual void ChangeActiveState(bool active)
        {
            IsActive = active;
            OnActiveStateChanged?.Invoke(active);
        }
        public void Press()
        {
            OnPressed?.Invoke(Index);
        }

        private void ActiveStateChanged(bool active)
        {
            image.color = active ?  _colorDefault : _colorSelected;
        }
    }
}