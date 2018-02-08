using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Mojehra
{
    public class Button
    {
        int buttonX, buttonY;
        string Name;
        Texture2D Texture; Rectangle rect;

        public int ButtonX
        {
            get
            {
                return buttonX;
            }
        }

        public int ButtonY
        {
            get
            {
                return buttonY;
            }
        }

        public Button(string name, Texture2D texture, int buttonX, int buttonY)
        {
            this.Name = name;
            this.Texture = texture;
            this.buttonX = buttonX;
            this.buttonY = buttonY;
            rect = new Rectangle((int)ButtonX, (int)ButtonY, Texture.Width, Texture.Height);
        }

        /**
         * @return true: If a player enters the button with mouse
         */
        public bool enterButton(MouseState mouse)
        {
            if (mouse.X < buttonX + Texture.Width &&
                mouse.X > buttonX &&
                mouse.Y < buttonY + Texture.Height &&
                mouse.Y > buttonY)
            {
                return true;
            }
            return false;
        }

        public void Update(GameTime gameTime, MouseState mouse)
        {
            if (enterButton(mouse) && mouse.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed)
            {
                switch (Name)
                {
                    case "buy_normal_fish": //the name of the button
                        int i=0; //jen priklad
                        if (i >= 10)
                        {
                            //do shit
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, rect, Color.White);
        }
    }
}