using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace X23
{			  [ExecuteInEditMode]
public class PlayerActionBar1 : MonoBehaviour
{
		
		public KeyCode KeyMoveActiveLeft = KeyCode.X;
		public KeyCode KeyMoveActiveRight = KeyCode.C;
		public KeyCode KeyActivateSelectedPrimary = KeyCode.E;
		public KeyCode KeyActivateSelectedSecondary = KeyCode.R;
		public KeyCode KeyActivateSelectedTertiary = KeyCode.F;
		public KeyCode KeyDropSelected = KeyCode.G;
		public Color ColorActiveBorder = Color.yellow;
		public Color ColorActiveBackground = Color.cyan;
		public GameObject _SlotPrefab;
		public UnityEngine.UI.GridLayoutGroup _ActionBarGridDisplay;
		public List<ActionBarSlot> _lAllSlots;
		public int _iBarSize = 10;
		public int _iActiveDefault = 0;
		public bool _SlotsSpawned = false;
		[SerializeField] int _iSlotActive;
		[SerializeField] int _iSlotPrevious;
		[SerializeField] float _timer;
		//[InspectorButton1("MoveSelectLeft")] public string btnDoMoveSelectLeft;
		//[InspectorButton1("MoveSelectRight")] public string btnDoMoveSelectRight;
		
		void Awake()
		{
			_timer = 0f;
		}

		void OnEnable()
		{
			_ActionBarGridDisplay = GetComponent<UnityEngine.UI.GridLayoutGroup>();
			if (_lAllSlots.Count == 0) _lAllSlots = new List<ActionBarSlot>();
			//_iSlotActive = _iActiveDefault;
			for (int i = 0; i < _iBarSize; i++)
			{
				//var slot = new ActionBarSlot();
				//slot.iParentBarSlotId = i;
				//_lAllSlots.Add(slot);
			}
			if (!_SlotsSpawned)
			{
				for (int i = 0; i < _iBarSize; i++)
				//foreach (var slotDefinition in _lAllSlots)
				{
					GameObject go = Instantiate(_SlotPrefab);
					///go.SetActive(false); 
					go.transform.SetParent(transform);
					ActionBarSlot _slotInstance = go.AddComponent<ActionBarSlot>();
					_slotInstance._ParentActionBar = this;
					_slotInstance.iParentBarSlotId = i;
					go.name = this.name + "_Slot" + i;
					//go.SetActive(true);
				}
				_SlotsSpawned = true;
			}
			SetActiveSlot(_iActiveDefault);
		}

		private void Update()
		{
			_timer += Time.deltaTime;
		//}

		//private void OnGUI()
		//{
			if (Input.GetKeyDown(KeyMoveActiveLeft))
			{
				MoveSelectLeft();
			}
			if (Input.GetKeyDown(KeyMoveActiveRight))
			{
				MoveSelectRight();
			}

			if (Input.GetKeyDown(KeyDropSelected))
			{

			}

			if (Input.GetKeyDown(KeyActivateSelectedPrimary))
			{
				UseActiveSlot(0);
			}
			if (Input.GetKeyDown(KeyActivateSelectedSecondary))
			{
				UseActiveSlot(1);
			}
			if (Input.GetKeyDown(KeyActivateSelectedTertiary))
			{
				UseActiveSlot(2);
			}
		}

		void MoveSelectLeft()
		{
			int newSlotId = (_iSlotActive - 1);
			if (newSlotId == -1) newSlotId = _iBarSize - 1;
			SetActiveSlot(newSlotId);
		}
		void MoveSelectRight()
		{
			int newSlotId = (_iSlotActive + 1);
			if (newSlotId == _iBarSize) newSlotId = 0;
			SetActiveSlot(newSlotId);
		}

		void SetActiveSlot(int slotId)
		{
			if (slotId > -1 && slotId < _iBarSize)
			{
				_iSlotPrevious = _iSlotActive;
				_iSlotActive = slotId;

				_lAllSlots[_iSlotPrevious]._IsActiveInParentBar = false;
				_lAllSlots[_iSlotPrevious].borderImage.color = _lAllSlots[_iSlotPrevious].ColorBorder;
				_lAllSlots[_iSlotPrevious].backgroundImage.color = _lAllSlots[_iSlotPrevious].ColorBackground;

				_lAllSlots[_iSlotActive]._IsActiveInParentBar = true;
				_lAllSlots[_iSlotActive].borderImage.color = ColorActiveBorder;
				_lAllSlots[_iSlotActive].backgroundImage.color = ColorActiveBackground;
			}
		}

		void UseActiveSlot(int abilityId)
		{
			if (_lAllSlots[_iSlotActive]._HasContents)
			{
				_lAllSlots[_iSlotActive].ActivateSlotItem(abilityId);
			}
		}

		void DropActiveSlot()
		{
			if (_lAllSlots[_iSlotActive]._HasContents)
			{
				//_lAllSlots[_iSlotActive].ActivateSlotItem(abilityId);
			}
		}

	}
}
