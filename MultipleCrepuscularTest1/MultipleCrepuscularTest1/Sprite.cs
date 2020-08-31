using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MultipleCrepuscularTest1
{
    class Sprite
    {
        Texture2D Texture;
        Vector2 Position, Size;
        Rectangle DestinationRectangle;
        Color Color = Color.Black;

        public Sprite(Vector2 position, Vector2 size, Texture2D texture)
        {
            Texture = texture;
            Position = position;
            Size = size;
        }

        public void LoadContent(ContentManager content)
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), color);
        }
    }
}
