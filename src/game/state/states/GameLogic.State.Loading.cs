namespace GodotPlayground;

using Chickensoft.LogicBlocks;

public partial class GameLogic {
  public partial record State {
    public partial record Loading : State, IGet<Input.RequestedSceneLoaded>, IGet<Input.SceneReady> {
      public Loading() {
        this.OnEnter(() => {
          var scene = Get<Data>().SceneToLoadPath;
          if (scene is not null) {
            Output(new Output.RequestScene(scene));
          }
        });
      }

      // TODO consider if this could be its own repo? Probably lives nicely in Game, though.
      public Transition On(in Input.RequestedSceneLoaded input) {
        Output(new Output.ChangeScene(input.ScenePath));
        return ToSelf();
      }

      public Transition On(in Input.SceneReady input) => To<InGame>();
    }
  }
}
