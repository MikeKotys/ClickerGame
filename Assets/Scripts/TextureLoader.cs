using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace ClickerGame
{
	public class TextureLoader : IDisposable
	{
		private AssetManager AssetManager;
		private UiManager UiManager;
		private MainDisplay Display;

		private CancellationTokenSource DownloadCts;
		private AsyncOperationHandle<Texture2D> CurrentTextureHandle;

		public TextureLoader(AssetManager assetManager, UiManager uiManager)
		{
			AssetManager = assetManager;
			UiManager = uiManager;
		}

		public void SetDisplay(MainDisplay display)
		{
			Display = display;
		}

		public async Task LoadAndApplyAsync(IResourceLocation location)
		{
			if (DownloadCts != null)
			{
				DownloadCts.Cancel();
				DownloadCts.Dispose();
			}

			DownloadCts = new CancellationTokenSource();
			CancellationToken token = DownloadCts.Token;

			UiManager.UpdateStatus("Loading image...");

			AsyncOperationHandle<Texture2D> handle = Addressables.LoadAssetAsync<Texture2D>(location);

			try
			{
				await handle.Task;

				if (token.IsCancellationRequested)
				{
					Addressables.Release(handle);
					return;
				}

				if (handle.Status == AsyncOperationStatus.Succeeded)
				{
					if (CurrentTextureHandle.IsValid())
					{
						Addressables.Release(CurrentTextureHandle);
					}

					CurrentTextureHandle = handle;
					Display.SetTexture(handle.Result);
				}
				else
				{
					ApplyFallback(handle);
				}
			}
			catch
			{
				ApplyFallback(handle);
			}

			if (!token.IsCancellationRequested)
			{
				UiManager.UpdateStatus("Tap the object!");
			}
		}

		private void ApplyFallback(AsyncOperationHandle failedHandle)
		{
			if (failedHandle.IsValid())
			{
				Addressables.Release(failedHandle);
			}

			Display.SetTexture(AssetManager.FallbackTexture);
		}

		public void Dispose()
		{
			if (DownloadCts != null)
			{
				DownloadCts.Cancel();
				DownloadCts.Dispose();
			}

			if (CurrentTextureHandle.IsValid())
			{
				Addressables.Release(CurrentTextureHandle);
			}
		}
	}
}