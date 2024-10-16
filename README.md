<div align="center">
  <h1>Love 💖 Live2D</h1>
  <img src="./uwoh.gif" />
  <p>Unlock <a href="https://www.live2d.com/en/" title="Visit Live2D official website">Live2D Cubism Editor</a> Pro ✨</p>

  <p>You want to learn Live2D Cubism with all it's features, but the trial time & the features is limited and you can't yet afford the Pro license?<br/>Don't worry! This tool is build for you.</p>
</div>

## Supported Versions

- Live2D Cubism Editor v5.0+

## Prerequisites

- Install [Live2D Cubism Editor](https://www.live2d.com/en/cubism/download/editor/ "Live2D Cubism Editor")
- Install [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0 ".NET 8.0 SDK")

## How To Use

- Download LoveLive2D at [Releases](https://github.com/kiraio-moe/LoveLive2D/ "Releases") menu. Find your own platform then extract it.

### Windows

- Run `LoveLive2D.exe` (_**Run as Administrator if necessary**_) at the extracted archive.
- Select `CubismEditor{version}.exe` in your Live2D Cubism installation folder, either you want to patch or revoke the license.

### macOS

- Open a Terminal in the extracted folder.
- Add execute permission:

  ```bash
  chmod +x ./LoveLive2D
  ```

- Execute LL2D. (Run as **sudo** if necessary):

  ```bash
  ./LoveLive2D
  sudo ./LoveLive2D (optional)
  ```

- If you're experiencing LL2D keep getting killed by GateKeeper (`zsh: killed     sudo ./LoveLive2D`), run LL2D as `dotnet`:

  ```bash
  sudo dotnet ./LoveLive2D.dll
  ```

> [!NOTE]  
> Alternatively, you can patch Live2D without selecting menu.  
> Replace `<number>` with `1` to **Patch** or `0` to **Revoke** license.
>
> ```bash
> LoveLive2D.exe <absolute_path_to_live2d_executable> <number>
> sudo ./LoveLive2D <absolute_path_to_live2d_executable> <number>
> ```
>
> Example:
>
> ```bash
> LoveLive2D.exe "C:/Program Files/Live2D Cubism 5.1/Live2D Cubism Editor 5.1.exe" 1
> sudo ./LoveLive2D "/Applications/Live2D Cubism 5.1/Live2D Cubism Editor 5.1.app" 1
> ```
>
> ---
> If Live2D Cubism Editor ask you about offline use expiration date (forcing you to do online authentication), just ignore it forever. The patch will always bypass the offline expiration date.

## Disclaimer

If you have financial capability or want to seriously use Live2D Cubism for commercial purposes, it's recommended to buy the genuine license so that the developer can continue developing Live2D Cubism.

This tool is provided for educational and evaluational purposes only. Modifying software without proper authorization may violate terms of service and can lead to legal consequences. Use this tool responsibly and at your own risk.
