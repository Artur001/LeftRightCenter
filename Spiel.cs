// Spiel.cs
using System;
using System.Collections.Generic;

// Klasse, die das gesamte Spiel steuert - Enthält die Spielregeln und den Ablauf
class Spiel
{
    private Spieler aktuellerSpieler; // Der Spieler, der aktuell am Zug ist
    private List<Spieler> spielerListe; // Liste aller Spieler im Spiel
    private GUI gui; // Instanz der GUI zur Verwaltung der Ein- und Ausgaben
    private Becher becher; // Becher, der die Würfel enthält
    public static Spiel Instance; // Singleton-Instanz der Spielklasse

    // Konstruktor - Initialisiert das Spiel, GUI, Spieler und Becher
    public Spiel()
    {
        Instance = this;
        gui = new GUI();
        spielerListe = gui.FrageSpielerEingabe();
        becher = new Becher();
        SetzeStartSpieler();
        Spielen();
    }

    // Wählt zufällig einen Spieler, der das Spiel startet
    private void SetzeStartSpieler()
    {
        Random random = new Random();
        aktuellerSpieler = spielerListe[random.Next(spielerListe.Count)];
    }

    // Führt das Spiel durch, bis nur noch ein Spieler Chips hat
    public void Spielen()
    {
        int runde = 1;
        while (MehrAlsEinSpielerHatChips())
        {
            Console.WriteLine($"\n--- Runde {runde} ---");
            List<int> zahlen = aktuellerSpieler.SpieleZug(becher);
            aktuellerSpieler.VerarbeiteWuerfelergebnisse(zahlen);
            aktuellerSpieler = SpielerRechtsVonAktuellemSpieler();
            runde++;
            gui.PrintRangliste(spielerListe);
        }
        gui.PrintGewinner(spielerListe);
    }

    // Gibt den Spieler rechts vom aktuellen Spieler zurück (zyklisch)
    private Spieler SpielerRechtsVonAktuellemSpieler()
    {
        int index = (spielerListe.IndexOf(aktuellerSpieler) + 1) % spielerListe.Count;
        return spielerListe[index];
    }

    // Überprüft, ob noch mehr als ein Spieler Chips hat
    private bool MehrAlsEinSpielerHatChips()
    {
        int count = 0;
        foreach (Spieler spieler in spielerListe)
        {
            if (spieler.HatNochChips)
            {
                count++;
            }
            if (count > 1)
            {
                return true;
            }
        }
        return false;
    }

    // Gibt einen Chip des aktuellen Spielers an den linken Nachbarn weiter
    public void ChipNachLinksWeitergeben(Spieler aktuellerSpieler)
    {
        int index = spielerListe.IndexOf(aktuellerSpieler);
        int linkerIndex = (index - 1 + spielerListe.Count) % spielerListe.Count;
        Spieler linkerSpieler = spielerListe[linkerIndex];
        if (aktuellerSpieler.HatNochChips)
        {
            aktuellerSpieler.GebeChipAb();
            linkerSpieler.ErhalteChip();
        }
    }

    // Gibt einen Chip des aktuellen Spielers an den rechten Nachbarn weiter
    public void ChipNachRechtsWeitergeben(Spieler aktuellerSpieler)
    {
        int index = spielerListe.IndexOf(aktuellerSpieler);
        int rechterIndex = (index + 1) % spielerListe.Count;
        Spieler rechterSpieler = spielerListe[rechterIndex];
        if (aktuellerSpieler.HatNochChips)
        {
            aktuellerSpieler.GebeChipAb();
            rechterSpieler.ErhalteChip();
        }
    }

    // Legt einen Chip des aktuellen Spielers in die Mitte (Chip wird entfernt)
    public void ChipInDieMitteLegen(Spieler aktuellerSpieler)
    {
        if (aktuellerSpieler.HatNochChips)
        {
            aktuellerSpieler.GebeChipAb();
        }
    }
}
