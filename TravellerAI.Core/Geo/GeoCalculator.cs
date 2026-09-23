namespace TravellerAI.Core.Geo;

public static class GeoCalculator
{
    private const double EarthRadiusKm = 6371.0;

    /// <summary>
    /// Great-circle distance between two WGS 84 points (haversine formula), km.
    /// </summary>
    public static double DistanceKm(double latitude1, double longitude1, double latitude2, double longitude2)
    {
        var dLat = ToRadians(latitude2 - latitude1);
        var dLon = ToRadians(longitude2 - longitude1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                + Math.Cos(ToRadians(latitude1)) * Math.Cos(ToRadians(latitude2)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        return EarthRadiusKm * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180;
}
