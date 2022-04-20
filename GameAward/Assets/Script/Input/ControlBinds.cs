//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Script/Input/ControlBinds.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @ControlBinds : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ControlBinds()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ControlBinds"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""e6da06d5-7c28-424d-868a-b694a5e25cda"",
            ""actions"": [
                {
                    ""name"": ""Cut"",
                    ""type"": ""Button"",
                    ""id"": ""86423ef4-cd60-46b0-a911-d3e8c3122f5b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SmoothCut"",
                    ""type"": ""Button"",
                    ""id"": ""fd16bfa5-cea0-49e6-8d30-0b3f063cc7ef"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""e0fb1e6e-eeab-48b0-8562-5c58ff20cd44"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3c068340-ecb8-4b38-b2b8-50ed213dffaa"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""6297fe16-93aa-46f9-a689-6c1f5566fc27"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveSelect"",
                    ""type"": ""PassThrough"",
                    ""id"": ""faf65f24-1006-4e99-9a72-096b0447acd1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test"",
                    ""type"": ""Button"",
                    ""id"": ""0e7cb05d-f2e7-4d92-bad7-7eadccb795ce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Clockwise"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b5c7489e-0bbd-4a9f-8694-5e2e1ab24787"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Aniticlockwise"",
                    ""type"": ""Button"",
                    ""id"": ""cb3f5ae3-70fd-48ed-8863-879e2b8a7880"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cut2"",
                    ""type"": ""Button"",
                    ""id"": ""be890c32-4b42-47ee-927a-4141f3cb0061"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""GameEnd"",
                    ""type"": ""Button"",
                    ""id"": ""8e1d9675-21bf-491c-ae38-d74448d2eb0d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8df9827e-5e58-47a6-abd1-1827d2f1c9f7"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cut"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd25c312-39d0-4c40-a8c7-95f27505fc6d"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cut"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be1395dc-f145-4c05-8149-0342ab0a10c2"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""210831e7-62b8-4dae-bdc0-9188adc22306"",
                    ""path"": ""<XInputController>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6bd30be5-7ac0-45a4-a7e9-2b08987c84f5"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0a1291bb-37c8-4f06-bf6d-8d5d38508a93"",
                    ""path"": ""<XInputController>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""cecf3959-f142-4597-91bf-5f9480b556ad"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6ae33bf2-a803-4edd-97f1-0b451b7447da"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d6f1f9be-f811-4204-bf53-f6fa7d0bc384"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5691de61-0f7e-40aa-a81e-be4fee6783f1"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""12c60478-55a4-4fdf-a1a9-73ff57debcd3"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow"",
                    ""id"": ""1660446e-6a29-4c51-841e-a40cbc1b7308"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""be2c2908-e617-404f-9343-986b233c8000"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0b64b11b-5d64-473d-9c56-015d820d8df0"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e04db5b1-5378-4bd2-a264-97700c98e706"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""178b521e-7f83-43e8-9fae-f2dd6b558609"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""cac26f88-a0c6-4625-b85e-1a90b20ffc4b"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""98f70ed9-a8c8-44d4-b622-786677d85981"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""One Modifier"",
                    ""id"": ""c3d5b743-b37d-4e7d-9e28-f6d35e7d4b1b"",
                    ""path"": ""OneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SmoothCut"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""c0361869-6c89-4816-bac8-0ab53348bd11"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SmoothCut"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""binding"",
                    ""id"": ""8d79e3d2-c592-4b83-b7ba-bfb77eae86e2"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SmoothCut"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""605c5b44-ce18-40a1-9126-5a3a8fee3209"",
                    ""path"": ""<XInputController>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrow"",
                    ""id"": ""4307ee6e-b325-4dc6-a7d6-28745e13b4ad"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveSelect"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""26c63aba-bf79-44c0-a41f-b8363ddf5954"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c8013ada-c211-47fc-abbc-123124da3ed0"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""05cff9eb-77ba-445f-9789-65795ccf802e"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""41e9fc0a-763f-43c2-ba84-8324c5979633"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""460962ca-21eb-4e36-b135-f54946c60a53"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""a4ee6dc0-352c-44f5-9da6-120e5221f7d2"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Clockwise"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""5c1310a0-e7e9-45c4-9e4a-825cb9134221"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Clockwise"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""298e79bf-3f74-426d-9ded-fc3ae0db430b"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Clockwise"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""813107a6-dda6-4ca6-8326-9d7779a86625"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Clockwise"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e754b0a4-b507-465c-856f-318a88a74958"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Clockwise"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""994aaf9d-cf0a-4cb2-8777-a478b2579392"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aniticlockwise"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d1159b25-646e-4f31-8958-c0edb4198b36"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cut2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83854369-68bb-4a02-8987-a11e4ddb0843"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cut2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ff7cc9b-1493-4824-abc4-d6cdef8cb797"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cut2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""615d755a-d1e5-4ed0-aae9-e4609b6daba4"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GameEnd"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""New action map"",
            ""id"": ""238ea256-8138-4467-a5a6-c3387240456f"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""d2a6cf0b-5aaa-4d53-900f-f2ada076ad20"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4d8a1348-916b-4a37-a0c6-041482611b2b"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Cut = m_Player.FindAction("Cut", throwIfNotFound: true);
        m_Player_SmoothCut = m_Player.FindAction("SmoothCut", throwIfNotFound: true);
        m_Player_Pause = m_Player.FindAction("Pause", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Select = m_Player.FindAction("Select", throwIfNotFound: true);
        m_Player_MoveSelect = m_Player.FindAction("MoveSelect", throwIfNotFound: true);
        m_Player_Test = m_Player.FindAction("Test", throwIfNotFound: true);
        m_Player_Clockwise = m_Player.FindAction("Clockwise", throwIfNotFound: true);
        m_Player_Aniticlockwise = m_Player.FindAction("Aniticlockwise", throwIfNotFound: true);
        m_Player_Cut2 = m_Player.FindAction("Cut2", throwIfNotFound: true);
        m_Player_GameEnd = m_Player.FindAction("GameEnd", throwIfNotFound: true);
        // New action map
        m_Newactionmap = asset.FindActionMap("New action map", throwIfNotFound: true);
        m_Newactionmap_Newaction = m_Newactionmap.FindAction("New action", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Cut;
    private readonly InputAction m_Player_SmoothCut;
    private readonly InputAction m_Player_Pause;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Select;
    private readonly InputAction m_Player_MoveSelect;
    private readonly InputAction m_Player_Test;
    private readonly InputAction m_Player_Clockwise;
    private readonly InputAction m_Player_Aniticlockwise;
    private readonly InputAction m_Player_Cut2;
    private readonly InputAction m_Player_GameEnd;
    public struct PlayerActions
    {
        private @ControlBinds m_Wrapper;
        public PlayerActions(@ControlBinds wrapper) { m_Wrapper = wrapper; }
        public InputAction @Cut => m_Wrapper.m_Player_Cut;
        public InputAction @SmoothCut => m_Wrapper.m_Player_SmoothCut;
        public InputAction @Pause => m_Wrapper.m_Player_Pause;
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Select => m_Wrapper.m_Player_Select;
        public InputAction @MoveSelect => m_Wrapper.m_Player_MoveSelect;
        public InputAction @Test => m_Wrapper.m_Player_Test;
        public InputAction @Clockwise => m_Wrapper.m_Player_Clockwise;
        public InputAction @Aniticlockwise => m_Wrapper.m_Player_Aniticlockwise;
        public InputAction @Cut2 => m_Wrapper.m_Player_Cut2;
        public InputAction @GameEnd => m_Wrapper.m_Player_GameEnd;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Cut.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCut;
                @Cut.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCut;
                @Cut.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCut;
                @SmoothCut.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSmoothCut;
                @SmoothCut.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSmoothCut;
                @SmoothCut.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSmoothCut;
                @Pause.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Select.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                @MoveSelect.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveSelect;
                @MoveSelect.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveSelect;
                @MoveSelect.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveSelect;
                @Test.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTest;
                @Test.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTest;
                @Test.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTest;
                @Clockwise.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnClockwise;
                @Clockwise.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnClockwise;
                @Clockwise.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnClockwise;
                @Aniticlockwise.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAniticlockwise;
                @Aniticlockwise.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAniticlockwise;
                @Aniticlockwise.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAniticlockwise;
                @Cut2.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCut2;
                @Cut2.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCut2;
                @Cut2.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCut2;
                @GameEnd.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGameEnd;
                @GameEnd.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGameEnd;
                @GameEnd.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGameEnd;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Cut.started += instance.OnCut;
                @Cut.performed += instance.OnCut;
                @Cut.canceled += instance.OnCut;
                @SmoothCut.started += instance.OnSmoothCut;
                @SmoothCut.performed += instance.OnSmoothCut;
                @SmoothCut.canceled += instance.OnSmoothCut;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @MoveSelect.started += instance.OnMoveSelect;
                @MoveSelect.performed += instance.OnMoveSelect;
                @MoveSelect.canceled += instance.OnMoveSelect;
                @Test.started += instance.OnTest;
                @Test.performed += instance.OnTest;
                @Test.canceled += instance.OnTest;
                @Clockwise.started += instance.OnClockwise;
                @Clockwise.performed += instance.OnClockwise;
                @Clockwise.canceled += instance.OnClockwise;
                @Aniticlockwise.started += instance.OnAniticlockwise;
                @Aniticlockwise.performed += instance.OnAniticlockwise;
                @Aniticlockwise.canceled += instance.OnAniticlockwise;
                @Cut2.started += instance.OnCut2;
                @Cut2.performed += instance.OnCut2;
                @Cut2.canceled += instance.OnCut2;
                @GameEnd.started += instance.OnGameEnd;
                @GameEnd.performed += instance.OnGameEnd;
                @GameEnd.canceled += instance.OnGameEnd;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // New action map
    private readonly InputActionMap m_Newactionmap;
    private INewactionmapActions m_NewactionmapActionsCallbackInterface;
    private readonly InputAction m_Newactionmap_Newaction;
    public struct NewactionmapActions
    {
        private @ControlBinds m_Wrapper;
        public NewactionmapActions(@ControlBinds wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_Newactionmap_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_Newactionmap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NewactionmapActions set) { return set.Get(); }
        public void SetCallbacks(INewactionmapActions instance)
        {
            if (m_Wrapper.m_NewactionmapActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_NewactionmapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public NewactionmapActions @Newactionmap => new NewactionmapActions(this);
    public interface IPlayerActions
    {
        void OnCut(InputAction.CallbackContext context);
        void OnSmoothCut(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnMoveSelect(InputAction.CallbackContext context);
        void OnTest(InputAction.CallbackContext context);
        void OnClockwise(InputAction.CallbackContext context);
        void OnAniticlockwise(InputAction.CallbackContext context);
        void OnCut2(InputAction.CallbackContext context);
        void OnGameEnd(InputAction.CallbackContext context);
    }
    public interface INewactionmapActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}
