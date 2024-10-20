// GUI.cs
using System;
using System.Collections.Generic;

// Klasse für die Benutzeroberfläche - Verwaltet die Eingaben und Ausgaben des Spiels
class GUI
{
    // Fragt die Spieler nach ihren Namen und erstellt eine Liste der Spieler
    public List<Spieler> FrageSpielerEingabe()
    {
        List<Spieler> spielerListe = new List<Spieler>();
        bool weitereSpieler = true;
        while (weitereSpieler)
        {
            Console.Write("Name des Spielers: ");
            string name = Console.ReadLine();
            Console.Write("Noch ein Spieler? (1 = ja, 2 = nein): ");
            string eingabe = Console.ReadLine();
            int antwort;
            // Validierung der Benutzereingabe, um sicherzustellen, dass nur 1 oder 2 eingegeben wird
            while (!int.TryParse(eingabe, out antwort) || (antwort != 1 && antwort != 2))
            {
                Console.WriteLine("Ungültige Eingabe. Bitte geben Sie 1 (ja) oder 2 (nein) ein.");
                Console.Write("Noch ein Spieler? (1 = ja, 2 = nein): ");
                eingabe = Console.ReadLine();
            }
            spielerListe.Add(new Spieler(name));
            weitereSpieler = (antwort == 1);
        }
        return spielerListe;
    }

    // Gibt die aktuelle Rangliste mit den Chip-Ständen aller Spieler aus
    public void PrintRangliste(List<Spieler> spielerListe)
    {
        Console.WriteLine("Aktueller Chip-Stand:");
        foreach (Spieler spieler in spielerListe)
        {
            Console.WriteLine($"{spieler.Name}: {spieler.AnzahlWuerfel} Chips");
        }
    }

    // Gibt den Gewinner des Spiels aus, sofern es noch einen Spieler mit Chips gibt
    public void PrintGewinner(List<Spieler> spielerListe)
    {
        Spieler gewinner = null;
        foreach (Spieler spieler in spielerListe)
        {
            if (spieler.HatNochChips)
            {
                gewinner = spieler;
                break;
            }
        }
        if (gewinner != null)
        {
            Console.WriteLine($"Gewinner: {gewinner.Name}");
        }
        else
        {
            Console.WriteLine("Kein Gewinner gefunden.");
        }
    }
}
