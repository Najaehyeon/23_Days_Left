namespace DaysLeft.Menu
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The popup screen handles notificaction.
    /// The screen has be <see cref="FusionMenuUIScreen.IsModal"/> true.
    /// </summary>
    public class UIMessagePopup : UIScreen, IPopup
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
        public virtual void OpenPopup<T>(T data)
        {
            if (data is not TooltipData tooltipData)
                return;

            _header.text = tooltipData.header;
            _text.text = tooltipData.msg;

            Show();
        }
    }

    public class TooltipData
    {
        public string msg;
        public string header;

        public TooltipData(string msg, string header)
        {
            this.msg = msg;
            this.header = header;
        }
    }
}