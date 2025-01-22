namespace GodotPlayground;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;

public interface ITryFadeOutButton : IButton { }

[Meta(typeof(IAutoNode))]
public partial class TryFadeOutButton : Button, ITryFadeOutButton {
  #region Dependencies
  [Dependency] IBlackoutRepo BlackoutRepo => this.DependOn<IBlackoutRepo>();
  #endregion

  #region Dependency Lifecycle
  public void OnResolved() => Pressed += () =>
    BlackoutRepo.RequestFadeOut(3f);

  #endregion

  #region Godot Lifecycle
  public override void _Notification(int what) => this.Notify(what);

  #endregion
}
