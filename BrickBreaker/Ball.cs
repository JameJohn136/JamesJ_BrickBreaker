﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrickBreaker // HELLO THERE THIS IS A COMMIT TEST FROKM JAMES IF YOU SEE THIS THEN IT WORKED
{
    public class Ball
    {
        public int x, y, xSpeed, ySpeed, size, damage;
        public int speed = 1;
        public Color colour;
        public Random rand = new Random();
        public bool stuck = false;
        public int xStuck = 0;
        
        public Ball(int _x, int _y, int _xSpeed, int _ySpeed, int _ballSize)
        {
            x = _x;
            y = _y;
            xSpeed = _xSpeed;
            ySpeed = _ySpeed;
            size = _ballSize;
        }

        public void Move()
        {
            if (!stuck)
            {
                x += xSpeed * speed;
                y += ySpeed * speed;
            }
            else
            {
                x = GameScreen.paddle.x + xStuck;

                if (!GameScreen.stickyPaddle)
                {
                    stuck = false;
                }
            }
        }

        public bool BlockCollision(Block b)
        {
            Rectangle blockRec = new Rectangle(b.x, b.y, b.width, b.height);
            Rectangle ballRec = new Rectangle(x, y, size, size);

            if (ballRec.IntersectsWith(blockRec))
            {
                if (GameScreen.fireBallTimer == 0 || GameScreen.balls[0] != this)
                {
                    // Get the range of specific points it may hit
                    if (x + (size / 2) <= b.x || x + (size / 2) >= b.x + b.width) // Hits either side
                    {
                        xSpeed *= -1;

                        if (xSpeed > 0)
                        {
                            b.x = b.x - size;
                        }
                        else if (xSpeed < 0)
                        {
                            b.x = b.x + size;
                        }
                    }
                    else // hits anywhere else
                    {
                        ySpeed *= -1;

                        if (ySpeed > 0)
                        {
                            b.y = b.y + size;
                        }
                        else if (ySpeed < 0)
                        {
                            b.y = b.y - size;
                        }
                    }
                }
            }

            return blockRec.IntersectsWith(ballRec);
        }

        public void PaddleCollision(Paddle p)
        {
            Rectangle ballRec = new Rectangle(x, y, size, size);
            Rectangle paddleRec = new Rectangle(p.x, p.y, p.width, p.height);

            if (ballRec.IntersectsWith(paddleRec))
            {
                if (ySpeed > 0)
                {
                    y = p.y - size;
                }
                else if (ySpeed < 0)
                {
                    y = p.y + size;
                }

                ySpeed *= -1;

                if (GameScreen.stickyPaddle && !stuck)
                {
                    stuck = true;
                    xStuck = x - GameScreen.paddle.x;
                }
            }
        }

        public void WallCollision(UserControl UC)
        {
            // Collision with left wall
            if (x <= 0)
            {
                xSpeed *= -1;
            }
            // Collision with right wall
            if (x >= (UC.Width - size))
            {
                xSpeed *= -1;
            }
            // Collision with top wall
            if (y <= 2)
            {
                ySpeed *= -1;
            }
        }

        public bool BottomCollision(UserControl UC)
        {
            bool didCollide = false;

            if (y >= UC.Height)
            {
                didCollide = true;
            }

            return didCollide;
        }

    }
}
