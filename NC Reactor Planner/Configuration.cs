﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace NC_Reactor_Planner
{
    public struct ConfigFile
    {
        public Version saveVersion;
        public FissionValues Fission;
        public Dictionary<string, FuelValues> Fuels;
        public Dictionary<string, CoolerValues> Coolers;

        public ConfigFile(Version sv, FissionValues fs, Dictionary<string, FuelValues> f, Dictionary<string, CoolerValues> c)
        {
            saveVersion = sv;
            Fission = fs;
            Fuels = f;
            Coolers = c;
        }
    }

    public struct FuelValues
    {
        public double BasePower;
        public double BaseHeat;
        public double FuelTime;

        public FuelValues(double bp, double bh, double ft)
        {
            BasePower = bp;
            BaseHeat = bh;
            FuelTime = ft;
        }
    }

    public struct CoolerValues
    {
        public double HeatActive;
        public double HeatPassive;
        public string Requirements;

        public CoolerValues(double hp, double ha, string req)
        {
            HeatPassive = hp;
            HeatActive = ha;
            Requirements = req;
        }
    }

    public struct FissionValues
    {
        public double Power;
        public double FuelUse;
        public double HeatGeneration;
        public int MinSize;
        public int MaxSize;
        public int NeutronReach;
        public double ModeratorExtraPower;
        public double ModeratorExtraHeat;

        public FissionValues(double p, double fu, double hg, int ms, int mxs, int nr, double mep, double meh)
        {
            Power = p;
            FuelUse = fu;
            HeatGeneration = hg;
            MinSize = ms;
            MaxSize = mxs;
            NeutronReach = nr;
            ModeratorExtraPower = mep;
            ModeratorExtraHeat = meh;
        }
    }

    public static class Configuration
    {
        //[TODO] incapsulation .\_/.
        public static FissionValues Fission;
        public static Dictionary<string, FuelValues> Fuels;
        public static Dictionary<string, CoolerValues> Coolers;

        private static FileInfo configFileInfo;

        public static bool Load(FileInfo file)
        {
            configFileInfo = file;
            ConfigFile cf;
            using (StreamReader sr = File.OpenText(file.FullName))
            {
                JsonSerializer jss = new JsonSerializer();
                try
                {
                    cf = (ConfigFile)jss.Deserialize(sr, typeof(ConfigFile));
                }
                catch(Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\r\nConfig file was corrupt!");
                    return false;
                }
            }
            if((cf.Fuels == null) | (cf.Coolers == null))
            {
                System.Windows.Forms.MessageBox.Show("Invalid config file contents!");
                return false;
            }
            Fission = cf.Fission;
            Fuels = cf.Fuels;
            Coolers = cf.Coolers;
            Reactor.ReloadValuesFromConfig();
            return true;
        }

        public static void Save(FileInfo file)
        {
            configFileInfo = file;
            using (TextWriter tw = File.CreateText(file.FullName))
            {
                JsonSerializer jss = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.Indented,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
                };

                ConfigFile cf = new ConfigFile(Reactor.saveVersion, Fission, Fuels, Coolers);
                jss.Serialize(tw, cf);
            }
        }

        public static void ResetToDefaults()
        {
            Coolers = new Dictionary<string, CoolerValues>();
            SetDefaultCoolers();

            Fuels = new Dictionary<string, FuelValues>();
            SetDefaultFuels();

            SetDefaultFission();
        }

        private static void SetDefaultFuels()
        {
            Fuels.Add("TBU", new FuelValues(60, 18, 144000));
            Fuels.Add("TBU Oxide", new FuelValues(84, 22.5, 144000));
            Fuels.Add("LEU-233", new FuelValues(144, 60, 64000));
            Fuels.Add("LEU-233 Oxide", new FuelValues(201.6, 75, 64000));
            Fuels.Add("HEU-233", new FuelValues(576, 360, 64000));
            Fuels.Add("HEU-233 Oxide", new FuelValues(806.4, 450, 64000));
            Fuels.Add("LEU-235", new FuelValues(120, 50, 72000));
            Fuels.Add("LEU-235 Oxide", new FuelValues(168, 62.5, 72000));
            Fuels.Add("HEU-235", new FuelValues(480, 300, 72000));
            Fuels.Add("HEU-235 Oxide", new FuelValues(672, 375, 72000));
            Fuels.Add("LEN-236", new FuelValues(90, 36, 102000));
            Fuels.Add("LEN-236 Oxide", new FuelValues(126, 45, 102000));
            Fuels.Add("HEN-236", new FuelValues(360, 216, 102000));
            Fuels.Add("HEN-236 Oxide", new FuelValues(504, 270, 102000));
            Fuels.Add("LEP-239", new FuelValues(105, 40, 92000));
            Fuels.Add("LEP-239 Oxide", new FuelValues(147, 50, 92000));
            Fuels.Add("HEP-239", new FuelValues(420, 240, 92000));
            Fuels.Add("HELP-239  Oxide", new FuelValues(588, 300, 92000));
            Fuels.Add("LEP-241", new FuelValues(165, 70, 60000));
            Fuels.Add("LEP-241 Oxide", new FuelValues(231, 87.5, 60000));
            Fuels.Add("HEP-241", new FuelValues(660, 420, 60000));
            Fuels.Add("HEP-241 Oxide", new FuelValues(924, 525, 60000));
            Fuels.Add("MOX-239", new FuelValues(155.4, 57.5, 84000));
            Fuels.Add("MOX-241", new FuelValues(243.6, 97.5, 56000));
            Fuels.Add("LEA-242", new FuelValues(192, 94, 54000));
            Fuels.Add("LEA-242 Oxide", new FuelValues(268.8, 117.5, 54000));
            Fuels.Add("HEA-242", new FuelValues(768, 564, 54000));
            Fuels.Add("HEA-242 Oxide", new FuelValues(1075.2, 705, 54000));
            Fuels.Add("LECm-243", new FuelValues(210, 112, 52000));
            Fuels.Add("LECm-243 Oxide", new FuelValues(294, 140, 52000));
            Fuels.Add("HECm-243", new FuelValues(840, 672, 52000));
            Fuels.Add("HECm-243 Oxide", new FuelValues(1176, 840, 52000));
            Fuels.Add("LECm-245", new FuelValues(162, 68, 68000));
            Fuels.Add("LECm-245 Oxide", new FuelValues(226.8, 85, 68000));
            Fuels.Add("HECm-245", new FuelValues(648, 408, 68000));
            Fuels.Add("HECm-245 Oxide", new FuelValues(907.2, 510, 68000));
            Fuels.Add("LECm-247", new FuelValues(138, 54, 78000));
            Fuels.Add("LECm-247 Oxide", new FuelValues(193.2, 67.5, 78000));
            Fuels.Add("HECm-247", new FuelValues(552, 324, 78000));
            Fuels.Add("HECm-247 Oxide", new FuelValues(772.8, 405, 78000));
            Fuels.Add("LEB-248", new FuelValues(135, 52, 86000));
            Fuels.Add("LEB-248 Oxide", new FuelValues(189, 65, 86000));
            Fuels.Add("HEB-248", new FuelValues(540, 312, 86000));
            Fuels.Add("HEB-248 Oxide", new FuelValues(756, 390, 86000));
            Fuels.Add("LECf-249", new FuelValues(216, 116, 60000));
            Fuels.Add("LECf-249 Oxide", new FuelValues(302.4, 145, 60000));
            Fuels.Add("HECf-249", new FuelValues(864, 696, 60000));
            Fuels.Add("HECf-249 Oxide", new FuelValues(1209.6, 870, 60000));
            Fuels.Add("LECf-251", new FuelValues(225, 120, 58000));
            Fuels.Add("LECf-251 Oxide", new FuelValues(315, 150, 58000));
            Fuels.Add("HECf-251", new FuelValues(900, 720, 58000));
            Fuels.Add("HECf-251 Oxide", new FuelValues(1260, 900, 58000));
        }

        private static void SetDefaultCoolers()
        {
            Coolers.Add("Water", new CoolerValues(60, 300, "At least one Reactor Cell or active Moderator"));
            Coolers.Add("Redstone", new CoolerValues(90, 6400, "At least one Reactor Cell"));
            Coolers.Add("Quartz", new CoolerValues(70, 6000, "At least one active Moderator"));
            Coolers.Add("Gold", new CoolerValues(120, 9600, "At least one active Water cooler & active Redstone cooler"));
            Coolers.Add("Glowstone", new CoolerValues(130, 8000, "At least two active Moderators"));
            Coolers.Add("Lapis", new CoolerValues(120, 5600, "At least one Reactor Cell and one Reactor Casing"));
            Coolers.Add("Diamond", new CoolerValues(150, 14000, "One active Water cooler and one active Quartz cooler"));
            Coolers.Add("Helium", new CoolerValues(140, 13200, "ONLY one active Redstone cooler and at least one Reactor Casing"));
            Coolers.Add("Enderium", new CoolerValues(120, 10800, "Three Reactor Casings (has to be in a corner)"));
            Coolers.Add("Cryotheum", new CoolerValues(160, 12800, "At least two Reactor Cells"));
            Coolers.Add("Iron", new CoolerValues(80, 4800, "At least one active Gold cooler"));
            Coolers.Add("Emerald", new CoolerValues(160, 7200, "At least one active Moderator and one Reactor Cell"));
            Coolers.Add("Copper", new CoolerValues(80, 5200, "At least one active Glowstone Cooler"));
            Coolers.Add("Tin", new CoolerValues(120, 6000, "Two Lapis Coolers on opposite sides (same axis)"));
            Coolers.Add("Magnesium", new CoolerValues(110, 7200, "At least one Reactor Casing and one active Moderator"));
        }

        private static void SetDefaultFission()
        {
            Fission.Power = 1.0;
            Fission.FuelUse = 1.0;
            Fission.HeatGeneration = 1.0;
            Fission.MinSize = 1;
            Fission.MaxSize = 24;
            Fission.ModeratorExtraPower = 1.0;
            Fission.ModeratorExtraHeat = 2.0;
            Fission.NeutronReach = 4;
        }
    }
}