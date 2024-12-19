using System;
using System.Collections.Generic;
using Avalonia.Input;

namespace BatchProcess.Data;

public class KeyboardManager
{
    private Dictionary<Key, bool> KeyState = [];
    private Dictionary<Key, List<Action<bool>>> KeyAction = [];

    public bool IsKeyDown(Key key)
    {
        return KeyState.GetValueOrDefault(key);
    }

    public void BindKeyStateChanged(Key key, Action<bool> action)
    {
        if (KeyAction.TryGetValue(key, out var actions))
        {
            actions.Add(action);
            return;
        }
        
        KeyAction[key] = [action];
    }

    public void SetKeyState(Key key, bool state)
    {
        KeyState[key] = state;

        if (!KeyAction.TryGetValue(key, out var actions))
        {
            return;
        }

        foreach (var action in actions)
        {
            action(state);
        }
    }
}