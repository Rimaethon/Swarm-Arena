using Data;
using Managers;
using Rimaethon.Runtime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class UISoundButton:UIButton
	{
		[SerializeField] private Sprite soundOnImage;
		[SerializeField] private Sprite soundOffImage;
		[SerializeField] private bool isSFX;
		[SerializeField] private TextMeshProUGUI text;
		[SerializeField] private Image image;
		private SettingsData settingsData;

		protected override void Awake()
		{
			base.Awake();
			settingsData = SaveManager.Instance.GetSettingsData();
			HandleVisual();
		}

		private void HandleVisual()
		{
			bool isOptionOn = isSFX ? settingsData.IsSFXOn : settingsData.IsMusicOn;

			if (text != null)
			{
				text.text = isOptionOn ? "On" : "Off";
			}

			if (image != null)
			{
				image.sprite = isOptionOn ? soundOnImage : soundOffImage;
			}
		}

		protected override void DoOnClick()
		{
			base.DoOnClick();
			if(isSFX)
			{
				settingsData.IsSFXOn = !settingsData.IsSFXOn;
				HandleVisual();
			}
			else
			{
				settingsData.IsMusicOn = !settingsData.IsMusicOn;
				HandleVisual();
			}
			SaveManager.Instance.SetSettingsData(settingsData);
		}
	}
}
