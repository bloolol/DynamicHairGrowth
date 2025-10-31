// Decompiled with JetBrains decompiler
// Type: DynamicHairGrowth.ShavingKit
// Assembly: DynamicHairGrowth, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4CA2C7CF-8C0E-4318-B35F-7FF448EDFE4C
// Assembly location: D:\SteamLibrary\steamapps\common\Grand Theft Auto V\scripts\DynamicHairGrowth.dll

using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;

namespace DynamicHairGrowth
{
    public class ShavingKit : Script
    {
        private List<Vector3> coords = new List<Vector3>()
    {
      new Vector3(-811.5411f, 181.117f, 76.74077f),
      new Vector3(1970.997f, 3818.996f, 33.42872f),
      new Vector3(-17.27159f, -1437.106f, 31.10154f),
      new Vector3(0.2505768f, 526.8067f, 170.6172f)
    };
        private bool active;
        private Camera cam;

        public ShavingKit() => this.Tick += new EventHandler(this.OnTick);

        private void OnTick(object sender, EventArgs e)
        {
            if (!this.active)
            {
                foreach (Vector3 coord in this.coords)
                {
                    Vector3 vector3 = Game.Player.Character.Position;
                    if ((double)vector3.DistanceTo(coord) <= 1.0)
                    {
                        Utils.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to use Shaving Kit.");
                        if (Game.IsControlJustPressed(Control.Context))
                        {
                            Vector3 position = Game.Player.Character.Position;
                            vector3 = new Vector3();
                            Vector3 rotation = vector3;
                            this.cam = World.CreateCamera(position, rotation, 50f);
                            this.cam.AttachTo(Game.Player.Character, new Vector3(0.25f, 0.5f, 0.0f));
                            this.cam.PointAt(Game.Player.Character);
                            World.RenderingCamera = this.cam;
                            Game.Player.CanControlCharacter = false;
                            this.active = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME, new InputArgument[0]);
                Utils.DisplayHelpTextThisFrame("~r~SHAVING KIT~s~ ~BLIP_BARBER~~n~Current Hair Length: " + (object)(Main.GetCurrentPlayerHairGrowth().HairLength + 1) + "/5~n~Current Facial Length: " + (object)(Main.GetCurrentPlayerHairGrowth().FacialLength + 1) + "/4~n~~INPUT_COVER~ Trim Hair~n~~INPUT_CONTEXT~ Trim Facial~n~~INPUT_FRONTEND_CANCEL~ Exit");
                if (Game.IsControlJustPressed(Control.Cover))
                {
                    Main.GetCurrentPlayerHairGrowth().DecreaseHair();
                    Main.UpdateAllHair();
                }
                if (Game.IsControlJustPressed( Control.Context))
                {
                    Main.GetCurrentPlayerHairGrowth().DecreaseFacial();
                    Main.UpdateAllHair();
                }
                if (Game.IsControlJustPressed( Control.FrontendCancel))
                {
                    this.cam.Delete();
                    World.RenderingCamera = (Camera)null;
                    Game.Player.CanControlCharacter = true;
                    this.active = false;
                }
            }
        }
    }
}
