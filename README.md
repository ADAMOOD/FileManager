# 📁 FILEMANAGER3000

**FILEMANAGER3000** je jednoduchý konzolový správce souborů napsaný v C#. Program umožňuje statistiku přípon souborů, vytváření záloh a prohlížení záloh. Projekt využívá knihovny **BetterConsoleTables** pro přehledné tabulky a standardní .NET knihovny pro práci se soubory.

---

## ⚙️ Funkcionality

1. **Statistika přípon souborů**
   - Vyberte adresář a (volitelně) seznam přípon oddělených středníkem (`;`).  
   - Program vypíše počet souborů a velikost pro každou příponu.  
   - Výstup je zobrazen v přehledné tabulce.  

2. **Vytvoření zálohy**
   - Vyberte adresář, ze kterého se mají zálohovat soubory.  
   - Vytvoří se ZIP soubor s aktuálními soubory v časově označené podsložce.  
   - Program vypíše délku trvání operace.  

3. **Prohlížení záloh**
   - Program vyhledá všechny soubory `backup.zip` v zadaném adresáři a podadresářích.  
   - Umožňuje vybrat zálohu podle čísla a otevře ji ve **Windows Exploreru**.  

---

## 🖥️ Použití

Po spuštění programu se zobrazí menu:

```
A -> File extension statistics
B -> Create backup file
C -> List backup files
```

- Pohyb a volba: **klávesa A/B/C**  
- Chybové vstupy jsou zvýrazněny červeně a po 0,75s se vrátíte zpět do menu.

### Statistika přípon
- Prompt: `File extension` (např. `.txt;.cs`)  
- Pokud necháte prázdné, zobrazí všechny soubory v adresáři.  

### Záloha
- Prompt: `Directory` (výchozí `C:\tmp`)  
- Vytvoří podsložku s časovým razítkem a v ní soubor `backup.zip`.  

### Prohlížení záloh
- Prompt: `Select folder by number`  
- Otevře vybranou zálohu v Průzkumníku Windows.

---

## 📂 Mapa projektu

```
FileManager/
│
├── Program.cs           # Hlavní soubor s menu a funkcionalitou
├── ExtensionStatistic.cs # Reprezentace statistik přípon
└── Items/
    └── Helpers.cs       # Pomocné metody (např. vymazání oblasti konzole)
```

---

## 🔧 Poznámky

- Používá **BetterConsoleTables** pro výpis statistik.  
- Pro práci se ZIP soubory využívá **System.IO.Compression**.  
- Chyby jsou zobrazeny červeně, výstupy informativní žlutě.  
- Program byl testován na Windows.  

---

