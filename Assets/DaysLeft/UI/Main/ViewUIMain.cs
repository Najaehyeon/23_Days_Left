namespace DaysLeft.Menu
{
    public class ViewUIMain : UIScreen
    {
        public void OnPressedSettingButton()
        {
            Controller.Show<ViewUISetting>();
        }

        public void OnPressedExitButton()
        {
           
        }

        public void OnPressedPlayButton()
        {
            Global.Instance.SceneLoader.LoadScene(_23DaysLeft.Utils.SceneType.Main);
        }
    }
}
