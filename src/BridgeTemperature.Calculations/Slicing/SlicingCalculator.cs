using BridgeTemperature.Calculations.Sections;
using BridgeTemperature.Shared.Extensions;
using BridgeTemperature.Shared.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BridgeTemperature.Calculations.Slicing
{
    internal class SlicingCalculator
    {
        public SectionSlice GetSlice(IList<PointD> section, double upperY, double lowerY)
        {
            var lowerCoordinates = this.LowerSection(section, lowerY);
            var upperCoordinates = this.UpperSection(lowerCoordinates, upperY);
            var sectionSlice = this.CalculateProperties(upperCoordinates);
            return sectionSlice;
        }

        private SectionSlice CalculateProperties(IList<PointD> coordinates)
        {
            var calculations = new SectionPropertiesCalculations(coordinates);
            var slice = new SectionSlice
            {
                Area = calculations.A,
                CentreOfGravityY = calculations.Y0
            };
            return slice;
        }

        private IList<PointD> LowerSection(IList<PointD> section, double elevation)
        {
            var compressedSection = new List<PointD>();

            for (int i = 0; i <= section.Count - 2; i++)
            {
                var pointA = section[i];
                var pointB = section[i + 1];
                if ((pointA.Y - pointB.Y).IsApproximatelyEqualTo(0))
                {
                    if ((pointA.Y >= elevation) && (pointB.Y >= elevation))
                    {
                        compressedSection.Add(pointA);
                        compressedSection.Add(pointB);
                    }
                }
                else
                {
                    var pointPP = this.IntersectionPoint(pointA, pointB, elevation);
                    if (this.IsPointInsideSection(pointA, pointB, pointPP))
                    {
                        if (pointA.Y > pointPP.Y)
                        {
                            compressedSection.Add(pointA);
                            compressedSection.Add(pointPP);
                        }
                        else
                        {
                            compressedSection.Add(pointPP);
                            compressedSection.Add(pointB);
                        }
                    }
                    else
                    {
                        if ((pointA.Y >= elevation) && (pointB.Y >= elevation))
                        {
                            compressedSection.Add(pointA);
                            compressedSection.Add(pointB);
                        }
                    }
                }
            }
            if (!compressedSection.First().Equals(compressedSection.Last()))
            {
                compressedSection.Add(compressedSection.First().Clone());
            }
            return compressedSection;
        }

        private List<PointD> UpperSection(IList<PointD> compressedSection, double elevation)
        {
            List<PointD> parabolicSection = new List<PointD>();

            for (int i = 0; i <= compressedSection.Count - 2; i++)
            {
                var pointA = compressedSection[i];
                var pointB = compressedSection[i + 1];
                if ((pointA.Y - pointB.Y).IsApproximatelyEqualTo(0))
                {
                    if ((pointA.Y <= elevation) && (pointB.Y <= elevation))
                    {
                        parabolicSection.Add(pointA);
                        parabolicSection.Add(pointB);
                    }
                }
                else
                {
                    var pointPP = this.IntersectionPoint(pointA, pointB, elevation);
                    if (this.IsPointInsideSection(pointA, pointB, pointPP))
                    {
                        if (pointA.Y > pointPP.Y)
                        {
                            parabolicSection.Add(pointPP);
                            parabolicSection.Add(pointB);
                        }
                        else
                        {
                            parabolicSection.Add(pointA);
                            parabolicSection.Add(pointPP);
                        }
                    }
                    else
                    {
                        if ((pointA.Y <= elevation) && (pointB.Y <= elevation))
                        {
                            parabolicSection.Add(pointA);
                            parabolicSection.Add(pointB);
                        }
                    }
                }
            }
            if (parabolicSection.Count > 0)
            {
                if (!((parabolicSection[0].X.IsApproximatelyEqualTo(parabolicSection[parabolicSection.Count - 1].X)) && (parabolicSection[0].Y.IsApproximatelyEqualTo(parabolicSection[parabolicSection.Count - 1].Y))))
                {
                    PointD P = new PointD();
                    P.X = parabolicSection[0].X;
                    P.Y = parabolicSection[0].Y;
                    parabolicSection.Add(P);
                }
            }

            return parabolicSection;
        }

        private PointD IntersectionPoint(PointD point1, PointD point2, double elevation)
        {
            var xa = point1.X;
            var xb = point2.X;
            var ya = point1.Y;
            var yb = point2.Y;

            var point = new PointD
            {
                Y = elevation,
                X = ((elevation - ya) * (xb - xa)) / (yb - ya) + xa
            };
            return point;
        }

        private bool IsPointInsideSection(PointD pointA, PointD pointB, PointD pointP)
            => ((Math.Min(pointA.X, pointB.X) <= pointP.X) && (pointP.X <= Math.Max(pointA.X, pointB.X)) && (Math.Min(pointA.Y, pointB.Y) <= pointP.Y)) && (pointP.Y <= Math.Max(pointA.Y, pointB.Y));
    }
}