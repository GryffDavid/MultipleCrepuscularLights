using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MultipleCrepuscularTest1
{    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D Flare, Box, Background;
        RenderTarget2D OcclusionMap, CrepuscularColorMap, CrepuscularLightMap1, CrepuscularLightMap2, Buffer1, Buffer2;

        VertexPositionColorTexture[] CrepVertices;

        List<CrepuscularLight> CrepLightList = new List<CrepuscularLight>();

        Matrix Projection = Matrix.CreateOrthographicOffCenter(0, 1280, 720, 0, -10, 10);
        static Random Random = new Random();

        BasicEffect BasicEffect;
        Effect CrepuscularEffect;

        List<Sprite> SpriteList = new List<Sprite>();

        public static BlendState BlendBlack = new BlendState()
        {
            ColorBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.One,

            AlphaBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.One
        };


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

        }
        
        protected override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Flare = Content.Load<Texture2D>("Flare1");
            Box = Content.Load<Texture2D>("Box");
            Background = Content.Load<Texture2D>("Background");

            CrepLightList.Add(new CrepuscularLight()
            {
                Position = new Vector2(1280 / 2, 720 / 2),
                Decay = 0.9999f,
                Exposure = 0.23f,
                Density = 0.826f,
                Weight = 0.5358767f
            });

            CrepLightList.Add(new CrepuscularLight()
            {
                Position = new Vector2(200, 200),
                Decay = 0.9999f,
                Exposure = 0.23f,
                Density = 0.826f,
                Weight = 0.2358767f
            });

            CrepuscularEffect = Content.Load<Effect>("Crepuscular");
            CrepuscularEffect.Parameters["Projection"].SetValue(Projection);

            SpriteList = new List<Sprite>();

            for (int i = 0; i < 15; i++)
            {
                SpriteList.Add(new Sprite(new Vector2(Random.Next(50, 1270), Random.Next(50, 670)), new Vector2(64, 64), Box));
            }

            SpriteList.Add(new Sprite(new Vector2(180, 230), new Vector2(32, 32), Box));


            CrepuscularColorMap = new RenderTarget2D(GraphicsDevice, 1280, 720);
            CrepuscularLightMap1 = new RenderTarget2D(GraphicsDevice, 1280, 720);
            CrepuscularLightMap2 = new RenderTarget2D(GraphicsDevice, 1280, 720);

            Buffer1 = new RenderTarget2D(GraphicsDevice, 1280, 720);
            Buffer2 = new RenderTarget2D(GraphicsDevice, 1280, 720);

            OcclusionMap = new RenderTarget2D(GraphicsDevice, 1280, 720);


            CrepVertices = new VertexPositionColorTexture[4];
            CrepVertices[0] = new VertexPositionColorTexture(new Vector3(-1, 1, 0), Color.White, new Vector2(0, 0));
            CrepVertices[1] = new VertexPositionColorTexture(new Vector3(1, 1, 0), Color.White, new Vector2(1, 0));
            CrepVertices[2] = new VertexPositionColorTexture(new Vector3(-1, -1, 0), Color.White, new Vector2(0, 1));
            CrepVertices[3] = new VertexPositionColorTexture(new Vector3(1, -1, 0), Color.White, new Vector2(1, 1));

            //Projection = Matrix.CreateOrthographicOffCenter(0, 1280, 720, 0, -10, 10);

            BasicEffect = new BasicEffect(GraphicsDevice);
            BasicEffect.Projection = Projection;
            
        }
        
        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            CrepLightList[0].Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            #region Occlusion Map
            GraphicsDevice.SetRenderTarget(OcclusionMap);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            foreach (Sprite sprite in SpriteList)
            {
                sprite.Draw(spriteBatch, Color.Black);
            }
            spriteBatch.End(); 
            #endregion

            #region Crepuscular Color Map
            GraphicsDevice.SetRenderTarget(CrepuscularColorMap);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            foreach (Sprite sprite in SpriteList)
            {
                sprite.Draw(spriteBatch, Color.Black);
            }
            spriteBatch.End(); 
            #endregion

            #region Crepuscular Light Map1
            GraphicsDevice.SetRenderTarget(CrepuscularLightMap1);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            {
                CrepuscularLight light = CrepLightList[0];

                spriteBatch.Draw(Flare, new Rectangle((int)(light.Position.X), (int)(light.Position.Y), Flare.Width / 3, Flare.Height / 3), null,
                                 Color.Goldenrod, 0, new Vector2(Flare.Width / 2, Flare.Height / 2), SpriteEffects.None, 0);

            }
            spriteBatch.Draw(OcclusionMap, OcclusionMap.Bounds, Color.White);
            spriteBatch.End(); 
            #endregion

            #region Crepuscular Light Map2
            GraphicsDevice.SetRenderTarget(CrepuscularLightMap2);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            {
                CrepuscularLight light = CrepLightList[1];

                spriteBatch.Draw(Flare, new Rectangle((int)(light.Position.X), (int)(light.Position.Y), Flare.Width / 3, Flare.Height / 3), null,
                                 Color.Goldenrod, 0, new Vector2(Flare.Width / 2, Flare.Height / 2), SpriteEffects.None, 0);

            }
            spriteBatch.Draw(OcclusionMap, OcclusionMap.Bounds, Color.White);
            spriteBatch.End();
            #endregion

            #region Buffer1
            GraphicsDevice.SetRenderTarget(Buffer1);
            GraphicsDevice.Clear(Color.Transparent);
            {
                CrepuscularLight light = CrepLightList[0];

                CrepuscularEffect.Parameters["LightPosition"].SetValue(light.Position / new Vector2(1280, 720));
                CrepuscularEffect.Parameters["decay"].SetValue(light.Decay);
                CrepuscularEffect.Parameters["exposure"].SetValue(light.Exposure);
                CrepuscularEffect.Parameters["density"].SetValue(light.Density);
                CrepuscularEffect.Parameters["weight"].SetValue(light.Weight);
                CrepuscularEffect.Parameters["ColorMap"].SetValue(CrepuscularColorMap);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendBlack);
                CrepuscularEffect.CurrentTechnique.Passes[0].Apply();
                
                //CrepuscularLightMap1 is a combination of the flare sprite with the OcclusionMap drawn on top
                spriteBatch.Draw(CrepuscularLightMap1, CrepuscularLightMap1.Bounds, Color.White);
                spriteBatch.End(); 

            }
            #endregion

            #region Buffer2
            GraphicsDevice.SetRenderTarget(Buffer2);
            GraphicsDevice.Clear(Color.Transparent);
            {
                CrepuscularLight light = CrepLightList[1];

                CrepuscularEffect.Parameters["LightPosition"].SetValue(light.Position / new Vector2(1280, 720));
                CrepuscularEffect.Parameters["decay"].SetValue(light.Decay);
                CrepuscularEffect.Parameters["exposure"].SetValue(light.Exposure);
                CrepuscularEffect.Parameters["density"].SetValue(light.Density);
                CrepuscularEffect.Parameters["weight"].SetValue(light.Weight);
                CrepuscularEffect.Parameters["ColorMap"].SetValue(CrepuscularColorMap);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendBlack);
                CrepuscularEffect.CurrentTechnique.Passes[0].Apply();

                //CrepuscularLightMap2 is a combination of the flare sprite with the OcclusionMap drawn on top
                spriteBatch.Draw(CrepuscularLightMap2, CrepuscularLightMap2.Bounds, Color.White);
                spriteBatch.End();

            }
            #endregion


            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            spriteBatch.Draw(Buffer1, Buffer1.Bounds, Color.White);
            spriteBatch.Draw(Buffer2, Buffer1.Bounds, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
