using System.Collections; using System.Collections.Generic; using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// 
/// </summary>
public class WebSendReceiver : MonoBehaviour
{
    WebSendReceiver _instance;
    List<UnityWebRequestAsyncOperation> _MainRequestPool;
    int _poolSize;
    
    [SerializeField] int poolSizeNew = 4;
    [SerializeField] int poolSizeMax = 64;

    public struct RequestBlock
    {
        public string protocol; // http, https, ftp, smtp
        public string host; // xyz.example.x23.uk
        public int port; // 80 / 443 / 8080
        public string type; // CREATE, GET, POST, DELETE, HEAD, PUT
        public string[,] headers;
        public string[,] cookies;
        public string uri;
        public string[,] gets;
        public string[,] posts;
    }
    
    void Start()
    {
        // Only one instance.
        if (_instance == null) _instance = this;
        else gameObject.GetComponent<WebSendReceiver>().enabled = false;
        // Create List
        _MainRequestPool = new List<UnityWebRequestAsyncOperation>();
    }

    void Awake()
    {
        for (int i = 0 ; i != poolSizeNew -1 ; i++)
        {
            _MainRequestPool.Add(new UnityWebRequestAsyncOperation());
        }
    }

    public int CreateRequest(RequestBlock rb)
    {
        string url = rb.protocol + "://" + rb.host + ":" + rb.port.ToString() + rb.uri;
        url = UnityWebRequest.EscapeURL(url);
        //switch 
        return 0;
    }

    public int CheckRequest(int id)
    {

        return 0;
    }


}
