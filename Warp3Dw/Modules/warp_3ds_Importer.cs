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
using System.Collections;
using System.IO;
using System.Net;

namespace Warp3Dw
{
	/// <summary>
	/// Summary description for warp_3ds_Importer.
	/// </summary>
	public class warp_3ds_Importer
	{
		int currentJunkId;
		int nextJunkOffset;

		string currentObjectName;
		warp_Object currentObject;
		bool endOfStream;

        Hashtable _objects = new Hashtable();

		public warp_3ds_Importer()
		{
		}

        public Hashtable importFromFile( string name, string path )
		{
            Stream fs;
            _objects.Clear();

            if (path.StartsWith ("http", StringComparison.OrdinalIgnoreCase))
            {
                WebRequest webrq = WebRequest.Create( path );
                var response = webrq.GetResponse ();
                fs = response.GetResponseStream() ;
                response.Dispose ();
            }
            else
            {
                fs = new FileStream( path, FileMode.Open );
            }

			BinaryReader br = new BinaryReader( fs );
			return importFromStream( name, br );
		}

        public Hashtable importFromStream( string name, BinaryReader inStream )
		{
            _objects.Clear();

			readJunkHeader(inStream);
			if (currentJunkId != 0x4D4D)
			{
				return null;
			}

			try
			{
				for (;;)
				{
					readNextJunk( name, inStream);
				}
			}
			catch(Exception)
			{
                // ignored
			}

            inStream.Close();

            return _objects;
		}

		void readJunkHeader(BinaryReader inStream)
		{
			currentJunkId = readShort(inStream);
			nextJunkOffset = readInt(inStream);
			endOfStream = currentJunkId < 0;
		}

		int readInt(BinaryReader inStream)
		{
			return inStream.ReadByte() | (inStream.ReadByte() << 8) | (inStream.ReadByte() << 16) | (inStream.ReadByte() << 24);
		}

		int readShort(BinaryReader inStream)
		{
			return (inStream.ReadByte() | (inStream.ReadByte() << 8));
		}

		void readNextJunk(string name, BinaryReader inStream)
		{
			readJunkHeader(inStream);

			if (currentJunkId == 0x3D3D /* mesh block */)
			{
				return;
			}

			if (currentJunkId == 0x4000 /* object block */)
			{
				currentObjectName = readString(inStream);
				return;
			}

            if (currentJunkId == 0x4100 /* triangular polygon object */)
            {
                currentObject = new warp_Object();
                _objects.Add(name + "_" + currentObjectName, currentObject);

				return;
			}

			if (currentJunkId == 0x4110 /* vertex list */)
			{
				readVertexList(inStream);
				return;
			}

			if (currentJunkId == 0x4120 /* point list */)
			{
				readPointList(inStream);
				return;
			}

			if (currentJunkId == 0x4140 /* mapping coordinates */)
			{
				readMappingCoordinates(inStream);
				return;
			}

			skipJunk(inStream);
		}

		string readString(BinaryReader inStream)
		{
			string result = "";
			byte inByte;
			while ( (inByte = inStream.ReadByte ()) != 0)
			{
				result += (char) inByte;
			}

			return result;
		}

		float readFloat(BinaryReader inStream)
		{
			int bits = readInt(inStream);

			int s = ((bits >> 31) == 0) ? 1 : -1;
			int e = ((bits >> 23) & 0xff);
			int m = (e == 0) ? (bits & 0x7fffff) << 1 :	(bits & 0x7fffff) | 0x800000;

            double v = s * (double)m * (Math.Pow(2, e - 150));

            return (float)v;
        }

        void skipJunk(BinaryReader inStream)
        {
            try {
                for (int i = 0; (i < nextJunkOffset - 6) && (!endOfStream); i++) {
                    endOfStream = inStream.ReadByte () < 0;
                }
            } catch (Exception) {
                endOfStream = true;
            }

        }

		void readVertexList(BinaryReader inStream)
		{
			float x, y, z;
			int vertices = readShort(inStream);
			for (int i = 0; i < vertices; i++)
			{
				x = readFloat(inStream);
				y = readFloat(inStream);
				z = readFloat(inStream);

				currentObject.addVertex(new warp_Vector(x, -y, z));
			}
		}

		void readPointList(BinaryReader inStream)
		{
			int v1, v2, v3;
			int triangles = readShort(inStream);
			for (int i = 0; i < triangles; i++)
			{
				v1 = readShort(inStream);
				v2 = readShort(inStream);
				v3 = readShort(inStream);

				readShort(inStream);

				currentObject.addTriangle(currentObject.vertex(v1),
										  currentObject.vertex(v2),
										  currentObject.vertex(v3));
			}
		}

		void readMappingCoordinates(BinaryReader inStream)
		{
			int vertices = readShort(inStream);
			for (int i = 0; i < vertices; i++)
			{
				currentObject.vertex(i).u = readFloat(inStream);
				currentObject.vertex(i).v = readFloat(inStream);
			}
		}
	}
}
