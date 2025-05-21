using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HUD_Controller : MonoBehaviour
{
    RectTransform _rect;
    Canvas _canvas;
    CanvasGroup _group;
    Image _background;
    EventSystem _eventsys;
    public GameObject MainPlayDisplayGO;    // Root Canvas for in-game Head-Up-Display.
    public GameObject AvatarSelectCreateGO;  // Root Canvas for avatar select / create screen.
    public GameObject ServerSelectListGO;  // Root Canvas for server select / connect screen.

    [SerializeField] Vector3 _cursorLocation;
    [SerializeField] GameObject _currentlySelectedGO;

    void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _canvas = GetComponent<Canvas>();
        _group = GetComponent<CanvasGroup>();
        _background = GetComponent<Image>();
        _eventsys = GameObject.FindObjectOfType<EventSystem>();
    }

    public void OpenPanel(int id, bool force)
    {
    }

    void Update()
    {
        _cursorLocation = Input.mousePosition;
        _currentlySelectedGO = _eventsys.currentSelectedGameObject;
    }

    void OnEnable()
    {
        Debug.Log("HUD Enabled.");
    }

    void OnDisable()
    {
        Debug.Log("HUD Disabled");
    }

    //public GameObject particle;
    void XUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//            if (Physics.Raycast(ray))
//                Instantiate(particle, transform.position, transform.rotation);
        }
    }

}
