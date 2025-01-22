namespace GodotPlayground;

using System;

public interface IAppRepo {
  public event Action? FullscreenToggled;
  public event Action? ApplicationQuit;
  public void ToggleFullscreen();
  public void QuitApplication();
}

public partial class AppRepo : IAppRepo {
  public event Action? FullscreenToggled;
  public event Action? ApplicationQuit;

  public void ToggleFullscreen() => FullscreenToggled?.Invoke();
  public void QuitApplication() => ApplicationQuit?.Invoke();
}
