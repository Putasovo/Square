using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mojehra
{
    /// <remarks>
    /// A number tile
    /// </remarks>
    class NumberTile
    {
        #region Fields

        // original length of each side of the tile
        int originalSideLength;

        // whether or not this tile is the correct number
        bool isCorrectNumber;

        // drawing support
        string konstrTextura;
        Texture2D texture;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;

        // Increment 5: field for blinking tile texture
        private Texture2D blinkTexture;
        private Texture2D winTexture;

        // Increment 5: field for current texture
        private Texture2D currentTexture;

        // blinking support
        const int TotalBlinkMilliseconds = 4000;
        int elapsedBlinkMilliseconds = 0;
        const int FrameBlinkMilliseconds = 1000;
        int elapsedFrameMilliseconds = 0;

        // Increment 4: fields for shrinking support
        const int TotalShrinkMilliseconds = 4000;
        int elapsedShrinkMilliseconds = 0;

        // Increment 4: fields to keep track of visible, blinking, and shrinking
        public bool _blinking = false;
        private bool _isBlinking
        {
            get            {                return _blinking;            }
            set            {                _blinking = value;            }
        }

        private bool _visible = true;
        private bool _isVisible
        {
            get            {                return _visible;            }
            set            {                _visible = value;            }
        }

        private bool _shrinking;
        private bool _isShrinking
        {
            get            { return _shrinking;            }
            set            { _shrinking = value;            }
        }

        // Increment 4: fields for click support
        private ButtonState _oldMouseState = ButtonState.Released;

        // Increment 5: sound effect field
        SoundEffect tileSound;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="center">the center of the tile</param>
        /// <param name="sideLength">the side length for the tile</param>
        /// <param name="number">the number for the tile</param>
        /// <param name="correctNumber">the correct number</param>
        public NumberTile(ContentManager contentManager, Vector2 center, int sideLength,
            int number, int correctNumber, string konstrTextura)
        {
            // set original side length field
            originalSideLength = sideLength;

            // load content for the tile and create draw rectangle
            LoadContent(contentManager, number, konstrTextura);
            drawRectangle = new Rectangle((int)center.X - sideLength /2 ,
                 (int)center.Y - sideLength / 2, sideLength, sideLength );
           
            // set isCorrectNumber flag
            isCorrectNumber = number == correctNumber;

            // Increment 5: load sound effect field to correct or incorrect sound effect
            // based on whether or not this tile is the correct number
            if (isCorrectNumber)
            {
                tileSound = contentManager.Load<SoundEffect>(@"audio/explosion");
            }
            else
            {
                tileSound = contentManager.Load<SoundEffect>(@"audio/loser");
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the tile based on game time and mouse state
        /// </summary>
        /// <param name="gameTime">the current GameTime</param>
        /// <param name="mouse">the current mouse state</param>
        /// <return>true if the correct number was guessed, false otherwise</return>
        public bool Update(GameTime gameTime, MouseState mouse)
        {
            // Increments 4 and 5: add code for shrinking and blinking support
            if (_blinking)
            {
                elapsedBlinkMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedBlinkMilliseconds > TotalBlinkMilliseconds)
                {
                    // _visible = false;
                    return true;
                    //elapsedBlinkMilliseconds = 0;
                }
                else    //do the blinking
                {
                    elapsedFrameMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                    if (elapsedFrameMilliseconds / 103 % 2 == 1)
                    {
                        sourceRectangle.X = texture.Width / 2;
                    }
                    else
                    {
                        sourceRectangle.X = 0;
                    }
                }
            }
            else if (_shrinking)
            {
                elapsedShrinkMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                //drawRectangle.Width = (int)(originalSideLength - ( (float)TotalShrinkMilliseconds /
                //    MathHelper.Clamp(
                //        (TotalShrinkMilliseconds - elapsedShrinkMilliseconds), 0, TotalShrinkMilliseconds )
                //    ));
                drawRectangle.Width -= 2;
                if (drawRectangle.Width > 0)
                {
                    
                    int delta = (drawRectangle.Height - drawRectangle.Width) / 2;
                    drawRectangle.X += delta;
                    drawRectangle.Y += delta;
                    drawRectangle.Height = drawRectangle.Width;
                }
                else
                {
                    _visible = false;
                    _shrinking = false;
                }
            }
            else
            {
                // Increment 4: add code to highlight/unhighlight the tile
                if (drawRectangle.Contains(mouse.Position) )
                {
                    sourceRectangle.X = texture.Width / 2;

                    if ( mouse.LeftButton == ButtonState.Released && _oldMouseState == ButtonState.Pressed )
                    {

                        if (isCorrectNumber)
                        {
                            tileSound.Play();
                            sourceRectangle.X = 0;
                            _blinking = true;
                            currentTexture = blinkTexture;
                        }
                        else
                        {
                            tileSound.Play();
                            sourceRectangle.X = 0;
                            _shrinking = true;
                        }
                    }
                }
                else
                {
                    sourceRectangle.X = 0;
                }
            }
            _oldMouseState = mouse.LeftButton;

            // Increment 5: play sound effect

            // if we get here, return false
            return false;
        }

        /// <summary>
        /// Draws the number tile
        /// </summary>
        /// <param name="spriteBatch">the SpriteBatch to use for the drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Increments 3, 4, and 5: draw the tile
            if (_isVisible)
            {
                spriteBatch.Draw(currentTexture, drawRectangle, sourceRectangle, Color.White);
            }
        }
        #endregion

        #region Private methods

        /// <summary>
        /// Loads the content for the tile
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="number">the tile number</param>
        private void LoadContent(ContentManager contentManager, int number, string konstrTextura)
        {
            texture = contentManager.Load<Texture2D>(@"gfx/" + konstrTextura);

            // convert the number to a string
            string numberString = ConvertIntToString(number);
                        
            // Increment 3: load content for the tile and set source rectangle
            //texture = contentManager.Load<Texture2D>(@"graphics/" + numberString);
            //sourceRectangle = new Rectangle(0, 0, texture.Width / 2, texture.Height / 1);
            sourceRectangle = new Rectangle(0, 0, 32, 32);
            // Increment 5: load blinking tile texture
            //blinkTexture = contentManager.Load<Texture2D>(@"graphics/blinking" + ConvertIntToString(number));
            // Increment 5: set current texture
            currentTexture = texture;
        }

        /// <summary>
        /// Converts an integer to a string for the corresponding number
        /// </summary>
        /// <param name="number">the integer to convert</param>
        /// <returns>the string for the corresponding number</returns>
        private String ConvertIntToString(int number)
        {
            switch (number)
            {
                case 1:
                    return "one";
                case 2:
                    return "two";
                case 3:
                    return "three";
                case 4:
                    return "four";
                case 5:
                    return "five";
                case 6:
                    return "six";
                case 7:
                    return "seven";
                case 8:
                    return "eight";
                case 9:
                    return "nine";
                default:
                    throw new Exception("Unsupported number for number tile");
            }
        }
        #endregion
    }
}