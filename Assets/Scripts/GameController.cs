using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;
using Zenject;

namespace ClickerGame
{
	public class GameController : Zenject.IInitializable, ITickable, IDisposable
	{
		private UiManager UiManager;
		private AssetManager AssetManager;
		private InputManager InputManager;
		private TextureLoader TextureLoader;

		private MainDisplay SpawnedDisplay;
		private int Score;
		private IResourceLocation CurrentLocation;

		public GameController(
			UiManager uiManager,
			AssetManager assetManager,
			InputManager inputManager,
			TextureLoader textureLoader)
		{
			UiManager = uiManager;
			AssetManager = assetManager;
			InputManager = inputManager;
			TextureLoader = textureLoader;
		}

		public async void Initialize()
		{
			UiManager.UpdateStatus("Initializing...");
			await AssetManager.InitializeAsync();

			GameObject spawnedObj = await AssetManager.InstantiateDisplayAsync();
			SpawnedDisplay = spawnedObj.AddComponent<MainDisplay>();

			TextureLoader.SetDisplay(SpawnedDisplay);

			StartRound();
		}

		private void StartRound()
		{
			if (AssetManager.AvailableLocations.Count == 0)
			{
				return;
			}

			IResourceLocation randomLocation;

			if (AssetManager.AvailableLocations.Count > 1)
			{
				do
				{
					int index = UnityEngine.Random.Range(0, AssetManager.AvailableLocations.Count);
					randomLocation = AssetManager.AvailableLocations[index];
				}
				while (randomLocation == CurrentLocation);
			}
			else
			{
				randomLocation = AssetManager.AvailableLocations[0];
			}

			CurrentLocation = randomLocation;
			Task task = TextureLoader.LoadAndApplyAsync(randomLocation);
		}

		public void Tick()
		{
			if (InputManager.TryGetTapTarget(out GameObject target))
			{
				if (SpawnedDisplay != null && target == SpawnedDisplay.gameObject)
				{
					HandleCorrectTap();
				}
				else
				{
					HandleMiss();
				}
			}
		}

		private void HandleCorrectTap()
		{
			Score++;
			UiManager.UpdateScore(Score);
			StartRound();
		}

		private void HandleMiss()
		{
			if (SpawnedDisplay != null)
			{
				Task task = SpawnedDisplay.FlashRedAsync();
			}
		}

		public void Dispose()
		{
			AssetManager.ReleaseResources();
		}
	}
}