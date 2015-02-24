using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace JewelClicker
{
    class Engine
    {
        private Jewel[,] jewels; 
        public const int NUM_ROWS = 10;
        public const int NUM_COLS = 10;

        private const int MS_PER_FRAME = 20;
        private double timeSinceLastFrame;

        public const int SCORE_HEIGHT = 50;
        private SpriteFont scoreFont;

        private int score, biggestGroupSize;
        private Jewel jewelInBiggestGroup;

        public Engine()
        {
            jewels = new Jewel[NUM_ROWS, NUM_COLS];
            this.timeSinceLastFrame = 0;
            this.score = 0;
            this.jewelInBiggestGroup = null;
            this.biggestGroupSize = 1;
        }
        
        public void OnMouseUp(MouseState mouseState)
        {
            Jewel clickedJewel = GetClickedJewel(mouseState.X, mouseState.Y);
            if (clickedJewel != null)
            {
                if (clickedJewel.isSpinning)
                {
                    // Change its color, and if it's now in a group, freeze it
                    clickedJewel.ChangeColor();
                    ArrayList group = GetJewelGroup(clickedJewel);
				    if (group.Count > 1){
                        foreach (Jewel jewel in group)
                        {
                            jewel.Freeze();
                        }
				    }
                    // If this is now the biggest group, update the score
				    if (group.Count > biggestGroupSize)
                    {
					    biggestGroupSize = group.Count;
					    score = biggestGroupSize * 1000;
					    // If there is a previous biggest, make it not solid
					    if (jewelInBiggestGroup != null){
						    ArrayList previousBiggestGroup = GetJewelGroup(jewelInBiggestGroup);
						    foreach (Jewel jewel in previousBiggestGroup){
                                jewel.isSolid = false;
                                jewel.isInBiggestGroup = false;
						    }
					    }
					    // Make the new biggest group solid
					    foreach (Jewel jewel in group){
                            jewel.isSolid = true;
                            jewel.isInBiggestGroup = true;
					    }
					    jewelInBiggestGroup = clickedJewel;
				    }
                }
            }
        }

        public void LoadScoreFont(ContentManager content)
        {
            scoreFont = content.Load<SpriteFont>("ScoreFont");
        }

        private Jewel GetClickedJewel(int mouseX, int mouseY)
        {
            int x = mouseX / Jewel.WIDTH;
            int y = mouseY / (Jewel.HEIGHT + Jewel.VERTICAL_PADDING) ;
            if (x < NUM_COLS && y < NUM_ROWS)
            {
                return jewels[x, y];
            }
            else return null;
        }

        public void UpdateJewels(GameTime gameTime)
        {            
            timeSinceLastFrame += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeSinceLastFrame >= MS_PER_FRAME)
            {
                foreach (Jewel jewel in jewels)
                {
                    jewel.Update(gameTime);
                }
                timeSinceLastFrame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawJewels(spriteBatch);
            DrawScore(spriteBatch);
        }

        private void DrawJewels(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (Jewel jewel in jewels)
            {
                jewel.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        public void MakeJewels()
        {
            for (int i = 0; i < NUM_ROWS; i++)
            {
                for (int j = 0; j < NUM_COLS; j++)
                {
                    Jewel jewel = new Jewel(i, j);
                    jewels[i, j] = jewel;
                }
            }
        }

        // Return the group of jewels, including this one, by searching recursively
        // A jewel is in the group if it is next to a jewel in this group, and is the same color.
        public ArrayList GetJewelGroup(Jewel jewel)
        {
            ArrayList jewelGroup = new ArrayList();
            GetGroup(jewel, jewelGroup);
            return jewelGroup;
        }

        // Second parameter is an array to store the results of the recursive function in.
        private void GetGroup(Jewel current, ArrayList jewelGroup)
        {
            if (!jewelGroup.Contains(current))
                jewelGroup.Add(current);
            // If it's already in the group, we've checked this one already
            else
                return;
            if (current.y > 0)
            {
                Jewel up = jewels[current.x, current.y - 1];
                if (current.color == up.color)
                {
                    GetGroup(up, jewelGroup);
                }
            }
            if (current.y < (Engine.NUM_ROWS - 1))
            {
                Jewel down = jewels[current.x, current.y + 1];
                if (current.color == down.color)
                {
                    GetGroup(down, jewelGroup);
                }
            }
            if (current.x > 0)
            {
                Jewel left = jewels[current.x - 1, current.y];
                if (current.color == left.color)
                {
                    GetGroup(left, jewelGroup);
                }
            }
            if (current.x < (Engine.NUM_COLS - 1))
            {
                Jewel right = jewels[current.x + 1, current.y];
                if (current.color == right.color)
                {
                    GetGroup(right, jewelGroup);
                }
            }
        }

        private void DrawScore(SpriteBatch spriteBatch)
        {
            int x = 5;
            int y = NUM_ROWS * (Jewel.HEIGHT + Jewel.VERTICAL_PADDING) + 10;
            spriteBatch.Begin();
            spriteBatch.DrawString(scoreFont, "Score: " + score, new Vector2(x, y), Color.Yellow);
            spriteBatch.End();
        }
    }
}
