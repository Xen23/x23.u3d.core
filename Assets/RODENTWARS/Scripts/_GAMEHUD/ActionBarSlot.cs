using UnityEngine;
namespace X23
{
	// Spawned by an PlayerActionBar1
	[ExecuteInEditMode]
	public class ActionBarSlot : MonoBehaviour
	{
		public PlayerActionBar1 _ParentActionBar;
		/*
		public ActionBarSlot(PlayerActionBar1 ParentActionBar, int SlotId)
		{
			_ParentActionBar = ParentActionBar;
			iParentBarSlotId = SlotId;
		}
		public ActionBarSlot(PlayerActionBar1 ParentActionBar, int SlotId, Color BackgroundColor)
		{
			_ParentActionBar = ParentActionBar;
			iParentBarSlotId = SlotId;
			ColorBackground = BackgroundColor;
		}
		public ActionBarSlot(PlayerActionBar1 ParentActionBar, int SlotId, Color BackgroundColor, Color BorderColor)
		{
			_ParentActionBar = ParentActionBar;
			iParentBarSlotId = SlotId;
			ColorBackground = BackgroundColor;
			ColorBorder = BorderColor;
		}
		*/
		public int iParentBarSlotId;
		public GameObject goContents = null;
		public bool _HasContents = false;
		public bool _Initialised = false;
		public UnityEngine.UI.Image borderImage;
		public UnityEngine.UI.Image backgroundImage;
		public UnityEngine.UI.Button buttonInfo;
		public UnityEngine.UI.Image buttonIcon;
		public Color ColorBorder = Color.black;
		public Color ColorBackground = Color.grey;
		public bool _IsActiveInParentBar;
		private void OnEnable()
		{
			if (_ParentActionBar == null) _ParentActionBar = GetComponentInParent<PlayerActionBar1>();
			if (_Initialised) return;
			borderImage = transform.Find("border_image").GetComponent<UnityEngine.UI.Image>();
			backgroundImage = borderImage.transform.Find("background_image").GetComponent<UnityEngine.UI.Image>();
			Transform trContents = backgroundImage.transform.Find("_Contents");
			if (!(trContents == null)) goContents = trContents.gameObject;
			buttonInfo = trContents.transform.Find("btn").GetComponent<UnityEngine.UI.Button>();
			buttonIcon = buttonInfo.transform.Find("icon").GetComponent<UnityEngine.UI.Image>();
			if (goContents != null && borderImage != null && backgroundImage != null) _Initialised = true;
			if (_Initialised)
			{
				borderImage.color = ColorBorder;
				backgroundImage.color = ColorBackground;
				if (_ParentActionBar != null) _ParentActionBar._lAllSlots.Insert(iParentBarSlotId, this);
			}
		}

		public void ContentsGet()
		{
		}

		public void ContentsSet()
		{
		}

		public void ContentsClear()
		{
		}
		public void ActivateSlotItem(int ability)
		{
			if (ability == 0)
			{
				// Primary
			}
			else if (ability == 1)
			{ 
				// Secondary
			}
			else if (ability == 2)
			{
				// Tertiary
			}
			else if (ability == 3)
			{
				// Eventually..
			}
		}
	}
}
