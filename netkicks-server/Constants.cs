﻿public class Constants
{
    public const int MAX_PLAYERS_PER_MATCH = 10;
    public const int WORLD_UPDATE_FREQUENCY = 1;
    public const int SEND_BALL_POSITION_FREQUENCY = 25;
}

public class NetworkMessageType
{
    public const byte REQUEST_MATCH_DATA = 0;
    public const byte BALL_POSITION_UPDATE = 1;
}

