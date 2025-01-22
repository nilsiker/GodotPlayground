namespace GodotPlayground;
using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

[Meta]
[LogicBlock(typeof(State), Diagram = true)]
public partial class GameLogic
  : LogicBlock<GameLogic.State>,
    IGameLogic {
  public override Transition GetInitialState() => To<State>();
}
