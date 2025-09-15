public class FeatureCollection
{
    // Problem 5 - JSON model for USGS GeoJSON feed (minimal subset needed)
    // We only need Features -> each Feature has Properties with Mag and Place.
    public List<Feature> Features { get; set; }
}

public class Feature
{
    public FeatureProperties Properties { get; set; }
}

public class FeatureProperties
{
    // 'mag' can be null in the USGS feed; if absent JSON deserializer will leave default 0.0
    // We will treat 0.0 with empty place as invalid downstream.
    public double Mag { get; set; }
    public string Place { get; set; }
}