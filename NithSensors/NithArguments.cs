namespace NITHdmis.NithSensors
{
    public enum NithArguments
    {
        pos_yaw,            // Gyroscopic yaw position
        pos_pitch,          // Gyroscopic pitch position
        pos_roll,           // Gyroscopic roll position
        acc_yaw,            // Gyroscopic yaw acceleration
        acc_pitch,          // Gyroscopic pitch acceleration
        acc_roll,           // Gyroscopic roll acceleration
        cal_gyro,           // Gyroscope calibration
        cal_accel,          // Accelerometer calibration
        cal_sys,            // General system calibration
        cal_mag,            // Magnetometer calibration
        press,              // Positive pressure from a pressure sensor
        NaF
    }
}