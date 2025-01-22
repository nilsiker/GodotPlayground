namespace GodotPlayground;

using System;

public interface IBlackoutRepo : IDisposable {
  public event Action<float>? FadeOutRequested;
  public event Action<float>? FadeInRequested;
  public event Action? FadeOutFinished;
  public void RequestFadeOut(float time);
  public void RequestFadeIn(float time);
  public void OnFadeOutFinished();
}

public class BlackoutRepo : IBlackoutRepo {
  public event Action<float>? FadeOutRequested;
  public event Action<float>? FadeInRequested;
  public event Action? FadeOutFinished;

  public void RequestFadeOut(float time) => FadeOutRequested?.Invoke(time);
  public void RequestFadeIn(float time) => FadeInRequested?.Invoke(time);
  public void OnFadeOutFinished() => FadeOutFinished?.Invoke();

  public void Dispose() {
    FadeOutRequested = null;
    FadeInRequested = null;

    GC.SuppressFinalize(this);
  }

}
