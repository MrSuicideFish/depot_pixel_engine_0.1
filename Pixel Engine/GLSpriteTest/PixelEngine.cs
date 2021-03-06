﻿//INTERNAL
using System;
using System.Threading;
using System.Collections.Generic;

//XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//ENGINE
using PixelEngine.Engine;
using PixelEngine.Engine.UI;
using PixelEngine.Graphics;
using WorldManager = PixelEngine.Engine.World.WorldManager;

//PHYSICS
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.DebugView;

//EXTERNAL
using System.Windows.Forms;

//Input overrides
using ButtonState   = Microsoft.Xna.Framework.Input.ButtonState;
using Keys          = Microsoft.Xna.Framework.Input.Keys;

namespace PixelEngine
{
    public class PixelEngine : Game
    {
        //private DebugViewXNA DebugView;

        public GraphicsDeviceManager m_Graphics = null;
        public SpriteBatch m_SpriteBatch        = null;

        public static Camera GAME_CAMERA { get; private set; }

        /// <summary>
        /// PHYSICS
        /// </summary>
        public static World PHYSICS_WORLD { get; private set; }

        public delegate void PHYS_UPDATE( World _phyWorld );
        public static event PHYS_UPDATE OnPhysicsUpdate;

        //TESTING STUFF
        public SortedList<int, Texture2D> SpriteSheets { get; private set; }
        public SortedList<int, GameObject> SceneObjects = new SortedList<int, GameObject>( );

        public static PixelEngine ENGINE { get; private set; }

        public PixelEngine( )
        {
            Debug.Print( "Initializing Engine..." );

            m_Graphics = new GraphicsDeviceManager( this );
            Window.Title = "Pixel Engine 0.4a"; 

            Content.RootDirectory = "Content";
            //Game = this;

            SpriteSheets = new SortedList<int, Texture2D>( );

            //Initialize Physics
            Debug.Print( "Initializing Physics Engine" );

            //Open new thread to physics pipeline
            PHYSICS_WORLD = new World( new Vector2( 0f, 0f ) );

            Debug.Print( "Initializing World" );
            while ( WorldManager.Initialize( ) != 0 ) { }

            //Initialize UI
            Engine.UI.UI.Initialize( );
        }

        protected override void Initialize( )
        {
            base.Initialize( );

            //add sprite texture
            SpriteSheets.Add( 0, Content.Load<Texture2D>( "PikaSprite" ) );

            //Create main camera
            GameObject _camObj = new GameObject( "SCENE_CAMERA" );
            GAME_CAMERA = new Camera( GraphicsDevice.Viewport, _camObj );
            _camObj.AddComponent( GAME_CAMERA );

            //WorldManager.LoadWorld( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) + "\\TestWorldB.world" );

            //WorldManager.SaveWorld( "TestWorldB" );
        }

        protected override void LoadContent( )
        {
            Debug.Print( "Engine Initialized!" );

            Debug.Print( "Loading Content..." );
            PHYSICS_WORLD.Clear( );

            //create sprite batch
            m_SpriteBatch = new SpriteBatch( GraphicsDevice );

            Debug.Print( "Engine Ready!" );
        }

        protected override void UnloadContent( )
        {
            Debug.Print( "Unloading Content..." );
        }

        protected override void Update( GameTime gameTime )
        {
            var deltaTime = ( float )gameTime.ElapsedGameTime.TotalSeconds;

            if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed || Keyboard.GetState( ).IsKeyDown( Keys.Escape ) )
                Exit( );

            //Do Physics step
            PHYSICS_WORLD.Step( Math.Min( ( float )gameTime.ElapsedGameTime.TotalSeconds, ( 1f / 30f ) ) );

            //Dispatch Physics Events
            if ( OnPhysicsUpdate != null )
            {
                OnPhysicsUpdate( PHYSICS_WORLD );
            }

            //Update loaded scene
            WorldManager.Update( gameTime );

            base.Update( gameTime );
        }

        protected override void Draw( GameTime gameTime )
        {
            //Clear the back-buffer
            GraphicsDevice.Clear( Color.Black );

            //Begin batch
            var viewMatrix = GAME_CAMERA.GetViewTransformMatrix( );
            m_SpriteBatch.Begin( SpriteSortMode.BackToFront, transformMatrix: viewMatrix );

            //Draw loaded scene
            WorldManager.Draw
                (
                    m_SpriteBatch,
                    gameTime
                );

            //End batch
            m_SpriteBatch.End( );

            //Parent callback
            base.Draw( gameTime );
        }

        /*#################################
        * VIEWPORT MANAGEMENT
        *#################################*/
        private static int ObjectCount;

        GameObject GetObjectById( int _id )
        {
            foreach ( KeyValuePair<int, GameObject> _obj in SceneObjects )
            {
                if ( _obj.Value.m_InstanceId == _id )
                {
                    return _obj.Value;
                }
            }

            return null;
        }

        /*#################################
         * SPRITE SHEET MANAGEMENT
         *#################################*/
        public static Texture2D GetSpriteTexture2D( int sheetIdx )
        {
            return ENGINE.SpriteSheets[sheetIdx];
        }

        /*#################################
         * ENGINE MACROS
         *#################################*/
    }
}
