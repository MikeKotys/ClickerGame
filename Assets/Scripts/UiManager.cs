using UnityEngine;
using TMPro;

namespace ClickerGame
{
	public class UiManager : MonoBehaviour
	{
		public TextMeshProUGUI StatusText;
		public TextMeshProUGUI ScoreText;

		public void UpdateStatus(string message)
		{
			StatusText.text = message;
		}

		public void UpdateScore(int score)
		{
			ScoreText.text = $"Score: {score}";
		}
	}
}