using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Buttons
{
	public sealed class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
	{
		[SerializeField]
		private float handleRange = 1;
		[SerializeField]
		private float deadZone;
		[SerializeField]
		private RectTransform rectTransform;
		[SerializeField]
		private RectTransform handleRectTransform;
		private UnityEngine.Camera cam;
		private Canvas canvas;
		private Vector2 input = Vector2.zero;
		public float Horizontal => input.x;
		public float Vertical => input.y;

		private void Start()
		{
			canvas = GetComponentInParent<Canvas>();
		}

		public void OnDrag(PointerEventData eventData)
		{
			Vector2 position = RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
			Vector2 radius = rectTransform.sizeDelta / 2;
			input = (eventData.position - position) / (radius * canvas.scaleFactor);
			HandleInput(input.magnitude, input.normalized);
			handleRectTransform.anchoredPosition = input * radius * handleRange;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			OnDrag(eventData);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			input = Vector2.zero;
			handleRectTransform.anchoredPosition = Vector2.zero;
		}

		private void HandleInput(float magnitude, Vector2 normalised)
		{
			if (magnitude > deadZone)
			{
				if (magnitude > 1)
				{
					input = normalised;
				}
			}
			else
			{
				input = Vector2.zero;
			}
		}
	}
}
