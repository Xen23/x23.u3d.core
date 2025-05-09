namespace XenTek.Core
{
    /// <summary>
    /// Interface for backend communication in the XenTek framework.
    /// Handles server requests for updates, authentication, and game actions.
    /// </summary>
    public interface IBackendService
    {
        void CheckForUpdates(string projectName, System.Action<string> onComplete);
        void AuthenticateUser(string username, string password, System.Action<bool> onComplete);
    }
}