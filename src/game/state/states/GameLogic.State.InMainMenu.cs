namespace GodotPlayground;

using Chickensoft.LogicBlocks;

public partial class GameLogic {
  public partial record State {
    public partial record InMainMenu : State {
      public InMainMenu() {
        this.OnEnter(() => {

        });
      }
    }
  }
}
