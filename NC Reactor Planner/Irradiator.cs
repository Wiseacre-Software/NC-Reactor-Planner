﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;

namespace NC_Reactor_Planner
{
    public class Irradiator: Block
    {
        public int ModeratedNeutronFlux { get; set; }
        public int HeatPerFlux { get; set; }
        public int HeatPerTick { get => ModeratedNeutronFlux * HeatPerFlux; }
        public double EfficiencyMultiplier { get; set; }
        public override bool Valid { get => ModeratedNeutronFlux > 0; }

        public Irradiator(string displayName, Bitmap texture, Vector3 position, IrradiatorValues values) : base(displayName, BlockTypes.Irradiator, texture, position)
        {
            this.HeatPerFlux = values.IrradiatorHeatPerFlux;
            this.EfficiencyMultiplier = values.IrradiatorEfficiencyMultiplier;
            RevertToSetup();
        }

        public Irradiator(string displayName, Bitmap texture, Vector3 position) : base(displayName, BlockTypes.Irradiator, texture, position)
        {
            this.HeatPerFlux = Configuration.Fission.IrradiatorHeatPerFlux;
            this.EfficiencyMultiplier = Configuration.Fission.IrradiatorEfficiencyMultiplier;
            RevertToSetup();
        }

        public Irradiator(Irradiator parent, Vector3 newPosition) : this(parent.DisplayName, parent.Texture, newPosition)
        {
            this.HeatPerFlux = parent.HeatPerFlux;
            this.EfficiencyMultiplier = parent.EfficiencyMultiplier;
        }

        public override void RevertToSetup()
        {
            ModeratedNeutronFlux = 0;
            SetCluster(-1);
        }

        public void UpdateStats()
        {
            if (!this.Valid)
                Reactor.functionalBlocks--;
        }

        public override string GetToolTip()
        {
            StringBuilder tb = new StringBuilder(); //TooltipBuilder
            tb.Append(base.GetToolTip());

            if (Position == Palette.dummyPosition)
            {
                tb.AppendLine("Direct flux into this block");
                tb.AppendLine("to perform recipes.");
                tb.AppendLine("Increases heat multiplier.");
                tb.AppendLine($"Only adds {EfficiencyMultiplier*100}% of positional efficiency.");
                tb.AppendLine("Heat per flux: " + HeatPerFlux);
            }
            else
            {
                tb.AppendLine("Total flux: " + ModeratedNeutronFlux);
                tb.AppendLine("Heat per flux: " + HeatPerFlux);
                tb.AppendLine("Total heat: " + HeatPerTick);
                tb.AppendLine($"Adds {EfficiencyMultiplier * 100}% of positional efficiency.");
            }

            return tb.ToString();
        }

        public override Block Copy(Vector3 newPosition)
        {
            return new Irradiator(this, newPosition);
        }
    }
}