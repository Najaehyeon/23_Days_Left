namespace DaysLeft.Menu
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Device;

    /// <summary>
    /// All screens are registed by this controller in the <see cref="_screens"/> list.
    /// Every screen gets a reference of this controller, the assigned <see cref="_config"/> and <see cref="_connection"/> wrapper.
    /// The first screen in the <see cref="_screens"/> list is the screen that is shown on app start.
    /// Controller is used to progress from one screen to another <see cref="Show{S}()"/>.
    /// E.g. Show&lt;FusionMenuUILoading&gt;().
    /// When deriving a screen the base type will still be functionally to use for Get() and Show(). But only the derived type or the base are useable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UIController : MonoBehaviour
    {
        /// <summary>
        /// The list of screens. The first one is the default screen shown on start.
        /// </summary>
        [SerializeField] protected UIScreen[] _screens;
        /// <summary>
        /// A type to screen lookup to support <see cref="Get{S}()"/>
        /// </summary>
        protected Dictionary<Type, UIScreen> _screenLookup;
        /// <summary>
        /// The current active screen.
        /// </summary>
        protected UIScreen _activeScreen;

        protected IPopup _popupHandler;

        /// <summary>
        /// Unity awake method. Populates internal structures based on the <see cref="_screens"/> list.
        /// </summary>
        protected virtual void Awake()
        {
            _screenLookup = new Dictionary<Type, UIScreen>();

            foreach (var screen in _screens)
            {
                screen.Controller = this;

                var t = screen.GetType();
                while (true)
                {
                    _screenLookup.Add(t, screen);
                    if (t.BaseType == null || typeof(UIScreen).IsAssignableFrom(t) == false || t.BaseType == typeof(UIScreen))
                    {
                        break;
                    }

                    t = t.BaseType;
                }

                if (typeof(IPopup).IsAssignableFrom(t))
                {
                    _popupHandler = (IPopup)screen;
                }
            }

            foreach (var screen in _screens)
            {
                screen.Init();
            }
        }

        /// <summary>
        /// The Unity start method to enable the default screen.
        /// </summary>
        protected virtual void Start()
        {
            if (_screens != null && _screens.Length > 0)
            {
                // First screen is displayed by default
                _screens[0].Show();
                _activeScreen = _screens[0];
            }
        }

        public void ShowMain()
        {
            if (_screens != null && _screens.Length > 0)
            {
               // First screen is displayed by default
                _screens[0].Show();
                _activeScreen = _screens[0];
            }
        }

        /// <summary>
        /// Show a sreen will automaticall disable the current active screen and call animations.
        /// </summary>
        /// <typeparam name="S">Screen type</typeparam>
        public virtual void Show<S>() where S : UIScreen
        {
            if (_screenLookup.TryGetValue(typeof(S), out var result))
            {
                if (result.IsModal == false && _activeScreen != result && _activeScreen)
                {
                    _activeScreen.Hide();
                }
                if (_activeScreen != result)
                {
                    result.Show();
                }
                if (result.IsModal == false)
                {
                    _activeScreen = result;
                }
            }
            else
            {
                Debug.LogError($"Show() - Screen type '{typeof(S).Name}' not found");
            }
        }

        public void Hide()
        {
            if (_activeScreen != null)
            {
                _activeScreen.Hide();
                _activeScreen = null;
            }

        }

        /// <summary>
        /// Get a screen based on type.
        /// </summary>
        /// <typeparam name="S">Screen type</typeparam>
        /// <returns>Screen object</returns>
        public virtual S Get<S>() where S : UIScreen
        {
            if (_screenLookup.TryGetValue(typeof(S), out var result))
            {
                return result as S;
            }
            else
            {
                Debug.LogError($"Show() - Screen type '{typeof(S).Name}' not found");
                return null;
            }
        }

        /// <summary>
        /// Show the popup/notification.
        /// </summary>
        /// <param name="msg">Popup message</param>
        /// <param name="header">Popup header</param>
        public void Popup(string msg, string header = default)
        {
            if (_popupHandler == null)
            {
                Debug.LogError("Popup() - no popup handler found");
            }
            else
            {
                TooltipData tooltipData = new(msg, header);
                _popupHandler.OpenPopup(tooltipData);
            }
        }
    }
}