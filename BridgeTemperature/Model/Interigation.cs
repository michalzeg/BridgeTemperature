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
		private readonly int numberOfSlices = 1500; //CHECK ACCURACY LATER
		//private readonly PointD pointOfMomentIntegration = new PointD(0, 0);

		public double Moment { get; private set; }
		public double NormalForce { get; private set; }

		//private IIntegrable section;
		//private Func<double, double> distributionFunction;
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

				double sectionTypeMultiplier = (section.Type == SectionType.Fill) ? 1 : -1;

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
			//calculate section properties
			SectionPropertiesCalculations calculations = new SectionPropertiesCalculations(coordinates);
			SectionSlice slice = new SectionSlice();
			slice.Area = calculations.A;
			slice.CentreOfGravityY = calculations.Y0;
			return slice;
		}
		private IList<PointD> lowerSection(IList<PointD> section, double a) //lower boundary of the slice
		{

			IList<PointD> compressedSection = new System.Collections.Generic.List<PointD>();
			//przekroj - lista wspołrzędnych całego przekroju, punkt początkowy i końcowy się pokrywają
			// y=a - położenie strefy sciskanej (współrzędna y strefy sciskanej w globalnym ukł. Współrzędnych), os obojetna
			PointD A; //pierwszy punktu (i);
			PointD B; //kolejny punkt (i+1);
			PointD PP; //punkt przeciecia prostych
			for (int i = 0; i <= section.Count - 2; i++) //pętla "przelatująca" po wszystkich bokach przekroju
			{
				A = section[i];
				B = section[i + 1];
				//Sprawdzenie czy proste są równoległe (czy prosta wyznaczajca bok jest pozioma)
				if ((A.Y - B.Y).IsApproximatelyEqualTo(0))
				{
					//prosta pozioma (równoległa)
					//sprawdzenie czy prosta leży ponad strefą sciskaną (punkty należą do przekroju ściskanego)
					if ((A.Y >= a) && (B.Y >= a))
					{
						//punkt należy do przekroju sciskanego => dodaj do listy punktów strefy sciskanej
						compressedSection.Add(A);
						compressedSection.Add(B);
					}
				}
				else
				{
					//prosta nie pozioma (nie równoległa)
					//wyznaczenie punktu przeciecia (PP) prostej wyznaczającej bok z prosta wyznaczająca granice
					//strefy sciskanej
					PP = this.intersectionPoint(A, B, a);
					//sprawdzenie czy punkt przeciecia lezy na odcinku miedzy punktem a i b
					if (this.isPointInsideSection(A, B, PP))
					{
						//jesli punkt lezy na odcinku
						if (A.Y > PP.Y)
						{
							//jezeli punkt A lezy "wyzej" niz punkt przeciecia
							compressedSection.Add(A);
							compressedSection.Add(PP);
						}
						else
						{
							//jezeli punkt A lezy nizej niz punkt przeciecia
							compressedSection.Add(PP);
							compressedSection.Add(B);
						}
					}
					else
					{
						//punkt przeciecia nie lezy na odcinku AB, sprawdzenie czy odcinek AB leży w strefie ściskanej
						if ((A.Y >= a) && (B.Y >= a))
						{
							//odcinek lezy w strefie sciskanej
							//dodanie go do listy
							compressedSection.Add(A);
							compressedSection.Add(B);
						}
					}
				}

			}
			//sprawdzenie czy pierwszy i ostatni punkt przekroju sie pokrywają
			//jesli nie to dodatnie pierwszego punktu na koniec
			if (!(((compressedSection[0].X).IsApproximatelyEqualTo(compressedSection[compressedSection.Count - 1].X)) && (compressedSection[0].Y.IsApproximatelyEqualTo(compressedSection[compressedSection.Count - 1].Y))))
			{
				//nie jest okej, punkty sie nie pokrywaja
				PointD P = new PointD();
				P.X = compressedSection[0].X;
				P.Y = compressedSection[0].Y;
				compressedSection.Add(P);
			}
			return compressedSection;
		} //przekroj strefy sciskanej

		/*public List<PointD> CoordinatesOfLinearSection(List<PointD> compressedSection, double a)
		{
			//funkcja wyznacza obwiednie strefy sciskanej z liniowym wykresem naprezen
			//paramter przekrojSciskany to wspolrzedne strefy sciskanej
			//parametr a to położenie y=a osi dla której naprezenia zaczynają być liniowe
			List<PointD> linearSection = new System.Collections.Generic.List<PointD>();
			//przekroj - lista wspołrzędnych całego przekroju, punkt początkowy i końcowy się pokrywają
			// y=a - położenie strefy sciskanej (współrzędna y strefy sciskanej w globalnym ukł. Współrzędnych)
			PointD A; //pierwszy punktu (i);
			PointD B; //kolejny punkt (i+1);
			PointD PP; //punkt przeciecia prostych
			for (int i = 0; i <= compressedSection.Count - 2; i++) //pętla "przelatująca" po wszystkich bokach przekroju
			{
				A = compressedSection[i];
				B = compressedSection[i + 1];
				//Sprawdzenie czy proste są równoległe (czy prosta wyznaczajca bok jest pozioma)
				if (A.Y - B.Y == 0)
				{
					//prosta pozioma (równoległa)
					//sprawdzenie czy prosta leży ponad strefą sciskaną (punkty należą do przekroju o naprezeniu liniowym)
					if ((A.Y >= a) && (B.Y >= a))
					{
						//punkt należy do przekroju sciskanego => dodaj do listy punktów strefy liniowej
						linearSection.Add(A);
						linearSection.Add(B);
					}
				}
				else
				{
					//prosta nie pozioma (nie równoległa)
					//wyznaczenie punktu przeciecia (PP) prostej wyznaczającej bok z prosta wyznaczająca granice
					//strefy liniowej
					PP = this.intersectionPoint(A, B, a);
					//sprawdzenie czy punkt przeciecia lezy na odcinku miedzy punktem a i b
					if (this.isPointInsideSection(A, B, PP))
					{
						//jesli punkt lezy na odcinku
						if (A.Y > PP.Y)
						{
							//jezeli punkt A lezy "wyzej" niz punkt przeciecia
							linearSection.Add(A);
							linearSection.Add(PP);
						}
						else
						{
							//jezeli punkt A lezy nizej niz punkt przeciecia
							linearSection.Add(PP);
							linearSection.Add(B);
						}
					}
					else
					{
						//punkt przeciecia nie lezy na odcinku AB, sprawdzenie czy odcinek AB leży w strefie liniowej
						if ((A.Y >= a) && (B.Y >= a))
						{
							//odcinek lezy w strefie liniowej
							//dodanie go do listy
							linearSection.Add(A);
							linearSection.Add(B);
						}
					}
				}

			}
			//sprawdzenie czy pierwszy i ostatni punkt przekroju sie pokrywają
			//jesli nie to dodatnie pierwszego punktu na koniec
			if (linearSection.Count > 0)
			{
				if (!((linearSection[0].X == linearSection[linearSection.Count - 1].X) && (linearSection[0].Y == linearSection[linearSection.Count - 1].Y)))
				{
					//nie jest okej, punkty sie nie pokrywaja
					PointD P = new PointD();
					P.X = linearSection[0].X;
					P.Y = linearSection[0].Y;
					linearSection.Add(P);
				}
			}
			return linearSection;
		}//przekroj o liniowym naprezeniu*/
		private List<PointD> upperSection(IList<PointD> compressedSection, double a)//upper boundary of the slice
		{
			//funkcja wyznacza obwiednie strefy sciskanej z parabolicznym wykresem naprezen
			//paramter przekrojSciskany to wspolrzedne strefy sciskanej
			//parametr a to położenie y=a osi dla której naprezenia zaczynają być liniowe, wyznaczamy przekroj ponizej
			List<PointD> parabolicSection = new System.Collections.Generic.List<PointD>();
			//przekroj - lista wspołrzędnych całego przekroju, punkt początkowy i końcowy się pokrywają
			// y=a - położenie strefy sciskanej (współrzędna y strefy sciskanej w globalnym ukł. Współrzędnych)
			PointD A; //pierwszy punktu (i);
			PointD B; //kolejny punkt (i+1);
			PointD PP; //punkt przeciecia prostych
			for (int i = 0; i <= compressedSection.Count - 2; i++) //pętla "przelatująca" po wszystkich bokach przekroju
			{
				A = compressedSection[i];
				B = compressedSection[i + 1];
				//Sprawdzenie czy proste są równoległe (czy prosta wyznaczajca bok jest pozioma)
				if ((A.Y - B.Y).IsApproximatelyEqualTo(0))
				{
					//prosta pozioma (równoległa)
					//sprawdzenie czy prosta leży pod granica strefy liniowej (punkty należą do przekroju o naprezeniu parabolicznym)
					if ((A.Y <= a) && (B.Y <= a))
					{
						//punkt należy do przekroju sciskanego parabolicznego => dodaj do listy punktów strefy parabolicznej
						parabolicSection.Add(A);
						parabolicSection.Add(B);
					}
				}
				else
				{
					//prosta nie pozioma (nie równoległa)
					//wyznaczenie punktu przeciecia (PP) prostej wyznaczającej bok z prosta wyznaczająca granice
					//strefy liniowej
					PP = this.intersectionPoint(A, B, a);
					//sprawdzenie czy punkt przeciecia lezy na odcinku miedzy punktem A i B
					if (this.isPointInsideSection(A, B, PP))
					{
						//jesli punkt lezy na odcinku
						if (A.Y > PP.Y)
						{
							//jezeli punkt A lezy "wyzej" niz punkt przeciecia
							parabolicSection.Add(PP);
							parabolicSection.Add(B);
						}
						else
						{
							//jezeli punkt A lezy nizej niz punkt przeciecia
							parabolicSection.Add(A);
							parabolicSection.Add(PP);
						}
					}
					else
					{
						//punkt przeciecia nie lezy na odcinku AB, sprawdzenie czy odcinek AB leży w strefie parabolicznej
						if ((A.Y <= a) && (B.Y <= a))
						{
							//odcinek lezy w strefie sciskanej
							//dodanie go do listy
							parabolicSection.Add(A);
							parabolicSection.Add(B);
						}
					}
				}

			}
			//sprawdzenie czy pierwszy i ostatni punkt przekroju sie pokrywają
			//jesli nie to dodatnie pierwszego punktu na koniec
			if (parabolicSection.Count > 0)
			{
				if (!((parabolicSection[0].X.IsApproximatelyEqualTo(parabolicSection[parabolicSection.Count - 1].X)) && (parabolicSection[0].Y.IsApproximatelyEqualTo(parabolicSection[parabolicSection.Count - 1].Y))))
				{
					//nie jest okej, punkty sie nie pokrywaja
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
			//funkcja wyszukuje punkty przeciecia
			//boków przekroju z prosta wyznaczająca strefę ściskaną
			//funkcja nie sprawdza czy proste są równoległe
			//punkt a1 i a2 wyznaczają prostą a wartość y=a wyznacza położenie prostej wyznaczającej położenie
			//strefy sciskanej
			double xa, xb, ya, yb;
			xa = a1.X;
			xb = a2.X;
			ya = a1.Y;
			yb = a2.Y;

			PointD P = new PointD(); //punkt przeciecia
			P.Y = a; //współrzędna y punktu przeciecia równa sie współrzędnej prostej (prosta pozioma)
			P.X = ((a - ya) * (xb - xa)) / (yb - ya) + xa;
			return P;
		}
		private bool isPointInsideSection(PointD A, PointD B, PointD P)
		{
			//funkcja sprawdza czy punkt P lezy wewnąrz odcinka AB. 
			//sprawdzenie polega na porownaniu odleglosc |AB| = |AP| +|PB|
			/*double AB, AP, PB; //odpowiednie odługość ocinków
			AB = Math.Sqrt(Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y,2));
			AP = Math.Sqrt(Math.Pow(A.X - P.X, 2) + Math.Pow(A.Y - P.Y, 2));
			PB = Math.Sqrt(Math.Pow(P.X - B.X, 2) + Math.Pow(P.Y - B.Y, 2));
			if (Math.Round(AB,6) == Math.Round(AP,6) + Math.Round(PB,6))
			{
				return true; //punkt lezy na odcinku
			}
			else
			{
				return false;
			}*/
			if (((Math.Min(A.X, B.X) <= P.X) && (P.X <= Math.Max(A.X, B.X)) && (Math.Min(A.Y, B.Y) <= P.Y)) && (P.Y <= Math.Max(A.Y, B.Y)))
			{
				//punkt lezy na odcinku
				return true;
			}
			else
			{
				return false;
			}
		}


		/*public double ResultantOfParabolicSection(List<PointD> parabolicSection, double y2, double y)
		{
			//okresla wypadkową siłe ze strefy parabolicznej
			//strefaparaboliczna - lsita współrzednych punktów przekroju strefy parabolicznej
			// y - współrzedna y osi obojętnej
			// y2 - wspolrzedna y osi na ktorej odksztalcenia wynosza 2 promile. Potrzebne do okreslenia rownania paraboli  
			double ymax = y2 - y; //odległość od odkształcenia równego 0 do okształcenia równego ec2
			//zmienne pomocnicze
			double a = this.fcd*ymax/3;
			double c = 1 / ymax;
			double V = 0.0;
			double dx;
			double dy;
			double xi;
			double yi;
			//objetosc strefy sciskanej, wartosc wypadkowej sily w betonie
			for (int i = 0; i <= parabolicSection.Count - 2; i++)
			{
				dx = parabolicSection[i + 1].X - parabolicSection[i].X;
				dy = parabolicSection[i + 1].Y - parabolicSection[i].Y;
				xi = parabolicSection[i].X;
				yi = parabolicSection[i].Y - y; //sprowadzenie do ukladu lokalnego, os obojętna ma współrzędną y = 0.
				//V = V + a * dx * (Math.Pow(c * yi - 1, 3) - Math.Pow(c * dy, 3) / 4 + Math.Pow(c * dy, 2) * (c * yi - 1) - 0.5 * 3 * c * dy * Math.Pow(c * yi - 1, 2)) + this.fcd * dy * (dx / 2 + xi);
				V = V + a * dx * (c * c * c * dy * dy * dy / 4 + Math.Pow(c * yi - 1, 3) + c * c * dy * dy * (c * yi - 1) + 0.5 * 3 * c * dy * (c * yi - 1) * (c * yi - 1)) + fcd * dy * (dx / 2 + xi);
			}
			return V;
		}
		public double FirstMomentOfAreaOfParabolicSection(List<PointD> parabolicSection, double y2, double y)
		{
			//okresla moment statyczny  strefy parabolicznej do obliczania ramienia sił wewnętrznych
			//strefaparaboliczna - lsita współrzednych punktów przekroju strefy parabolicznej
			// y - współrzedna y osi obojętnej
			// y2 - wspolrzedna y osi na ktorej odksztalcenia wynosza 2 promile. Potrzebne do okreslenia rownania paraboli  
			double ymax = y2 - y; //odległość od odkształcenia równego 0 do okształcenia równego 2promile
			//zmienne pomocnicze
			double c = 1 / ymax;
			double S = 0.0;
			double dx;
			double dy;
			double xi;
			double yi;
			for (int i = 0; i <= parabolicSection.Count - 2; i++)
			{
				dx = parabolicSection[i + 1].X - parabolicSection[i].X;
				dy = parabolicSection[i + 1].Y - parabolicSection[i].Y;
				xi = parabolicSection[i].X;
				yi = parabolicSection[i].Y -y;//sprowadzenie do układu lokalnego 
				//S = S + this.fcd * dx * (Math.Pow(c, 2) * (Math.Pow(dy, 4) / 20 + Math.Pow(dy, 3) * yi / 4 + Math.Pow(dy * yi, 2) / 2 + dy * Math.Pow(yi, 3) / 2 + Math.Pow(yi, 4) / 4) - c * (Math.Pow(dy, 3) / 6 + 2 * Math.Pow(dy, 2) * yi / 3 + dy * yi * yi + 2 * Math.Pow(yi, 3) / 3) + (dy / 6 + dy * yi / 2 + yi * yi / 2) - 1 / (12 * c * c)) + this.fcd * dy * (dx * dy / 3 + dy * xi / 2 + dx * yi / 2 + xi * yi);
				S = S + this.fcd / (c * c) * dx * (c * c * c * c * dy * dy * dy * dy / 20 + c * c * c * c * dy * dy * dy * yi / 4 + c * c * c * c * dy * dy * yi * yi / 2 + c * c * c * c * dy * yi * yi * yi / 4 - c * c * c * dy * dy * dy / 6 - 2 * c * c * c * dy * dy * yi / 3 - c * c * c * dy * yi * yi - 2 * c * c * c * yi * yi * yi / 3 + c * c * yi * yi / 6 + c * c * dy * yi / 2 + c * c * yi * yi / 2 - 1 / 12) + this.fcd * dy * (dx * dy / 3 + dy * xi / 2 + dx * yi / 2 + xi * yi);
			}
			return S;
		}
		public double ResultantOfLinearSection(List<PointD> linearSection)
		{
			double x1, x2, y1, y2;
			double V = 0;
			for (int i = 0; i <= linearSection.Count - 2; i++) 
			{
				x1 = linearSection[i].X;
				x2 = linearSection[i+1].X;
				y1=linearSection[i].Y;
				y2 = linearSection[i+1].Y;
				V = V + (x1 - x2) * (y2 + y1);
			}
			V = 0.5 * V * this.fcd;
			return V;
		}
		public double FirstMomentOfAreaOfLinearSection(List<PointD> linearSection)
		{
			double x1, x2, y1, y2;
			double S = 0;
			for (int i = 0; i <= linearSection.Count - 2; i++)
			{
				x1 = linearSection[i].X;
				x2 = linearSection[i + 1].X;
				y1 = linearSection[i].Y;
				y2 = linearSection[i + 1].Y;
				S = S + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
			}
			S = S * this.fcd / 6;
			return S;
		}
	}*/

	}
}
