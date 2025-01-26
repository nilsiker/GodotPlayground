namespace GodotPlayground;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;

[Meta(typeof(IAutoNode)), Tool]
public partial class Fog : TextureRect {
  [Export] private float _ripplingSpeed = 1f;
  [Export] private Vector2 _wind = Vector2.Zero;

  private Vector3 _playerOffset = Vector3.Zero;
  private Vector3 _timeBasedOffset = Vector3.Zero;

  public override void _Process(double delta) {
    var noiseTexture = (NoiseTexture2D)Texture;
    var noise = (FastNoiseLite)noiseTexture.Noise;

    _timeBasedOffset.X = (float)delta * _wind.X;
    _timeBasedOffset.Y = (float)delta * _wind.Y;
    _timeBasedOffset.Z = (float)delta * _ripplingSpeed;

    noise.Offset += _timeBasedOffset;
  }
}
