using System;
using Rimaethon.Runtime.UI;
using UnityEngine;

namespace _Scripts.UI
{
	public class UIClosePageButton:UIButton
	{
		[SerializeField] protected GameObject pageToClose;
		protected override void DoOnClick()
		{
			base.DoOnClick();
			pageToClose.SetActive(false);
		}
	}
}
