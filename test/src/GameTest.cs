namespace GodotPlayground;

using System.Threading.Tasks;
using Godot;
using Chickensoft.GoDotTest;
using Chickensoft.GodotTestDriver;

public class GameTest : TestClass {
  private Game _game = default!;
  private Fixture _fixture = default!;

  public GameTest(Node testScene) : base(testScene) { }

  [SetupAll]
  public async Task Setup() {
    _fixture = new Fixture(TestScene.GetTree());
    _game = await _fixture.LoadAndAddScene<Game>();
  }

  [CleanupAll]
  public void Cleanup() => _fixture.Cleanup();

  [Test]
  public void TestButtonUpdatesCounter() {
  }
}
