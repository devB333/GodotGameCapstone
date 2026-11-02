namespace IsoMetricCamera.Scripts.Interactable;
using Godot;
using System;

public interface IInteractableState
{
    void Enter();
    void Exit();
    void Update();
    void Clicked();
}
