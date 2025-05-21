using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HUD_MainPlayDisplay : MonoBehaviour
{
    RectTransform _rect;
    Canvas _canvas;
    CanvasGroup _group;
    Animator _animator;
    Image _background;
    EventSystem _eventsys;

    [Header("Component Values")]
    public string chr_Name;
    public string chr_Location;
    public string chr_LevelXP;
    public Image chr_Portrait;
    public float com_Direction;
    public List<Vector2> com_Icons;
    public Vector3 mim_Location;
    public float mim_Direction;
    public int mim_Zoom;
    public GameObject tar_GameObject;
    public string tar_Name;
    public float tar_Health;
    public Image tar_Portrait;
    public string sco_Score;
    public string sco_Cheese;
    public string sco_Glory;
    public Image crh_ImageBackground;
    public Image crh_ImageCentre;
    public Color crh_ImageCentreColor;

    [Header("ScreenOverlayGameObjects")]
    public GameObject AvatarOverlayGO;  // Root Canvas for in-game avatar stat / config screen.
    public GameObject BallOverlayGO;  // Root Canvas for battleball stat / config screen.
    public GameObject InventoryOverlayGO;    // Root Canvas for in-game inventory items list.

    [SerializeField] Vector3 _cursorLocation;
    [SerializeField] GameObject _currentlySelectedGO;

    Animator[] _childAnimators;

    void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _canvas = GetComponent<Canvas>();
        _group = GetComponent<CanvasGroup>();
        _background = GetComponent<Image>();
        _animator = GetComponent<Animator>();
        _eventsys = GameObject.FindObjectOfType<EventSystem>();
        _childAnimators = GetComponentsInChildren<Animator>();
        foreach(var anim in _childAnimators)
        {
           anim.SetTrigger("Start");
        }
    }
    void Update()
    {
        _cursorLocation = Input.mousePosition;
        _currentlySelectedGO = _eventsys.currentSelectedGameObject;
    }

    void OnEnable()
    {
        foreach (var anim in _childAnimators)
        {
            anim.SetTrigger("Normal");
        }
    }

}
