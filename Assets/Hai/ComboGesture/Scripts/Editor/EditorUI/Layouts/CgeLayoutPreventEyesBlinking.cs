﻿using System;
using Hai.ComboGesture.Scripts.Editor.EditorUI.Effectors;
using UnityEditor;
using UnityEngine;

namespace Hai.ComboGesture.Scripts.Editor.EditorUI.Layouts
{
    public class CgeLayoutPreventEyesBlinking
    {
        private readonly CgeLayoutCommon _common;
        private readonly CgeEditorEffector _editorEffector;

        public CgeLayoutPreventEyesBlinking(CgeLayoutCommon common, CgeEditorEffector editorEffector)
        {
            _common = common;
            _editorEffector = editorEffector;
        }

        public void Layout(Rect position)
        {
            GUILayout.Label("Select face expressions with <b>both eyes closed</b>.", CgeLayoutCommon.LargeFont);
            GUILayout.BeginArea(new Rect(0, CgeLayoutCommon.SingleLineHeight * 3, position.width, CgeLayoutCommon.GuiSquareHeight * 8));
            var allClips = _editorEffector.AllDistinctAnimations();
            var mod = Math.Max(3, Math.Min(8, (int)Math.Sqrt(allClips.Count)));
            for (var element = 0; element < allClips.Count; element++)
            {
                GUILayout.BeginArea(CgeLayoutCommon.RectAt(element % mod, element / mod));
                DrawBlinkingSwitch(allClips[element]);
                GUILayout.EndArea();
            }
            GUILayout.EndArea();
            GUILayout.Box(
                "",
                GUIStyle.none,
                GUILayout.Width(CgeLayoutCommon.GuiSquareHeight + CgeLayoutCommon.GuiSquareHeight * mod + CgeLayoutCommon.SingleLineHeight * 2),
                GUILayout.Height(CgeLayoutCommon.GuiSquareHeight + CgeLayoutCommon.GuiSquareHeight * (allClips.Count / mod) + CgeLayoutCommon.SingleLineHeight * 2)
            );
        }

        private void DrawBlinkingSwitch(AnimationClip element)
        {
            var isRegisteredAsBlinking = _editorEffector.MutableBlinking().Contains(element);

            if (isRegisteredAsBlinking)
            {
                CgeLayoutCommon.DrawColoredBackground(new Color(0.44f, 0.65f, 1f));
            }
            GUILayout.BeginArea(new Rect((CgeLayoutCommon.GuiSquareWidth - CgeLayoutCommon.PictureWidth) / 2, 0, CgeLayoutCommon.PictureWidth, CgeLayoutCommon.PictureHeight));
            _common.DrawPreviewOrRefreshButton(element);
            GUILayout.EndArea();

            GUILayout.Space(CgeLayoutCommon.PictureHeight);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField(element, typeof(AnimationClip), true);
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button(isRegisteredAsBlinking ? "Blinking" : ""))
            {
                if (isRegisteredAsBlinking)
                {
                    _editorEffector.MutableBlinking().Remove(element);
                }
                else
                {
                    _editorEffector.MutableBlinking().Add(element);
                }
            }
        }

    }
}