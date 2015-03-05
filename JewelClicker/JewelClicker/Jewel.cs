using System;
using System.Collections;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace JewelClicker
{
    class Jewel : DrawableGameComponent
    {
        public const int HEIGHT = 32;
        public const int WIDTH = 32;
        public const int VERTICAL_PADDING = 6;

        private const int NUM_FRAMES = 8;

        private static readonly String[] COLORS = { "blue", "green", "grey", "orange", "pink", "yellow" };
        private static Dictionary<String, Texture2D> jewelImages;
        private static Random rand;

        private Texture2D image;
        private int frameIndex, xOffset, yOffset;
 
        public int x, y;
        public String color;
        public bool isSpinning, isInBiggestGroup, isSolid, toDelete;

        private SpriteBatch spriteBatch;

        public Jewel(Game game) : base(game)
        {
            this.frameIndex = 0;
            this.color = RandomColor();
            this.isSpinning = true;
            this.isInBiggestGroup = false;
            this.isSolid = false;
            this.toDelete = false;
            this.image = jewelImages[color];
            this.spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
        }

        public void setOffset()
        {
            this.xOffset = WIDTH * this.x;
            this.yOffset = (HEIGHT + VERTICAL_PADDING) * this.y;
        }

        public static void LoadJewelImages(ContentManager content)
        {
            if (jewelImages == null)
            {
                jewelImages = new Dictionary<string, Texture2D>();
            }
            foreach (String color in COLORS)
            {
                Texture2D jewelImage = content.Load<Texture2D>("Jewels/" + color);
                jewelImages.Add(color, jewelImage);
            }       
        }


        public override void Draw(GameTime gameTime)
        {
            Rectangle spriteRect = new Rectangle(frameIndex * WIDTH, 0, WIDTH, HEIGHT);
            spriteBatch.Begin();
            spriteBatch.Draw(image, new Vector2(xOffset, yOffset), spriteRect, Color.White);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (isSpinning)
            {
                frameIndex++;
                if (frameIndex >= NUM_FRAMES)
                {
                    frameIndex = 0;
                }
            }
        }
        

        public void ChangeColor()
        {
            String newColor;
	        // loop until it gets a new color
            do
            {
                newColor = RandomColor();
            } while (newColor == color);
	        color = newColor;
            image = jewelImages[color];
        }

        private String RandomColor()
        {
            if (rand == null)
            {
                rand = new Random();
            }
            int randColor = rand.Next(COLORS.Length);
            return COLORS[randColor];
        }

        public void Freeze()
        {
            isSpinning = false;
            frameIndex = 0;
        } 
    }
}
