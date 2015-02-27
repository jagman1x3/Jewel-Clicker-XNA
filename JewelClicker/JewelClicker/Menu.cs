using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JewelClicker
{
    class Menu
    {
        private MenuButton[] buttons;

        public Menu(int numButtons)
        {
            buttons = new MenuButton[numButtons];
        }
            
        public void LoadButtons(ContentManager content, GraphicsDeviceManager graphics)
        {
            foreach (MenuButton b in buttons)
            {
                b.LoadTextures(content, graphics);
            }
        }

        public void UnloadButtons()
        {
            foreach (MenuButton b in buttons)
            {
                b.UnloadTextures();
            }
        }

        public void DrawButtons(SpriteBatch sb)
        {
            foreach (MenuButton b in buttons)
            {
                b.Draw(sb);
            }
        }
     
    }

    class MenuButton
    {
        private static Texture2D blackTexture;
        private String text;
        private Color color, clickedColor;
        private int x, y, width, height;
        private Texture2D texture, clickedTexture;

        public MenuButton(String t, Color c, int x, int y, int w, int h)
        {
            text = t;
            color = c;
            width = w;
            height = h;
            this.x = x;
            this.y = y;
        }

        public void LoadTextures(ContentManager content, GraphicsDeviceManager graphics)
        {
            texture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new [] { color });
            clickedTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            clickedTexture.SetData<Color>(new Color[] { clickedColor });
            if (blackTexture == null)
            {
                blackTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
                blackTexture.SetData<Color>(new Color[] { clickedColor });
            }
        }

        public void UnloadTextures()
        {
            texture.Dispose();
            clickedTexture.Dispose();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(blackTexture, new Rectangle(x - 1, y - 1, width + 2, width + 2), Color.White);
            sb.Draw(texture, new Rectangle(x, y, width, height), Color.White);
            sb.End();
        }
    }

}
