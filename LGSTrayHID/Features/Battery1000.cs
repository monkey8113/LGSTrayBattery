using LGSTrayPrimitives;
using static LGSTrayPrimitives.PowerSupplyStatus;

namespace LGSTrayHID.Features
{
    public static class Battery1000
    {
        public static async Task<BatteryUpdateReturn?> GetBatteryAsync(HidppDevice device)
        {
            Hidpp20 buffer = new byte[7] { 0x10, device.DeviceIdx, device.FeatureMap[0x1000], 0x00 | HidppDevices.SW_ID, 0x00, 0x00, 0x00 };
            Hidpp20 ret = await device.Parent.WriteRead20(device.Parent.DevShort, buffer);

            if (ret.Length == 0) { return null; }

            int mv = -1;
            double batPercent = ret.GetParam(0);
            byte statusByte = ret.GetParam(2);
            
            var status = statusByte switch
            {
                0 => POWER_SUPPLY_STATUS_DISCHARGING,
                1 or 2 => POWER_SUPPLY_STATUS_CHARGING,
                3 => POWER_SUPPLY_STATUS_FULL,
                4 => POWER_SUPPLY_STATUS_CHARGING,  // Some devices use 4 for charging
                _ => POWER_SUPPLY_STATUS_NOT_CHARGING,
            };

            // Determine charging state (status byte values 1, 2, and 4 indicate charging)
            bool isCharging = statusByte == 1 || statusByte == 2 || statusByte == 4;

            return new BatteryUpdateReturn
            {
                batteryPercentage = (int)batPercent,
                status = (byte)status,
                batteryMVolt = mv,
                isCharging = isCharging
            };
        }
    }
}
