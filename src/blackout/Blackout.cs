namespace GodotPlayground;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;

public interface IBlackout : IColorRect {
  public void FadeOut(float time);
  public void FadeIn(float time);
}

[Meta(typeof(IAutoNode))]
public partial class Blackout : ColorRect, IBlackout {
  private Tween? _tween;

  #region State
  public IBlackoutRepo BlackoutRepo { get; set; } = default!;
  #endregion

  public override void _Ready() {
    BlackoutRepo = new BlackoutRepo();
    BlackoutRepo.FadeInRequested += FadeIn;
    BlackoutRepo.FadeOutRequested += FadeOut;
    this.Provide();
  }

  public void FadeOut(float time) {
    if (_tween is not null && _tween.IsRunning()) {
      _tween.Kill();
    }
    Visible = true;
    var color = Color;
    color.A = 0;
    Color = color;  // FIXME probably wont be needing this, but I'm feeling like it right now!
    _tween = CreateTween();
    _tween.TweenProperty(this, "color:a", 1.0, time);
    _tween.TweenCallback(Callable.From(BlackoutRepo.OnFadeOutFinished));
  }

  public void FadeIn(float time) {
    if (_tween is not null && _tween.IsRunning()) {
      _tween.Kill();
    }

    _tween = CreateTween();
    _tween.TweenProperty(this, "color:a", 0.0, time);
    _tween.TweenCallback(Callable.From(() => Visible = false));
  }
}
