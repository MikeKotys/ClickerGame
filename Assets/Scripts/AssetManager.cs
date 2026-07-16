using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace ClickerGame
{
	public class AssetManager : MonoBehaviour
	{
		public AssetReference DisplayPrefabRef;
		public AssetReference FallbackTextureRef;
		public AssetLabelReference GameImagesLabel;

		public Texture2D FallbackTexture { get; private set; }
		public List<IResourceLocation> AvailableLocations { get; private set; } = new List<IResourceLocation>();

		private AsyncOperationHandle<Texture2D> FallbackHandle;
		private AsyncOperationHandle<IList<IResourceLocation>> LocationsHandle;
		private AsyncOperationHandle<GameObject> PrefabHandle;
		private GameObject DisplayInstance;

		public async Task InitializeAsync()
		{
			FallbackHandle = Addressables.LoadAssetAsync<Texture2D>(FallbackTextureRef);
			FallbackTexture = await FallbackHandle.Task;

			LocationsHandle = Addressables.LoadResourceLocationsAsync(GameImagesLabel);
			IList<IResourceLocation> locations = await LocationsHandle.Task;
			AvailableLocations.AddRange(locations);
		}

		public async Task<GameObject> InstantiateDisplayAsync()
		{
			var handle = Addressables.InstantiateAsync(DisplayPrefabRef);
			DisplayInstance = await handle.Task;
			PrefabHandle = handle;
			return DisplayInstance;
		}

		public void ReleaseResources()
		{
			if (DisplayInstance != null)
			{
				Addressables.ReleaseInstance(DisplayInstance);
				DisplayInstance = null;
			}

			if (PrefabHandle.IsValid())
			{
				Addressables.Release(PrefabHandle);
			}

			if (FallbackHandle.IsValid())
			{
				Addressables.Release(FallbackHandle);
			}
			if (LocationsHandle.IsValid())
			{
				Addressables.Release(LocationsHandle);
			}
		}

		private void OnDestroy()
		{
			ReleaseResources();
		}
	}
}