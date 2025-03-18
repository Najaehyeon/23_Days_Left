using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaysLeft.Menu
{
    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;

    /// <summary>
    /// The screen base class contains a lot of accessors (e.g. Config, Connection, ConnectArgs) for convenient access.
    /// </summary>
    public abstract class UIScreen : MonoBehaviour
    {
        /// <summary>
        /// Cached Hide animation hash.
        /// </summary>
        protected static readonly int HideAnimHash = Animator.StringToHash("Hide");
        /// <summary>
        /// Cached Show animation hash.
        /// </summary>
        protected static readonly int ShowAnimHash = Animator.StringToHash("Show");

        /// <summary>
        /// Is modal flag must be set for overlay screens.
        /// </summary>
        [SerializeField] private bool _isModal;
        /// <summary>
        /// The list of screen plugins for the screen. The actual plugin scripts can be distributed insde the UI hierarchy but must be liked here.
        /// </summary>
        [SerializeField] private List<ScreenPlugin> _plugins;
        /// <summary>
        /// The animator object.
        /// </summary>
        private Animator _animator;
        /// <summary>
        /// The hide animation coroutine.
        /// </summary>
        private Coroutine _hideCoroutine;

        /// <summary>
        /// The list of screen plugins.
        /// </summary>
        public List<ScreenPlugin> Plugins => _plugins;
        /// <summary>
        /// Is modal property.
        /// </summary>
        public bool IsModal => _isModal;
        /// <summary>
        /// Is the screen currently showing.
        /// </summary>
        public bool IsShowing { get; private set; }
        /// <summary>
        /// The menu UI controller that owns this screen.
        /// </summary>
        public UIController Controller { get; set; }

        /// <summary>
        /// Unity start method to find the animator.
        /// </summary>
        protected virtual void Start()
        {
            TryGetComponent(out _animator);
        }

        /// <summary>
        /// Unit awake method to be overwritten by derived screens.
        /// </summary>
        public virtual void Awake()
        {
        }

        /// <summary>
        /// The screen init method is called during <see cref="FusionMenuUIController{T}.Awake()"/> after all screen have been assigned and configured.
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        /// The screen hide method.
        /// </summary>
        public virtual void Hide()
        {
            if (_animator)
            {
                if (_hideCoroutine != null)
                {
                    StopCoroutine(_hideCoroutine);
                }

                _hideCoroutine = StartCoroutine(HideAnimCoroutine());
                return;
            }

            IsShowing = false;

            foreach (var p in _plugins)
            {
                p.Hide(this);
            }

            gameObject.SetActive(false);
        }

        /// <summary>
        /// The screen show method.
        /// </summary>
        public virtual void Show()
        {
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
                if (_animator.gameObject.activeInHierarchy && _animator.HasState(0, ShowAnimHash))
                {
                    _animator.Play(ShowAnimHash, 0, 0);
                }
            }

            gameObject.SetActive(true);

            IsShowing = true;

            foreach (var p in _plugins)
            {
                p.Show(this);
            }
        }

        /// <summary>
        /// Play the hide animation wrapped in a coroutine.
        /// Forces the target framerate to 60 during the transition animations.
        /// </summary>
        /// <returns>When done</returns>
        private IEnumerator HideAnimCoroutine()
        {
#if UNITY_IOS || UNITY_ANDROID
      var changedFramerate = false;
      if (Config.AdaptFramerateForMobilePlatform) {
        if (Application.targetFrameRate < 60) {
          Application.targetFrameRate = 60;
          changedFramerate = true;
        }
      }
#endif

            _animator.Play(HideAnimHash);
            yield return null;
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }

#if UNITY_IOS || UNITY_ANDROID
      if (changedFramerate) {
        new FusionMenuGraphicsSettings().Apply();
      }
#endif

            gameObject.SetActive(false);
        }
    }
}
