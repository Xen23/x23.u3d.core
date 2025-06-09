using UnityEngine;
namespace Xen23.Core
{
    public class XenTekCore : BaseManager
    {
        public override void Initialize()
        {
            //UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
            if (Config != null)
            {
                Log("Found config!");
                IsInitialized = true;
            }
        }

        void Start()
        {
            Initialize();
            
        }

        void Awake()
        {

        }
        void Update()
        {

        }
    }
}
