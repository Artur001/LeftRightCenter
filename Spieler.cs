// Spieler.cs
using System;
using System.Collections.Generic;

// Klasse für die Spieler des Spiels
class Spieler
{
    // Anzahl der Chips, die der Spieler aktuell besitzt
    private int chips;
    // Name des Spielers
    private string name;

    // Konstruktor der Spielerklasse, der den Namen setzt und den Chipstand initialisiert
    public Spieler(string name)
    {
        this.name = name;
        this.chips = 3;
    }

    // Eigenschaft, um den Namen des Spielers abzurufen
    public string Name { get { return name; } }
    // Gibt zurück, ob der Spieler noch Chips hat
    public bool HatNochChips { get { return chips > 0; } }
    // Berechnet, wie viele Würfel der Spieler verwenden kann (maximal 3)
    public int AnzahlWuerfel { get { return Math.Min(chips, 3); } }

    // Erhöht den Chipstand um 1
    public void ErhalteChip() { chips++; }
    // Verringert den Chipstand um 1, sofern der Spieler noch mindestens einen Chip hat
    public void GebeChipAb() { if (chips > 0) { chips--; } }

    // Führt den Zug des Spielers aus, indem die Würfelergebnisse verwendet werden
    public List<int> SpieleZug(Becher becher)
    {
        Console.WriteLine($"{name} ist an der Reihe und würfelt.");
        becher.Schuettle(); // Stelle sicher, dass der Becher geschüttelt wird
        List<int> wuerfelErgebnisse = becher.GetZahlen(AnzahlWuerfel);
        Console.WriteLine($"{name} hat folgende Zahlen gewürfelt: {string.Join(", ", wuerfelErgebnisse)}");
        return wuerfelErgebnisse;
    }

    // Verarbeitet die Würfelergebnisse und gibt entsprechend Chips weiter
    public void VerarbeiteWuerfelergebnisse(List<int> zahlen)
    {
        foreach (int zahl in zahlen)
        {
            if (zahl == 4)
            {
                Console.WriteLine($"{name} gibt einen Chip nach links weiter.");
                Spiel.Instance.ChipNachLinksWeitergeben(this);
            }
            else if (zahl == 5)
            {
                Console.WriteLine($"{name} gibt einen Chip nach rechts weiter.");
                Spiel.Instance.ChipNachRechtsWeitergeben(this);
            }
            else if (zahl == 6)
            {
                Console.WriteLine($"{name} legt einen Chip in die Mitte.");
                Spiel.Instance.ChipInDieMitteLegen(this);
            }
        }
    }
}
