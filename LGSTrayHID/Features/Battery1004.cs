using LGSTrayPrimitives;
using static LGSTrayPrimitives.PowerSupplyStatus;

namespace LGSTrayHID.Features
{
    public static class Battery1004
    {
        public static async Task<BatteryUpdateReturn?> GetBatteryAsync(HidppDevice device)
{
    Hidpp20 buffer = new byte[7] { 0x10, device.DeviceIdx, device.FeatureMap[0x1004], 0x10 | HidppDevices.SW_ID, 0x00, 0x00, 0x00 };
    Hidpp20 ret = await device.Parent.WriteRead20(device.Parent.DevShort, buffer);

    if (ret.Length == 0) { return null; }

    int mv = -1;
    double batPercent = ret.GetParam(0);
    byte statusByte = ret.GetParam(2);
    
    var (status, isCharging) = statusByte switch
    {
        0 => (PowerSupplyStatus.Discharging, false),
        1 => (PowerSupplyStatus.Charging, true),
        2 => (PowerSupplyStatus.Charging, true),
        3 => (PowerSupplyStatus.Full, true),
        _ => (PowerSupplyStatus.NotCharging, false)
    };

    return new BatteryUpdateReturn(
        (int)batPercent,
        status,
        mv,
        isCharging
    );
}
    }
}
