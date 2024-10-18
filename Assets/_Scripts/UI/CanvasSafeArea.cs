using UnityEngine;

namespace UI
{
	public class CanvasSafeArea : MonoBehaviour
	{
		private Vector2 maxAnchor;
		private Vector2 minAnchor;
		private RectTransform rectTransform;
		private Rect safeArea;

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			safeArea = Screen.safeArea;
			minAnchor = safeArea.position;
			maxAnchor = safeArea.position + safeArea.size;
			minAnchor.x /= Screen.width;
			minAnchor.y = 0;
			maxAnchor.x /= Screen.width;
			maxAnchor.y /= Screen.height;
			maxAnchor.y = Mathf.Max(0.96f, maxAnchor.y);
			rectTransform.anchorMin = minAnchor;
			rectTransform.anchorMax = maxAnchor;
		}
	}
}
