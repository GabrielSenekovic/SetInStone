// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Entities/Player/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Land"",
            ""id"": ""b1a86202-c3cb-4277-9c8a-5306eb619b06"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""5daef96d-9adb-4dde-a794-339c9a209428"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""StopMove"",
                    ""type"": ""Button"",
                    ""id"": ""346174c1-6170-406a-a28d-fac7517d4969"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""MoveVertical"",
                    ""type"": ""Button"",
                    ""id"": ""501d4f70-edb2-4c19-9c05-f87afd38a95b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""StopMoveVertical"",
                    ""type"": ""Button"",
                    ""id"": ""1e59c7e9-bdfb-4f83-ba86-f584a1fb0ed9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""f7949efb-0dd4-42aa-96d8-49a52c079782"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""StopJump"",
                    ""type"": ""Button"",
                    ""id"": ""87b6d986-37f6-4e25-8fd0-597843e07db4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""Special"",
                    ""type"": ""Button"",
                    ""id"": ""598a6d33-d47b-4533-8258-c35dbceceb5d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""StopSpecial"",
                    ""type"": ""Button"",
                    ""id"": ""ad09f85c-2a32-4082-9fd7-1e532e234ccf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""Pulka"",
                    ""type"": ""Button"",
                    ""id"": ""82d0d11a-43de-4a98-ad87-0a4714a23820"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Dismount"",
                    ""type"": ""Button"",
                    ""id"": ""970e6510-9008-4169-86f4-501f5ab5f070"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DebugDecrease"",
                    ""type"": ""Button"",
                    ""id"": ""8723e059-bcad-44bc-9647-2c0edeb8810f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""DebugIncrease"",
                    ""type"": ""Button"",
                    ""id"": ""82eeb9c9-896c-4d39-82cc-70c5d35ab842"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""75e94c9d-fb7f-4bbb-82b3-bc7858138738"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Duck"",
                    ""type"": ""Button"",
                    ""id"": ""c372c4e5-53e9-4fa5-981d-39a300899881"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StandUp"",
                    ""type"": ""Button"",
                    ""id"": ""a49c9bab-3e14-44ad-832c-3b1cd7e6ebc7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""25791b6d-c53a-4927-8cd6-45b4afbff4b8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu"",
                    ""type"": ""Button"",
                    ""id"": ""5cbd0840-c6c1-49cd-a688-65314ae37b0d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""0da2473c-9074-405e-a4a6-27dea1135147"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""1599a4be-c26a-49c0-940b-fa90f668a956"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Movement - Gamepad"",
                    ""id"": ""15173e95-229b-40a0-bb65-73bf5d18a609"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""19e1f414-3c82-447c-b52e-43f1528c759a"",
                    ""path"": ""<SwitchProControllerHID>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""592c9b58-f241-43cc-9cc5-e794039156fe"",
                    ""path"": ""<SwitchProControllerHID>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Movement - SNES"",
                    ""id"": ""67585a40-95f1-4b6f-8e94-992b09d82682"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d77fd4d7-ef0e-4bbe-ac5f-45393d2418e7"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""432347c1-60d1-48f7-a4aa-c9f9a2fe8539"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Movement - Keyboard"",
                    ""id"": ""455c6ad9-785d-47a1-9a36-14a99192a54e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""39601d97-dfc4-4c36-8dce-a4bc53382c98"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""10b43018-778b-4b22-ba37-8dc4cf5f477a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""dd5a9e63-2137-42c3-a193-9dd6420ffa73"",
                    ""path"": ""<SwitchProControllerHID>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e610ec1f-3e24-4e2f-b238-0b738e1e136f"",
                    ""path"": ""<HID::USB Gamepad >/button3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a9512ec-6c18-4945-9ba2-79944c6b8f2c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e5e2386-ef08-4aa3-80ec-9d98a1b55e74"",
                    ""path"": ""<SwitchProControllerHID>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""StopJump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e0f89c8-22e9-485d-ae14-05d541d6c53a"",
                    ""path"": ""<HID::USB Gamepad >/button3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""StopJump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c0f3e954-f8fd-4aa1-a938-d5ac2e53a375"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""StopJump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fee09eda-97dc-4b3e-a4d9-c49e0696dcb2"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Special"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5637813e-3050-4c3a-92ea-4cd85468fa5a"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Pulka"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""186a3a4c-444c-4f50-863d-944f59593fad"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""DebugDecrease"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3090fe9-2ed0-449f-b341-7c2fe9dcdb2b"",
                    ""path"": ""<Keyboard>/u"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""DebugIncrease"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db1f1aab-7df6-4b73-97ad-22da0881af3a"",
                    ""path"": ""<SwitchProControllerHID>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""28c6ea1f-c820-4916-9f2b-40d13fd7c5ac"",
                    ""path"": ""<HID::USB Gamepad >/button4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""697b87ba-4e20-4afb-b774-7c17cd068f08"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Movement - Gamepad"",
                    ""id"": ""123e9e0b-c1b9-42a4-9463-f1804ba539e1"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StopMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""60cc4db5-671e-4c5d-95b1-86b1b3852dd5"",
                    ""path"": ""<SwitchProControllerHID>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""StopMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""25cab982-086b-4045-848e-162a0f6896d7"",
                    ""path"": ""<SwitchProControllerHID>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""StopMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Movement - SNES"",
                    ""id"": ""d32b69e3-4445-4596-be2b-715e817686fc"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StopMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""79c3440d-5872-4c14-8a54-a93c7adac34b"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""StopMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""5e314438-97f7-487e-ad17-430c3ac1d02a"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""StopMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Movement - Keyboard"",
                    ""id"": ""113a3585-e4b4-480f-bca0-080039d09835"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StopMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ed1047d9-4e5a-4516-a57f-21f0d5e5fd43"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""StopMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""2cd222a8-9222-4365-b499-d779c1d33fdc"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""StopMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9814cd7e-6804-43c5-98cb-8efc2f60ddef"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Duck"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3c444c2-5e6e-444f-8f48-0603fb7a202c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""StandUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f86f519b-8216-41fc-b3c4-3d3986af3f09"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Dismount"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4d39726f-addb-4385-acd6-83ccc9976499"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c1ea2e53-38f8-46cb-a19f-220170bbd6b6"",
                    ""path"": ""<Keyboard>/alt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""1adac4b2-59bb-43cc-aac7-da82ed0695eb"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""33772c21-a81d-4450-993b-2e65e3deca06"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""04c52497-2618-4531-afeb-dee05c6c11e2"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c1336ec1-03a2-4589-938d-4692434480a2"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a52fdaa6-1820-4705-8c73-8229a6e4bd14"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9ffc0d43-0243-4cd5-a42a-0c74213e80c0"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2acc1cf0-e8b6-47c2-9840-ade919b0fc73"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""StopSpecial"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""MoveVertical - Keyboard"",
                    ""id"": ""85015770-4070-4e46-95f6-a05c25901823"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveVertical"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""9400cf5a-370c-4920-8d0c-6ab33aed45fc"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveVertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""10281c07-9670-4b36-a6a5-5ee585d7d5ef"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveVertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""StopMoveVertical - Keyboard"",
                    ""id"": ""a2bb3260-8981-4c08-af6b-8f1c8de8887e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StopMoveVertical"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""80077d02-a3e0-4442-833e-e514176b88b9"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StopMoveVertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""1d58265b-cd5c-461e-93ba-53cdf6817397"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StopMoveVertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Player"",
            ""bindingGroup"": ""Player"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<SwitchProControllerHID>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<HID::USB Gamepad >"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""KeyboardMouse"",
            ""bindingGroup"": ""KeyboardMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Land
        m_Land = asset.FindActionMap("Land", throwIfNotFound: true);
        m_Land_Move = m_Land.FindAction("Move", throwIfNotFound: true);
        m_Land_StopMove = m_Land.FindAction("StopMove", throwIfNotFound: true);
        m_Land_MoveVertical = m_Land.FindAction("MoveVertical", throwIfNotFound: true);
        m_Land_StopMoveVertical = m_Land.FindAction("StopMoveVertical", throwIfNotFound: true);
        m_Land_Jump = m_Land.FindAction("Jump", throwIfNotFound: true);
        m_Land_StopJump = m_Land.FindAction("StopJump", throwIfNotFound: true);
        m_Land_Special = m_Land.FindAction("Special", throwIfNotFound: true);
        m_Land_StopSpecial = m_Land.FindAction("StopSpecial", throwIfNotFound: true);
        m_Land_Pulka = m_Land.FindAction("Pulka", throwIfNotFound: true);
        m_Land_Dismount = m_Land.FindAction("Dismount", throwIfNotFound: true);
        m_Land_DebugDecrease = m_Land.FindAction("DebugDecrease", throwIfNotFound: true);
        m_Land_DebugIncrease = m_Land.FindAction("DebugIncrease", throwIfNotFound: true);
        m_Land_Attack = m_Land.FindAction("Attack", throwIfNotFound: true);
        m_Land_Duck = m_Land.FindAction("Duck", throwIfNotFound: true);
        m_Land_StandUp = m_Land.FindAction("StandUp", throwIfNotFound: true);
        m_Land_Pause = m_Land.FindAction("Pause", throwIfNotFound: true);
        m_Land_Menu = m_Land.FindAction("Menu", throwIfNotFound: true);
        m_Land_Navigate = m_Land.FindAction("Navigate", throwIfNotFound: true);
        m_Land_Zoom = m_Land.FindAction("Zoom", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Land
    private readonly InputActionMap m_Land;
    private ILandActions m_LandActionsCallbackInterface;
    private readonly InputAction m_Land_Move;
    private readonly InputAction m_Land_StopMove;
    private readonly InputAction m_Land_MoveVertical;
    private readonly InputAction m_Land_StopMoveVertical;
    private readonly InputAction m_Land_Jump;
    private readonly InputAction m_Land_StopJump;
    private readonly InputAction m_Land_Special;
    private readonly InputAction m_Land_StopSpecial;
    private readonly InputAction m_Land_Pulka;
    private readonly InputAction m_Land_Dismount;
    private readonly InputAction m_Land_DebugDecrease;
    private readonly InputAction m_Land_DebugIncrease;
    private readonly InputAction m_Land_Attack;
    private readonly InputAction m_Land_Duck;
    private readonly InputAction m_Land_StandUp;
    private readonly InputAction m_Land_Pause;
    private readonly InputAction m_Land_Menu;
    private readonly InputAction m_Land_Navigate;
    private readonly InputAction m_Land_Zoom;
    public struct LandActions
    {
        private @PlayerControls m_Wrapper;
        public LandActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Land_Move;
        public InputAction @StopMove => m_Wrapper.m_Land_StopMove;
        public InputAction @MoveVertical => m_Wrapper.m_Land_MoveVertical;
        public InputAction @StopMoveVertical => m_Wrapper.m_Land_StopMoveVertical;
        public InputAction @Jump => m_Wrapper.m_Land_Jump;
        public InputAction @StopJump => m_Wrapper.m_Land_StopJump;
        public InputAction @Special => m_Wrapper.m_Land_Special;
        public InputAction @StopSpecial => m_Wrapper.m_Land_StopSpecial;
        public InputAction @Pulka => m_Wrapper.m_Land_Pulka;
        public InputAction @Dismount => m_Wrapper.m_Land_Dismount;
        public InputAction @DebugDecrease => m_Wrapper.m_Land_DebugDecrease;
        public InputAction @DebugIncrease => m_Wrapper.m_Land_DebugIncrease;
        public InputAction @Attack => m_Wrapper.m_Land_Attack;
        public InputAction @Duck => m_Wrapper.m_Land_Duck;
        public InputAction @StandUp => m_Wrapper.m_Land_StandUp;
        public InputAction @Pause => m_Wrapper.m_Land_Pause;
        public InputAction @Menu => m_Wrapper.m_Land_Menu;
        public InputAction @Navigate => m_Wrapper.m_Land_Navigate;
        public InputAction @Zoom => m_Wrapper.m_Land_Zoom;
        public InputActionMap Get() { return m_Wrapper.m_Land; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(LandActions set) { return set.Get(); }
        public void SetCallbacks(ILandActions instance)
        {
            if (m_Wrapper.m_LandActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_LandActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnMove;
                @StopMove.started -= m_Wrapper.m_LandActionsCallbackInterface.OnStopMove;
                @StopMove.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnStopMove;
                @StopMove.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnStopMove;
                @MoveVertical.started -= m_Wrapper.m_LandActionsCallbackInterface.OnMoveVertical;
                @MoveVertical.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnMoveVertical;
                @MoveVertical.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnMoveVertical;
                @StopMoveVertical.started -= m_Wrapper.m_LandActionsCallbackInterface.OnStopMoveVertical;
                @StopMoveVertical.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnStopMoveVertical;
                @StopMoveVertical.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnStopMoveVertical;
                @Jump.started -= m_Wrapper.m_LandActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnJump;
                @StopJump.started -= m_Wrapper.m_LandActionsCallbackInterface.OnStopJump;
                @StopJump.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnStopJump;
                @StopJump.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnStopJump;
                @Special.started -= m_Wrapper.m_LandActionsCallbackInterface.OnSpecial;
                @Special.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnSpecial;
                @Special.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnSpecial;
                @StopSpecial.started -= m_Wrapper.m_LandActionsCallbackInterface.OnStopSpecial;
                @StopSpecial.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnStopSpecial;
                @StopSpecial.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnStopSpecial;
                @Pulka.started -= m_Wrapper.m_LandActionsCallbackInterface.OnPulka;
                @Pulka.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnPulka;
                @Pulka.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnPulka;
                @Dismount.started -= m_Wrapper.m_LandActionsCallbackInterface.OnDismount;
                @Dismount.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnDismount;
                @Dismount.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnDismount;
                @DebugDecrease.started -= m_Wrapper.m_LandActionsCallbackInterface.OnDebugDecrease;
                @DebugDecrease.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnDebugDecrease;
                @DebugDecrease.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnDebugDecrease;
                @DebugIncrease.started -= m_Wrapper.m_LandActionsCallbackInterface.OnDebugIncrease;
                @DebugIncrease.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnDebugIncrease;
                @DebugIncrease.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnDebugIncrease;
                @Attack.started -= m_Wrapper.m_LandActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnAttack;
                @Duck.started -= m_Wrapper.m_LandActionsCallbackInterface.OnDuck;
                @Duck.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnDuck;
                @Duck.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnDuck;
                @StandUp.started -= m_Wrapper.m_LandActionsCallbackInterface.OnStandUp;
                @StandUp.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnStandUp;
                @StandUp.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnStandUp;
                @Pause.started -= m_Wrapper.m_LandActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnPause;
                @Menu.started -= m_Wrapper.m_LandActionsCallbackInterface.OnMenu;
                @Menu.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnMenu;
                @Menu.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnMenu;
                @Navigate.started -= m_Wrapper.m_LandActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnNavigate;
                @Zoom.started -= m_Wrapper.m_LandActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnZoom;
            }
            m_Wrapper.m_LandActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @StopMove.started += instance.OnStopMove;
                @StopMove.performed += instance.OnStopMove;
                @StopMove.canceled += instance.OnStopMove;
                @MoveVertical.started += instance.OnMoveVertical;
                @MoveVertical.performed += instance.OnMoveVertical;
                @MoveVertical.canceled += instance.OnMoveVertical;
                @StopMoveVertical.started += instance.OnStopMoveVertical;
                @StopMoveVertical.performed += instance.OnStopMoveVertical;
                @StopMoveVertical.canceled += instance.OnStopMoveVertical;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @StopJump.started += instance.OnStopJump;
                @StopJump.performed += instance.OnStopJump;
                @StopJump.canceled += instance.OnStopJump;
                @Special.started += instance.OnSpecial;
                @Special.performed += instance.OnSpecial;
                @Special.canceled += instance.OnSpecial;
                @StopSpecial.started += instance.OnStopSpecial;
                @StopSpecial.performed += instance.OnStopSpecial;
                @StopSpecial.canceled += instance.OnStopSpecial;
                @Pulka.started += instance.OnPulka;
                @Pulka.performed += instance.OnPulka;
                @Pulka.canceled += instance.OnPulka;
                @Dismount.started += instance.OnDismount;
                @Dismount.performed += instance.OnDismount;
                @Dismount.canceled += instance.OnDismount;
                @DebugDecrease.started += instance.OnDebugDecrease;
                @DebugDecrease.performed += instance.OnDebugDecrease;
                @DebugDecrease.canceled += instance.OnDebugDecrease;
                @DebugIncrease.started += instance.OnDebugIncrease;
                @DebugIncrease.performed += instance.OnDebugIncrease;
                @DebugIncrease.canceled += instance.OnDebugIncrease;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Duck.started += instance.OnDuck;
                @Duck.performed += instance.OnDuck;
                @Duck.canceled += instance.OnDuck;
                @StandUp.started += instance.OnStandUp;
                @StandUp.performed += instance.OnStandUp;
                @StandUp.canceled += instance.OnStandUp;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Menu.started += instance.OnMenu;
                @Menu.performed += instance.OnMenu;
                @Menu.canceled += instance.OnMenu;
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
            }
        }
    }
    public LandActions @Land => new LandActions(this);
    private int m_PlayerSchemeIndex = -1;
    public InputControlScheme PlayerScheme
    {
        get
        {
            if (m_PlayerSchemeIndex == -1) m_PlayerSchemeIndex = asset.FindControlSchemeIndex("Player");
            return asset.controlSchemes[m_PlayerSchemeIndex];
        }
    }
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardMouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface ILandActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnStopMove(InputAction.CallbackContext context);
        void OnMoveVertical(InputAction.CallbackContext context);
        void OnStopMoveVertical(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnStopJump(InputAction.CallbackContext context);
        void OnSpecial(InputAction.CallbackContext context);
        void OnStopSpecial(InputAction.CallbackContext context);
        void OnPulka(InputAction.CallbackContext context);
        void OnDismount(InputAction.CallbackContext context);
        void OnDebugDecrease(InputAction.CallbackContext context);
        void OnDebugIncrease(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnDuck(InputAction.CallbackContext context);
        void OnStandUp(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnMenu(InputAction.CallbackContext context);
        void OnNavigate(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
    }
}
