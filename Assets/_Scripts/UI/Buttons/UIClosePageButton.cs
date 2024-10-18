using UnityEngine;

namespace UI.Buttons
{
	public class UIClosePageButton : UIButton
	{
		[SerializeField]
		protected GameObject pageToClose;

		protected override void DoOnClick()
		{
			base.DoOnClick();
			pageToClose.SetActive(false);
		}
	}
}
