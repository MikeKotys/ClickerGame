using UnityEngine;
using UnityEngine.InputSystem;

namespace ClickerGame
{
	public class InputManager : MonoBehaviour
	{
		public bool TryGetTapTarget(out GameObject target)
		{
			target = null;

			if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
			{
				if (Camera.main == null)
				{
					return false;
				}

				Vector2 mousePos = Mouse.current.position.ReadValue();
				Ray ray = Camera.main.ScreenPointToRay(mousePos);

				if (Physics.Raycast(ray, out RaycastHit hit))
				{
					target = hit.transform.gameObject;
				}

				return true;
			}

			return false;
		}
	}
}