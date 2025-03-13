namespace DaysLeft.Menu
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The popup screen handles notificaction.
    /// The screen has be <see cref="FusionMenuUIScreen.IsModal"/> true.
    /// </summary>
    public partial class UIPopup : UIScreen, IPopup
    {
        /// <summary>
        /// The text field for the message.
        /// </summary>
        [SerializeField] protected TMPro.TMP_Text _text;
        /// The text field for the header.
        /// </summary>
        [SerializeField] protected TMPro.TMP_Text _header;
        /// <summary>
        /// The okay button.
        /// </summary>
        [SerializeField] protected Button _button;

        /// <summary>
        /// Open the screen in overlay mode
        /// </summary>
        /// <param name="msg">Message</param>
        /// <param name="header">Header, can be null</param>
        public virtual void OpenPopup(string msg, string header)
        {
            _header.text = header;
            _text.text = msg;

            Show();
        }
    }
}