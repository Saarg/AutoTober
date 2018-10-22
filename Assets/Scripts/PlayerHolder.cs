using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;

public class PlayerHolder : MonoBehaviour {

	[SerializeField] WheelVehicle _vehicle;
	public WheelVehicle Vehicle { get { return _vehicle; }}
	[SerializeField] UIManager _ui;
	public UIManager Ui { get { return _ui; }}
	[SerializeField] Camera _camera;
	public Camera Camera { get { return _camera; }}
}
