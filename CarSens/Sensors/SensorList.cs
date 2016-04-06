using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CarSens.Sensors
{
    interface SensorList
    {
        void initialize(System.ComponentModel.BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs e);

        DataTable getConnectedSensors();

        bool isFinished();
    }
}
