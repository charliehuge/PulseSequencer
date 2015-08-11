using UnityEngine;
using System.Collections;

public class GlowPatternFollower : PatternFollower 
{
	private enum Direction
	{
		Idle,
		Up,
		Down
	}
	
	[SerializeField] private Renderer _renderer;
	[SerializeField] private Light _light;
	[SerializeField] private Color _glowColor;
	[SerializeField, Range(0.1f, 10f)] private float _upSpeed = 1f;	
	[SerializeField, Range(0f, 10f)] private float _upIntensity = 2f;
	[SerializeField, Range(0.1f, 10f)] private float _downSpeed = 0.5f;
	[SerializeField, Range(0f, 10f)] private float _downIntensity = 0.5f;

	private Material _material;
	private Direction _direction;
	private float _lerpAmount;

	private void Start()
	{
		_light.color = _glowColor;
		_light.intensity = _downIntensity;

		_material = _renderer.material;
		_material.SetColor("_GlowColor", _glowColor);
		_material.SetFloat("_GlowIntensity", _downIntensity);
	}

	private void Update()
	{
		switch (_direction)
		{
		case Direction.Up:
			_lerpAmount += _upSpeed*Time.deltaTime;
			
			if (_lerpAmount > 1f)
			{
				_lerpAmount = 1f;
				_direction = Direction.Down;
			}
			break;
		case Direction.Down:
			_lerpAmount -= _downSpeed*Time.deltaTime;
			
			if (_lerpAmount < 0f)
			{
				_lerpAmount = 0f;
				_direction = Direction.Idle;
			}
			break;
		default:
			return;
		}
		
		_light.intensity = Mathf.Lerp(_downIntensity, _upIntensity, _lerpAmount);
		_material.SetFloat("_GlowIntensity", _lerpAmount);
	}
	
	protected override void OnStepTriggered(int stepIndex, double pulseTime)
	{
		_direction = Direction.Up;
	}
}
