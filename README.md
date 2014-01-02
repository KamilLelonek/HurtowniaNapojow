HurtowniaNapojow
================

Projekt na bazy danych.

1. Skopiuj plik `HurtowniaNapojow.accdb` z folderu `\Projekt` na Dropboxie do folderu `\HurtowniaNapojow\Database`
2. W VS wybierz `View` > `Server Explorer (Ctrl + W, L)` a następnie polecenie `Connect to Database` (Ikona wtyczki z +)
3. Jako `Data source` wybierz `Microsoft Access Database File (OLE DB)` a w `Database file name` wskaż bazę w folderze `\HurtowniaNapojow\Database` i wybierz `OK`
4. Następnie kliknij prawym przyciskiem na folder `Database` w `Solution Explorer` i wybierz `Add` > `New item`, a następnie wyszukaj DataSet. Jako nazwę nowego elementu podaj `HurtowniaNapojowDataSet`
5. Do oknie designera, które zostanie otwarte wybraniu `OK` przeciągnij wszystkie tabele z widoku `Server Explorer` > `Data Connections` > `HurtowniaNapojow.accdb` > `Tables`. Rozmieść tabele w czytelny sposób w designerze, zapisz go i zamknij.
6. Następnie w `Solution Explorer` wybierz `Database` > `HurtowniaNapojowDataSet.xsd` > `HurtowniaNapojowDataSet.Designer.cs` zamień wszystkie wystąpienia `ConnectionString1` na `ConnectionString`. Zapisz plik i zamknij.
7. Tak skonfigurowana baza danych powinna umożliwić zbudowanie, skompilowanie i uruchomienie projektu.
