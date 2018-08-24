using System;
using System.Collections.Generic;
using BridgeTemperature.Extensions;
using BridgeTemperature.Helpers;
using BridgeTemperature.SectionProperties;

namespace BridgeTemperature.IntegrationFunctions
{
    internal class SlicingCalculator
    {
        public SectionSlice GetSlice(IList<PointD> section, double upperY, double lowerY)
        {
            var lowerCoordinates = this.lowerSection(section, lowerY);
            var upperCoordinates = this.UpperSection(lowerCoordinates, upperY);
            var sectionSlice = this.calculateProperties(upperCoordinates);
            return sectionSlice;
        }

        private SectionSlice calculateProperties(IList<PointD> coordinates)
        {
            var calculations = new SectionPropertiesCalculations(coordinates);
            var slice = new SectionSlice
            {
                Area = calculations.A,
                CentreOfGravityY = calculations.Y0
            };
            return slice;
        }

        private IList<PointD> lowerSection(IList<PointD> section, double a)
        {
            var compressedSection = new System.Collections.Generic.List<PointD>();

            for (int i = 0; i <= section.Count - 2; i++)
            {
                var pointA = section[i];
                var pointB = section[i + 1];
                if ((pointA.Y - pointB.Y).IsApproximatelyEqualTo(0))
                {
                    if ((pointA.Y >= a) && (pointB.Y >= a))
                    {
                        compressedSection.Add(pointA);
                        compressedSection.Add(pointB);
                    }
                }
                else
                {
                    var pointPP = this.IntersectionPoint(pointA, pointB, a);
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
                        if ((pointA.Y >= a) && (pointB.Y >= a))
                        {
                            compressedSection.Add(pointA);
                            compressedSection.Add(pointB);
                        }
                    }
                }
            }
            if (!(((compressedSection[0].X).IsApproximatelyEqualTo(compressedSection[compressedSection.Count - 1].X)) && (compressedSection[0].Y.IsApproximatelyEqualTo(compressedSection[compressedSection.Count - 1].Y))))
            {
                PointD P = new PointD();
                P.X = compressedSection[0].X;
                P.Y = compressedSection[0].Y;
                compressedSection.Add(P);
            }
            return compressedSection;
        }

        private List<PointD> UpperSection(IList<PointD> compressedSection, double a)
        {
            List<PointD> parabolicSection = new System.Collections.Generic.List<PointD>();

            for (int i = 0; i <= compressedSection.Count - 2; i++)
            {
                var pointA = compressedSection[i];
                var pointB = compressedSection[i + 1];
                if ((pointA.Y - pointB.Y).IsApproximatelyEqualTo(0))
                {
                    if ((pointA.Y <= a) && (pointB.Y <= a))
                    {
                        parabolicSection.Add(pointA);
                        parabolicSection.Add(pointB);
                    }
                }
                else
                {
                    var pointPP = this.IntersectionPoint(pointA, pointB, a);
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
                        if ((pointA.Y <= a) && (pointB.Y <= a))
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

        private PointD IntersectionPoint(PointD a1, PointD a2, double a)
        {
            var xa = a1.X;
            var xb = a2.X;
            var ya = a1.Y;
            var yb = a2.Y;

            var point = new PointD
            {
                Y = a,
                X = ((a - ya) * (xb - xa)) / (yb - ya) + xa
            };
            return point;
        }

        private bool IsPointInsideSection(PointD pointA, PointD pointB, PointD pointP)
            => ((Math.Min(pointA.X, pointB.X) <= pointP.X) && (pointP.X <= Math.Max(pointA.X, pointB.X)) && (Math.Min(pointA.Y, pointB.Y) <= pointP.Y)) && (pointP.Y <= Math.Max(pointA.Y, pointB.Y));
    }
}