using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Buttons
{
	public class UIButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
	{
		[HideInInspector]
		public Button Button;
		[HideInInspector]
		public RectTransform RectTransform;
		protected Tween ScaleDownTween;
		protected Tween ScaleUpTween;
		protected Vector3 OriginalScale => Vector3.one;

		protected virtual void Awake()
		{
			Button = GetComponent<Button>();
			RectTransform = GetComponent<RectTransform>();
		}

		protected virtual void OnEnable()
		{
			transform.localScale = OriginalScale;
		}

		protected virtual void OnDisable()
		{
			ScaleDownTween.Kill();
			ScaleUpTween.Kill();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (!Button.isActiveAndEnabled)
			{
				return;
			}

			DoOnClick();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (!Button.isActiveAndEnabled)
			{
				return;
			}

			DoOnPointerDown();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (!Button.isActiveAndEnabled)
			{
				return;
			}

			DoOnPointerUp();
		}

		protected virtual void DoOnClick()
		{
			ScaleDownTween.Kill();
			ScaleUpTween.Kill();

			//				AudioManager.Instance.PlaySFX(SFXClips.UIButtonSound);
			RectTransform.localScale = OriginalScale;
		}

		protected virtual void DoOnPointerDown()
		{
			ScaleDownTween = RectTransform.DOScale(OriginalScale * 0.95f, 0.075f).SetUpdate(UpdateType.Fixed);
		}

		protected virtual void DoOnPointerUp()
		{
			ScaleUpTween = RectTransform.DOScale(OriginalScale, 0.075f).SetUpdate(UpdateType.Fixed);
		}
	}
}
