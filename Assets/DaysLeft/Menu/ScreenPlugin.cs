namespace DaysLeft.Menu
{
    using UnityEngine;

    /// <summary>
    /// Screen plugin are usually a UI features that is shared between multiple screens.
    /// The plugin must be registered at <see cref="UIScreen.Plugins"/> and receieve Show and Hide callbacks.
    /// </summary>
    public class ScreenPlugin : MonoBehaviour
    {
        /// <summary>
        /// The parent screen is shown.
        /// </summary>
        /// <param name="screen">Parent screen</param>
        public virtual void Show(UIScreen screen)
        {
        }

        /// <summary>
        /// The parent screen is hidden.
        /// </summary>
        /// <param name="screen">Parent screen</param>
        public virtual void Hide(UIScreen screen)
        {
        }
    }
}
