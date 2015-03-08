using System.Runtime.InteropServices;

namespace i2c
{
    public static class I2CNativeLib
    {
        [DllImport("i2c.so", EntryPoint = "Get", SetLastError = true)]
        public static extern int Get(string i2Cbusid, string deviceaddress, string dataaddress);

        [DllImport("i2c.so", EntryPoint = "Set", SetLastError = true)]
        public static extern int Set(string i2Cbusid, string deviceaddress, string dataaddress, string datavalue, int force);
    }
}