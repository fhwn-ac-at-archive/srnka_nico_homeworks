// Copyright (c) 2020 Kevin Perry
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

using System;

namespace Coordinator
{
    /// <summary>
    /// Simple representation of naive 3D coordinates.
    /// Coordinates are represented as triples of double values.
    /// This representation is agnostic of specific coordinate systems.
    /// </summary>
    public readonly struct Coordinates
    {
        /// <summary>
        /// Create a new Coordinates object.
        /// Coordinates are represented as triples of double values.
        /// This representation is agnostic of specific coordinate systems.
        /// </summary>
        /// <param name="x">
        /// The 'x' coordinate.
        /// It must be a finite (zero, normal, or subnormal) double value.
        /// </param>
        /// <param name="y">
        /// The 'y' coordinate.
        /// It must be a finite (zero, normal, or subnormal) double value.
        /// </param>
        /// <param name="y">
        /// The optional 'z' coordinate.
        /// It must be a finite (zero, normal, or subnormal) double value.
        /// </param>
        /// <exception c="System.ArgumentOutOfRangeException">
        /// Thrown when at least one of the supplied coordinates is not a fine (zero, normal, or subnormal) double value.
        /// </exception>
        public Coordinates(double x, double y, double z = 0.0)
        {
            if (!Double.IsFinite(x))
                throw new ArgumentOutOfRangeException("x");

            if (!Double.IsFinite(y))
                throw new ArgumentOutOfRangeException("y");

            if (!Double.IsFinite(z))
                throw new ArgumentOutOfRangeException("z");

            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Retrieve the stored X coordinate.
        /// </summary>
        public double X { get; }
        /// <summary>
        /// Retrieve the stored Y coordinate.
        /// </summary>
        public double Y { get; }

        /// <summary>
        /// Retrieve the stored Z coordinate.
        /// </summary>
        public double Z { get; }

        /// <summary>
        /// Provide the string representation of the stored coordinates.
        /// The output is formatted as "(X: {X}, Y: {Y}, Z: {Z})" where the coordinates are restricted to 5 decimal places.
        /// </summary>
        public override string ToString() => $"(X: {X:F5}, Y: {Y:F5}, Z: {Z:F5})";
    }

    public static class Converter
    {
        private const double DEGREE2RADIAN = 0.01745329251994329577;
        private const double RADIAN2DEGREE = 57.29577951308232088;
        private const double EPSILON = 1E-10;

        private static double Phi2Z(double eccent, double ts)
        {
            const double HALF_PI = Math.PI / 2;

            var eccenth = eccent / 2;
            var phi = HALF_PI - 2 * Math.Atan(ts);
            for (var i = 0; i <= 15; ++i)
            {
                var con = eccent * Math.Sin(phi);
                var delta_phi = HALF_PI - 2 * Math.Atan(ts * Math.Pow((1 - con) / (1 + con), eccenth)) - phi;

                phi += delta_phi;
                if (Math.Abs(delta_phi) <= 1E-10)
                    return phi;
            }

            throw new System.ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Convert geodetic coordinates (latitude, longitude, and height in radians) to geocentric coordinates (x,y and z in meters) according to the provided ellipsoid parameters.
        /// </summary>
        private static Coordinates GeodeticToGeocentric(Coordinates input, double es, double a)
        {
            var lon = input.X;
            var lat = input.Y;
            var height = input.Z;

            var sin_lat = Math.Sin(lat);
            var cos_lat = Math.Cos(lat);

            // earth radius at location
            var rn = a / (Math.Sqrt(1.0 - es * sin_lat * sin_lat));

            var x = rn * cos_lat * Math.Cos(lon);
            var y = rn * cos_lat * Math.Sin(lon);
            var z = (rn * (1 - es) + height) * sin_lat;

            return new Coordinates(x, y, z);
        }

        /// <summary>
        /// Convert geocentric coordinates (x, y, and z in meters) to geodetic coordinates (latitude, longitude, and height in radians) according to the provided ellipsoid parameters.
        /// </summary>
        private static Coordinates GeocentricToGeodetic(Coordinates input, double es, double a, double b)
        {
            const double eps = 1E-12;

            // distance between semi-minor axis and locatio
            var p = Math.Sqrt(input.X * input.X + input.Y * input.Y);

            // distance between center and location
            var rr = Math.Sqrt(input.X * input.X + input.Y * input.Y + input.Z * input.Z);

            // sin of geocentric latitude
            var ct = input.Z / rr;
            // cos of geocentric latitude
            var st = p / rr;

            var rx = 1.0 / Math.Sqrt(1.0 - es * (2.0 - es) * st * st);

            var cphi0 = st * (1.0 - es) * rx;
            var sphi0 = ct * rx;

            var cphi = 0.0;
            var sphi = 0.0;
            var height = 0.0;
            for (var i = 0; i < 30; ++i)
            {
                // earth radius at location
                var rn = a / Math.Sqrt(1.0 - es * sphi0 * sphi0);

                height = p * cphi0 + input.Z * sphi0 - rn * (1 - es * sphi0 * sphi0);

                var rk = es * rn / (rn + height);
                rx = 1.0 / Math.Sqrt(1.0 - rk * (2.0 - rk) * st * st);

                cphi = st * (1.0 - rk) * rx;
                sphi = ct * rx;

                var sdphi = sphi * cphi0 - cphi * sphi0;
                if (sdphi * sdphi <= eps * eps)
                    break;

                cphi0 = cphi;
                sphi0 = sphi;
            }

            var lon = Math.Atan2(input.Y, input.X);
            var lat = Math.Atan(sphi / Math.Abs(cphi));

            return new Coordinates(lon, lat, height);
        }

        private readonly struct ToWGS84
        {
            public ToWGS84(
                double dx, double dy, double dz,
                double rx, double ry, double rz,
                double s)
            {
                DeltaX = dx;
                DeltaY = dy;
                DeltaZ = dz;

                RotationX = rx;
                RotationY = ry;
                RotationZ = rz;

                Scaling = s;
            }

            public double DeltaX { get; }
            public double DeltaY { get; }
            public double DeltaZ { get; }
            public double RotationX { get; }
            public double RotationY { get; }
            public double RotationZ { get; }
            public double Scaling { get; }
        }

        private static Coordinates GeocentricToWGS84(Coordinates input, ToWGS84 towgs84)
        {
            var x = towgs84.Scaling
                * (input.X - towgs84.RotationZ * input.Y + towgs84.RotationY * input.Z)
                + towgs84.DeltaX;

            var y = towgs84.Scaling
                * (towgs84.RotationZ * input.X + input.Y - towgs84.RotationX * input.Z)
                + towgs84.DeltaY;

            var z = towgs84.Scaling
                * (-towgs84.RotationY * input.X + towgs84.RotationX * input.Y + input.Z)
                + towgs84.DeltaZ;

            return new Coordinates(x, y, z);
        }

        /// <summary>
        /// Converts Austria Lambert (EPSG=31286) coordinates to WGS84 (EPSG=3857) coordinates.
        /// </summary>
        /// <param name="input">
        /// The input according to MGI/Austria Lambert (EPSG=31286)
        /// </param>
        /// <returns>
        /// The corresponding coordinates in WGS84 (EPSG=3857).
        /// </returns>
        public static Coordinates AustriaLambertToWGS84(Coordinates input)
        {
            // we're stealing this implementation from proj4js
            // what follows is a bunch of magic values
            // don't do this at home

            // Center Meridian= 13.333333333
            const double lon0 = (40.0 / 3) * DEGREE2RADIAN;

            // False Easting (x0)= 400000
            // Flase Northing (y0)= 400000
            const double x0 = 400000;
            const double y0 = 400000;

            // Ellipsoid Bessel 1841
            const double bessel_a = 6377397.155;
            const double bessel_e = 0.08169683087473478;
            const double bessel_es = 0.006674372174974933;

            // Ellipsoid WGS84
            const double wgs84_a = 6378137;
            const double wgs84_b = 6356752.314245179;
            const double wgs84_es = 0.006694379990141316;

            const double f0 = 1.8345139919919868;
            const double ns = 0.7373626271677453;
            const double con = 1.356184817558575;
            const double rh = 5851760.423457716;

            var towgs84 = new ToWGS84(
                577.326, 90.129, 463.919,
                0.00002490487879859686, 0.00000714615365955456, 0.00002568058068837212,
                1.0000024232);

            // convert cartesian to longitude/latitude
            // adapt for false easting and northing
            var x = input.X - x0;
            var y = rh - (input.Y - y0);

            var rh1 = Math.Sqrt(x * x + y * y);
            var theta = (rh1 != 0.0) ? Math.Atan2(x, y) : 0.0;
            var ts = Math.Pow((rh1 / (bessel_a * f0)), con);

            // propagate exception when phi doesn't converge
            var lon = theta / ns + lon0;
            var lat = Phi2Z(bessel_e, ts);

            // transform geodetic to geocentric
            var geocentric = GeodeticToGeocentric(new Coordinates(lon, lat), bessel_es, bessel_a);

            // datum transformation
            var transformed = GeocentricToWGS84(geocentric, towgs84);

            var geodetic = GeocentricToGeodetic(transformed, wgs84_es, wgs84_a, wgs84_b);

            // convert radians back to degrees
            lon = geodetic.X * RADIAN2DEGREE;
            lat = geodetic.Y * RADIAN2DEGREE;

            var c = new Coordinates(lon, lat);
            return c;
        }
    }
}
