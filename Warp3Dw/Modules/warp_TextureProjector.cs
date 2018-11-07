/*
 * Copyright (c) Peter Walser and Dean Lunz
 * All rights reserved.
 *
 * Licensed under the Creative Commons Attribution Share-Alike 2.5 Canada
 * license: http://creativecommons.org/licenses/by-sa/2.5/ca/
 * 
 * Revised and renamed for WhiteCore-Sim, https://whitecore-sim.org
 *  2014 - 2018
 * Greythane:  greythane@gmail.com
 */

namespace Warp3Dw
{
	/// <summary>
	/// Summary description for warp_TextureProjector.
	/// </summary>
	public class warp_TextureProjector
	{
		public static void projectFrontal (warp_Object obj)
		{
			obj.rebuild ();
			warp_Vector min = obj.minimum ();
			warp_Vector max = obj.maximum ();
			float du = 1 / (max.x - min.x);
			float dv = 1 / (max.y - min.y);
		
			for (int i = 0; i < obj.vertices; i++)
			{
				obj.fastvertex [i].u = (obj.fastvertex [i].pos.x - min.x) * du;
				obj.fastvertex [i].v = 1 - (obj.fastvertex [i].pos.y - min.y) * dv;
			}
		}

		public static void projectTop (warp_Object obj)
		{
			obj.rebuild ();
			warp_Vector min = obj.minimum ();
			warp_Vector max = obj.maximum ();
			float du = 1 / (max.x - min.x);
			float dv = 1 / (max.z - min.z);
			for (int i = 0; i < obj.vertices; i++)
			{
				obj.fastvertex [i].u = (obj.fastvertex [i].pos.x - min.x) * du;
				obj.fastvertex [i].v = (obj.fastvertex [i].pos.z - min.z) * dv;
			}
		}
		public static void projectCylindric(warp_Object obj)
		{
			obj.rebuild();
			warp_Vector min = obj.minimum();
			warp_Vector max = obj.maximum();
			float dz = 1 / (max.z - min.z);
			for (int i = 0; i < obj.vertices; i++)
			{
				obj.fastvertex[i].pos.buildCylindric();
				obj.fastvertex[i].u = obj.fastvertex[i].pos.theta / (2 * 3.14159265f);
				obj.fastvertex[i].v = (obj.fastvertex[i].pos.z - min.z) * dz;
			}
		}		
	}
}
