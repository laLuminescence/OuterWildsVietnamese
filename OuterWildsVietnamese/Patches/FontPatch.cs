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

            //Luminescence291123: Somehow _defaultFontSpacing got yeeted in the new update, so I won't be using it, but the old line is kept just in case
            __instance._mainTextField.font = __instance._fontInUse;
            __instance._nameTextField.font = __instance._fontInUse;

            DialogueOptionUI requiredComponent = __instance._optionBox.GetRequiredComponent<DialogueOptionUI>();
            requiredComponent.textElement.font = __instance._fontInUse;

            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(DialogueBoxVer2), "SetMainFieldDialogueText")]
        public static void SetMainFieldDialogueTextPostfix(DialogueBoxVer2 __instance)
        {
            __instance._mainTextField.lineSpacing = .95f;
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

                    //Luminescence010726: Somehow the position indicator that appears when you are in near orbit around a planet (and only in near orbit!), also uses the same font VCR_OSD_MONO (the other time it uses SpaceMono)
                    //Luminescence010726: Luckily, the text alignment is uniquely LowerCenter, so I can use that to specifically rule out the indicator text
                    //Luminescence010726: Also the text when using the ship's signal scope needs to be ruled out too
                    if (__instance._textContainerList[i].textElement.alignment.ToString() != "LowerCenter")
                    {
                        __instance._textContainerList[i].textElement.rectTransform.localScale = __instance._textContainerList[i].originalScale * 25;
                    }
                    if(__instance._textContainerList[i].textElement.rectTransform.name == "DistanceLabel")
                    {
                        __instance._textContainerList[i].textElement.rectTransform.localScale = __instance._textContainerList[i].originalScale * 5;
                        __instance._textContainerList[i].textElement.alignment = TextAnchor.LowerCenter;
                    }
                    if (__instance._textContainerList[i].textElement.rectTransform.name == "FrequencyLabel")
                    {
                        __instance._textContainerList[i].textElement.rectTransform.localScale = __instance._textContainerList[i].originalScale * 4f;
                    }
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
