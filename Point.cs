using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    //Location values and whether the point can be traversed
    private int posX;
    private int posY;

    //Values for finding a path
    public int gCost = 0;
    public int hCost = 0;

    //The parent point of the current point, used to work out the path to follow
    private Point parent;

    //Constructor for the point
    public Point(int x, int y)
    {
        posX = x;
        posY = y;
        gCost = 99999;
        parent = null;
    }

    public Point(int x, int y, Point p)
    {
        posX = x;
        posY = y;
        gCost = 99999;
        parent = p;
    }

    public int getfCost()
    {
         return gCost + hCost;
        
    }

    public int getX()
    {
        return posX;
    }

    public int getY()
    {
        return posY;
    }

    public Point getParent()
    {
        return parent;
    }
}



