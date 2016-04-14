CqrsDemo
========

c# Beispielprojekt mit einer minimalistischen CQRS-Struktur


Ziel dieses Projekts ist eine CQRS Beispielanwendung in c# mit einem besonderen Ansatz. Um die wesentlichen Elemente von CQRS deutlich herauszustellen, verzichte ich auf viele - oft auch unreflektiert angewandte - Optimierungen und Abstraktionen.



Stand

Aktuell (2016-04-14) ist das Modell mit Event Sourcing gut testabgedeckt und vollständig implementiert. Ein zweites alternatives Modell mit SQL Zugriff ist 
nur lückenhaft implementiert.

Zwischen beiden Modellen können Sie für die Tests in "Spezifikation.cs" und für das echte System in CqrsGmbH_Web.cs umstellen. Für die SQL Variante sind Connection Strings notwendig! Die Tests erstellen Datenbank "cqrsdemo_test" und Tabellen automatisch. Das Echtsystem benötigt dann eine Db mit identischer Struktur.




Vorbemerkungen

Vorweg zwei Hinweise: Die Motivation dieses Codes ist es, die Konzepte einer CQRS + Event Sourcing Anwendung zu zeigen, möglichst freigelegt von allem Ballast. Das Ergebnis ist eine klar strukturierte Anwendung mit sehr starker Entkopplung. 

CQRS und Event Sourcing sind voneinander unabhängige, weitgehend orthogonale Konzepte. Sie können und werden erfolgreich auch einzeln angewendet. Nach meiner Meinung ist Event sourcing jedoch der beste Persistenzmechanismum für viele Komponenten mit CQRS Architektur, da beide Konzepte auf der Sprache der Fachdomäne aufbauen. Um eine abgerundete Anwendung zeigen zu können, verbinde ich beide in dieser Beispielanwendung.

Das Projekt ist zunächst weitgehend frei von externen Abhängigkeiten. Insbesondere aber verwendet und enthält es bewusst kein "Framework" für CQRS. Statt dessen sind alle Bestandteile des Systems so einfach wie möglich direkt implementiert. Dabei habe ich allerdings Wert auf das Design der Abhängigkeiten gelegt, so dass zumindest grundsätzlich ein Ausbau hin zu einer echten Anwendung, auch als verteiltes System, möglich ist. Die soll jedoch nicht darüber hinwegtäuschen, dass dieses Projekt in keiner Weise produktionstauglich ist. Der Code ist vielmehr ein Kommunikationsmedium.

Hinweis zum Quellcode: Das Projekt verwendet mehrere NuGet Pakete. Falls Sie den Quellcode kompilieren wollen, sollten Sie diese mithilfe der VS-Funktion "Enable NuGet package restore" vorbereiten (Kontextmenü der Solution im Solution Explorer).


Einleitung

CQRS - das Command and Query Responsibility Segregation Pattern - findet dort Anwendung, wo ein einzelnes Domänen(-objekt-)modell für Geschäftslogik und Abfragen einen schlechten Kompromiss darstellen würde. Dies ist insbesondere dann der Fall, wenn die Geschäftslogik der Fachdomäne komplex ist, Änderungen des Zustands des Systems konkurrierend von mehreren Benutzern bewirkt werden, und generell Anforderungen eher verhaltensorientiert als datenorientiert formuliert werden, da die Bedienung von Abfragen notwendigerweise datenorientiert ist. Um eine sinnvolle CQRS Beispielanwendung zu erstellen, braucht es also eine minimale Fachdomäne, die dies erfüllt.

In der Fachdomäne des Beispielprojekts geht es um eine - recht triviale - Warenwirtschaft. Die Benutzer des Systems sollen in Bestellannahme und Warendisposition.

Die wesentlichen Begriffe der Fachdomäne sind "Kunde", "Auftrag", "Produkt", "Nachbestellung", "Wareneingang", "Lagerbestand" und "Verfügbarkeit". Dabei ist die Verfügbarkeit der Lagerbestand abzüglich bereits erfasster Aufträge zuzüglich im Zulauf befindlicher Nachbestellungen. Liefertermine werden nicht berücksichtigt.

Dabei treffe ich eine Reihe etwas unrealistischer Annahmen: ein Kundenname besteht aus einer Zeichenkette, ebenso seine Adresse. Ein Auftrag enthält nur eine Position mit genau einem Produkt, allerdings mit einer variablen Menge. Preisangaben werden weder im Einkauf noch im Verkauf berücksichtigt.


