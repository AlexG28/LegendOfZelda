/*Aleksandr Gyumushyan
 * Final programming project 
 * legend of zelda 2018 ultimate edition
 * latest version as of June 16, 2018 (maid code is done)
 * TODO: comments 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Finalproject1
{
    public partial class Form2 : Form
    {
        // refresh bool 
        bool refreshOrNot = true; // if the program is refreshed, its false 

        // player stats and location
        //int playerBox.X = 400, playerBox.Y = 400;
        int playerHealth = 100;
        int playerSpeed = 4;
        int playerMovementDirection = 0; // controls in which direction the player is moving 
        // the animation variables for the player
        int imageCountHorizontal = 1; // -1,-2,-3 is for moving left // 1,2,3 is for moving right // 0 is for nothing
        int imageCountVertical; // -1,-2,-3 is for moving down // 1,2,3 is for moving up // 0 is for nothing

        // controls if the loop is running or not         
        bool loopState = false;

        // makes sure that the enemy attacks with a 1 second interval 
        bool fireballMovesOrNot = true; // if it is true, a new fireball is created 

        // timer last tick variabls
        int lastTickMovement;
        int lastTickAnimation;
        int timer1000LastTick;

        // timer intervals
        int timerIntervalMovement = 10;
        int timerIntervalAnimation = 100;
        int timerInterval1000 = 1000;       

        // constants that control where the player is going 
        const int PLAYER_MOVE_UP = 1;
        const int PLAYER_MOVE_RIGHT = 2;
        const int PLAYER_MOVE_DOWN = 3;
        const int PLAYER_MOVE_LEFT = 4;

        // controls if the projectile is moving or not 
        bool projectileCreated = false;


        // controls if the boomerang is moving or not BOOMERANG BOOMERANG BOOMERANG MOOBERANG A;SLDKFJASLFKJSALDKFJAS FCUK THIS PROGRAM 
        bool boomerangCreated = false;
        // controls if the boomerang is going away from the player in a straing line or coming back in with a curve
        bool boomerangDirection = false; // false is going in a straing line, true is going towards the player with a curve 
        int initialBoomerangDirection = 0;
        int boomerangImageCount = 0;

        // sword attack variables 
        int swordAttackDirection;

        // used to calculate the projectile direction so that it directly hits the player
        float projectileXSpeed, projectileYSpeed;
        const int TOTAL_SPEED_PROJECTILE = 5;

        // used to calculate the projectile direction so that it directly hits the player
        float boomerangXSpeed, boomerangYSpeed;
        const int TOTAL_SPEED_BOOMERANG = 10;

        
        // player picture and health
        RectangleF playerBox;        
        Font playerHealthText = new Font("Ariel", 10.0f);

        // fireball projectile rectanglef 
        RectangleF enemyProjectileBox;

        // boomerang box 
        RectangleF boomerangBox;

        //sword rectanglef 
        RectangleF playerSwordBox;

        //obstacle rectangleF 
        RectangleF obstacleBox;

        // one rectangle for body and one for head so that the head can move separatly
        RectangleF enemyBoxBody; // enemy dragon without head     
        RectangleF enemyBoxHead; // enemy dragon head     
        Font enemyHealthText = new Font("Ariel", 10.0f); // health of th dragon
        int enemyHealth = 100; // the health of the enemy
        bool enemyHeadMove = false;

        //player healthbar colour
        Pen healthbarColourPlayer;

        // enemy healthbar colour
        Pen healthbarColourEnemy;

        // the differense of position between the rectangle of the body and the head of the dragon 
        int dragonHeadDifferenceX = 6; // these will change randomly so that the dragons head is bobing
        int dragonHeadDifferenceY = 15;

        // boolean that controls the delay that the sword has
        //bool swordUsed = false; // true if the player pressed the button the the sword popped out 

        private void Form2_Load(object sender, EventArgs e)
        {
            CreateProjectile();                   
        }

        public Form2()
        {
            InitializeComponent();
            // sets the location of the player                         
            playerBox = new RectangleF(400, 400, 40, 40);

            // sets the location of the dragon's head and body 
            enemyBoxBody = new RectangleF(640, 70, 100, 100);            
            enemyBoxHead = new RectangleF(enemyBoxBody.X + dragonHeadDifferenceX, enemyBoxBody.Y + dragonHeadDifferenceY, 50, 50);

            // sets the location of the projectile near the dragons mouth so that it looks like the dragon is breathing fire 
            enemyProjectileBox = new RectangleF(enemyBoxBody.X + 10, enemyBoxBody.Y + 25, 40, 40);

            // sets up the sword box 
            playerSwordBox = new RectangleF(playerSwordBox.X, playerSwordBox.Y, 20, 40);

            //healthbar location and thickness for player         
            healthbarColourPlayer = new Pen(Color.FromArgb(0, 225, 26)); // green
            healthbarColourPlayer.Width = 5.0f;

            //healthbar location and thickness for enemy
            healthbarColourEnemy = new Pen(Color.FromArgb(0, 225, 26)); // green
            healthbarColourEnemy.Width = 5.0f;

            // set the obstacle 
            obstacleBox = new RectangleF(800, 400, 250, 58);

            // set up the boomerang      
            boomerangBox = new RectangleF(playerBox.X, playerBox.Y, 40, 40);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // draw the obstacle wall 
            e.Graphics.DrawImage(Properties.Resources.obsticle, obstacleBox);

            // draws the head and the body of the dragon 
            e.Graphics.DrawImage(Properties.Resources.MainDragonWithoutHead, enemyBoxBody);
            e.Graphics.DrawImage(Properties.Resources.MainDragonHead, enemyBoxHead);

            // this draws the health of both the player and the dragon as well as the health numbers
            e.Graphics.DrawString(playerHealth + "/100", playerHealthText, Brushes.Black, playerBox.X, playerBox.Y - 18); //  NORMALLY SHOWS PLAYER HEALTH IN NUMBER
            e.Graphics.DrawLine(healthbarColourPlayer, playerBox.X, playerBox.Y - 20, playerBox.X + playerBox.Width, playerBox.Y - 20); //NEW VERSION
            e.Graphics.DrawString(enemyHealth + "/100", enemyHealthText, Brushes.Black, enemyBoxBody.X + 28, enemyBoxBody.Y - 20);
            e.Graphics.DrawLine(healthbarColourEnemy, enemyBoxBody.X, enemyBoxBody.Y - 20, enemyBoxBody.X + enemyBoxBody.Width, enemyBoxBody.Y - 20); //NEW VERSION

            
            switch (imageCountVertical) // this switch draws all the vertical animation pictures 
            {               
                case 0:
                    break;
                case -3:
                    e.Graphics.DrawImage(Properties.Resources.moveDown3, playerBox);
                    break;
                case -2:
                    e.Graphics.DrawImage(Properties.Resources.moveDown2, playerBox);
                    break;

                case -1:
                    e.Graphics.DrawImage(Properties.Resources.moveDown1, playerBox);
                    break;

                case 1:
                    e.Graphics.DrawImage(Properties.Resources.moveUp1, playerBox);
                    break;
                case 2:
                    e.Graphics.DrawImage(Properties.Resources.moveUp2, playerBox);
                    break;

                case 3:
                    e.Graphics.DrawImage(Properties.Resources.moveUp3, playerBox);
                    break;
            }

            switch (imageCountHorizontal) // this switch draws all the horizontal animation pictures 
            {
                case 0:
                    break;
                case -3:
                    e.Graphics.DrawImage(Properties.Resources.moveLeft3, playerBox);
                    break;
                case -2:
                    e.Graphics.DrawImage(Properties.Resources.moveLeft2, playerBox);
                    break;

                case -1:
                    e.Graphics.DrawImage(Properties.Resources.moveLeft1, playerBox);
                    break;

                case 1:
                    e.Graphics.DrawImage(Properties.Resources.moveRight1, playerBox);
                    break;
                case 2:
                    e.Graphics.DrawImage(Properties.Resources.moveRight2, playerBox);
                    break;

                case 3:
                    e.Graphics.DrawImage(Properties.Resources.moveRight3, playerBox);
                    break;
            }

            switch (boomerangImageCount) // this switch draws the boomerang animation so that it looks like its spinning
            {
                case 1:
                    e.Graphics.DrawImage(Properties.Resources.boomerang1, boomerangBox);
                    break;
                case 2:
                    e.Graphics.DrawImage(Properties.Resources.boomerang2, boomerangBox);
                    break;
                case 3:
                    e.Graphics.DrawImage(Properties.Resources.boomerang3, boomerangBox);
                    break;
                case 4:
                    e.Graphics.DrawImage(Properties.Resources.boomerang4, boomerangBox);
                    break;
            }
          
            switch (swordAttackDirection) //this switch draws all the swords 
            {
                case 1:
                    playerSwordBox = new RectangleF(playerSwordBox.X, playerSwordBox.Y, 20, 40);    
                    e.Graphics.DrawImage(Properties.Resources.swordUp, playerSwordBox);
                    break;

                case 2:
                    playerSwordBox = new RectangleF(playerSwordBox.X, playerSwordBox.Y, 40, 20);    
                e.Graphics.DrawImage(Properties.Resources.swordRight, playerSwordBox);
                    break;

                case 3:
                    playerSwordBox = new RectangleF(playerSwordBox.X, playerSwordBox.Y, 20, 40);    
                    e.Graphics.DrawImage(Properties.Resources.swordDown, playerSwordBox);
                    break;

                case 4:
                    playerSwordBox = new RectangleF(playerSwordBox.X, playerSwordBox.Y, 40, 20);    
                    e.Graphics.DrawImage(Properties.Resources.swordLeft, playerSwordBox);
                    break;
            }

            // this draws the fireball
            e.Graphics.DrawImage(Properties.Resources.fireBall, enemyProjectileBox);
        }

        public void MainTimer() // in each one check for the direction of the attack and the direction of mvoement 
        {
            loopState = true; // makes sure that the loop actually runs 
            enemyHeadMove = true; // makes sure that the dragons head is always bobing
            lastTickMovement = Environment.TickCount;
            lastTickAnimation = Environment.TickCount;
            timer1000LastTick = Environment.TickCount;

            while (loopState == true) // this is the only loop in the whole program 
            {
                // this timer is for movement of the player, sword and fireball 
                TimerInterval1();

                // this timer is for animation of the boomerang and the player
                TimerInterval2();

                // this timer is for the head of the dragon bobvc
                TimerInterval3(); 

                // this refreshes the whole game 
                Application.DoEvents();
                if (refreshOrNot == true) { Refresh(); } // refresh everything
            }
        }

        void TimerInterval1() // first timer
        {
            if (Environment.TickCount - lastTickMovement >= timerIntervalMovement) // this is the interval for movement 
            {
                refreshOrNot = true;
                if (playerMovementDirection == PLAYER_MOVE_UP) // if the player is moving Up
                {
                    playerBox.Y = playerBox.Y - playerSpeed;

                    PlayerBoundsUp();// check for boundaries 
                    PlayerBoundsObstacleUp();
                    PlayerBoundsEnemyUp();

                    playerBox = new RectangleF(playerBox.X, playerBox.Y, playerBox.Width, playerBox.Height); //update the playerbox
                }

                else if (playerMovementDirection == PLAYER_MOVE_RIGHT) // if the player is moving right 
                {
                    playerBox.X = playerBox.X + playerSpeed;

                    PlayerBoundsRight();// check for boundaries   
                    PlayerBoundsObstacleRight();
                    PlayerBoundsEnemyRight();
                    playerBox = new RectangleF(playerBox.X, playerBox.Y, playerBox.Width, playerBox.Height);//update the playerbox
                }

                else if (playerMovementDirection == PLAYER_MOVE_DOWN) // player moving down 
                {
                    playerBox.Y = playerBox.Y + playerSpeed;

                    PlayerBoundsDown();// check for boundaries 
                    PlayerBoundsObstacleDown();
                    PlayerBoundsEnemyDown();
                    playerBox = new RectangleF(playerBox.X, playerBox.Y, playerBox.Width, playerBox.Height);//update the playerbox
                }

                else if (playerMovementDirection == PLAYER_MOVE_LEFT) // player moving left 
                {
                    playerBox.X = playerBox.X - playerSpeed;

                    PlayerBoundsLeft(); // check for boundaries 
                    PayerBoundsObstacleLeft();

                    PlayerBoundsEnemyLeft();
                    playerBox = new RectangleF(playerBox.X, playerBox.Y, playerBox.Width, playerBox.Height);//update the playerbox
                }

                if (projectileCreated == true) // moves the projectile towards the player
                {
                    enemyProjectileBox.X = enemyProjectileBox.X + projectileXSpeed;
                    enemyProjectileBox.Y = enemyProjectileBox.Y + projectileYSpeed;

                    ProjectileBounds();
                }
                
                if (swordAttackDirection == 1) //up
                {
                    // if the sword is facing up, place it in this direction
                    playerSwordBox.X = playerBox.X + (playerBox.Width / 2); 
                    playerSwordBox.Y = playerBox.Y - 50;
                }

                else if (swordAttackDirection == 2) //right
                {
                    // if the sword is facing right, place it in this direction
                    playerSwordBox.X = playerBox.X + 50;
                    playerSwordBox.Y = playerBox.Y + (playerBox.Height / 2);
                }

                else if (swordAttackDirection == 3) // down
                {
                    // if the sword is facing down, place it in this direction
                    playerSwordBox.X = playerBox.X + (playerBox.Width / 2);
                    playerSwordBox.Y = playerBox.Y + 50;
                }

                else if (swordAttackDirection == 4) // left
                {
                    // if the sword is facing left, place it in this direction
                    playerSwordBox.X = playerBox.X - 50;
                    playerSwordBox.Y = playerBox.Y + (playerBox.Height / 2);
                }


                if (boomerangCreated == true) // if the boomerang is thrown
                {
                    if (boomerangDirection == false) // going away from the player in a straight line 
                    {
                        switch (initialBoomerangDirection) // the direction in which the player was moving when the boomerang was thrown
                        {
                            case 1:
                                boomerangBox.Y = boomerangBox.Y - TOTAL_SPEED_BOOMERANG; // up
                                break;

                            case 2:
                                boomerangBox.X = boomerangBox.X + TOTAL_SPEED_BOOMERANG; // right
                                break;

                            case 3:
                                boomerangBox.Y = boomerangBox.Y + TOTAL_SPEED_BOOMERANG; // down
                                break;

                            case 4:
                                boomerangBox.X = boomerangBox.X - TOTAL_SPEED_BOOMERANG; // left
                                break;
                        }
                        BoomerangBounds(); // check the boomerang bounds
                    }
                    else if (boomerangDirection == true) // the boomerang is moving towards the player
                    {
                        CreateBoomerang(); // calculate the rise and run so that the boomerang always curvs towards the player
                        boomerangBox.X = boomerangBox.X + boomerangXSpeed; 
                        boomerangBox.Y = boomerangBox.Y + boomerangYSpeed;

                        //if the boomerang intersects with the head or the body
                        if (boomerangBox.IntersectsWith(enemyBoxBody) && boomerangBox.IntersectsWith(enemyBoxHead))  
                        {
                            enemyHealth = enemyHealth - 10; // enemy takes damage
                            HealthbarColourEnemy(); // update healthbar and check if the enemy is dead 
                            CheckEnemyHealth();

                            boomerangImageCount = 0; // reset the boomerange and make it invisible to the player (off the screen)
                            boomerangBox.X = -1000;
                            boomerangBox.Y = -1000;
                            boomerangDirection = false;
                            boomerangCreated = false;
                        }

                        if (boomerangBox.IntersectsWith(playerBox)) // if the boomerange intersects with the player on its way back
                        {
                            boomerangImageCount = 0;// reset the boomerange and make it invisible to the player (off the screen)
                            boomerangBox.X = -1000;
                            boomerangBox.Y = -1000;
                            boomerangDirection = false;
                            boomerangCreated = false;
                        }
                    }
                }

                lastTickMovement = Environment.TickCount;
            }
        }

        void TimerInterval2() // second timer
        {
            if (Environment.TickCount - lastTickAnimation >= timerIntervalAnimation) // this is the interval for animations 
            {
                refreshOrNot = true; // makes sure that the screen is refreshed 

                // checks if the sword is intercepting with the dragon only if the sword is actually out 
                if (swordAttackDirection != 0) 
                {
                    EnemyTakeDamageFromSword();
                }

                if (swordAttackDirection == 0)// if the sword is not out 
                {
                    if (playerMovementDirection == PLAYER_MOVE_UP) // if the player is moving Up
                    {
                        imageCountHorizontal = 0;
                        if (imageCountVertical < 0 && imageCountVertical > -4)// this prevents the player from flickering
                        {
                            imageCountVertical = 1;
                        }
                        imageCountVertical = imageCountVertical + 1; 

                        if (imageCountVertical == 4) // if the image counter reaches the last animation, it resets back
                        {
                            imageCountVertical = 1;
                        }
                    }

                    else if (playerMovementDirection == PLAYER_MOVE_RIGHT) // if the player is moving right 
                    {
                        imageCountVertical = 0;
                        if (imageCountHorizontal < 0 && imageCountHorizontal > -4)// this makes sure that the imagecount is never 0 (no flickering)
                        {
                            imageCountHorizontal = 1;
                        }

                        imageCountHorizontal = imageCountHorizontal + 1;//adds one to the image counter so that it changes

                        if (imageCountHorizontal == 4)// if the image counter reaches the last animation, it resets back
                        {
                            imageCountHorizontal = 1;
                        }

                        //playerBox = new RectangleF(playerBox.X, playerBox.Y, playerBox.Width, playerBox.Height);
                    }

                    else if (playerMovementDirection == PLAYER_MOVE_DOWN)
                    {
                        imageCountHorizontal = 0;
                        if (imageCountVertical > 0 && imageCountVertical < 4)// this makes sure that the imagecount is never 0 (no flickering)
                        {
                            imageCountVertical = -1;
                        }

                        imageCountVertical = imageCountVertical - 1;//subtracts one to the image counter so that it changes

                        if (imageCountVertical == -4)// if the image counter reaches the last animation, it resets back
                        {
                            imageCountVertical = -1;
                        }

                        //playerBox = new RectangleF(playerBox.X, playerBox.Y, playerBox.Width, playerBox.Height);
                    }

                    else if (playerMovementDirection == PLAYER_MOVE_LEFT) //playerMoveDirection == 4
                    {
                        imageCountVertical = 0;
                        if (imageCountHorizontal > 0 && imageCountHorizontal < 4)// this makes sure that the imagecount is never 0 (no flickering)
                        {
                            imageCountHorizontal = -1;
                        }

                        imageCountHorizontal = imageCountHorizontal - 1;//subtracts one to the image counter so that it changes

                        if (imageCountHorizontal == -4)// if the image counter reaches the last animation, it resets back
                        {
                            imageCountHorizontal = -1;
                        }
                    }
                }

                /////////////////////////////////////////////////////////////////////////////// THESE LONG LINES SEPARATE THE CODE TO MAKE IT MORE ORGANAIZED

                // this makes sure that the player is always pointing to the direction of the sword even if they are moving 
                if (swordAttackDirection == 1 && playerMovementDirection != 0) // if the sword is out (up) and the player is not stationary 
                {
                    imageCountHorizontal = 0;
                    if (imageCountVertical < 0 && imageCountVertical > -4)// this makes sure that the imagecount is never 0 (no flickering)
                    {
                        imageCountVertical = 1;
                    }
                    
                    imageCountVertical = imageCountVertical + 1;//adds one to the image counter so that it changes

                    if (imageCountVertical == 4)// if the image counter reaches the last animation, it resets back
                    {
                        imageCountVertical = 1;
                    }
                }

                else if (swordAttackDirection == 2 && playerMovementDirection != 0)// if the sword is out (right) and the player is not stationary 
                {
                    imageCountVertical = 0;
                    if (imageCountHorizontal < 0 && imageCountHorizontal > -4)// this makes sure that the imagecount is never 0 (no flickering)
                    {
                        imageCountHorizontal = 1;
                    }
                    imageCountHorizontal = imageCountHorizontal + 1;//adds one to the image counter so that it changes

                    if (imageCountHorizontal == 4)// if the image counter reaches the last animation, it resets back
                    {
                        imageCountHorizontal = 1;
                    }
                }

                else if (swordAttackDirection == 3 && playerMovementDirection != 0)// if the sword is out (down) and the player is not stationary 
                {
                    imageCountHorizontal = 0;
                    if (imageCountVertical > 0 && imageCountVertical < 4)// this makes sure that the imagecount is never 0 (no flickering)
                    {
                        imageCountVertical = -1;
                    }

                    imageCountVertical = imageCountVertical - 1;//subtracts one from the image counter so that it changes

                    if (imageCountVertical == -4)// if the image counter reaches the last animation, it resets back
                    {
                        imageCountVertical = -1;
                    }
                }

                else if (swordAttackDirection == 4 && playerMovementDirection != 0)// if the sword is out (left) and the player is not stationary 
                {
                    imageCountVertical = 0;
                    if (imageCountHorizontal > 0 && imageCountHorizontal < 4) // this makes sure that the imagecount is never 0 (no flickering)
                    {
                        imageCountHorizontal = -1;
                    }

                    imageCountHorizontal = imageCountHorizontal - 1;//subtracts one from the image counter so that it changes

                    if (imageCountHorizontal == -4)// if the image counter reaches the last animation, it resets back
                    {
                        imageCountHorizontal = -1;
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////////

                if (boomerangCreated == true) //boomerang animations
                {
                    if (boomerangImageCount >= 1) // if its less than or equal to one, add one to the animation counter
                    {
                        boomerangImageCount++;
                    }

                    if (boomerangImageCount == 5) // if it reaches 5, reset back to 1
                    {
                        boomerangImageCount = 1;
                    }
                }

                lastTickAnimation = Environment.TickCount;
            }
        }

        void TimerInterval3() // third timer
        {
            if (Environment.TickCount - timer1000LastTick >= timerInterval1000)
            {
                refreshOrNot = true;

                // this makes sure that the head of the dragon is bobing in 4 random directions
                if (enemyHeadMove == true) // this is always equal to true if the timer is running
                {
                    timer1000LastTick = Environment.TickCount;
                    Random randomNumberGenerator = new Random();

                    //generates random number which controls in which direction the head is going to bob 
                    int headMovementDirection = randomNumberGenerator.Next(1, 6); // 1 for up, 2 for right, 3 for down, 4 for left, 5 for center

                    switch (headMovementDirection) // head is randomly moved to any of the 5 positions
                    {
                        case 1:
                            dragonHeadDifferenceY = 13;
                            dragonHeadDifferenceX = 6;
                            break;
                        case 2:
                            dragonHeadDifferenceX = 8;
                            dragonHeadDifferenceY = 15;
                            break;

                        case 3:
                            dragonHeadDifferenceY = 17;
                            dragonHeadDifferenceX = 6;
                            break;
                        case 4:
                            dragonHeadDifferenceX = 4;
                            dragonHeadDifferenceY = 15;
                            break;
                        case 5:
                            dragonHeadDifferenceX = 6;
                            dragonHeadDifferenceY = 15;
                            break;
                    }

                    //update the head location
                    enemyBoxHead = new RectangleF(enemyBoxBody.X + dragonHeadDifferenceX, enemyBoxBody.Y + dragonHeadDifferenceY, 50, 50); 
                }

                if (fireballMovesOrNot == false) //if the fireball is not created
                {
                    projectileCreated = true;
                    fireballMovesOrNot = true;
                }
            }
        }
        
        void CreateProjectile() // this subprogram creates the exact rise and run so that the fireball always directly hits the player
        {
            float rise, run, hypotenuse;
            rise = playerBox.Location.Y - enemyProjectileBox.Location.Y;
            run = playerBox.Location.X - enemyProjectileBox.Location.X;

            hypotenuse = (float)Math.Sqrt(Math.Pow(rise, 2) + Math.Pow(run, 2));

            projectileYSpeed = rise / hypotenuse * TOTAL_SPEED_PROJECTILE;
            projectileXSpeed = run / hypotenuse * TOTAL_SPEED_PROJECTILE;

            projectileCreated = true;
        }

        void CreateBoomerang()// this subprogram creates the exact rise and run so that the boomerang always returns directly to the player
        {
            float rise, run, hypotenuse;
            rise = playerBox.Location.Y - boomerangBox.Location.Y;
            run = playerBox.Location.X - boomerangBox.Location.X;

            hypotenuse = (float)Math.Sqrt(Math.Pow(rise, 2) + Math.Pow(run, 2));

            boomerangYSpeed = rise / hypotenuse * TOTAL_SPEED_BOOMERANG;
            boomerangXSpeed = run / hypotenuse * TOTAL_SPEED_BOOMERANG;
            
            boomerangCreated = true;
        }

        void ProjectileBounds() // this subprogram checks the boundaries of the projectile
        {
            if (enemyProjectileBox.X < 0 ||
                enemyProjectileBox.Y < 0 ||
                enemyProjectileBox.X > ClientSize.Width - enemyProjectileBox.Width ||
                enemyProjectileBox.Y > ClientSize.Height - enemyProjectileBox.Height || 
                enemyProjectileBox.IntersectsWith(obstacleBox)) // if it hits any of the screen borders, it goes back to the head of the dragon and starts going again
            {              
                if (fireballMovesOrNot == true) // if the fireball is 'active'
                {
                    enemyProjectileBox.X = enemyBoxBody.X + 10;
                    enemyProjectileBox.Y = enemyBoxBody.Y + 25;
                    CreateProjectile();
                }
            }

            if (enemyProjectileBox.IntersectsWith(playerBox)) // if the fireball hits the player
            {              
                playerHealth = playerHealth - 5; // subtracts health
                CheckPlayerHealth(); // checks if the player is dead 
                HealthbarColourPlayer(); // updates the healthbar

                enemyProjectileBox.X = enemyBoxBody.X + 10; // resets the projectile back to dragons head
                enemyProjectileBox.Y = enemyBoxBody.Y + 25;

                projectileCreated = false;
                if (fireballMovesOrNot == true)
                {
                    projectileCreated = true;
                    CreateProjectile();
                }
            }            
        }


        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            //if the player presses keys
            if (e.KeyCode == Keys.Left)
            {
                playerMovementDirection = 4; // makes the player go left

                if (boomerangCreated == false) // this makes sure that the boomerang is thrown in the right direction even if the player is stationary
                {
                    initialBoomerangDirection = playerMovementDirection;
                }
            }

            else if (e.KeyCode == Keys.Right)
            {
                playerMovementDirection = 2; // makes the player go right

                if (boomerangCreated == false)// this makes sure that the boomerang is thrown in the right direction even if the player is stationary
                {
                    initialBoomerangDirection = playerMovementDirection;
                }
         
            }

            else if (e.KeyCode == Keys.Up)
            {
                playerMovementDirection = 1; // makes the player go up 

                if (boomerangCreated == false)// this makes sure that the boomerang is thrown in the right direction even if the player is stationary
                {
                    initialBoomerangDirection = playerMovementDirection;
                }
            }

            else if (e.KeyCode == Keys.Down)
            {
                playerMovementDirection = 3; // makes the player go down 

                if (boomerangCreated == false)// this makes sure that the boomerang is thrown in the right direction even if the player is stationary
                {
                    initialBoomerangDirection = playerMovementDirection;
                }
            }

            /////////////////////////////////////////////////////////////////////////////////

            if (e.KeyCode == Keys.W)
            {
                swordAttackDirection = 1;
                //EnemyTakeDamage();
            }

            else if (e.KeyCode == Keys.D)
            {   
                swordAttackDirection = 2;
                //EnemyTakeDamage();               
            }

            else if (e.KeyCode == Keys.S)
            {
                swordAttackDirection = 3;
                //EnemyTakeDamage();             
            }

            else if (e.KeyCode == Keys.A)
            {
                swordAttackDirection = 4;
                //EnemyTakeDamage();              
            }

            /////////////////////////////////

            if (e.KeyCode == Keys.E) // if the player presses E
            {
                if (boomerangCreated == false) // if the boomerang is not already thrown
                {
                    boomerangBox.X = playerBox.X; // makes the boomerang location equal to the player location
                    boomerangBox.Y = playerBox.Y;
                    boomerangImageCount = 1; // starts the animation and the movement of the boomerang                    
                    boomerangCreated = true;
                }
            }
        }

        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            // when the user lets go of a key, the player stops moving 
            if (e.KeyCode == Keys.Left)
            {
                if (playerMovementDirection == 4) // if the player is going left, they stop going left 
                {
                    playerMovementDirection = 0;
                }
            }

            else if (e.KeyCode == Keys.Right)
            {
                if (playerMovementDirection == 2)// if the player is going right, they stop going right 
                {
                    playerMovementDirection = 0;
                }
            }

            else if (e.KeyCode == Keys.Up) // if the player is going up, they stop going up 
            {
                if (playerMovementDirection == 1)
                { 
                    playerMovementDirection = 0;
                }
            }

            else if (e.KeyCode == Keys.Down) // if the player is going donw, they stop going down
            {
                if (playerMovementDirection == 3)
                {
                    playerMovementDirection = 0;
                }
            }

            //////////////////////////////////////
            
            //sword attack direction
            if (e.KeyCode == Keys.W)
            {
                if (swordAttackDirection == 1)
                {
                    swordAttackDirection = 0;
                }
            }

            else if (e.KeyCode == Keys.D)
            {
                if (swordAttackDirection == 2)
                {
                    swordAttackDirection = 0;
                }
            }
            else if (e.KeyCode == Keys.S)
            {
                if (swordAttackDirection == 3)
                {
                    swordAttackDirection = 0;
                }
            }
            else if (e.KeyCode == Keys.A)
            {
                if (swordAttackDirection == 4)
                {
                    swordAttackDirection = 0;
                }
            }
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            if (loopState == false) // starts the timer when the second form is shown
            {
                MainTimer();
            }
        }

        void PlayerBoundsLeft()// checks the left bounds of the player box 
        {
            if (playerBox.X <= 0)
            {
                playerBox.X = 0;
            }
        } 

        void PlayerBoundsUp()// checks the top bounds of the player box 
        {
            if (playerBox.Y <= 0)
            {
                playerBox.Y = 0;
            }
        } 

        void PlayerBoundsDown()// checks the bottom bounds of the player box 
        {
            if (playerBox.Y >= ClientSize.Height - playerBox.Height)
            {
                playerBox.Y = ClientSize.Height - (int)playerBox.Height;
            }
        } 

        void PlayerBoundsRight()// checks the right bounds of the player box 
        {
            if (playerBox.X >= ClientSize.Width - playerBox.Width)
            {
                playerBox.X = ClientSize.Width - (int)playerBox.Width;
            }
        }

        
        void BoomerangBoundsLeft()// checks the left bounds of the Boomerang box 
        {
            if (boomerangBox.X <= 0)
            {
                boomerangDirection = true;
            }
        }

        void BoomerangBoundsUp()// checks the top bounds of the Boomerang box 
        {
            if (boomerangBox.Y <= 0)
            {
                boomerangDirection = true;
            }
        }

        void BoomerangBoundsDown()// checks the bottom bounds of the Boomerang box 
        {
            if (boomerangBox.Y >= ClientSize.Height - boomerangBox.Height)
            {
                 boomerangDirection = true;
            }
        }

        void BoomerangBoundsRight()// checks the right bounds of the Boomerang box 
        {
            if (boomerangBox.X >= ClientSize.Width - boomerangBox.Width)
            {
                boomerangDirection = true;
            }
        }

        void PlayerBoundsObstacleUp() // going up 
        {
            if (playerBox.Y <= obstacleBox.Y + obstacleBox.Height && 
                playerBox.Y + playerBox.Height >= obstacleBox.Y + obstacleBox.Height &&
                playerBox.X > obstacleBox.X - playerBox.Width &&
                playerBox.X < obstacleBox.X + obstacleBox.Width)
            {
                playerBox.Y = (int)obstacleBox.Y + (int)obstacleBox.Height;
            }
        }

        void PlayerBoundsObstacleDown() // going down 
        {
            if (playerBox.Y + playerBox.Height >= obstacleBox.Y &&
                playerBox.Y + playerBox.Height <= obstacleBox.Y + obstacleBox.Height &&
                playerBox.X > obstacleBox.X - playerBox.Width &&
                playerBox.X < obstacleBox.X + obstacleBox.Width)
            {
                playerBox.Y = (int)obstacleBox.Y - (int)playerBox.Height;
            }
        }

        void PlayerBoundsObstacleRight()// going right
        {
            if (playerBox.X + playerBox.Width >= obstacleBox.X &&
                playerBox.X + playerBox.Width <= obstacleBox.X + obstacleBox.Width &&
                playerBox.Y + playerBox.Height > obstacleBox.Y &&
                playerBox.Y < obstacleBox.Y + obstacleBox.Height)
            {
                playerBox.X = (int)obstacleBox.X - (int)playerBox.Width;
            }
        }

       void PayerBoundsObstacleLeft() // going left
        {
            if (playerBox.X <= obstacleBox.X + obstacleBox.Width &&
                playerBox.X + playerBox.Width >= obstacleBox.X &&
                playerBox.Y + playerBox.Height > obstacleBox.Y &&
                playerBox.Y < obstacleBox.Y + obstacleBox.Height)
            {
                playerBox.X = (int)obstacleBox.X + (int)obstacleBox.Width;
            }
        }

        ////////////////////////////////////////////////////////////////////

        void PlayerBoundsEnemyUp() // going up 
        {
            if (playerBox.Y <= enemyBoxBody.Y + enemyBoxBody.Height &&
                playerBox.Y + playerBox.Height >= enemyBoxBody.Y + enemyBoxBody.Height &&
                playerBox.X > enemyBoxBody.X - playerBox.Width &&
                playerBox.X < enemyBoxBody.X + enemyBoxBody.Width)
            {
                playerBox.Y = (int)enemyBoxBody.Y + (int)enemyBoxBody.Height;
            }
        }
        void PlayerBoundsEnemyDown() // going down 
        {
            if (playerBox.Y + playerBox.Height >= enemyBoxBody.Y &&
                playerBox.Y + playerBox.Height <= enemyBoxBody.Y + enemyBoxBody.Height &&
                playerBox.X > enemyBoxBody.X - playerBox.Width &&
                playerBox.X < enemyBoxBody.X + enemyBoxBody.Width)
            {
                playerBox.Y = (int)enemyBoxBody.Y - (int)playerBox.Height;
            }
        }

        void PlayerBoundsEnemyRight()// going right
        {
            if (playerBox.X + playerBox.Width >= enemyBoxBody.X &&
                playerBox.X + playerBox.Width <= enemyBoxBody.X + enemyBoxBody.Width &&
                playerBox.Y + playerBox.Height > enemyBoxBody.Y &&
                playerBox.Y < enemyBoxBody.Y + enemyBoxBody.Height)
            {
                playerBox.X = (int)enemyBoxBody.X - (int)playerBox.Width;
            }
        }

        void PlayerBoundsEnemyLeft() // going left
        {
            if (playerBox.X <= enemyBoxBody.X + enemyBoxBody.Width &&
                playerBox.X + playerBox.Width >= enemyBoxBody.X &&
                playerBox.Y + playerBox.Height > enemyBoxBody.Y &&
                playerBox.Y < enemyBoxBody.Y + enemyBoxBody.Height)
            {
                playerBox.X = (int)enemyBoxBody.X + (int)enemyBoxBody.Width;
            }
        }

        ///////////////////////////////////////////////////////////////////////

        void BoomerangBounds() // check for the boomerang boundaries 
        {
            // if the boomerang reaches any of the screen boundaries, the boomerang starts to come back
            BoomerangBoundsDown(); 
            BoomerangBoundsUp();
            BoomerangBoundsLeft();
            BoomerangBoundsRight();

            // if the boomerange intersects with the obsticle, then it is sent back
            if (boomerangBox.IntersectsWith(obstacleBox))
            {
                boomerangDirection = true;
            }

            // if the boomerang intersects with the head or the body of the enemy, then the enemy takes 10 damage
            if (boomerangBox.IntersectsWith(enemyBoxBody) || boomerangBox.IntersectsWith(enemyBoxHead)) 
            {
                boomerangDirection = true; // boomerange starts going back 
                enemyHealth = enemyHealth - 10;
                HealthbarColourEnemy(); // update the colour of the enemy's healthbar 
                CheckEnemyHealth(); // check if the enemy is dead 
            }
        }       

        void CheckPlayerHealth() // check players health 
        {
            if (playerHealth <= 0) // if player's helaht is 0 or lower 
            {
                loopState = false; // stop the loop
                this.Hide();
                Form3 frmGame = new Form3(); // show the defeat screen 
                frmGame.Show();   
            }
        }

        void CheckEnemyHealth() // check if the enemys
        {
            if (enemyHealth <= 0) // if the health is 0 or lower
            {
                loopState = false; // stops the time r
                this.Hide();
                Form4 frmGame = new Form4(); // show the victory screen 
                frmGame.Show();
            }
        }

        // this subprogram changes the colour of the healthbar for the player based on the players health
        void HealthbarColourPlayer()
        {
            if (playerHealth >= 80 && playerHealth <= 100)
            {
                healthbarColourPlayer.Color = Color.FromArgb(0, 225, 26); // green
            }
            else if (playerHealth >= 60 && playerHealth <= 80)
            {
                healthbarColourPlayer.Color = Color.FromArgb(158, 225, 92); // light green  
            }
            else if (playerHealth >= 40 && playerHealth <= 60)
            {
                healthbarColourPlayer.Color = Color.FromArgb(226, 226, 25); // yellow
            }
            else if (playerHealth >= 20 && playerHealth <= 40)
            {
                healthbarColourPlayer.Color = Color.FromArgb(225, 196, 0); // orange 
            }
            else if (playerHealth >= 0 && playerHealth <= 20)
            {
                healthbarColourPlayer.Color = Color.FromArgb(228, 52, 0); // red
            }
        }

        // this subprogram changes the colour of the healthbar for the enemy based on the enemy's health
        void HealthbarColourEnemy()
        {
            if (enemyHealth >= 80 && enemyHealth <= 100)
            {
                healthbarColourEnemy.Color = Color.FromArgb(0, 225, 26); // green
            }
            else if (enemyHealth >= 60 && enemyHealth <= 80)
            {
                healthbarColourEnemy.Color = Color.FromArgb(158, 225, 92); // light green  
            }
            else if (enemyHealth >= 40 && enemyHealth <= 60)
            {
                healthbarColourEnemy.Color = Color.FromArgb(226, 226, 25); // yellow
            }
            else if (enemyHealth >= 20 && enemyHealth <= 40)
            {
                healthbarColourEnemy.Color = Color.FromArgb(225, 196, 0); // orange 
            }
            else if (enemyHealth >= 0 && enemyHealth <= 20)
            {
                healthbarColourEnemy.Color = Color.FromArgb(228, 52, 0); // red
            }
        }

        // this subprogram checks if the sword is intersecting witht he enemy
        void EnemyTakeDamageFromSword()
        {
            if (playerSwordBox.IntersectsWith(enemyBoxBody) || playerSwordBox.IntersectsWith(enemyBoxHead))
            { // if the sword is intersecting with the enemy, it takes damage
                fireballMovesOrNot = false; // stops the fireball from moving 
                enemyHealth = enemyHealth - 1; // subtracts 1 health from the enemy
                HealthbarColourEnemy(); // update colour 
                CheckEnemyHealth(); // check if the enemy died 
            }
        }
    }
}
