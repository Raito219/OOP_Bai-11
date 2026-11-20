using System;

namespace SensorSystem
{

    public interface ISensor
    {
        double ReadValue();
        string GetUnit();
    }
}
