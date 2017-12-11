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
 *          mcs -target:library billiardlogic.cs -r:System.Drawing.dll -out:billardlogic.dll
 */
public class billiardlogic{
    private System.Random randomgenerator = new System.Random();
    public double get_random_direction()
    {
        double randomnumber;
        double ball_angle_randians;

        randomnumber = randomgenerator.NextDouble();
        ball_angle_randians = (randomnumber * 180) / System.Math.PI;
      
        return ball_angle_randians;
    }
}