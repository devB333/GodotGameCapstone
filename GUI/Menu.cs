using Godot;
using System;

public partial class Menu : Control
{

    public override void _Ready()
    {
        //Connects function to signal in Godot
        var btn = GetNode<Button>("Button");
        btn.Pressed += OnButtonPressed;
    }

    private void OnButtonPressed()
    {
       MenuManager.Instance.CheckMenuState();
    }

}
