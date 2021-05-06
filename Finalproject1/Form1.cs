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

// THIS IS WHERE INTRO SCREEN SHOULD BE 
namespace Finalproject1
{
    public partial class Form1 : Form
    {
        //make text, font and location for all the instructions and logos 
        RectangleF backgroundImage = new RectangleF(0,0,1300,700);
        RectangleF gameLogoImage = new RectangleF(520, 50, 260, 140);

        PointF instructions1Point = new PointF(200, 200);
        Font instructions1Font = new Font("Ariel", 25.0f);
        string instructions1Text = "Move with arrow Keys";

        PointF instructions2Point = new PointF(200, 300);
        Font instructions2Font = new Font("Ariel", 25.0f);
        string instructions2Text = "Attack with WASD";

        PointF instructions3Point = new PointF(200, 400);
        Font instructions3Font = new Font("Ariel", 25.0f);
        string instructions3Text = "Throw boomerang with E";


        private void btnStart_Click(object sender, EventArgs e) // when the user presses the start button
        {
            // go to form 2 
            Form2 frmGame = new Form2();
            frmGame.Show();
            this.Hide();                 
        }      

        public Form1()
        {
            InitializeComponent();           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // draw background
            e.Graphics.DrawImage(Properties.Resources.background2, backgroundImage);
            // draw logo
            e.Graphics.DrawImage(Properties.Resources.Zelda_Logo_svg, gameLogoImage);
            // draw instructions
            e.Graphics.DrawString(instructions1Text, instructions1Font, Brushes.White, instructions1Point);
            e.Graphics.DrawString(instructions2Text, instructions2Font, Brushes.White, instructions2Point);
            e.Graphics.DrawString(instructions3Text, instructions3Font, Brushes.White, instructions3Point);
        }                 
    }
}
