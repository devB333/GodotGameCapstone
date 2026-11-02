using Godot;
using System;
using System.Collections.Generic;

public interface IStateParent
{
    public List<IState> states { get; }
    public Dictionary<string, IState> stateNameMap { get; }
    public IState activeState { get; set; }
    
    
    public void switch_state(string stateName);

}
