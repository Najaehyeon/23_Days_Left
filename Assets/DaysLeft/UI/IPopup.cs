public interface IPopup
{
    /// <summary>
    /// Open screen with message.
    /// </summary>
    /// <param name="msg">Message</param>
    /// <param name="header">Header</param>
    void OpenPopup<T>(T data);
}
