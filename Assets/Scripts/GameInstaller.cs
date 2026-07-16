using Zenject;

namespace ClickerGame
{
	public class GameInstaller : MonoInstaller
	{
		public UiManager UiManagerInstance;
		public AssetManager AssetManagerInstance;
		public InputManager InputManagerInstance;

		public override void InstallBindings()
		{
			Container.Bind<UiManager>().FromInstance(UiManagerInstance).AsSingle();
			Container.Bind<AssetManager>().FromInstance(AssetManagerInstance).AsSingle();
			Container.Bind<InputManager>().FromInstance(InputManagerInstance).AsSingle();

			Container.BindInterfacesAndSelfTo<TextureLoader>().AsSingle();
			Container.BindInterfacesAndSelfTo<GameController>().AsSingle().NonLazy();
		}
	}
}