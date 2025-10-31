// Decompiled with JetBrains decompiler
// Type: Utils
// Assembly: DynamicHairGrowth, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4CA2C7CF-8C0E-4318-B35F-7FF448EDFE4C
// Assembly location: D:\SteamLibrary\steamapps\common\Grand Theft Auto V\scripts\DynamicHairGrowth.dll

using GTA;
using GTA.Native;
using System;

public static class Utils
{
    public static void SetComponentVariation(
      this Ped ped,
      int componentId,
      int drawableId,
      int textureId,
      int paletteId = 0)
    {
        Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5]
        {
      (InputArgument) ped,
      (InputArgument) componentId,
      (InputArgument) drawableId,
      (InputArgument) textureId,
      (InputArgument) paletteId
        });
    }

    public static void TaskPlayAnim(
      this Ped ped,
      string animDict,
      string animFile,
      int duration,
      int flag,
      float time = 0.0f,
      float speed = 1f)
    {
        Function.Call(Hash.REQUEST_ANIM_DICT, new InputArgument[1]
        {
      (InputArgument) animDict
        });
        DateTime dateTime = DateTime.Now + TimeSpan.FromMilliseconds(1000.0);
        do
        {
            if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, new InputArgument[1]
            {
        (InputArgument) animDict
            }))
                Script.Yield();
            else
                goto label_4;
        }
        while (!(DateTime.Now >= dateTime));
        return;
    label_4:
        Function.Call(Hash.TASK_PLAY_ANIM, new InputArgument[11]
        {
      (InputArgument) ped,
      (InputArgument) animDict,
      (InputArgument) animFile,
      (InputArgument) 8f,
      (InputArgument) (-4f),
      (InputArgument) duration,
      (InputArgument) flag,
      (InputArgument) 0.0f,
      (InputArgument) false,
      (InputArgument) false,
      (InputArgument) false
        });
        Script.Wait(0);
        if ((double)time != 0.0)
            Function.Call(Hash.SET_ENTITY_ANIM_CURRENT_TIME, new InputArgument[4]
            {
        (InputArgument) ped,
        (InputArgument) animDict,
        (InputArgument) animFile,
        (InputArgument) time
            });
        if ((double)speed == 1.0)
            return;
        Function.Call(Hash.SET_ENTITY_ANIM_SPEED, new InputArgument[4]
        {
      (InputArgument) ped,
      (InputArgument) animDict,
      (InputArgument) animFile,
      (InputArgument) speed
        });
    }

    public static void PlayAmbientSpeech(
      this Ped ped,
      string speechFile,
      bool immediately = false,
      string[] queue = null)
    {
        string str = speechFile;
        if (immediately)
            Function.Call(Hash.STOP_CURRENT_PLAYING_AMBIENT_SPEECH, new InputArgument[1]
            {
        (InputArgument) ped
            });
        int num;
        if (queue != null && queue.Length != 0)
            num = !Function.Call<bool>(Hash.DOES_CONTEXT_EXIST_FOR_THIS_PED, new InputArgument[3]
            {
        (InputArgument) ped,
        (InputArgument) str,
        (InputArgument) 0
            }) ? 1 : 0;
        else
            num = 0;
        if (num != 0)
        {
            for (int index = 0; index < queue.Length; ++index)
            {
                if (Function.Call<bool>(Hash.DOES_CONTEXT_EXIST_FOR_THIS_PED, new InputArgument[3]
                {
          (InputArgument) ped,
          (InputArgument) queue[index],
          (InputArgument) 0
                }))
                {
                    str = queue[index];
                    break;
                }
            }
        }
        Function.Call(Hash.SET_AUDIO_FLAG, new InputArgument[2]
        {
      (InputArgument) "IsDirectorModeActive",
      (InputArgument) 1
        });
        Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, new InputArgument[3]
        {
      (InputArgument) ped,
      (InputArgument) str,
      (InputArgument) "SPEECH_PARAMS_FORCE"
        });
        Function.Call(Hash.SET_AUDIO_FLAG, new InputArgument[2]
        {
      (InputArgument) "IsDirectorModeActive",
      (InputArgument) 0
        });
    }

    public static void DisplayHelpTextThisFrame(string text)
    {
        Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, new InputArgument[1]
        {
      (InputArgument) "CELL_EMAIL_BCON"
        });
        for (int startIndex = 0; startIndex < text.Length; startIndex += 99)
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1]
            {
        (InputArgument) text.Substring(startIndex, Math.Min(99, text.Length - startIndex))
            });
        Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, new InputArgument[4]
        {
      (InputArgument) 0,
      (InputArgument) 0,
      (InputArgument) (Function.Call<bool>(Hash.IS_HELP_MESSAGE_BEING_DISPLAYED, new InputArgument[0]) ? 0 : 1),
      (InputArgument) (-1)
        });
    }
}
