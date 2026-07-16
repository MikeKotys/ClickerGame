# Clicker Game (Addressables + Zenject)

## Description
Test task: a game scene with an object whose textures are loaded asynchronously via Addressables. 

## Tech Stack
* Unity 6000.4.10f1
* Addressables (configured for remote loading)
* Zenject / Extenject (Dependency Injection)
* Input System

## Features
* **Architecture:** Based on SOLID principles. Game logic (GameController) is strictly separated from visual representation (MainDisplay), asset management (AssetManager), and user input (InputManager). Dependencies are wired via Zenject (GameInstaller).
* **Addressables & CDN:** Game textures are packed into a separate remote group. The resource catalog and asset bundles are downloaded over HTTPS (hosted on GitHub Pages). Source textures remain in the project for structural demonstration.
* **Asynchronous Operations:** Fully non-blocking loading using `async/await`. Implemented operation cancellation via `CancellationToken` if the player triggers a new round before the current download completes.
* **Memory Safety:** Strict Addressables lifecycle. All `AsyncOperationHandle` instances are correctly released on teardown to prevent memory leaks.

## How to Run
1. Clone the repository.
2. Open the project in Unity.
3. Open the scene Assets/Scenes/Main.unity.
4. Open the `Addressables Groups` window (Window -> Asset Management -> Addressables -> Groups) and ensure the `Play Mode Script` dropdown is set to **Use Existing Build**. This is required to fetch textures from the actual remote CDN.
5. Enter Play Mode.