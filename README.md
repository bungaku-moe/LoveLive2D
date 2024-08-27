> [!NOTE]  
> Please read the entire README, so that no duplicate issues are required.

<div align="center">
  <h1>Love 💖 Live2D</h1>
  <img src="./uwoh.gif" />
  <p>Unlock <a href="https://www.live2d.com/en/" title="Visit Live2D official website">Live2D Cubism</a> Pro ✨</p>

  <p>You want to learn Live2D Cubism with all it's features, but the trial time & the features is limited and you can't afford the Pro license?<br/>Don't worry! This tool is fit for you.</p>
</div>

## Supported Versions

- Live2D Cubism Editor v5.0+

## Prerequisites

- Install [Live2D Cubism Editor](https://www.live2d.com/en/cubism/download/editor/ "Live2D Cubism Editor")
- Install [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0 ".NET 6.0 SDK") or [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0 ".NET 8.0 SDK")

## How To Use

- Download LoveLive2D at [Releases](https://github.com/kiraio-moe/LoveLive2D/ "Releases") menu. Find your own platform then extract it.

### Windows

- Run `LoveLive2D.exe` (_**Run as Administrator if necessary**_) at the extracted archive.
- Select `CubismEditor{version}.exe` in your Live2D Cubism installation folder, either you want to patch or revoke the license.
- Alternatively, you can use Terminal. Replace `<action>` with `1` to **Patch** or `0` to **Revoke** license.

  ```bash
  LoveLive2D.exe <full_path_to_live2d_executable> <action>
  ```

### macOS

- Whitelist/allow `LoveLive2D` in the Developer settings, so it can run.
- Open a Terminal in the extracted folder.
- Add Execute permission:

  ```bash
  chmod +x ./LoveLive2D
  ```

- _Because of a library LoveLive2D used isn't yet implement APIs for macOS, you need to run LoveLive2D using Terminal_. Run with **root** permission.  
  Replace `<action>` with `1` to **Patch** or `0` to **Revoke** license:

  ```bash
  sudo ./LoveLive2D <full_path_to_live2d_executable.app> <action>
  ```

> [!NOTE]  
> If Live2D Cubism Editor ask you about offline use expiration date (forcing you to do online authentication), just ignore it forever. The patch will always bypass the offline expiration date.

## Disclaimer

If you have financial capability or want to seriously use Live2D Cubism for commercial purposes, it's recommended to buy the genuine license so that the developer can continue developing Live2D Cubism.

This tool is provided for educational and evaluational purposes only. Modifying software without proper authorization may violate terms of service and can lead to legal consequences. Use this tool responsibly and at your own risk.
