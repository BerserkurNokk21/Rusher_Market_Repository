//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Inputs/PlayerInputs.inputactions
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

public partial class @PlayerInputs: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""PlayerActions"",
            ""id"": ""5ee93d65-6806-4abe-a39b-c662c3b6d28b"",
            ""actions"": [
                {
                    ""name"": ""Mover"",
                    ""type"": ""Value"",
                    ""id"": ""f74b2535-fce7-40ed-b1fe-8c8c0ea85983"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PickItem"",
                    ""type"": ""Button"",
                    ""id"": ""ba68bc8f-73a3-4723-9b21-2f4529ce4b9e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""84d8077d-8038-40cc-9870-9b5dbcd43174"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DropItem"",
                    ""type"": ""Button"",
                    ""id"": ""76dc93b8-81b1-4c61-95e1-11afeb3188af"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""b3c07620-db6f-4d80-b512-2f36d6a0089a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mover"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8e4f2358-f0ed-4b0e-b63a-9a3d339df410"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a46a9739-d937-428c-ab38-7bb1d5c1d0f4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2feb61b7-ae82-453f-81f9-c088bd60f1d4"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d6cd70d7-b31c-41c5-8e80-26b07b909303"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""03fdd13d-9bbf-49a0-8942-ed2dffea4dd2"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PickItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc2d9420-e7f1-432a-9faf-1c65e58e0b15"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b5d8f659-f5cf-4eab-a05f-7db1d7f06191"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DropItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerActions
        m_PlayerActions = asset.FindActionMap("PlayerActions", throwIfNotFound: true);
        m_PlayerActions_Mover = m_PlayerActions.FindAction("Mover", throwIfNotFound: true);
        m_PlayerActions_PickItem = m_PlayerActions.FindAction("PickItem", throwIfNotFound: true);
        m_PlayerActions_Attack = m_PlayerActions.FindAction("Attack", throwIfNotFound: true);
        m_PlayerActions_DropItem = m_PlayerActions.FindAction("DropItem", throwIfNotFound: true);
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

    // PlayerActions
    private readonly InputActionMap m_PlayerActions;
    private List<IPlayerActionsActions> m_PlayerActionsActionsCallbackInterfaces = new List<IPlayerActionsActions>();
    private readonly InputAction m_PlayerActions_Mover;
    private readonly InputAction m_PlayerActions_PickItem;
    private readonly InputAction m_PlayerActions_Attack;
    private readonly InputAction m_PlayerActions_DropItem;
    public struct PlayerActionsActions
    {
        private @PlayerInputs m_Wrapper;
        public PlayerActionsActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Mover => m_Wrapper.m_PlayerActions_Mover;
        public InputAction @PickItem => m_Wrapper.m_PlayerActions_PickItem;
        public InputAction @Attack => m_Wrapper.m_PlayerActions_Attack;
        public InputAction @DropItem => m_Wrapper.m_PlayerActions_DropItem;
        public InputActionMap Get() { return m_Wrapper.m_PlayerActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActionsActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActionsActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsActionsCallbackInterfaces.Add(instance);
            @Mover.started += instance.OnMover;
            @Mover.performed += instance.OnMover;
            @Mover.canceled += instance.OnMover;
            @PickItem.started += instance.OnPickItem;
            @PickItem.performed += instance.OnPickItem;
            @PickItem.canceled += instance.OnPickItem;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @DropItem.started += instance.OnDropItem;
            @DropItem.performed += instance.OnDropItem;
            @DropItem.canceled += instance.OnDropItem;
        }

        private void UnregisterCallbacks(IPlayerActionsActions instance)
        {
            @Mover.started -= instance.OnMover;
            @Mover.performed -= instance.OnMover;
            @Mover.canceled -= instance.OnMover;
            @PickItem.started -= instance.OnPickItem;
            @PickItem.performed -= instance.OnPickItem;
            @PickItem.canceled -= instance.OnPickItem;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @DropItem.started -= instance.OnDropItem;
            @DropItem.performed -= instance.OnDropItem;
            @DropItem.canceled -= instance.OnDropItem;
        }

        public void RemoveCallbacks(IPlayerActionsActions instance)
        {
            if (m_Wrapper.m_PlayerActionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActionsActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActionsActions @PlayerActions => new PlayerActionsActions(this);
    public interface IPlayerActionsActions
    {
        void OnMover(InputAction.CallbackContext context);
        void OnPickItem(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnDropItem(InputAction.CallbackContext context);
    }
}
