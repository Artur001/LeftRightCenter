// Becher.cs
using System;
using System.Collections.Generic;

// Klasse für den Becher, der die Würfel enthält und die Würfelwürfe durchführt
class Becher
{
    private const int ANZ_WUERFEL = 3; // Anzahl der Würfel im Becher
    private List<Wuerfel> wuerfel;

    // Konstruktor - Erstellt die Würfel im Becher
    public Becher()
    {
        wuerfel = new List<Wuerfel>();
        for (int i = 0; i < ANZ_WUERFEL; i++)
        {
            wuerfel.Add(new Wuerfel());
        }
    }

    // Schüttelt alle Würfel, sodass jeder Würfel eine neue Zufallszahl erhält
    public void Schuettle()
    {
        foreach (var wuerfel in wuerfel)
        {
            wuerfel.Wuerfle();
        }
    }

    // Gibt die Zahlen der geschüttelten Würfel basierend auf der Anzahl zurück
    public List<int> GetZahlen(int anzahl)
    {
        List<int> zahlen = new List<int>();
        for (int i = 0; i < anzahl; i++)
        {
            zahlen.Add(wuerfel[i].LetzteZahl);
        }
        return zahlen;
    }
}
