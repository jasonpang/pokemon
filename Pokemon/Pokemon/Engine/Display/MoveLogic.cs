using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.IO;
using System.IO.Compression;
using System.Globalization;

namespace Pokemon.Engine.Display
{
    class MoveLogic
    {
        KeyboardState KeyboardState;
        int MoveCount, MoveDirection, TileH, ScrollX, ScrollY, TileW;
        public void Exit() { }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState = Keyboard.GetState();
            if (KeyboardState.IsKeyDown(Keys.Escape)) this.Exit();
            if (MoveCount <= 0)
            {
                if (KeyboardState.IsKeyDown(Keys.Up))
                {
                    MoveDirection = 0;
                    MoveCount = TileH;
                }
                else if (KeyboardState.IsKeyDown(Keys.Down))
                {
                    MoveDirection = 1;
                    MoveCount = TileH;
                    +ScrollY;
                }
                else if (KeyboardState.IsKeyDown(Keys.Left))
                {
                    MoveDirection = 2;
                    MoveCount = TileW;
                }
                else if (KeyboardState.IsKeyDown(Keys.Right))
                {
                    MoveDirection = 3;
                    MoveCount = TileW;
                    +ScrollX;
                }
            }
            else if (MoveCount > 0)
            {
                if (MoveDirection == 0)
                {
                    MapOffsetY -= ScrollY;
                    MoveCount -= ScrollY;
                }
                if (MoveDirection == 1)
                {
                    MapOffsetY += ScrollY;
                    MoveCount -= ScrollY;
                }
                if (MoveDirection == 2)
                {
                    MapOffsetX -= ScrollX;
                    MoveCount -= ScrollX;
                }
                if (MoveDirection == 3)
                {
                    MapOffsetX += ScrollX;
                    MoveCount -= ScrollX;
                }
                if (MapOffsetX < 0)
                {
                    MapOffsetX = TileW - ScrollX;
                    MapX--;
                }
                else if (MapOffsetX >= TileW)
                {
                    MapOffsetX = 0;
                    MapX++;
                }
                if (MapOffsetY < 0)
                {
                    MapOffsetY = TileH - ScrollY;
                    MapY--;
                }
                else if (MapOffsetY >= TileH)
                {
                    MapOffsetY = 0; 
                    MapY++;
                }
                if (MapX < 0)
                {
                    MapX = 0; MapOffsetX = 0;
                    MoveCount = 0;
                }
                if (MapX > MapWidth - MapDisplayX - 1)
                {
                    MapX = MapWidth - MapDisplayX - 1;
                    MapOffsetX = TileW;
                    MoveCount = 0;
                }
                if (MapY < 0)
                {
                    MapY = 0; MapOffsetY = 0; MoveCount = 0;
                }
                if (MapY > MapHeight - MapDisplayY - 1)
                {
                    MapY = MapHeight - MapDisplayY - 1;
                    MapOffsetY = TileH;
                    MoveCount = 0;
                }
            }
            if (MoveCount < 10)
            {
                if (KeyboardState.IsKeyDown(Keys.Up))
                {
                    if (MoveDirection == 0)
                        MoveCount += TileH;
                    +ScrollY;
                }
                else if (KeyboardState.IsKeyDown(Keys.Down))
                {
                    if (MoveDirection == 1)
                        MoveCount += TileH;
                    +ScrollY;
                }
                else if (KeyboardState.IsKeyDown(Keys.Left))
                {
                    if (MoveDirection == 2)
                        MoveCount += TileW;
                    +ScrollX;
                }
                else if (KeyboardState.IsKeyDown(Keys.Right))
                {
                    if (MoveDirection == 3)
                        MoveCount += TileW;
                    +ScrollX;
                }
            }
            // TODO: Add your update logic here   
            base.Update(gameTime);
        }
    }

}
            
            
enum State{	Idle,	MoveUp,	MoveRight,	MoveDown,	MoveLeft}class Player{	Vector2 position;	Vector2 destination;	float speed;	State state;	public Player()	{		position = new Vector2(32, 32);		destination = position;		speed = 128;		state = State.Idle;	}		public void Update(KeyboardState keyState, float elapsed)	{		switch (state)		{			#region Idle			case State.Idle:				if (keyState.IsKeyDown(Keys.Up))				{					destination.Y -= 32;					state = State.MoveUp;					direction = Direction.Up;				}				else if (keyState.IsKeyDown(Keys.Down))				{					destination.Y += 32;					state = State.MoveDown;					direction = Direction.Down;				}				else if (keyState.IsKeyDown(Keys.Left))				{					destination.X -= 32;					state = State.MoveLeft;					direction = Direction.Left;				}				else if (keyState.IsKeyDown(Keys.Right))				{					destination.X += 32;					state = State.MoveRight;					direction = Direction.Right;				}				break;			#endregion			#region MoveUp			case State.MoveUp:				if (position.Y - speed * elapsed < destination.Y)				{					position.Y = destination.Y;					state = State.Idle;				}				else					position.Y -= speed * elapsed;				break;			#endregion			#region MoveDown			case State.MoveDown:				if (position.Y + speed * elapsed > destination.Y)				{					position.Y = destination.Y;					state = State.Idle;				}				else					position.Y += speed * elapsed;				break;			#endregion			#region MoveLeft			case State.MoveLeft:				if (position.X - speed * elapsed < destination.X)				{					position.X = destination.X;					state = State.Idle;				}				else					position.X -= speed * elapsed;				break;			#endregion			#region MoveRight			case State.MoveRight:				if (position.X + speed * elapsed > destination.X)				{					position.X = destination.X;					state = State.Idle;				}				else					position.X += speed * elapsed;				break;			#endregion		}	}}

    case State.MoveRight:				if (position.X + speed * elapsed > destination.X) // if potential movement surpasses destination				{					position.X = destination.X; // move directly to destination					state = State.Idle;				}				else // otherwise					position.X += speed * elapsed; // move normally