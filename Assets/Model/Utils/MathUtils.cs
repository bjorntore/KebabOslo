using System;
using System.Collections;

public static class MathUtils
{

    public static double Distance(int fromX, int fromZ, int toX, int toZ)
    {
        return Math.Sqrt(Math.Pow(Math.Abs(toX - fromX), 2) + Math.Pow(Math.Abs(toZ - fromZ), 2));
    }

    //public static int LinearConversion(double minTo, double maxTo, int minFrom, int maxFrom)
    //{

    //}

    public static int LinearConversionInverted(double actualFrom, double maxFrom, int maxTo)
    {
        return Convert.ToInt32(maxTo * (maxFrom - actualFrom) / maxFrom);
    }

}
