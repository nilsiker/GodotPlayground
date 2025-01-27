namespace GodotPlayground;

using System;
using Chickensoft.GodotNodeInterfaces;

public interface ICameraRepo : IDisposable {
  public event Action<float, float, float>? ShakeRequested;
  public event Action<INode2D>? TargetFollowed;
  public void RequestShake(float intensity, float duration, float rate);
  public void FollowTarget(INode2D target);
}

public class CameraRepo : ICameraRepo {
  public event Action<float, float, float>? ShakeRequested;
  public event Action<INode2D>? TargetFollowed;

  public void RequestShake(float intensity, float duration, float rate) =>
    ShakeRequested?.Invoke(intensity, duration, rate);
  public void FollowTarget(INode2D target) =>
    TargetFollowed?.Invoke(target);

  public void Dispose() {
    ShakeRequested = null;
    GC.SuppressFinalize(this);
  }

}
