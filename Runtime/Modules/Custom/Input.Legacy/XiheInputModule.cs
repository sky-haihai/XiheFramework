using System;
using System.Collections.Generic;
using UnityEngine;
using XiheFramework.Runtime.Base;

namespace XiheFramework.Runtime.Input {
    public class XiheInputModule : GameModuleBase {
        public override int Priority => 0;
        //public List<ActionKeyPair> actionKeyPairs;
        [SerializeField] private List<ActionKeyPair> bindingSetting = new();

        [SerializeField] private List<TwoDirectionalKeySet> twoAxisKeySetting = new();
        // private float m_WASDInputAcceleration = 1f;

        // private float m_ArrowInputMultiplier = 0f;
        // private float m_ArrowInputAcceleration = 1f;

        public bool allowInput = true;
        private readonly Dictionary<string, KeyCode> m_KeyActionBinds = new();

        private readonly Dictionary<string, TwoDirectionalKeySet> m_TwoAxisKeyBinds = new();

        private bool m_IsLastFrameFocused;
        private Vector2 m_LastFrameMousePosition;

        private Vector2 m_MouseDeltaPosition;

        // private float m_WASDInputMultiplier = 0f;

        private readonly Dictionary<string, float> m_TwoDirectionalMultipliers = new();

        protected override void OnInstantiated() {
            foreach (var pair in bindingSetting) {
                if (m_KeyActionBinds.ContainsKey(pair.action))
                    Debug.LogErrorFormat(
                        "[XIHE INPUT] Multiple keycodes are assigning to a same action, ignoring the action-key pair [{0},{1}]",
                        pair.action, pair.key.ToString());

                m_KeyActionBinds.Add(pair.action, pair.key);
            }

            foreach (var pair in twoAxisKeySetting) {
                if (m_TwoAxisKeyBinds.ContainsKey(pair.name))
                    Debug.LogErrorFormat(
                        "[XIHE INPUT] Multiple keycodes are assigning to a same twoAxisBinding, ignoring [{0},{1},{2},{3},{4}]",
                        pair.name, pair.forward.ToString(), pair.backward.ToString(), pair.left.ToString(),
                        pair.right.ToString());

                m_TwoAxisKeyBinds.Add(pair.name, pair);
            }
        }

        protected override void OnUpdate() {
            UpdateMouseDeltaPosition();
        }

        private void OnApplicationFocus(bool hasFocus) {
            m_MouseDeltaPosition = Vector2.zero;
            if (!hasFocus) m_IsLastFrameFocused = false;
        }

        // public KeyCode GetKeycode(KeyActionTypes action) {
        //     return keyActionBinds[action];
        // }

        public void SetKeycode(string action, KeyCode keyCode) {
            if (m_KeyActionBinds.ContainsKey(action))
                m_KeyActionBinds[action] = keyCode;
            else
                m_KeyActionBinds.Add(action, keyCode);
        }

        public KeyCode GetKeyCode(string keyActionTypes) {
            if (m_KeyActionBinds.ContainsKey(keyActionTypes)) return m_KeyActionBinds[keyActionTypes];

            Debug.LogErrorFormat("[XIHE INPUT]key action type is not assigned with a valid key code");
            return KeyCode.None;
        }

        public bool ContainsKeyAction(string keyActionTypes) {
            return m_KeyActionBinds.ContainsKey(keyActionTypes);
        }

        public bool GetKeyDown(string keyActionTypes) {
            return allowInput && UnityEngine.Input.GetKeyDown(GetKeyCode(keyActionTypes));
        }

        public bool GetKey(string keyActionTypes) {
            return allowInput && UnityEngine.Input.GetKey(GetKeyCode(keyActionTypes));
        }

        public bool GetKeyUp(string keyActionTypes) {
            return allowInput && UnityEngine.Input.GetKeyUp(GetKeyCode(keyActionTypes));
        }

        public bool GetMouseUp(int mouseType) {
            var btnUp = mouseType switch {
                0 => UnityEngine.Input.GetButtonUp("Submit"),
                1 => UnityEngine.Input.GetButtonUp("Cancel"),
                _ => throw new ArgumentOutOfRangeException(nameof(mouseType), mouseType, null)
            };
            return allowInput && UnityEngine.Input.GetMouseButtonUp(mouseType);
        }

        public bool GetMouseDown(int mouseType) {
            var btnDown = mouseType switch {
                0 => UnityEngine.Input.GetButtonDown("Submit"),
                1 => UnityEngine.Input.GetButtonDown("Cancel"),
                2 => false,
                _ => throw new ArgumentOutOfRangeException(nameof(mouseType), mouseType, null)
            };
            return allowInput && UnityEngine.Input.GetMouseButtonDown(mouseType);
        }

        public bool GetMouse(int mouseType) {
            var btn = mouseType switch {
                0 => UnityEngine.Input.GetButton("Submit"),
                1 => UnityEngine.Input.GetButton("Cancel"),
                _ => throw new ArgumentOutOfRangeException(nameof(mouseType), mouseType, null)
            };
            return allowInput && UnityEngine.Input.GetMouseButton(mouseType);
        }

        public Vector2 GetXZInput() {
            return new(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
        }

        public Vector2 GetMouseXZInput() {
            return new(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
        }

        public Vector2 GetTwoAxisInput(string setName, float acceleration = 1f) {
            if (!m_TwoAxisKeyBinds.ContainsKey(setName)) return Vector2.zero;

            var set = m_TwoAxisKeyBinds[setName];

            var mul = 0f;
            if (!m_TwoDirectionalMultipliers.ContainsKey(setName))
                m_TwoDirectionalMultipliers.Add(setName, 0f);
            else
                mul = m_TwoDirectionalMultipliers[setName];

            var input = Vector2.zero; //w,s,a,d
            var holding = false;
            if (UnityEngine.Input.GetKey(set.forward)) {
                input.y += mul;
                holding = true;
            }

            if (UnityEngine.Input.GetKey(set.backward)) {
                input.y -= mul;
                holding = true;
            }

            if (UnityEngine.Input.GetKey(set.left)) {
                input.x -= mul;
                holding = true;
            }

            if (UnityEngine.Input.GetKey(set.right)) {
                input.x += mul;
                holding = true;
            }

            if (holding)
                mul += Time.deltaTime * acceleration;
            else
                mul -= Time.deltaTime * acceleration;

            mul = Mathf.Clamp01(mul);
            m_TwoDirectionalMultipliers[setName] = mul;

            return input;
        }

        public Vector2 GetMouseDeltaPosition() {
            return m_MouseDeltaPosition;
        }

        private void UpdateMouseDeltaPosition() {
            if (m_IsLastFrameFocused) {
                //prevent mouse delta position from being calculated when the game is not focused
                var currentFrameMousePosition = UnityEngine.Input.mousePosition;
                m_MouseDeltaPosition = new Vector2(currentFrameMousePosition.x, currentFrameMousePosition.y) - m_LastFrameMousePosition;
                m_LastFrameMousePosition = currentFrameMousePosition;
            }
            else {
                m_LastFrameMousePosition = UnityEngine.Input.mousePosition;
            }

            m_IsLastFrameFocused = Application.isFocused;
        }

        public void DisableInput() {
            allowInput = false;
        }

        public void EnableInput() {
            allowInput = true;
        }

        public bool AnyKey() {
            return UnityEngine.Input.anyKey;
        }

        public bool AnyKeyDown() {
            return UnityEngine.Input.anyKeyDown;
        }
        
        [Serializable]
        public struct ActionKeyPair {
            public string action;
            public KeyCode key;

            public ActionKeyPair(string action, KeyCode key) {
                this.action = action;
                this.key = key;
            }
        }

        [Serializable]
        public struct TwoDirectionalKeySet {
            public string name;
            public KeyCode forward;
            public KeyCode backward;
            public KeyCode left;
            public KeyCode right;
        }

        // [Serializable]
        // private class ActionKeyCodeDictionary : SerializableDictionary<KeyActionTypes, KeyCode> { }
    }
}
