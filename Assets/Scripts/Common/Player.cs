using System;

public interface Player
{
    double GetSensorInput(int idx);

    double GetError();
}