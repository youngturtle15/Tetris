public enum TileColor
{
    LightBlue,
    Blue,
    Orange,
    Yellow,
    Green,
    Purple,
    Red
}

public class TetrominoData
{
    // 일자
    public static int[,,] IBlockArray = new int[4, 4, 4]
    {
        {
            { 0,0,0,0},
            { 1,1,1,1},
            { 0,0,0,0},
            { 0,0,0,0}
        },
        {
            { 0,0,1,0},
            { 0,0,1,0},
            { 0,0,1,0},
            { 0,0,1,0}
        },
        {
            { 0,0,0,0},
            { 1,1,1,1},
            { 0,0,0,0},
            { 0,0,0,0}
        },
        {
            { 0,0,1,0},
            { 0,0,1,0},
            { 0,0,1,0},
            { 0,0,1,0}
        }
    };

    // J자
    public static int[,,] JBlockArray = new int[4, 4, 4]
    {
        {
            { 0,2,0,0},
            { 0,2,2,2},
            { 0,0,0,0},
            { 0,0,0,0}
        },
        {
            { 0,0,2,2},
            { 0,0,2,0},
            { 0,0,2,0},
            { 0,0,0,0}
        },
        {
            { 0,0,0,0},
            { 0,2,2,2},
            { 0,0,0,2},
            { 0,0,0,0}
        },
        {
            { 0,0,2,0},
            { 0,0,2,0},
            { 0,2,2,0},
            { 0,0,0,0}
        }
    };

    // L자
    public static int[,,] LBlockArray = new int[4, 4, 4]
    {
        {
            { 0,0,0,3},
            { 0,3,3,3},
            { 0,0,0,0},
            { 0,0,0,0}
        },
        {
            { 0,0,3,0},
            { 0,0,3,0},
            { 0,0,3,3},
            { 0,0,0,0}
        },
        {
            { 0,0,0,0},
            { 0,3,3,3},
            { 0,3,0,0},
            { 0,0,0,0}
        },
        {
            { 0,3,3,0},
            { 0,0,3,0},
            { 0,0,3,0},
            { 0,0,0,0}
        }
    };

    // O자
    public static int[,,] OBlockArray = new int[4, 4, 4]
    {
        {
            { 0,4,4,0},
            { 0,4,4,0},
            { 0,0,0,0},
            { 0,0,0,0}
        },
        {
            { 0,4,4,0},
            { 0,4,4,0},
            { 0,0,0,0},
            { 0,0,0,0}
        },
        {
            { 0,4,4,0},
            { 0,4,4,0},
            { 0,0,0,0},
            { 0,0,0,0}
        },
        {
            { 0,4,4,0},
            { 0,4,4,0},
            { 0,0,0,0},
            { 0,0,0,0}
        }
    };

    // S자
    public static int[,,] SBlockArray = new int[4, 4, 4]
    {
        {
            { 0,0,5,5},
            { 0,5,5,0},
            { 0,0,0,0},
            { 0,0,0,0}
        },
        {
            { 0,0,5,0},
            { 0,0,5,5},
            { 0,0,0,5},
            { 0,0,0,0}
        },
        {
            { 0,0,0,0},
            { 0,0,5,5},
            { 0,5,5,0},
            { 0,0,0,0}
        },
        {
            { 0,5,0,0},
            { 0,5,5,0},
            { 0,0,5,0},
            { 0,0,0,0}
        }
    };

    // T자
    public static int[,,] TBlockArray = new int[4, 4, 4]
    {
        {
            { 0,0,6,0},
            { 0,6,6,6},
            { 0,0,0,0},
            { 0,0,0,0}
        },
        {
            { 0,0,6,0},
            { 0,0,6,6},
            { 0,0,6,0},
            { 0,0,0,0}
        },
        {
            { 0,0,0,0},
            { 0,6,6,6},
            { 0,0,6,0},
            { 0,0,0,0}
        },
        {
            { 0,0,6,0},
            { 0,6,6,0},
            { 0,0,6,0},
            { 0,0,0,0}
        }
    };

    // Z자
    public static int[,,] ZBlockArray = new int[4, 4, 4]
    {
        {
            { 0,7,7,0},
            { 0,0,7,7},
            { 0,0,0,0},
            { 0,0,0,0}
        },
        {
            { 0,0,0,7},
            { 0,0,7,7},
            { 0,0,7,0},
            { 0,0,0,0}
        },
        {
            { 0,0,0,0},
            { 0,7,7,0},
            { 0,0,7,7},
            { 0,0,0,0}
        },
        {
            { 0,0,7,0},
            { 0,7,7,0},
            { 0,7,0,0},
            { 0,0,0,0}
        }
    };
}
