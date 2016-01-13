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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Exiting += Game1_Exiting;
            soundEffect = Content.Load<SoundEffect>("blurp");
            sundEffectInstance = soundEffect.CreateInstance();
            sw = new StreamWriter("port.txt");
            //thread = new Thread(new ThreadStart(Kiir));
            t = new System.Timers.Timer(100);
            t.Elapsed += new System.Timers.ElapsedEventHandler(Kiir);
            t.Elapsed += new System.Timers.ElapsedEventHandler(Play);
        }

        public void Kiir(object source, ElapsedEventArgs e)
        {
            string s = serialPort.ReadLine();
            sw.Write(s+"\r");
        }

        public void Play(object source, ElapsedEventArgs e)
        {
            try
            {
                string[] data = serialPort.ReadLine().Split('#');
                if(data[0] != "")
                {
                    sundEffectInstance.Pitch = (float.Parse(data[0])-515f)/515f;
                }
            }
            catch{}
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
            serialPort = new SerialPort("COM7",9600);
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;
            serialPort.Open();
            //thread.Start();
            soundEffect.Play();
            sundEffectInstance.Play();
            sundEffectInstance.Volume = 1f;
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
