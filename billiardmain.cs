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
 *          mcs -target:library billiardmain.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:billiardframe.dll -r:billiardlogic.dll -out:billardmain.exe
 */

using System;
using System.Windows.Forms;

public class billiardmain{
    public static void Main(){
        System.Console.WriteLine("The billiard program has begun");
        billiardframe program = new billiardframe();
        Application.Run(program);
        System.Console.WriteLine("The billiard program has closed");
    }
}