using System;
using System.Collections.Generic;

namespace i2c
{
    internal abstract class I2C<TKey>
    {
        private readonly Action<string> _logger;
        internal static Dictionary<TKey, int> Constants { get; set; }

        protected int Set16(string i2Cbusid, string deviceaddress, string dataaddress, string datavalue, int force)
        {
            var value = (UInt16)Convert.ToInt16(datavalue, 16);

            var add1 = (UInt16)Convert.ToInt16(dataaddress, 16);
            var add2 = ++add1;
            
            var msb = GetAsHexString(value >> 8);
            var lsb = GetAsHexString(value & 0xFF);
            _logger(string.Format("16-bit: Writing 16-bit Value: {0} as 2 8-bit values {1} and {2}", GetAsHexString(value), msb, lsb));


            _logger(string.Format("16-bit: Writing {0} to address {1}", msb, GetAsHexString(add1)));
            var data = I2CNativeLib.Set(i2Cbusid, deviceaddress, GetAsHexString(add1), msb, force); //set msb byte/8 bits
            _logger(string.Format("16-bit: Response to msb: {0}", data));


            _logger(string.Format("16-bit: Writing {0} to address {1}", lsb, GetAsHexString(add2)));
            data |= I2CNativeLib.Set(i2Cbusid, deviceaddress, GetAsHexString(add2), lsb, force); //set lsb byte/8 bits
            _logger(string.Format("16-bit: Response to msb |= lsb: {0}", data));
            return data;
 
        }

        protected byte Set8(string i2Cbusid, string deviceaddress, string dataaddress, string datavalue, int force)
        {
            //8-bit
            _logger(string.Format("8-bit: Writing {0} to address {1}", datavalue, dataaddress));
            
            return (byte)I2CNativeLib.Set(i2Cbusid, deviceaddress, dataaddress, datavalue, force);
        }

        private static byte Get(string i2Cbusid, string deviceaddress, string dataaddress)
        {
            return (byte)I2CNativeLib.Get(i2Cbusid, deviceaddress, dataaddress);
        }

        internal string Busid = string.Empty;
        internal bool DoWork;

        protected I2C(Action<string> logger)
        {
            _logger = logger;
            Constants = new Dictionary<TKey, int>();
        }

        internal static int GetConstantAsByte(TKey key)
        {
            int value;
            Constants.TryGetValue(key, out value);
            return value;
        }
        internal static string GetConstantAsString(TKey key)
        {
            int value;
            Constants.TryGetValue(key, out value);
            return "0x" + value.ToString("X").PadLeft(2, '0');
        }
        internal static string GetAsHexString(uint value)
        {
            return "0x" + value.ToString("X").PadLeft(2, '0');
        }
        internal static string GetAsHexString(int value)
        {
            return "0x" + value.ToString("X").PadLeft(2, '0');
        }
        internal byte GetValue8(TKey deviceAddress, TKey dataAddress)
        {
            var result = Get(
                Busid,
                GetConstantAsString(deviceAddress),
                GetConstantAsString(dataAddress)
            );
            return result;
        }
        internal UInt16 GetValue16(TKey deviceAddress, TKey dataAddress)
        {
            var result = (UInt16)(Get(Busid, GetConstantAsString(deviceAddress), GetConstantAsString(dataAddress)) << 8);
            result |= Get(Busid, GetConstantAsString(deviceAddress), GetConstantAsString(dataAddress));

            return result;
        }
        internal abstract void Start();
        internal virtual void Stop()
        {
            DoWork = false;
        }
    }
}
