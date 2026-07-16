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

### Option 1: Evaluating Logic in Editor
1. Clone the repository and open the project in Unity.
2. Open the `Addressables Groups` window (Window -> Asset Management -> Addressables -> Groups).
3. Set the `Play Mode Script` dropdown to **Use Asset Database (fastest)**.
4. Enter Play Mode. 
*Note: `Library` folder was excluded from version control, therefore the "Use Existing Build" mode will require a new local build. Doing so generates new local hashes that will mismatch the provided CDN files, resulting in 404 errors in the Editor. Use the "Asset Database" mode to evaluate async logic, cancellation tokens, and Zenject architecture.*

### Option 2: Evaluating Remote CDN Delivery (Real Network Test)
The project is fully configured with a live GitHub Pages CDN. To see the actual network download in action without Editor cache interference:
1. Open **File -> Build Settings** and click **Build** (Standalone Windows).
2. Run the executable. The built player uses the pre-configured `StreamingAssets` catalog to successfully fetch asset bundles from the remote server.