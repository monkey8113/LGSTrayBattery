namespace LGSTrayPrimitives
{
    public enum PowerSupplyStatus : byte
    {
        POWER_SUPPLY_STATUS_DISCHARGING = 0,
        POWER_SUPPLY_STATUS_CHARGING = 1,
        POWER_SUPPLY_STATUS_FULL = 2,
        POWER_SUPPLY_STATUS_NOT_CHARGING = 3,
        POWER_SUPPLY_STATUS_UNKNOWN = 4
    }
}
