# LeftRightCenter
OBA
// Spieler.cs
using System;
using System.Collections.Generic;

abstract class Spieler
{
    protected string name;
    protected int chips;

    // Konstruktor für die Klasse Spieler - Initialisiert den Namen des Spielers und setzt die Anzahl der Chips auf 3
    public Spieler(string name)
    {
        this.name = name;
        this.chips = 3;
    }

    // Getter-Methode, um den Namen des Spielers zurückzugeben
    public string GetName() { return name; }
    // Getter-Methode, um die Anzahl der Chips des Spielers zurückzugeben
    public int GetChips() { return chips; }
    // Gibt die Anzahl der Würfel zurück, die ein Spieler verwenden darf (maximal 3)
    public int GetAnzahlWuerfel() { return Math.Min(chips, 3); }
    // Erhöht die Anzahl der Chips um 1
    public void ErhalteChip() { chips++; }
    // Verringert die Anzahl der Chips um 1, wenn der Spieler noch Chips hat
    public void GebeChipAb() { if (chips > 0) { chips--; } }

    // Methode für den Spielzug eines Spielers - Würfelt und verarbeitet die Ergebnisse
    public virtual void Spielzug(Becher becher)
    {
        Console.WriteLine($"{name} ist an der Reihe und würfelt.");
        List<int> zahlen = becher.Schuettle(GetAnzahlWuerfel());
        Console.WriteLine($"{name} hat folgende Zahlen gewürfelt: {string.Join(", ", zahlen)}");
        VerarbeiteWurfErgebnisse(zahlen);
    }

    // Verarbeitet die Ergebnisse des Würfelwurfs und führt entsprechende Aktionen aus (z.B. Chip weitergeben oder ablegen)
    protected virtual void VerarbeiteWurfErgebnisse(List<int> zahlen)
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
                GebeChipAb();
            }
        }
    }
}

// MenschlicherSpieler.cs
// Klasse für menschliche Spieler - Erbt von der abstrakten Klasse Spieler
class MenschlicherSpieler : Spieler
{
    public MenschlicherSpieler(string name) : base(name) { }
}

// KI_Spieler.cs
// Klasse für KI-Spieler - Erbt von der abstrakten Klasse Spieler und überschreibt den Spielzug
class KI_Spieler : Spieler
{
    public KI_Spieler(string name) : base(name) { }

    // Der KI-Spieler führt seinen Spielzug automatisch aus
    public override void Spielzug(Becher becher)
    {
        Console.WriteLine($"KI-Spieler {name} ist an der Reihe und würfelt automatisch.");
        List<int> zahlen = becher.Schuettle(GetAnzahlWuerfel());
        Console.WriteLine($"KI-Spieler {name} hat folgende Zahlen gewürfelt: {string.Join(", ", zahlen)}");
        VerarbeiteWurfErgebnisse(zahlen);
    }

    // Spezielle Logik für KI-Spieler - Bevorzugt Chips nach links weiterzugeben
    protected override void VerarbeiteWurfErgebnisse(List<int> zahlen)
    {
        foreach (int zahl in zahlen)
        {
            if (zahl == 4)
            {
                Console.WriteLine($"{name} (KI) gibt bevorzugt einen Chip nach links weiter.");
                Spiel.Instance.ChipNachLinksWeitergeben(this);
            }
            else if (zahl == 5)
            {
                Console.WriteLine($"{name} (KI) gibt einen Chip nach rechts weiter.");
                Spiel.Instance.ChipNachRechtsWeitergeben(this);
            }
            else if (zahl == 6)
            {
                Console.WriteLine($"{name} (KI) legt einen Chip in die Mitte.");
                GebeChipAb();
            }
        }
    }
}

// Becher.cs
using System;
using System.Collections.Generic;

// Klasse für den Becher, der die Würfel enthält
class Becher
{
    private List<Wuerfel> wuerfel;

    // Konstruktor - Initialisiert drei Würfel
    public Becher()
    {
        wuerfel = new List<Wuerfel>();
        for (int i = 0; i < 3; i++)
        {
            wuerfel.Add(new Wuerfel());
        }
    }

    // Methode, um eine bestimmte Anzahl von Würfeln zu schütteln und deren Ergebnisse zurückzugeben
    public List<int> Schuettle(int anzahl)
    {
        List<int> zahlen = new List<int>();
        for (int i = 0; i < anzahl; i++)
        {
            zahlen.Add(wuerfel[i].Wuerfle());
        }
        return zahlen;
    }
}

// Wuerfel.cs
using System;

// Klasse für den Würfel, der eine Zufallszahl zwischen 1 und MAX_NUMMER generiert
class Wuerfel
{
    private static readonly int MAX_NUMMER = 6;

    // Zufallszahlengenerator zentralisiert, um konsistente Zufälligkeit sicherzustellen
    private static Random random = Spiel.Instance.Zufallsgenerator;

    // Methode zum Würfeln - Gibt eine Zufallszahl zwischen 1 und MAX_NUMMER zurück
    public int Wuerfle()
    {
        return random.Next(1, MAX_NUMMER + 1);
    }
}

// GUI.cs
using System;
using System.Collections.Generic;

// Klasse für die Benutzeroberfläche - Verwaltet die Eingabe und Ausgabe
class GUI
{
    // Fragt die Spieler nach ihren Namen und ob sie menschlich oder KI sind, um die Spieler zu initialisieren
    public List<Spieler> FrageSpielerEingabe()
    {
        List<Spieler> spielerListe = new List<Spieler>();
        bool weitereSpieler = true;
        while (weitereSpieler)
        {
            Console.Write("Name des Spielers: ");
            string name = Console.ReadLine();
            Console.Write("Menschlicher Spieler (1) oder KI-Spieler (2)? ");
            string spielerTypEingabe = Console.ReadLine();
            int spielerTyp;
            while (!int.TryParse(spielerTypEingabe, out spielerTyp) || (spielerTyp != 1 && spielerTyp != 2))
            {
                Console.WriteLine("Ungültige Eingabe. Bitte geben Sie 1 (Menschlicher Spieler) oder 2 (KI-Spieler) ein.");
                spielerTypEingabe = Console.ReadLine();
            }
            if (spielerTyp == 1)
            {
                spielerListe.Add(new MenschlicherSpieler(name));
            }
            else
            {
                spielerListe.Add(new KI_Spieler(name));
            }
            Console.Write("Noch ein Spieler? (1 = ja, 2 = nein): ");
            string eingabe = Console.ReadLine();
            int antwort;
            while (!int.TryParse(eingabe, out antwort) || (antwort != 1 && antwort != 2))
            {
                Console.WriteLine("Ungültige Eingabe. Bitte geben Sie 1 (ja) oder 2 (nein) ein.");
                Console.Write("Noch ein Spieler? (1 = ja, 2 = nein): ");
                eingabe = Console.ReadLine();
            }
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
            Console.WriteLine($"{spieler.GetName()}: {spieler.GetChips()} Chips");
        }
    }

    // Gibt den Gewinner des Spiels aus
    public void PrintGewinner(List<Spieler> spielerListe)
    {
        Spieler gewinner = null;
        foreach (Spieler spieler in spielerListe)
        {
            if (spieler.GetChips() > 0)
            {
                gewinner = spieler;
                break;
            }
        }
        if (gewinner != null)
        {
            Console.WriteLine($"Gewinner: {gewinner.GetName()}");
        }
        else
        {
            Console.WriteLine("Kein Gewinner gefunden.");
        }
    }

    // Bietet dem Benutzer eine Option, das Spiel neu zu starten oder zu beenden
    public void SpielEndeOption()
    {
        Console.WriteLine("
Wählen Sie eine Option aus:");
        Console.WriteLine("1. Spiel neu starten");
        Console.WriteLine("2. Spiel beenden");
        string eingabe = Console.ReadLine();
        int antwort;
        while (!int.TryParse(eingabe, out antwort) || (antwort != 1 && antwort != 2))
        {
            Console.WriteLine("Ungültige Eingabe. Bitte geben Sie 1 (neu starten) oder 2 (beenden) ein.");
            eingabe = Console.ReadLine();
        }
        if (antwort == 1)
        {
            new Spiel();
        }
    }
}

// Spiel.cs
using System;
using System.Collections.Generic;

// Klasse, die das gesamte Spiel steuert - Enthält die Spielregeln und Ablaufsteuerung
class Spiel
{
    private List<Spieler> spielerListe;
    private Spieler aktuellerSpieler;
    private GUI gui;
    private Becher becher;
    public static Spiel Instance;
    public Random Zufallsgenerator;

    // Konstruktor - Initialisiert die Instanz des Spiels, GUI, Spieler und Becher
    public Spiel()
    {
        Instance = this;
        Zufallsgenerator = new Random();
        gui = new GUI();
        spielerListe = gui.FrageSpielerEingabe();
        becher = new Becher();
        SetzeStartSpieler();
        Spielen();
    }

    // Setzt zufällig einen Startspieler
    private void SetzeStartSpieler()
    {
        aktuellerSpieler = spielerListe[Zufallsgenerator.Next(spielerListe.Count)];
    }

    // Gibt den Spieler rechts vom aktuellen Spieler zurück
    private Spieler SpielerRechtsVonAktuellemSpieler()
    {
        int index = (spielerListe.IndexOf(aktuellerSpieler) + 1) % spielerListe.Count;
        return spielerListe[index];
    }

    // Steuerung des Spielablaufs - Jeder Spieler führt seinen Zug durch, bis nur noch einer Chips hat
    public void Spielen()
    {
        int runde = 1;
        while (MehrAlsEinSpielerHatChips())
        {
            Console.WriteLine($"\n--- Runde {runde} ---");
            aktuellerSpieler.Spielzug(becher);
            if (aktuellerSpieler == spielerListe[spielerListe.Count - 1])
            {
                gui.PrintRangliste(spielerListe);
            }
            aktuellerSpieler = SpielerRechtsVonAktuellemSpieler();
            runde++;
        }
        gui.PrintGewinner(spielerListe);
        gui.SpielEndeOption();
    }

    // Prüft, ob mehr als ein Spieler noch Chips hat (Abbruchbedingung für das Spiel)
    private bool MehrAlsEinSpielerHatChips()
    {
        int count = 0;
        foreach (Spieler spieler in spielerListe)
        {
            if (spieler.GetChips() > 0)
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
        if (aktuellerSpieler.GetChips() > 0)
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
        if (aktuellerSpieler.GetChips() > 0)
        {
            aktuellerSpieler.GebeChipAb();
            rechterSpieler.ErhalteChip();
        }
    }
}

// Program.cs
using System;

// Einstiegspunkt des Programms - Startet das Spiel
class Program
{
    static void Main(string[] args)
    {
        new Spiel();
    }
}
