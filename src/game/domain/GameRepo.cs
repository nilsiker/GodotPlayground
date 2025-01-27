namespace GodotPlayground;

using System;

public interface IGameRepo : IDisposable {
}
public partial class GameRepo : IGameRepo {


  public void Dispose() => GC.SuppressFinalize(this);

}
