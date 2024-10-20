// Wuerfel.cs
using System;

// Klasse für einen Würfel, der eine Zufallszahl zwischen 1 und MAX_NUMMER generiert
class Wuerfel
{
    private const int MAX_NUMMER = 6; // Maximaler Wert, den der Würfel zeigen kann
    private Random random; // Zufallsgenerator für den Würfelwurf
    private int letzteZahl; // Letzte gewürfelte Zahl

    // Eigenschaft, um die zuletzt gewürfelte Zahl abzurufen
    public int LetzteZahl { get { return letzteZahl; } }

    // Konstruktor - Initialisiert den Zufallsgenerator
    public Wuerfel()
    {
        random = new Random();
    }

    // Würfelt und speichert die gewürfelte Zahl in letzteZahl
    public void Wuerfle()
    {
        letzteZahl = random.Next(1, MAX_NUMMER + 1);
    }
}
