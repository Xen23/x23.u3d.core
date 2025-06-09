using System;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

namespace XenTek.Core
{
    public class XenTekHttpServer : BaseServer, IBackendService
    {
        private HttpListener listener;
        private Thread listenerThread;
        private bool isRunning;
        private string serverUrl => Config?.masterServerUrl ?? "http://localhost:8080";
        private string apiKey => Config?.masterServerApiKey ?? "";

        private void Awake()
        {
            StartServer();
        }

        private void OnDestroy()
        {
            StopServer();
        }

        public override void StartServer()
        {
            if (Config == null)
            {
                Debug.LogError("XenTekHttpServer failed to start: XenTekConfigSO not found.");
                return;
            }

            try
            {
                listener = new HttpListener();
                listener.Prefixes.Add(serverUrl + "/");
                listener.Start();
                isRunning = true;

                listenerThread = new Thread(ListenForRequests);
                listenerThread.IsBackground = true;
                listenerThread.Start();

                Log($"Server started at {serverUrl}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to start server: {e.Message}");
            }
        }

        public override void StopServer()
        {
            isRunning = false;
            listener?.Stop();
            listener?.Close();
            listenerThread?.Join(1000); // Wait for thread to close
            Log("Server stopped.");
        }

        private void ListenForRequests()
        {
            while (isRunning)
            {
                try
                {
                    var context = listener.GetContext(); // Blocks until a request is received
                    ProcessRequest(context);
                }
                catch (Exception e)
                {
                    if (isRunning) Debug.LogError($"Error processing request: {e.Message}");
                }
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            // Validate API key if provided
            if (!string.IsNullOrEmpty(apiKey))
            {
                string authHeader = request.Headers["Authorization"];
                if (authHeader != $"Bearer {apiKey}")
                {
                    SendResponse(response, 401, "Unauthorized: Invalid API key");
                    return;
                }
            }

            // Handle endpoints
            string responseString = "";
            string path = request.Url.AbsolutePath;

            if (path.StartsWith("/api/updates"))
            {
                string projectName = request.QueryString["project"];
                responseString = HandleCheckForUpdates(projectName);
            }
            else if (path == "/api/auth")
            {
                // For simplicity, assume credentials are sent as query parameters (not secure for production!)
                string username = request.QueryString["username"];
                string password = request.QueryString["password"];
                responseString = HandleAuthenticateUser(username, password);
            }
            else
            {
                SendResponse(response, 404, "Endpoint not found");
                return;
            }

            SendResponse(response, 200, responseString);
        }

        private void SendResponse(HttpListenerResponse response, int statusCode, string responseString)
        {
            response.StatusCode = statusCode;
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }

        // IBackendService implementation
        public void CheckForUpdates(string projectName, System.Action<string> onComplete)
        {
            string result = HandleCheckForUpdates(projectName);
            onComplete?.Invoke(result);
        }

        public void AuthenticateUser(string username, string password, System.Action<bool> onComplete)
        {
            string result = HandleAuthenticateUser(username, password);
            onComplete?.Invoke(result == "success");
        }

        private string HandleCheckForUpdates(string projectName)
        {
            // Placeholder: Implement your update logic here
            Log($"Checking updates for project: {projectName}");
            return $"Update info for {projectName}";
        }

        private string HandleAuthenticateUser(string username, string password)
        {
            // Placeholder: Implement your authentication logic here
            Log($"Authenticating user: {username}");
            return (username == "admin" && password == "password") ? "success" : "failure";
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}