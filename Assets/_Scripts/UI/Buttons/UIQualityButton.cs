using TMPro;
using UnityEngine;

namespace UI.Buttons
{
	public class UIQualityButton : UIButton
	{
		[SerializeField]
		private TextMeshProUGUI qualityText;
		private string[] qualityNames;

		protected override void Awake()
		{
			base.Awake();
			qualityNames = QualitySettings.names;
			qualityText.text = qualityNames[QualitySettings.GetQualityLevel()];
		}

		protected override void DoOnClick()
		{
			base.DoOnClick();
			QualitySettings.SetQualityLevel((QualitySettings.GetQualityLevel() + 1) % qualityNames.Length);
			qualityText.text = qualityNames[QualitySettings.GetQualityLevel()];
		}
	}
}
