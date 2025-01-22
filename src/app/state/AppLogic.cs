namespace GodotPlayground;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public interface IAppLogic : ILogicBlock<AppLogic.State>;

[Meta]
[LogicBlock(typeof(State), Diagram = true)]
public partial class AppLogic
  : LogicBlock<AppLogic.State>,
    IAppLogic {
  public override Transition GetInitialState() => To<State>();

  public static class Input {
    public record struct RequestFullscreenToggle;
    public record struct RequestApplicationQuit;
  }

  public static class Output {
    public record struct ToggleFullscreen;
    public record struct QuitApplication;
  }

  public partial record State : StateLogic<State>, IGet<Input.RequestFullscreenToggle>, IGet<Input.RequestApplicationQuit> {
    public State() {
      OnAttach(() => Get<IAppRepo>().FullscreenToggled += OnAppFullscreenToggled);
      OnDetach(() => Get<IAppRepo>().FullscreenToggled -= OnAppFullscreenToggled);
    }

    private void OnAppFullscreenToggled() => Output(new Output.ToggleFullscreen());

    public Transition On(in Input.RequestFullscreenToggle input) {
      Output(new Output.ToggleFullscreen());
      return ToSelf();
    }

    public Transition On(in Input.RequestApplicationQuit input) {
      Output(new Output.QuitApplication());
      return ToSelf();
    }
  }
}
