using UnityEngine;
using UnityEngine.Networking;

namespace Xen23.Core
{
    /// <summary>
    /// Implementation of IBackendService for communicating with the XenTek master server.
    /// Uses UnityWebRequest for HTTP requests and XenTekMenusSO for settings.
    /// </summary>
    public class BackendService : MonoBehaviour, IBackendService
    {
        private string serverUrl => Xen23ConfigSO.Instance?.masterServerUrl ?? "http://localhost:8080";
        private string apiKey => Xen23ConfigSO.Instance?.masterServerApiKey ?? "";

        public void CheckForUpdates(string projectName, System.Action<string> onComplete)
        {
            StartCoroutine(SendRequest($"/api/updates?project={projectName}", onComplete));
        }

        public void AuthenticateUser(string username, string password, System.Action<bool> onComplete)
        {
            // Placeholder: Implement actual authentication logic
            StartCoroutine(SendRequest("/api/auth", result => onComplete?.Invoke(result == "success")));
        }

        private System.Collections.IEnumerator SendRequest(string endpoint, System.Action<string> onComplete)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(serverUrl + endpoint))
            {
                if (!string.IsNullOrEmpty(apiKey))
                {
                    request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
                }

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    if (Xen23ConfigSO.Instance != null && Xen23ConfigSO.Instance.enableVerboseLogging)
                    {
                        Debug.Log($"Server request successful: {request.downloadHandler.text}");
                    }
                    onComplete?.Invoke(request.downloadHandler.text);
                }
                else
                {
                    Debug.LogError($"Server request failed: {request.error}");
                    onComplete?.Invoke(null);
                }
            }
        }
    }
}