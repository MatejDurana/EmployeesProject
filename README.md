
# EmployeesProject

Pre spustenie projektu je potrebné spustiť vytvorené migrácie pomocou príkazu `dotnet ef database update`. Údaje ako `ConnectionString` a `BaseUri` sa nachádzajú v súbore `Server/appsettings.json`.

Vytvorená databáza bude mať názov `EmployeeDb`.

Log súbory sa nachádzajú priečinku `Server/logs`. Súbory sú logované podľa dňa.

V projekte je použitý EntityFramework s CodeFirst prístupom. Services sú rozdelené na klientské a serverové pre každú entitu zvlášť. Dátová vrstva sa nachádza v rovnakej vrstve ako Server. Services využívajú triedu `ServiceResponse<T>`, ktorá obsahuje základné informácie o tom ako service odpovedá. Tie sú potom využívané napríklad aj pri výpise chýb v UI, kde sa vypisuje generická hláška o errore alebo ak je hodnota `ServiceResponse.Hidden` nastavená na `false`, vypíše sa chyba poslaná priamo zo servera, ktorá sa nachádza v `ServiceResponse.Message`.


