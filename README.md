# 🎓 Aplikace pro správu studentů a známek

Tato ASP.NET Core MVC aplikace slouží jako školní informační systém pro správu studentů, známek, předmětů a uživatelů. Projekt vznikl v rámci kurzu pořádaného VŠB-TUO.

## ✨ Hlavní funkce

- Správa uživatelů a jejich rolí (`student`, `učitel`, `admin`)
- Přehledné zobrazení studentů, předmětů a známek
- Možnost importu studentů ze souboru `.xml`
- Přístup k datům je řízen na základě rolí (role-based access)

## 👤 Role a oprávnění

| Role       | Přístup k datům               | Možnost úprav / mazání      |
|------------|-------------------------------|------------------------------|
| **Student** | Pouze vlastní známky           | ❌                           |
| **Učitel**  | Všichni studenti a jejich známky | ✅ může upravovat známky     |
| **Admin**   | Kompletní přístup ke všem datům | ✅ správa všeho v systému     |

## 🧰 Použité technologie

- **ASP.NET MVC** (.NET Framework / .NET Core)
- **Entity Framework** – databázová vrstva (ORM)
- **MS SQL Server** – relační databáze
- **Bootstrap** – responzivní design
- **monsterasp.net** – webhosting / nasazení
