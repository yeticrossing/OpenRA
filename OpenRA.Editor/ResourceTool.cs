﻿#region Copyright & License Information
/*
 * Copyright 2007-2011 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System;
using OpenRA.FileFormats;

using SGraphics = System.Drawing.Graphics;

namespace OpenRA.Editor
{
	class ResourceTool : ITool
	{
		ResourceTemplate resourceTemplate;

		public ResourceTool(ResourceTemplate resource) { resourceTemplate = resource; }

		public void Apply(Surface surface)
		{
			surface.Map.MapResources.Value[surface.GetBrushLocation().X, surface.GetBrushLocation().Y]
				= new TileReference<byte, byte>
				{
					Type = (byte)resourceTemplate.Info.ResourceType,
					Index = (byte)random.Next(resourceTemplate.Info.SpriteNames.Length)
				};

			var ch = new int2(surface.GetBrushLocation().X / Surface.ChunkSize,
				surface.GetBrushLocation().Y / Surface.ChunkSize);

			if (surface.Chunks.ContainsKey(ch))
			{
				surface.Chunks[ch].Dispose();
				surface.Chunks.Remove(ch);
			}
		}

		public void Preview(Surface surface, SGraphics g)
		{
			surface.DrawImage(g, resourceTemplate.Bitmap, surface.GetBrushLocation(), false, null);
		}

		Random random = new Random();
	}
}
