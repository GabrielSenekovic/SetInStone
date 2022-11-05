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
    public string currentScheme = "";

    InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    [SerializeField] Transform buttonListParent;

    List<KeybindButton> buttons = new List<KeybindButton>();
    [SerializeField] Button returnButton;

    List<InputDevice> currentDevices = new List<InputDevice>() { };

    bool initialized = false;

    public void Awake()
    {
        map = inputAction.actionMaps[0];
        InputUser.onChange += OnControlsChanged;
        buttons = buttonListParent.GetComponentsInChildren<KeybindButton>().ToList();
    }
    private void Start()
    {
        //Rebind("Jump", PlayerPrefs.GetString("Jump", "<Keyboard>/space"));
        //Rebind("Hookshot", PlayerPrefs.GetString("Hookshot", "<Keyboard>/q"));
        //Rebind("Attack", PlayerPrefs.GetString("Attack", "<Keyboard>/x"));
        //Rebind("Pulka", PlayerPrefs.GetString("Pulka", "<Keyboard>/z"));
        //Rebind("Left", PlayerPrefs.GetString("Left", "<Keyboard>/a"));
        //Rebind("Right", PlayerPrefs.GetString("Right", "<Keyboard>/d"));
        //Rebind("Down", PlayerPrefs.GetString("Down", "<Keyboard>/s"));
        //Rebind("Up", PlayerPrefs.GetString("Up", "<Keyboard>/w"));
        //Rebind("Map", PlayerPrefs.GetString("Map", "<Keyboard>/alt"));
        //Rebind("Pause", PlayerPrefs.GetString("Pause", "<Keyboard>/esc"));
    }

    public void SetDeviceAndScheme(InputDevice[] devices, string scheme)
    {
        currentDevices = devices.ToList();
        currentScheme = scheme;
    }

    void OnControlsChanged(InputUser user, InputUserChange change, InputDevice device)
    {
        //First the last device is unpaired
        //Then the new device is paired
        //THEN the control scheme changes
        if(device != null && change == InputUserChange.DevicePaired) //If new device is paired
        {
            currentDevices.Clear();
            currentDevices.Add(device);
        }
        if(device == null && change == InputUserChange.ControlSchemeChanged)
        {
            currentScheme = user.controlScheme.Value.name;
            SetBindingButtons();
            TurnOnOffCursor();
        }
    }
    void TurnOnOffCursor()
    {
        if(currentScheme != "KeyboardMouse")
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
    }

    public void SetBindingButtons()
    {
        for (int i = 0; i < buttons.Count;)
        {
            switch (buttons[i].action)
            {
                case "Jump":
                    OnSetBindingButton(ref i, 4);
                    break;
                case "Hookshot":
                    OnSetBindingButton(ref i, 6);
                    break;
                case "Attack":
                    OnSetBindingButton(ref i, 12);
                    break;
                case "Pulka":
                    OnSetBindingButton(ref i, 8);
                    break;
                case "Left": //Right has to be after in the list
                    OnSetBindingButton(ref i, 0);
                    break;
                case "Down": //Up has to be after in the list
                    OnSetBindingButton(ref i, 2);
                    break;
                case "Map":
                    OnSetBindingButton(ref i, 16);
                    break;
                case "Pause":
                    OnSetBindingButton(ref i, 15);
                    break;
                default:
                    i++; break;
            }
        }
        initialized = true;
    }

    void OnSetBindingButton(ref int index, int action)
    {
        List<InputControl> controls = GetControlsOfDevice(action);

        foreach (var c in controls)
        {
            //Debug.Log("Index of button to change: " + index + " amount of buttons: " + buttons.Count + " name of c: " + c.displayName);
            buttons[index].SetButtonText(c.displayName, false); //For this to work, Right has to be after Left in the list and so on
            index++;
        }
        if (controls.Count == 0)
        {
            index++; //If we're on the keyboard and not the mouse, we don't want the for loop to keep going forever. Just go on
        }
    }
    public List<InputControl> GetControlsOfDevice(int action)
    {
        InputControl currentControl = null;
        return map.actions[action].bindings.Where(b =>
        {
            foreach (InputDevice device in currentDevices)
            {
                currentControl = InputControlPath.TryFindControl(device, b.effectivePath);
                if(currentControl != null)
                {
                    return true;
                }
            }
            return false;
        }).Select(b => currentControl).ToList(); //Select returns the InputControl. Otherwise it would return an InputBinding, which we're not looking for
    }

    public void Rebind(string function)
    {
        Rebind(function, "");
    }
    void Rebind(string function, string newBinding = "")
    {
        switch (function)
        {
            case "Jump":
                OnRebind(new List<InputAction>() { map.actions[4], map.actions[5] }, function, newBinding);
                break;
            case "Hookshot":
                OnRebind(new List<InputAction>() { map.actions[6], map.actions[7] }, function, newBinding);
                break;
            case "Attack":
                OnRebind(new List<InputAction>() { map.actions[12] }, function, newBinding);
                break;
            case "Pulka":
                OnRebind(new List<InputAction>() { map.actions[8], map.actions[9] }, function, newBinding);
                break;
            case "Left":
                OnRebind(new List<InputAction>() { map.actions[0], map.actions[1] }, function, "negative", newBinding);
                break;
            case "Right":
                OnRebind(new List<InputAction>() { map.actions[0], map.actions[1] }, function, "positive", newBinding);
                break;
            case "Down":// OnRebind(new List<InputAction>() { map.actions[8], map.actions[9], map.actions[13]}, function);
                OnRebind(new List<InputAction>() { map.actions[2], map.actions[3] }, function, "negative", newBinding);
                break;
            case "Up":
                OnRebind(new List<InputAction>() { map.actions[2], map.actions[3] }, function, "positive", newBinding);
                break;
            case "Map":
                OnRebind(new List<InputAction>() { map.actions[16] }, function, newBinding);
                break;
            case "Pause":
                OnRebind(new List<InputAction>() { map.actions[15] }, function, newBinding);
                break;
            default: Debug.Break(); break;
        }
    }
    public void OnRebind(List<InputAction> actions, string name, string newBinding)
    {
        if(newBinding == "")
        {
            actions[0].Disable();
            rebindingOperation = actions[0].PerformInteractiveRebinding()
                .WithBindingGroup(currentScheme) //Only bind if has this scheme
                .OnMatchWaitForAnother(0.2f)
                .OnCancel(operation => { })
                .OnComplete(operation => RebindComplete(actions, name))
                .Start();
        }
        else
        {
            actions[0].Disable();
            actions[0].ApplyBindingOverride(newBinding, currentScheme);
            RebindComplete(actions, name);
        }
    }
    public void OnRebind(List<InputAction> actions, string name, string compositeName, string newBinding)
    {
        var bindingIndex = actions[0].bindings.IndexOf(x => x.isPartOfComposite && x.groups.Contains(currentScheme) && x.name == compositeName);
        actions[0].Disable();

        if (newBinding == "")
        {
            rebindingOperation = actions[0].PerformInteractiveRebinding(bindingIndex)
            .WithBindingGroup(currentScheme) //Only bind if has this scheme
            .OnMatchWaitForAnother(0.2f)
            .OnCancel(operation => { })
            .OnComplete(operation => RebindComplete(actions, name, bindingIndex))
            .Start();
        }
        else
        {
            InputBinding inputBinding = actions[0].bindings[bindingIndex];
            inputBinding.overridePath = newBinding;
            actions[0].ApplyBindingOverride(bindingIndex, inputBinding);
            RebindComplete(actions, name, bindingIndex);
        }
    }
    void RebindComplete(List<InputAction> actions, string name)
    {
        rebindingOperation?.Dispose();
        actions[0].Enable();

        int index = actions[0].GetBindingIndex(currentScheme);
        string path = actions[0].bindings[index].overridePath;

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
        rebindingOperation?.Dispose();
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

        for (int i = 0; i < buttons.Count; i++) //Find the button you changed and apply changes
        {
            if (buttons[i].action == name)
            {
                indexOfOriginalButton = i;
                buttons[i].SetButtonText(path, true);
                PlayerPrefs.SetString(name, path);
            }
        }
        for (int i = 0; i < buttons.Count; i++) //Go through all buttons to find conflicting bindings
        {
            if (i != indexOfOriginalButton && buttons[i].IsSamePath(path))
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
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] != ';')
            {
                currentWord += input[i];
            }
            if (input[i] == ';' || i == input.Length - 1)
            {
                groups.Add(currentWord);
                currentWord = "";
            }
        }
        for (int i = 0; i < groups.Count; i++)
        {
            if (groups[i] == currentScheme)
            {
                return true;
            }
        }
        return false;
    }
}