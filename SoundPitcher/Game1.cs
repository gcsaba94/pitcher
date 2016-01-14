using Microsoft.Xna.Framework;
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
        StreamWriter sw;
        Thread thread;
        System.Timers.Timer t;
        string s;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Exiting += Game1_Exiting;
            soundEffect = Content.Load<SoundEffect>("blurp");
            sundEffectInstance = soundEffect.CreateInstance();
            soundEffect2 = Content.Load<SoundEffect>("blurp");
            sundEffectInstance2 = soundEffect2.CreateInstance();
            sw = new StreamWriter("port.txt");
            //thread = new Thread(new ThreadStart(Kiir));
            t = new System.Timers.Timer(20);
            //t.Elapsed += new System.Timers.ElapsedEventHandler(Kiir);
            t.Elapsed += new System.Timers.ElapsedEventHandler(Play);
        }

        public void Kiir(object source, ElapsedEventArgs e)
        {
            s = serialPort.ReadLine();
            sw.Write(s+"\r");
            sw.Flush();
        }

        public void Play(object source, ElapsedEventArgs e)
        {
            try
            {
                /*thread = new Thread(new ThreadStart(play_thread));
                thread.Start();*/
                play_thread();
            }
            catch{ }
        }

        void play_thread()
        {
            s = serialPort.ReadLine();
            string[] data = s.Split('#');
            if (data[0] != "")
            {
                //sundEffectInstance.Volume = 1.0f;
                soundEffect.Play();
                sundEffectInstance.Play();
                sundEffectInstance.Pitch = (float.Parse(data[0]) - 500f) / 500f;
            }
            else
            {
                //sundEffectInstance.Volume = 0f;
                sundEffectInstance.Stop();
            }
            if (data[1] != "\r")
            {
                //sundEffectInstance2.Volume = 1.0f;
                soundEffect2.Play();
                sundEffectInstance2.Play();
                sundEffectInstance2.Pitch = (float.Parse(data[1]) - 500f) / 500f;
            }
            else
            {
                //sundEffectInstance2.Volume = 0f;
                sundEffectInstance2.Stop();
            }
        }

        private void Game1_Exiting(object sender, System.EventArgs e)
        {
            sw.Close();
            sw.Dispose();
            if(serialPort.IsOpen)
                serialPort.Close();
            /*if (thread.IsAlive)
                thread.Abort();*/
            t.Stop();
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
            SoundEffect.MasterVolume = 1.0f;
            sundEffectInstance.IsLooped = true;
            sundEffectInstance2.IsLooped = true;
            sundEffectInstance.Volume = 1.0f;
            sundEffectInstance2.Volume = 1.0f;
            t.Enabled = true;
            t.Start();

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

            base.Draw(gameTime);
        }
    }
}
