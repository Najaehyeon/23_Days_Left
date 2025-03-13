namespace DaysLeft.Menu
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The tooltip plugin can be used to add tooltip text to a button.
    /// The <see cref="ScreenPluginToolTip"/> screen will be shown.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ScreenPluginTooltip : ScreenPlugin
    {
        /// <summary>
        /// The header text of the tooltip popup. Can be null.
        /// </summary>
        [SerializeField] protected string _header;
        /// <summary>
        /// The tooltip text.
        /// </summary>
        [SerializeField, TextArea] protected string _tooltip;
        /// <summary>
        /// The button that activates the tooltip popup.
        /// </summary>
        [SerializeField] protected Button _button;

        private UIController _controller;

        /// <summary>
        /// Unity awake method to add the tooltip listener to the button.
        /// </summary>
        public virtual void Awake()
        {
            _button.onClick.AddListener(() => _controller.Popup(_tooltip, _header));
        }

        /// <summary>
        /// The parent screen is shown. Cache the UI controlller.
        /// </summary>
        /// <param name="screen">The parent screen</param>
        public override void Show(UIScreen screen)
        {
            base.Show(screen);

            _controller = screen.Controller;
        }

        /// <summary>
        /// The parent screen is hidden. Clear the cached controller.
        /// </summary>
        /// <param name="screen">Parent screen</param>
        public override void Hide(UIScreen screen)
        {
            base.Hide(screen);

            _controller = null;
        }
    }
}
