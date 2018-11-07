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
	public struct warp_Vector
	{
		public float x;		//Cartesian (default)
		public float y;		//Cartesian (default)
		public float z;		//Cartesian (default)
		public float r;      //Cylindric
		public float theta;  //Cylindric
		
		public warp_Vector (float xpos, float ypos, float zpos)
		{
			x = xpos;
			y = ypos;
			z = zpos;
            r = 0;
            theta = 0;
		}

		public warp_Vector normalize ()
			// Normalizes the vector
		{
			float dist = length ();
            if (warp_Math.FloatApproxEqual(dist, 0f))
				return this;
			float invdist = 1f / dist;
			x *= invdist;
			y *= invdist;
			z *= invdist;
			return this;
		}

		public warp_Vector reverse ()
			// Reverses the vector
		{
			x = -x;
			y = -y;
			z = -z;
			return this;
		}

		public float length ()
			// Lenght of this vector
		{
			return (float)Math.Sqrt (x * x + y * y + z * z);
		}

		public warp_Vector transform (warp_Matrix m)
			// Modifies the vector by matrix m
		{
			float newx = x * m.m00 + y * m.m01 + z * m.m02 + m.m03;
			float newy = x * m.m10 + y * m.m11 + z * m.m12 + m.m13;
			float newz = x * m.m20 + y * m.m21 + z * m.m22 + m.m23;

			return new warp_Vector (newx, newy, newz);
		}

		public void buildCylindric()
			// Builds the cylindric coordinates out of the given cartesian coordinates
		{
			r=(float)Math.Sqrt(x*x+y*y);
			theta=(float)Math.Atan2(x,y);
		}

		public void buildCartesian()
			// Builds the cartesian coordinates out of the given cylindric coordinates
		{
			x=r*warp_Math.cos(theta);
			y=r*warp_Math.sin(theta);
		}
		public static warp_Vector getNormal (warp_Vector a, warp_Vector b)
			// returns the normal vector of the plane defined by the two vectors
		{
			return vectorProduct (a, b).normalize ();
		}

		public static warp_Vector getNormal (warp_Vector a, warp_Vector b, warp_Vector c)
			// returns the normal vector of the plane defined by the two vectors
		{
			return vectorProduct (a, b, c).normalize ();
		}

		public static warp_Vector vectorProduct (warp_Vector a, warp_Vector b)
			// returns a x b
		{
			return new warp_Vector (a.y * b.z - b.y * a.z, a.z * b.x - b.z * a.x, a.x * b.y - b.x * a.y);
		}

		public static warp_Vector vectorProduct (warp_Vector a, warp_Vector b, warp_Vector c)
			// returns (b-a) x (c-a)
		{
			float ax = b.x - a.x;
			float ay = b.y - a.y;
			float az = b.z - a.z;

			float bx = c.x - a.x;
			float by = c.y - a.y;
			float bz = c.z - a.z;

			return new warp_Vector (ay * bz - by * az, az * bx - bz * ax, ax * by - bx * ay);
		}

		public static float angle (warp_Vector a, warp_Vector b)
			// returns the angle between 2 vectors
		{
			a.normalize ();
			b.normalize ();
			return (a.x * b.x + a.y * b.y + a.z * b.z);
		}

		public static warp_Vector add (warp_Vector a, warp_Vector b)
			// adds 2 vectors
		{
			a.x += b.x;
			a.y += b.y;
			a.z += b.z;

			return a;    // maybe should return a new vector?
		}

		public static warp_Vector sub (warp_Vector a, warp_Vector b)
			// substracts 2 vectors
		{
			a.x -= b.x;
			a.y -= b.y;
			a.z -= b.z;

			return a;    // maybe should return a new vector?
		}

		public static warp_Vector scale (float f, warp_Vector a)
			// substracts 2 vectors
		{
			a.x *= f;
			a.y *= f;
			a.z *= f;

			return a;    // maybe should return a new vector?
		}

		public static float len(warp_Vector a)
			// length of vector
		{
			return (float)Math.Sqrt(a.x*a.x+a.y*a.y+a.z*a.z);
		}
		public static readonly warp_Vector Zero = new warp_Vector ();
		public static readonly warp_Vector UnitX = new warp_Vector (1f, 0f, 0f);
		public static readonly warp_Vector UnitY = new warp_Vector (0f, 1f, 0f);
		public static readonly warp_Vector UnitZ = new warp_Vector (0f, 0f, 1f);
	}
}
