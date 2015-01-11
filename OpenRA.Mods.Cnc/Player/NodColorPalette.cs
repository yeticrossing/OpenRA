#region Copyright & License Information
/*
 * Copyright 2007-2014 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System;
using OpenRA.Graphics;

namespace OpenRA.Traits
{
	[Desc("Add this to the Player actor definition.")]
	public class NodColorPaletteInfo : ITraitInfo
	{
		[Desc("The name of the palette to base off.")]
		public readonly string BasePalette = "terrain";
		[Desc("The prefix for the resulting player palettes")]
		public readonly string BaseName = "playerSecondary";
		[Desc("Remap these indices to player colors.")]
		public readonly int[] RemapIndex = { 176, 178, 180, 182, 184, 186, 189, 191, 177, 179, 181, 183, 185, 187, 188, 190 };
		[Desc("The alternate color to remap with.")]
		public readonly HSLColor Color = new HSLColor(220,30,220);
		[Desc("The country to use an alternate color for.")]
		public readonly string Race = "nod";
		[Desc("Luminosity range to span.")]
		public readonly float Ramp = 0.05f;
		[Desc("Allow palette modifiers to change the palette.")]
		public readonly bool AllowModifiers = true;

		public object Create(ActorInitializer init) { return new NodColorPalette(init.self.Owner, this); }
	}

	public class NodColorPalette : ILoadsPalettes
	{
		readonly Player owner;
		readonly NodColorPaletteInfo info;

		public NodColorPalette(Player owner, NodColorPaletteInfo info)
		{
			this.owner = owner;
			this.info = info;
		}

		public void LoadPalettes(WorldRenderer wr)
		{

			IPaletteRemap remap;
			switch (owner.Country.Race == info.Race)
			{
				case true: 
					{
					remap = new PlayerColorRemap(info.RemapIndex, info.Color, info.Ramp);
					break;
					}
				default:
					{
					remap = new PlayerColorRemap(info.RemapIndex, owner.Color, info.Ramp);
					break;
					}
			}
			wr.AddPalette(info.BaseName + owner.InternalName, new ImmutablePalette(wr.Palette(info.BasePalette).Palette, remap), info.AllowModifiers);
		}
	}
}
