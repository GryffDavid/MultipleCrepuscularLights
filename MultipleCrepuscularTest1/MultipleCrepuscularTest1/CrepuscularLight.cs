using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MultipleCrepuscularTest1
{
    class CrepuscularLight
    {
        public Vector2 Position;
        public float Decay, Exposure, Density, Weight;
        public int NumSamples = 120;
    }
}
