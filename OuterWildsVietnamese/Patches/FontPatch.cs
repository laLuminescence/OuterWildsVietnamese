using HarmonyLib;
using OWML.ModHelper;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static OuterWildsVietnamese.SliderPatch;

namespace OuterWildsVietnamese
{
    [HarmonyPatch]
    public static class FontPatch
    {
        //Conversation Font patch
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DialogueBoxVer2), "InitializeFont")]
        public static bool InitializeFontPrefix(DialogueBoxVer2 __instance)
        {
            if (OuterWildsVietnamese.conversationFont != null)
            {
                __instance._fontInUse = OuterWildsVietnamese.conversationFont;
                __instance._dynamicFontInUse = OuterWildsVietnamese.conversationFont;
            }
            else
            {
                OuterWildsVietnamese.Log("Hearthian Font null");
                return true;
            }
            int customFontSize = 20;
            float customLineSpacing = 2.5f;

            //__instance._dynamicFontInUse = __instance._defaultDialogueFontDynamic;
            //Luminescence291123: Somehow _defaultFontSpacing got yeeted in the new update, so I won't be using it, but the old line is kept just in case
            //__instance._fontSpacingInUse = __instance._defaultFontSpacing;


            __instance._mainTextField.font = __instance._fontInUse;
            __instance._mainTextField.lineSpacing = __instance._fontSpacingInUse * customLineSpacing;
            __instance._mainTextField.fontSize = customFontSize;
            __instance._nameTextField.font = __instance._fontInUse;
            __instance._nameTextField.lineSpacing = __instance._fontSpacingInUse * customLineSpacing;
            __instance._nameTextField.fontSize = customFontSize;

            DialogueOptionUI requiredComponent =
                __instance._optionBox.GetRequiredComponent<DialogueOptionUI>();
            requiredComponent.textElement.font = __instance._fontInUse;
            requiredComponent.textElement.lineSpacing = __instance._fontSpacingInUse * customLineSpacing;
            requiredComponent.textElement.fontSize = customFontSize;
            return false;
        }

        //Input Prompt Font patch
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PromptManager), nameof(PromptManager.Awake))]
        public static void PromptManagerPostfix(PromptManager __instance)
        {
            if (OuterWildsVietnamese.promptFont != null)
                __instance._currentFont = OuterWildsVietnamese.promptFont;
            else
                OuterWildsVietnamese.Log("Prompt Font null");
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PromptManager), nameof(PromptManager.Start))]
        public static void PromptManagerStartPostfix(PromptManager __instance)
        {
            if (OuterWildsVietnamese.promptFont != null)
                __instance._currentFont = OuterWildsVietnamese.promptFont;
            else
                OuterWildsVietnamese.Log("Prompt Font null");
        }


        //Menu, Object and UI Font patch
        //Luminescence180626: I should instead look into FontAndLanguageController.AddTextElement, which calls InitializeFont at the end
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FontAndLanguageController), nameof(FontAndLanguageController.InitializeFont))]
        public static void InitFontPostfix(FontAndLanguageController __instance)
        {
            for (int i = 0; i < __instance._textContainerList.Count; i++)
            {
                //OuterWildsVietnamese.Log("OGFont: " + __instance._textContainerList[i].originalFont + " - " + __instance._textContainerList[i].textElement.text);
                //Luminescence290626: I know this is ridiculously stupid. But it finally WORKS lmaoooo. All thanks to the Log above.
                //Luminescence290626: THE PROJECT IS FINALLY COMPLETE.
                if (__instance._textContainerList[i].originalFont.ToString() == "VCR_OSD_MONO (UnityEngine.Font)")
                {
                    __instance._textContainerList[i].textElement.font = OuterWildsVietnamese.shipDisplayFont;
                    __instance._textContainerList[i].textElement.fontSize = 2;
                    __instance._textContainerList[i].textElement.rectTransform.localScale = __instance._textContainerList[i].originalScale * 25;
                }
                else if (__instance._textContainerList[i].originalFont.ToString() != "SpaceMono-Bold (UnityEngine.Font)"
                        && __instance._textContainerList[i].originalFont.ToString() != "SpaceMono-Regular (UnityEngine.Font)")
                {
                    __instance._textContainerList[i].textElement.font = OuterWildsVietnamese.menuFont;
                }
            }
        }
    }
}
