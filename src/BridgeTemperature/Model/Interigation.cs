using System;
using System.Linq;
using System.Collections.Generic;
using BridgeTemperature.Extensions;

using BridgeTemperature.Helpers;
using BridgeTemperature.SectionProperties;
using BridgeTemperature.Sections;
namespace BridgeTemperature.IntegrationFunctions
{
	public class SectionSlice
	{
		public double CentreOfGravityY { get; set; }
		public double Area { get; set; }
	}

	public interface IIntegration
	{
		double Moment { get; }
		double NormalForce { get; }
	}
	public interface IIntegrable
	{
		IList<PointD> Coordinates { get; }
		PointD CentreOfGravity { get; }
		SectionType Type { get; }
		double YMax { get; }
		double YMin { get; }
	}

	public class Integration : IIntegration
	{
		private readonly int numberOfSlices = 1500;
		public double Moment { get; private set; }
		public double NormalForce { get; private set; }

		public Integration()
		{
		}

		public void Reset()
		{
			Moment = 0;
			NormalForce = 0;
		}

		public void Integrate(IIntegrable section,PointD integrationPoint, Func<double, double> distributionFunction)
		{
			double resultantMoment = 0;
			double resultantNormalForce = 0;
			Slicing slicing = new Slicing();

			double sectionTypeMultiplier = (section.Type == SectionType.Void) ? -1 : 1;

			double currentY = section.YMin;
			double deltaY = (section.YMax - section.YMin) / this.numberOfSlices;
			do
			{
				SectionSlice slice = slicing.GetSlice(section.Coordinates, currentY + deltaY, currentY);
				currentY = currentY + deltaY;
				double value = distributionFunction(slice.CentreOfGravityY);
				double normalForce = value * slice.Area * sectionTypeMultiplier;
				double leverArm = Math.Abs(integrationPoint.Y - slice.CentreOfGravityY);
				double moment = ((slice.CentreOfGravityY < integrationPoint.Y) ? leverArm * value * slice.Area : -leverArm * value * slice.Area) * sectionTypeMultiplier;

				resultantMoment = resultantMoment + moment;
				resultantNormalForce = resultantNormalForce + normalForce;
			}
			while (currentY <= section.YMax);
			this.NormalForce = this.NormalForce+ resultantNormalForce;
			this.Moment = this.Moment + resultantMoment;
		}
	}

	class Slicing 
	{
		public SectionSlice GetSlice(IList<PointD> section, double upperY, double lowerY)
		{
			IList<PointD> lowerCoordinates = this.lowerSection(section, lowerY);
			IList<PointD> upperCoordinates = this.upperSection(lowerCoordinates, upperY);
			var sectionSlice = this.calculateProperties(upperCoordinates);
			return sectionSlice;
		}
		private SectionSlice calculateProperties(IList<PointD> coordinates)
		{
			SectionPropertiesCalculations calculations = new SectionPropertiesCalculations(coordinates);
			SectionSlice slice = new SectionSlice();
			slice.Area = calculations.A;
			slice.CentreOfGravityY = calculations.Y0;
			return slice;
		}
		private IList<PointD> lowerSection(IList<PointD> section, double a) 
		{
			IList<PointD> compressedSection = new System.Collections.Generic.List<PointD>();
			
			PointD A; 
			PointD B; 
			PointD PP; 
			for (int i = 0; i <= section.Count - 2; i++) 
			{
				A = section[i];
				B = section[i + 1];
				if ((A.Y - B.Y).IsApproximatelyEqualTo(0))
				{
					if ((A.Y >= a) && (B.Y >= a))
					{
						compressedSection.Add(A);
						compressedSection.Add(B);
					}
				}
				else
				{
					PP = this.intersectionPoint(A, B, a);
					if (this.isPointInsideSection(A, B, PP))
					{
						if (A.Y > PP.Y)
						{
							compressedSection.Add(A);
							compressedSection.Add(PP);
						}
						else
						{
							compressedSection.Add(PP);
							compressedSection.Add(B);
						}
					}
					else
					{
						if ((A.Y >= a) && (B.Y >= a))
						{
							compressedSection.Add(A);
							compressedSection.Add(B);
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
		private List<PointD> upperSection(IList<PointD> compressedSection, double a)
		{
			List<PointD> parabolicSection = new System.Collections.Generic.List<PointD>();
			PointD A; 
			PointD B; 
			PointD PP; 
			for (int i = 0; i <= compressedSection.Count - 2; i++) 
			{
				A = compressedSection[i];
				B = compressedSection[i + 1];
				if ((A.Y - B.Y).IsApproximatelyEqualTo(0))
				{
					if ((A.Y <= a) && (B.Y <= a))
					{
						parabolicSection.Add(A);
						parabolicSection.Add(B);
					}
				}
				else
				{
					PP = this.intersectionPoint(A, B, a);
					if (this.isPointInsideSection(A, B, PP))
					{
						if (A.Y > PP.Y)
						{
							parabolicSection.Add(PP);
							parabolicSection.Add(B);
						}
						else
						{
							parabolicSection.Add(A);
							parabolicSection.Add(PP);
						}
					}
					else
					{
						if ((A.Y <= a) && (B.Y <= a))
						{
							parabolicSection.Add(A);
							parabolicSection.Add(B);
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
		private PointD intersectionPoint(PointD a1, PointD a2, double a)
		{
			double xa, xb, ya, yb;
			xa = a1.X;
			xb = a2.X;
			ya = a1.Y;
			yb = a2.Y;

			PointD P = new PointD(); 
			P.Y = a; 
			P.X = ((a - ya) * (xb - xa)) / (yb - ya) + xa;
			return P;
		}
		private bool isPointInsideSection(PointD A, PointD B, PointD P)
		{
		
			if (((Math.Min(A.X, B.X) <= P.X) && (P.X <= Math.Max(A.X, B.X)) && (Math.Min(A.Y, B.Y) <= P.Y)) && (P.Y <= Math.Max(A.Y, B.Y)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
	}
}
