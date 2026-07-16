using System.Threading.Tasks;
using UnityEngine;

namespace ClickerGame
{
	public class MainDisplay : MonoBehaviour
	{
		private Renderer ObjectRenderer;
		private bool IsFlashing;
		private Color OriginalColor;

		private void Awake()
		{
			ObjectRenderer = GetComponent<Renderer>();
		}

		public void SetTexture(Texture2D texture)
		{
			if (ObjectRenderer != null && texture != null)
			{
				ObjectRenderer.material.mainTexture = texture;
			}
		}

		public async Task FlashRedAsync()
		{
			if (ObjectRenderer == null || IsFlashing)
			{
				return;
			}

			IsFlashing = true;
			OriginalColor = ObjectRenderer.material.color;
			ObjectRenderer.material.color = Color.red;

			await Task.Delay(200);

			StopFlashingAndResetColor();
		}

		private void StopFlashingAndResetColor()
		{
			if (ObjectRenderer != null && IsFlashing)
			{
				ObjectRenderer.material.color = OriginalColor;
			}

			IsFlashing = false;
		}
	}
}