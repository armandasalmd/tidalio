﻿using System.Collections.Generic;

namespace Tidalio.Newton
{
    public class TidalStationsNewton
    {
        public string type { get; set; }
        public IList<Feature> features { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public IList<double> coordinates { get; set; }
    }

    public class Properties
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string ContinuousHeightsAvailable { get; set; }
        public string Footnote { get; set; }
    }
}