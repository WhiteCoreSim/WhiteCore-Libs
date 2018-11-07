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

using System;

namespace Warp3Dw
{
	/// <summary>
	/// Summary description for warp_Lightmap.
	/// </summary>
	public class warp_Lightmap
	{
		public int[] diffuse = new int[65536];
		public int[] specular = new int[65536];

        readonly float [] sphere = new float [65536];
        readonly warp_Light [] light;
        readonly int lights;
        readonly int ambient;
        // int temp, overflow, color, r, g, b;
        int pos;

		public warp_Lightmap (warp_Scene scene)
		{
			scene.rebuild ();

			light = scene.light;
			lights = scene.lights;
			ambient = scene.environment.ambient;

			buildSphereMap ();
			rebuildLightmap ();
		}

		void buildSphereMap ()
		{
			float fnx, fny, fnz;
            int shperePos;
			for (int ny = -128; ny < 128; ny++)
			{
				fny = (float)ny / 128;
				for (int nx = -128; nx < 128; nx++)
				{
					shperePos = nx + 128 + ((ny + 128) << 8);
					fnx = (float)nx / 128;
					fnz = (float)(1 - Math.Sqrt (fnx * fnx + fny * fny));
					sphere [shperePos] = (fnz > 0) ? fnz : 0;
				}
			}
		}

		public void rebuildLightmap ()
		{
            //System.Console.WriteLine(">> Rebuilding Light Map  ...  [" + lights + " light sources]");

            // not used -greythane- 20160707 // warp_Vector l;
            float fnx, fny, phongfact, sheen, spread;
            int lmDiffuse, lmSpecular, cos, dr, dg, db, sr, sg, sb;
			for (int ny = -128; ny < 128; ny++)
			{
				fny = (float)ny / 128;
				for (int nx = -128; nx < 128; nx++)
				{
					pos = nx + 128 + ((ny + 128) << 8);
					fnx = (float)nx / 128;
					sr = sg = sb = 0;

					dr = warp_Color.getRed (ambient);
					dg = warp_Color.getGreen (ambient);
					db = warp_Color.getBlue (ambient);

					for (int i = 0; i < lights; i++)
					{
						// not used -greythane- 20160707 //l = light [i].v;
						lmDiffuse = light [i].diffuse;
						lmSpecular = light [i].specular;
						sheen = light [i].highlightSheen / 255f;
						spread = light [i].highlightSpread / 4096f;
						spread = (spread < 0.01f) ? 0.01f : spread;
						cos = (int)(255 * warp_Vector.angle (light [i].v, new warp_Vector (fnx, fny, sphere [pos])));
						cos = (cos > 0) ? cos : 0;
						dr += (warp_Color.getRed (lmDiffuse) * cos) >> 8;
						dg += (warp_Color.getGreen (lmDiffuse) * cos) >> 8;
						db += (warp_Color.getBlue (lmDiffuse) * cos) >> 8;
						phongfact = sheen *	(float)Math.Pow (cos / 255f, 1 / spread);
						sr += (int)(warp_Color.getRed (lmSpecular) * phongfact);
						sg += (int)(warp_Color.getGreen (lmSpecular) * phongfact);
						sb += (int)(warp_Color.getBlue (lmSpecular) * phongfact);
					}
					diffuse [pos] = warp_Color.getCropColor (dr, dg, db);
					specular [pos] = warp_Color.getCropColor (sr, sg, sb);
				}
			}
		}
	}
}
