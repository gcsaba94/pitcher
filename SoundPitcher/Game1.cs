﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Timers;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace SoundPitcher
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SerialPort serialPort;
        SoundEffect soundEffect, soundEffect2;
        SoundEffectInstance sundEffectInstance, sundEffectInstance2;
        string[] data;
        string s;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Exiting += Game1_Exiting;
            soundEffect = Content.Load<SoundEffect>("Aah-E");
            sundEffectInstance = soundEffect.CreateInstance();
            soundEffect2 = Content.Load<SoundEffect>("piano");
            sundEffectInstance2 = soundEffect2.CreateInstance();
        }


        private void Game1_Exiting(object sender, System.EventArgs e)
        {
            if(serialPort.IsOpen)
                serialPort.Close();
            sundEffectInstance.Stop();
            sundEffectInstance2.Stop();
            soundEffect = soundEffect2 = null; sundEffectInstance = sundEffectInstance2 = null;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            serialPort = new SerialPort("COM6",9600);
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;
            serialPort.Open();
            //thread.Start();
            SoundEffect.MasterVolume = 0.7f;
            sundEffectInstance.IsLooped = true;
            sundEffectInstance2.IsLooped = true;
            sundEffectInstance.Volume = 0.7f;
            sundEffectInstance2.Volume = 0.7f;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            try
            {
                s = serialPort.ReadLine();
                data = s.Split('#');
                if (data.Length > 1)
                {
                    if (data[0].Length > 0)
                    {
                        //sundEffectInstance.Volume = 1.0f;
                        sundEffectInstance.Pitch = (float.Parse(data[0]) - 1010f) / 1010f;
                        //soundEffect.Play();
                        sundEffectInstance.Play();
                    }
                    else
                    {
                        //sundEffectInstance.Volume = 0f;
                        sundEffectInstance.Pause();
                    }
                    if (data[1].Length > 1)
                    {
                        //sundEffectInstance2.Volume = 1.0f;
                        sundEffectInstance2.Pitch = (float.Parse(data[1]) - 1010f) / 1010f;
                        //soundEffect2.Play();
                        sundEffectInstance2.Play();
                    }
                    else
                    {
                        //sundEffectInstance2.Volume = 0f;
                        sundEffectInstance2.Pause();
                    }
                }
            }
            catch { }
            base.Draw(gameTime);
        }
    }
}
