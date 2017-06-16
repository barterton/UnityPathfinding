using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{

    private Node[,] map;

    MazeGenerator maze;

    public List<Point> path = new List<Point>();

    void Start()
    {
        maze = GetComponent<MazeGenerator>();
        map = maze.getMap();

        Point start = new Point(0, 0, new Point(0,0));
        Point end = new Point(24, 24);

        path = pathFind(start, end);

        List<GameObject> cubes = new List<GameObject>();

        int count = 0;

        for (int i = 0; i < path.Count; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            cube.transform.position = new Vector3(path[i].getX() * maze.getMazeScale(), 0.5F, path[i].getY() * maze.getMazeScale());
            cube.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

            if (path[i].getX() == end.getX() && path[i].getY() == end.getY())
            {
                cube.GetComponent<Renderer>().material.color = Color.green;
                cube.transform.localScale = new Vector3(2f, 2f, 2f);
            }
            count++;
            cubes.Add(cube);
        }
        print(count);
    }

    /**
     *
     *
     */
    private List<Point> pathFind(Point start, Point end)
    {
        print("Begin pathfinding");

        List<Point> open = new List<Point>();
        List<Point> closed = new List<Point>();

        open.Add(start);

        while (open.Count > 0)
        {
            Point current = open[0];

            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].getfCost() < current.getfCost() || open[i].getfCost() == current.getfCost() && open[i].hCost < current.hCost)
                {
                    current = open[i];
                }
            }

            open.Remove(current);

            if (current.getX() == end.getX() && current.getY() == end.getY())
            {
                return retrace(current, start);
            }

            foreach (Point neighbour in getNeighbours(current))
            {

                if (!contains(closed, neighbour))
                {

                    int moveToNeighbour = current.gCost + getDistance(current, neighbour);

                    if (moveToNeighbour < neighbour.gCost || !contains(open, neighbour))
                    {
                        neighbour.gCost = moveToNeighbour;
                        
                        neighbour.hCost = getDistance(neighbour, end);
                        

                        if (!contains(open, neighbour))
                        {
                            open.Add(neighbour);
                        }
                    }

                }
            }
     
            closed.Add(current);
        }
        print("Not found path");
        return null;
    }

    public List<Point> retrace(Point end, Point start)
    {
        List<Point> p = new List<Point>();

        Point current = end;

        while (current != start)
        {
            p.Add(current);
            current = current.getParent();
        }

        return p;
    }

    bool contains(List<Point> ps, Point p)
    {
        for (int i = 0; i < ps.Count; i++)
        {
            //If p exists in ps return true
            if (ps[i].getX() == p.getX() && ps[i].getY() == p.getY())
            {
                return true;
            }
        }
        return false;
    }

    int getDistance(Point p1, Point p2)
    {
        //abs distance on the straight line
        int distanceX = Mathf.Abs(p1.getX() - p2.getX());
        int distanceY = Mathf.Abs(p1.getY() - p2.getY());

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        else
        {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }


    /**
     * Method of getting the neighbours of a given point
	 * @param p, point to get the neighbouring points for
	 * @return a List of all the nieghbours locations
     *
     */
    private List<Point> getNeighbours(Point p)
    {

        //Create list to be returned
        List<Point> neighbours = new List<Point>();

        //For the four directions in which neighbours can be reached
        //If the node is within the bounds of the map dimensions
        //Add it to neighbours List

        //NORTH
        if (isValid(p.getX(), p.getY() + 1) && map[p.getX(), p.getY()].bottomWall == false)
        {
            neighbours.Add(new Point(p.getX(), p.getY() + 1, p));
        }
        //EAST
        if (isValid(p.getX() + 1, p.getY()) && map[p.getX(), p.getY()].rightWall == false)
        {
            neighbours.Add(new Point(p.getX() + 1, p.getY(), p));
        }
        //SOUTH
        if (isValid(p.getX(), p.getY() - 1) && map[p.getX(), p.getY()].topWall == false)
        {
            neighbours.Add(new Point(p.getX(), p.getY() - 1, p));
        }
        //WEST
        if (isValid(p.getX() - 1, p.getY()) && map[p.getX(), p.getY()].leftWall == false)
        {
            neighbours.Add(new Point(p.getX() - 1, p.getY(), p));
        }
                      
        return neighbours;
    }

    private bool isValid(int x, int y)
    {
        if (x < 0 || y < 0 || x > 25 || y > 25)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

