/*Author: Randy Le
 * Author's Email: Randy.l5933@csu.fullerton.edu
 * Course: CPSC 223N
 * 
 * Due Date: December 6, 2017
 * 
 * Source Files:
 *  1.billiardmain.cs
 *  2.billiardframe.cs
 *  3.billiardlogic.cs
 *  4.run.sh
 * Purpose of this entire program:
 *  -A ball game that we have to "catch" to gain point. Every time gaining a point, it increases the speed;
 * The source files in this program should be compiled in the order specified:
 *  1.billiardlogic.cs
 *  2.billiardframe.cs
 *  3.billiardmain.cs
 *  4.run.sh
 * Compile this file:
 *              mcs -target:library billardframe.cs -r:System.Winwos.Forms.dll -r:System.Drawing.dll -r:billiardlogic.dll -out:billiardframe.dll
 */
using System;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Timers;

public class billiardframe : Form
{ //Display of the program should be a 16:9 Ratio
    private const int form_height = 900;
    private const int form_width = form_height * 16 / 9;
    private const int control_region_height = 100; //Height of control panel where all the buttons go
    private const int graphic_area_height = form_height - control_region_height;
    private const int horizontal_adjustment = 8;
    //***************************************************************************

    //Display Variables
    private Button new_game_button = new Button();
    private Button next_ball_button = new Button();
    private Button exit_button = new Button();
    private Label ball_speed_label = new Label();
    private Label ball_location_label = new Label();
    private Label points_earned_label = new Label();
    //Preset Location 
    private Point new_game_button_location = new Point(20, form_height - control_region_height + 20);
    private Point next_ball_button_location = new Point(250, form_height - control_region_height + 20);
    private Point ball_speed_label_location = new Point(550, form_height - control_region_height + 20);
    private Point ball_location_label_location = new Point(850, form_height - control_region_height + 20);
    private Point points_earned_label_location = new Point(1150, form_height - control_region_height + 20);
    private Point exit_button_location = new Point(1400, form_height - control_region_height + 20);

    //Declare max and min sizes;
    private Size maxframesize = new Size(form_width, form_height);
    private Size minframesize = new Size(form_width, form_height);

    //**************************************************************************
    billiardlogic algorithm = new billiardlogic();

    private int points_earned = 0;
    private const int ball_radius = 20;
    private double ball_distance_moved_per_refresh = 1.0;
    private double ball_real_coord_x = (double)((form_width / 2) - ball_radius); //double variable initial coord including delta-x
    private double ball_real_coord_y = (double)((graphic_area_height / 2) - ball_radius); //double varaible initial coord include delta-y
    private int ball_int_coord_x; //rounding up the double variable into an integer
    private int ball_int_coord_y; //rounding up the double variable into an interger
    private double ballspeed = 1.0;
    private double delta_horizontal_x; //How much to move horizontally after computing the move per refresh and radian
    private double delta_vertical_y; //how much to move vertically after computing the move per refres and radian
    private double ball_angle_radians; //converting the degree in randian form
    private double graphic_refresh_rate = 30; //30hz = constant refresh rate during the execution of this program
    private double ball_update_rate = 30; //Units are Hz
    private static System.Timers.Timer ball_control_clock = new System.Timers.Timer();
    private static System.Timers.Timer graphic_area_refresh_clock = new System.Timers.Timer();
    //private bool ball_clock_active = false;
    private bool show_ball_active = true;

    //Variable of MouseEvent
    private int cursor_x;
    private int cursor_y;

    public billiardframe()
    {
        Size = new Size(form_width, form_height);
        BackColor = Color.FromArgb(210, 230, 241);
        Text = ("Billiard by Randy Le");
        MaximumSize = maxframesize;
        MinimumSize = minframesize;
        DoubleBuffered = true;

        //Set inital coordinates
        ball_int_coord_x = (int)(ball_real_coord_x);
        ball_int_coord_y = (int)(ball_real_coord_y);
        System.Console.WriteLine("Initial coordinates: ball_a_int_coord_x = {0}. ball_a_int_coord_y = {1}.", ball_int_coord_x, ball_int_coord_y);
        graphic_area_refresh_clock.Enabled = false; //Initally the clock controlling the rate of update the display is stopped
        ball_control_clock.Enabled = false;

        new_game_button.Text = "New Game";
        new_game_button.BackColor = Color.Yellow;
        new_game_button.Location = new_game_button_location;
        new_game_button.Size = new Size(150, 35);
        next_ball_button.Text = "Next Ball";
        next_ball_button.BackColor = Color.Yellow;
        next_ball_button.Location = next_ball_button_location;
        next_ball_button.Size = new Size(150, 35);
        ball_speed_label.Text = "Ball's speed = " + ballspeed + " pix/sec";
        ball_speed_label.BackColor = Color.Green;
        ball_speed_label.Location = ball_speed_label_location;
        ball_speed_label.Size = new Size(180, 35);
        ball_location_label.Text = "Ball's location = (" + (ball_int_coord_x + ball_radius) + "," + (ball_int_coord_y + ball_radius) + ")";
        ball_location_label.BackColor = Color.Green;
        ball_location_label.Location = ball_location_label_location;
        ball_location_label.Size = new Size(200, 35);
        points_earned_label.Text = "Points earned = " + points_earned;
        points_earned_label.BackColor = Color.Green;
        points_earned_label.Location = points_earned_label_location;
        points_earned_label.Size = new Size(150, 35);
        exit_button.Text = "Exit";
        exit_button.BackColor = Color.Yellow;
        exit_button.Location = exit_button_location;
        exit_button.Size = new Size(150, 35);


        //Controls & EventHandlers
        Controls.Add(new_game_button);
        Controls.Add(next_ball_button);
        Controls.Add(ball_speed_label);
        Controls.Add(ball_location_label);
        Controls.Add(points_earned_label);
        Controls.Add(exit_button);

        new_game_button.Click += new EventHandler(manage_new_game_button);
        next_ball_button.Click += new EventHandler(manage_next_ball_button);
        exit_button.Click += new EventHandler(closingprogram);

        graphic_area_refresh_clock.Elapsed += new ElapsedEventHandler(updatedisplay);
        ball_control_clock.Elapsed += new ElapsedEventHandler(updateball);

    }
    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics board = e.Graphics;
       
        board.FillRectangle(Brushes.Green, 0, form_height - control_region_height, form_width, form_height);
        if (show_ball_active == true)
        {
            board.FillEllipse(Brushes.Red, ball_int_coord_x, ball_int_coord_y, 2 * ball_radius, 2 * ball_radius);
        }
        base.OnPaint(e);
    }
    protected override void OnMouseDown(MouseEventArgs e)
    {
        cursor_x = e.X;
        cursor_y = e.Y;
        Invalidate();
        clickoncircle(cursor_x, cursor_y);
    }

    protected void clickoncircle(int x, int y)
    {
        if (x >= ball_int_coord_x && x <= ball_int_coord_x + (ball_radius * 2))
        {
            if (y >= ball_int_coord_y && y <= ball_int_coord_y + (ball_radius * 2))
            {
                points_earned += 1;
                points_earned_label.Text = "Points earned = " + points_earned;
                show_ball_active = false;
                graphic_area_refresh_clock.Enabled = false;

                ball_control_clock.Enabled = false;
                ball_distance_moved_per_refresh += points_earned + 1 * (.2);
                Invalidate();
            }
        }
    }

    protected void Startgraphicclock(double refreshrate){
        double elapsedtimebetweentics;
        if (refreshrate < 1.0) refreshrate = 1.0; //Avoid diving by a number close to zero;
        elapsedtimebetweentics = 1000.0 / refreshrate; //elapsedtimebetweentics has units milliseconds
        graphic_area_refresh_clock.Interval = (int)System.Math.Round(elapsedtimebetweentics);
        graphic_area_refresh_clock.Enabled = true;
    }

    protected void Startballclock(double updaterate){
        double elapsedtimebetweenballmoves;
        if (updaterate < 1.0) updaterate = 1.0;
        elapsedtimebetweenballmoves = 1000.0 / updaterate;
        ball_control_clock.Interval = (int)System.Math.Round(elapsedtimebetweenballmoves);
        ball_control_clock.Enabled = true;
    }

    protected void manage_new_game_button(Object sender, EventArgs events){
        //Speed Display
		ball_speed_label.Text = "Ball's speed = " + ballspeed + " pix/sec";

		ball_real_coord_x = (double)((form_width / 2) - ball_radius); //double variable initial coord including delta-x
		ball_real_coord_y = (double)((graphic_area_height / 2) - ball_radius); //double varaible initial coord include delta-y

        ball_distance_moved_per_refresh = 1.0;
        points_earned = 0;

		ball_control_clock.Enabled = false;
        graphic_area_refresh_clock.Enabled = false;

        show_ball_active = true;
        Invalidate();
    }
    protected void manage_next_ball_button(Object sender, EventArgs events){

        //Change speed display here
        ball_speed_label.Text = "Ball's speed = " + ball_distance_moved_per_refresh + " pix/sec";
        ball_real_coord_x = (double)((form_width / 2) - ball_radius); //double variable initial coord including delta-x
		ball_real_coord_y = (double)((graphic_area_height / 2) - ball_radius); //double varaible initial coord include delta-y

        ball_angle_radians = algorithm.get_random_direction();
		delta_horizontal_x = ball_distance_moved_per_refresh * System.Math.Cos(ball_angle_radians);
		delta_vertical_y = ball_distance_moved_per_refresh * System.Math.Sin(ball_angle_radians);

		Startgraphicclock(graphic_refresh_rate);
		Startballclock(ball_update_rate);
        graphic_area_refresh_clock.Enabled = true;
        ball_control_clock.Enabled = true;

		show_ball_active = true;
    }
    protected void closingprogram(Object sender, EventArgs events){
        System.Console.WriteLine("You pressed the exit_button, goodbye");
        Close();
    }

    protected void updatedisplay(Object sender, ElapsedEventArgs evt){
        points_earned_label.Text = "Points earned = " + points_earned;
        Invalidate(); //Weird: This creates an artificial events so that the graphic area will repaint itself.

    }
    protected void updateball(Object sender, ElapsedEventArgs evt){
        ball_real_coord_x = ball_real_coord_x + delta_horizontal_x;
        ball_real_coord_y = ball_real_coord_y - delta_vertical_y;
        ball_int_coord_x = (int)System.Math.Round(ball_real_coord_x);
        ball_int_coord_y = (int)System.Math.Round(ball_real_coord_y);

        //Have ball coordinates here
        ball_location_label.Text = "Ball's location = (" + (ball_int_coord_x + ball_radius) + "," + (ball_int_coord_y + ball_radius) + ")";
        //Have ricochet function here
        if ((ball_int_coord_x + ball_radius * 2) >= form_width || (ball_int_coord_x) <= 0){
            delta_horizontal_x = -delta_horizontal_x;
        }
        if (ball_int_coord_y <= 0 || (ball_int_coord_y + ball_radius * 2) >= form_height - control_region_height){
            delta_vertical_y = -delta_vertical_y;
        }
        //Determine if ball has passed beyond the graphic area

    }
}

