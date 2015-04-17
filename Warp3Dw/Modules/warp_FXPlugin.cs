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
    public abstract class warp_FXPlugin
    {
        public warp_Scene scene;
        public warp_Screen screen;

        public warp_FXPlugin( warp_Scene scene )
		{
			this.scene = scene;
			screen = scene.renderPipeline.screen;
		}
		
		public abstract void apply();
    }
}
