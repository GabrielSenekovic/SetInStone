using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;
using System.Linq;

public class InputChange : MonoBehaviour
{
    [SerializeField] InputActionAsset inputAction;
    InputActionMap map;
    string currentScheme = "";

    InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    [SerializeField]Transform buttonListParent;

    List<KeybindButton> buttons = new List<KeybindButton>();
    [SerializeField]Button returnButton;

    bool initialized = false;

    private void Awake()
    {
        map = inputAction.actionMaps[0];
        InputUser.onChange += OnControlsChanged;

        buttons = buttonListParent.GetComponentsInChildren<KeybindButton>().ToList();
    }
    void OnControlsChanged(InputUser user, InputUserChange change, InputDevice device)
    {
        Debug.Log("Changed to: " + device);
        SetBindingButtons(device, user.controlScheme.Value.name);
    }
    public void SetBindingButtons(InputDevice device, string currentScheme_in)
    {
        currentScheme = currentScheme_in;
        for (int i = 0; i < buttons.Count;)
        {
            switch (buttons[i].action)
            {
                case "Jump":
                    OnSetBindingButton(device, ref i, 4);
                    break;
                case "Hookshot":
                    OnSetBindingButton(device, ref i, 6);
                    break;
                case "Attack":
                    OnSetBindingButton(device, ref i, 12);
                    break;
                case "Pulka":
                    OnSetBindingButton(device, ref i, 8);
                    break;
                case "Left": //Right has to be after in the list
                    OnSetBindingButton(device, ref i, 0);
                    break;
                case "Down": //Up has to be after in the list
                    OnSetBindingButton(device, ref i, 2);
                    break;
                case "Map":
                    OnSetBindingButton(device, ref i, 16);
                    break;
                case "Pause":
                    OnSetBindingButton(device, ref i, 15);
                    break;
                default:
                    i++; break;
            }
        }
        initialized = true;
    }
    void OnSetBindingButton(InputDevice device, ref int index, int action)
    {
        InputControl currentControl = null;
        List<InputControl> controls = map.actions[action].bindings.Where(b => 
        {
            currentControl = InputControlPath.TryFindControl(device, b.effectivePath);
            return currentControl != null; //Makes sure that the control in question isn't null
        }).Select(b => currentControl).ToList(); //Select returns the InputControl. Otherwise it would return an InputBinding, which we're not looking for

        foreach(var c in controls)
        {
            buttons[index].SetButtonText(c.displayName, false); //For this to work, Right has to be after Left in the list and so on
            index++;
        }
        if(controls.Count == 0)
        {
            index++; //If we're on the keyboard and not the mouse, we don't want the for loop to keep going forever. Just go on
        }
    }

    public void Rebind(string function)
    {
        switch(function)
        {
            case "Jump": OnRebind(new List<InputAction>() { map.actions[4], map.actions[5] }, function);
                break;
            case "Hookshot":OnRebind(new List<InputAction>() { map.actions[6], map.actions[7]}, function);
                break;
            case "Attack": OnRebind(new List<InputAction>() { map.actions[12] }, function); 
                break;
            case "Pulka": OnRebind(new List<InputAction>() { map.actions[8], map.actions[9] }, function);
                break;
            case "Left": OnRebind(new List<InputAction>() { map.actions[0], map.actions[1] }, function, "negative");
                break;
            case "Right": OnRebind(new List<InputAction>() { map.actions[0], map.actions[1] }, function, "positive");
                break;
            case "Down":// OnRebind(new List<InputAction>() { map.actions[8], map.actions[9], map.actions[13]}, function);
                        OnRebind(new List<InputAction>() { map.actions[2], map.actions[3] }, function, "negative");
                break;
            case "Up": OnRebind(new List<InputAction>() { map.actions[2], map.actions[3] }, function, "positive");
                break;
            case "Map": OnRebind(new List<InputAction>() { map.actions[16]}, function);
                break;
            case "Pause": OnRebind(new List<InputAction>() { map.actions[15]}, function);
                break;
            default: Debug.Break(); break;
        }
    }
    public void OnRebind(List<InputAction> actions, string name)
    {
        actions[0].Disable();
        rebindingOperation = actions[0].PerformInteractiveRebinding()
                .WithBindingGroup(currentScheme) //Only bind if has this scheme
                .OnMatchWaitForAnother(0.2f)
                .OnCancel(operation => { })
                .OnComplete(operation => RebindComplete(actions, name))
                .Start();
    }
    public void OnRebind(List<InputAction> actions, string name, string compositeName)
    {
        actions[0].Disable();
        var bindingIndex = actions[0].bindings.IndexOf(x => x.isPartOfComposite && x.groups.Contains(currentScheme) && x.name == compositeName);

        rebindingOperation = actions[0].PerformInteractiveRebinding(bindingIndex)
            .WithBindingGroup(currentScheme) //Only bind if has this scheme
            .OnMatchWaitForAnother(0.2f)
            .OnCancel(operation => { })
            .OnComplete(operation => RebindComplete(actions, name, bindingIndex))
            .Start();
    }
    void RebindComplete(List<InputAction> actions, string name)
    {
        rebindingOperation.Dispose();
        actions[0].Enable();

        string path = actions[0].bindings[actions[0].GetBindingIndex(currentScheme)].overridePath;

        if (actions.Count > 1)
        {
            //Change the associated buttons
            //ie if you change jump, also change cancel jump
            for (int i = 1; i < actions.Count; i++)
            {
                actions[i].ApplyBindingOverride(path);
            }
        }
        ApplyChangesToButtons(path, name);
    }

    void RebindComplete(List<InputAction> actions, string name, int bindingIndex)
    {
        rebindingOperation.Dispose();
        actions[0].Enable();

        string path = actions[0].bindings[bindingIndex].overridePath;

        if (actions.Count > 1)
        {
            //Change the associated buttons
            //Make sure that only one of the two composites get changed, not both
            for (int i = 1; i < actions.Count; i++)
            {
                actions[i].ApplyBindingOverride(bindingIndex, path);
            }
        }
        ApplyChangesToButtons(path, name);
    }

    void ApplyChangesToButtons(string path, string name)
    {
        Game.Instance.keybindConflict = false; //Flag for if a conflict has been detected
        returnButton.gameObject.SetActive(true);
        int indexOfOriginalButton = 0;

        for(int i = 0; i < buttons.Count; i++) //Find the button you changed and apply changes
        {
            if (buttons[i].action == name)
            {
                indexOfOriginalButton = i;
                buttons[i].SetButtonText(path, true);
            }
        }
        for(int i = 0; i < buttons.Count; i++) //Go through all buttons to find conflicting bindings
        {
            if(i != indexOfOriginalButton && buttons[i].IsSamePath(path))
            {
                buttons[i].SetConflict(true);
                Game.Instance.keybindConflict = true;
                returnButton.gameObject.SetActive(false);
            }
            else
            {
                buttons[i].SetConflict(false);
            }
        }
        if (Game.Instance.keybindConflict)
        {
            buttons[indexOfOriginalButton].SetConflict(true); //If a conflict was still found, set current button into conflict
        }
    }

    bool GetIfContainedInGroup(string input, string currentScheme)
    {
        List<string> groups = new List<string>();
        string currentWord = "";
        for(int i = 0; i < input.Length; i++)
        {
            if(input[i] != ';')
            {
                currentWord += input[i];
            }
            if(input[i] == ';' || i == input.Length - 1)
            {
                groups.Add(currentWord);
                currentWord = "";
            }
        }
        for(int i = 0; i < groups.Count; i++)
        {
            if(groups[i] == currentScheme)
            {
                return true;
            }
        }
        return false;
    }
}