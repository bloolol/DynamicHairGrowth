using GTA;
using GTA.Math;
using GTA.Native;
using System;

using System.Windows.Forms;


namespace DynamicHairGrowth
{
    public class Main : Script
    {
        public static Main.HairGrowth michael = new Main.HairGrowth();
        public static Main.HairGrowth franklin = new Main.HairGrowth();
        public static Main.HairGrowth trevor = new Main.HairGrowth();
        public static int HairGrowthHours = 48;
        public static int FacialGrowthHours = 24;
        public static bool HairGrowthEnabled = true;
        public static bool FacialGrowthEnabled = true;
        private DateTime LastHairTime = World.CurrentDate;
        private DateTime LastFacialTime = World.CurrentDate;
        private bool timelapse;

        public Main() => this.Tick += new EventHandler(this.OnTick);

        public static void UpdateAllHair()
        {
            Main.UpdateMichaelHair();
            Main.UpdateFranklinHair();
            Main.UpdateTrevorHair();
            DynamicHairGrowth.Settings.Update();
        }

        private void ResetAllHair()
        {
            Main.michael.Reset();
            Main.franklin.Reset();
            Main.trevor.Reset();
            Main.UpdateAllHair();
        }

        private static void UpdateMichaelHair()
        {
            foreach (Ped nearbyPed in World.GetNearbyPeds(Game.Player.Character.Position, 50f))
            {
                if (nearbyPed.Model == (Model)PedHash.Michael)
                {
                    switch (Main.michael.FacialLength)
                    {
                        case 0:
                            nearbyPed.SetComponentVariation(1, 0, 0);
                            break;
                        case 1:
                            nearbyPed.SetComponentVariation(1, 1, 0);
                            break;
                        case 2:
                            nearbyPed.SetComponentVariation(1, 2, 0);
                            break;
                        case 3:
                            nearbyPed.SetComponentVariation(1, 4, 0);
                            break;
                    }
                    switch (Main.michael.HairLength)
                    {
                        case 0:
                            nearbyPed.SetComponentVariation(0, 0, 1);
                            nearbyPed.SetComponentVariation(2, 1, 0);
                            break;
                        case 1:
                            nearbyPed.SetComponentVariation(0, 3, 0);
                            nearbyPed.SetComponentVariation(2, 5, 0);
                            break;
                        case 2:
                            nearbyPed.SetComponentVariation(0, 0, 0);
                            nearbyPed.SetComponentVariation(2, 0, 0);
                            break;
                        case 3:
                            nearbyPed.SetComponentVariation(0, 0, 0);
                            nearbyPed.SetComponentVariation(2, 3, 0);
                            break;
                        case 4:
                            nearbyPed.SetComponentVariation(0, 0, 0);
                            nearbyPed.SetComponentVariation(2, 4, 0);
                            break;
                    }
                }
            }
        }

        private static void UpdateFranklinHair()
        {
            foreach (Ped nearbyPed in World.GetNearbyPeds(Game.Player.Character.Position, 50f))
            {
                if (nearbyPed.Model == (Model)PedHash.Franklin)
                {
                    if (Main.franklin.FacialLength <= 1)
                        nearbyPed.SetComponentVariation(1, 0, 0);
                    switch (Main.franklin.FacialLength)
                    {
                        case 2:
                            nearbyPed.SetComponentVariation(1, 4, 0);
                            break;
                        case 3:
                            nearbyPed.SetComponentVariation(1, 3, 0);
                            break;
                    }
                    switch (Main.franklin.HairLength)
                    {
                        case 0:
                            if (Main.franklin.FacialLength == 1)
                                nearbyPed.SetComponentVariation(0, 0, 3);
                            else if (Main.franklin.FacialLength == 0)
                                nearbyPed.SetComponentVariation(0, 0, 0);
                            nearbyPed.SetComponentVariation(2, 4, 0);
                            break;
                        case 1:
                            if (Main.franklin.FacialLength == 1)
                                nearbyPed.SetComponentVariation(0, 0, 3);
                            else if (Main.franklin.FacialLength == 0)
                                nearbyPed.SetComponentVariation(0, 0, 0);
                            nearbyPed.SetComponentVariation(2, 0, 0);
                            break;
                        case 2:
                            if (Main.franklin.FacialLength == 1)
                                nearbyPed.SetComponentVariation(0, 0, 5);
                            else
                                nearbyPed.SetComponentVariation(0, 0, 2);
                            nearbyPed.SetComponentVariation(2, 4, 0);
                            break;
                        case 3:
                            if (Main.franklin.FacialLength == 1)
                                nearbyPed.SetComponentVariation(0, 0, 5);
                            else
                                nearbyPed.SetComponentVariation(0, 0, 2);
                            nearbyPed.SetComponentVariation(2, 1, 0);
                            break;
                        case 4:
                            if (Main.franklin.FacialLength == 1)
                                nearbyPed.SetComponentVariation(0, 0, 5);
                            else
                                nearbyPed.SetComponentVariation(0, 0, 2);
                            nearbyPed.SetComponentVariation(2, 2, 0);
                            break;
                    }
                }
            }
        }

        private static void UpdateTrevorHair()
        {
            foreach (Ped nearbyPed in World.GetNearbyPeds(Game.Player.Character.Position, 50f))
            {
                if (nearbyPed.Model == (Model)PedHash.Trevor)
                {
                    if (Main.trevor.FacialLength <= 1)
                        nearbyPed.SetComponentVariation(1, 0, 0);
                    switch (Main.trevor.FacialLength)
                    {
                        case 2:
                            nearbyPed.SetComponentVariation(0, 0, 1);
                            nearbyPed.SetComponentVariation(1, 2, 0);
                            break;
                        case 3:
                            nearbyPed.SetComponentVariation(0, 0, 1);
                            nearbyPed.SetComponentVariation(1, 5, 0);
                            break;
                    }
                    switch (Main.trevor.HairLength)
                    {
                        case 0:
                            if (Main.trevor.FacialLength == 1)
                                nearbyPed.SetComponentVariation(0, 0, 3);
                            else if (Main.trevor.FacialLength == 0)
                                nearbyPed.SetComponentVariation(0, 0, 2);
                            nearbyPed.SetComponentVariation(2, 6, 0);
                            break;
                        case 1:
                            if (Main.trevor.FacialLength == 1)
                                nearbyPed.SetComponentVariation(0, 0, 1);
                            else if (Main.trevor.FacialLength == 0)
                                nearbyPed.SetComponentVariation(0, 0, 0);
                            nearbyPed.SetComponentVariation(2, 6, 0);
                            break;
                        case 2:
                            if (Main.trevor.FacialLength == 1)
                                nearbyPed.SetComponentVariation(0, 0, 1);
                            else
                                nearbyPed.SetComponentVariation(0, 0, 0);
                            nearbyPed.SetComponentVariation(2, 3, 0);
                            break;
                        case 3:
                            if (Main.trevor.FacialLength == 1)
                                nearbyPed.SetComponentVariation(0, 0, 1);
                            else
                                nearbyPed.SetComponentVariation(0, 0, 0);
                            nearbyPed.SetComponentVariation(2, 4, 0);
                            break;
                        case 4:
                            if (Main.trevor.FacialLength == 1)
                                nearbyPed.SetComponentVariation(0, 0, 1);
                            else
                                nearbyPed.SetComponentVariation(0, 0, 0);
                            nearbyPed.SetComponentVariation(2, 5, 0);
                            break;
                    }
                }
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (this.timelapse)
                Function.Call(Hash.ADD_TO_CLOCK_TIME, new InputArgument[3]
                {
          (InputArgument) 0,
          (InputArgument) 5,
          (InputArgument) 0
                });
            if (World.CurrentDate >= this.LastHairTime.AddHours((double)Main.HairGrowthHours))
            {
                this.LastHairTime = World.CurrentDate.AddHours((double)Main.HairGrowthHours);
                Main.michael.IncreaseHair();
                Main.franklin.IncreaseHair();
                Main.trevor.IncreaseHair();
                Main.UpdateAllHair();
            }
            if (!(World.CurrentDate >= this.LastFacialTime.AddHours((double)Main.FacialGrowthHours)))
                return;
            this.LastFacialTime = World.CurrentDate.AddHours((double)Main.FacialGrowthHours);
            Main.michael.IncreaseFacial();
            Main.franklin.IncreaseFacial();
            Main.trevor.IncreaseFacial();
            Main.UpdateAllHair();
        }

        public static Main.HairGrowth GetCurrentPlayerHairGrowth()
        {
            if (Game.Player.Character.Model == (Model)PedHash.Michael)
                return Main.michael;
            if (Game.Player.Character.Model == (Model)PedHash.Franklin)
                return Main.franklin;
            return Game.Player.Character.Model == (Model)PedHash.Trevor ? Main.trevor : Main.michael;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.J)
            {
                this.ResetAllHair();
                GTA.UI.Screen.ShowSubtitle("Reset hair", 2000);
            }
            if (e.KeyCode == Keys.K)
            {
                this.timelapse = !this.timelapse;
                GTA.UI.Screen.ShowSubtitle("timelapse: " + this.timelapse.ToString(), 2000);
            }
            if (e.KeyCode == Keys.Y)
                Game.Player.Character.CurrentVehicle.ApplyForce(Game.Player.Character.UpVector * 5f);
            if (e.KeyCode != Keys.U)
                return;
            //Camera camera = World.CreateCamera(Game.Player.Character.Position, new Vector3(), 50f);
            //camera.AttachTo(Game.Player.Character, new Vector3(0.25f, 0.5f, 0.0f));
            //camera.PointAt(Game.Player.Character);

            Camera camera = World.CreateCamera(Game.Player.Character.Position, Vector3.Zero, 50f);

            // Get the head bone as a PedBone object
            PedBone headBone = Game.Player.Character.Bones[Bone.SkelHead];

            // Attach camera to the head bone with offset
            camera.AttachTo(headBone, new Vector3(0.25f, 0.5f, 0.0f));

            // Point the camera at the head bone
            camera.PointAt(headBone);


            World.RenderingCamera = camera;
            Script.Wait(2000);
            camera.Delete();
            World.RenderingCamera = (Camera)null;
        }

        public class HairGrowth
        {
            public int HairLength;
            public int FacialLength;

            public void IncreaseHair()
            {
                if (!Main.HairGrowthEnabled)
                    return;
                ++this.HairLength;
                if (this.HairLength < 4)
                    return;
                this.HairLength = 4;
            }

            public void IncreaseFacial()
            {
                if (!Main.FacialGrowthEnabled)
                    return;
                ++this.FacialLength;
                if (this.FacialLength < 3)
                    return;
                this.FacialLength = 3;
            }

            public void Increase()
            {
                this.IncreaseHair();
                this.IncreaseFacial();
            }

            public void Decrease()
            {
                this.DecreaseHair();
                this.DecreaseFacial();
            }

            public void DecreaseHair()
            {
                --this.HairLength;
                if (this.HairLength >= 0)
                    return;
                this.HairLength = 0;
            }

            public void DecreaseFacial()
            {
                --this.FacialLength;
                if (this.FacialLength >= 0)
                    return;
                this.FacialLength = 0;
            }

            public void Reset()
            {
                this.HairLength = 0;
                this.FacialLength = 0;
            }
        }
    }
}
