namespace DaysLeft.Menu
{
    public class ViewUISetting : UIScreen
    {
        public void OnBackButtonPressed()
        {
            Controller.Show<ViewUIMain>();
        }
    }
}
