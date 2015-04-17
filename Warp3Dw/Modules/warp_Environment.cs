/*
 * Copyright (c) Peter Walser and Dean Lunz
 * All rights reserved.
 *
 * Licensed under the Creative Commons Attribution Share-Alike 2.5 Canada
 * license: http://creativecommons.org/licenses/by-sa/2.5/ca/
 * 
 * Revised and renamed for WhiteCore-Sim, https://whitecore-sim.org
 *  2014, 2015
 * Greythane:  greythane@gmail.com
 */


namespace Warp3Dw
{
	/// <summary>
	/// Summary description for warp_Environment.
	/// </summary>
	public class warp_Environment
	{
		public int ambient = 0;
		public int fogcolor = 0;
		public int fogfact = 0;
		public int bgcolor = unchecked((int)0xffffffff);

		public warp_Texture background;

		public void setBackground(warp_Texture t)
		{
			background=t;
		}
	}
}
