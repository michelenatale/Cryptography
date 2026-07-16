# Academic Proposal – Wissenschaftliche Weiterentwicklung der C ABI Bridge AOT Technologie

**Status**: ✅   
**Version**: 0.2.1   
**Last Updated**: 2026.07.16

## 1. Einleitung

Die moderne Kryptographie steht vor der Herausforderung, gleichzeitig sicher, interoperabel, reproduzierbar und langfristig auditierbar zu sein. In sicherheitskritischen Bereichen wie staatlichen Behörden, Banken, militärischen Systemen und der öffentlichen digitalen Infrastruktur steigt der Bedarf nach kryptographischen Modulen, die über klare Schnittstellen verfügen, deterministisch arbeiten und über Sprachgrenzen hinweg zuverlässig eingesetzt werden können.

Die hier vorgestellte Technologie – eine reproduzierbare, minimalistische und sprachübergreifende C‑ABI‑Bridge für kryptographische Funktionen – bildet eine innovative Grundlage für die Entwicklung solcher Module. Sie verbindet technische Pragmatik mit wissenschaftlicher Tiefe und eröffnet neue Perspektiven für Forschung, Normierung und praktische Implementierung. Die Architektur ist bewusst einfach gehalten, um Interoperabilität und Sicherheit zu maximieren, und gleichzeitig offen genug, um als Basis für weiterführende wissenschaftliche Arbeiten zu dienen.

Dieses Dokument stellt einen wissenschaftlichen Vorschlag („Academic Proposal“) dar, der die Technologie in einen akademischen Kontext einordnet und mögliche Forschungsrichtungen aufzeigt. Die Idee ist nicht als fertiges Produkt zu verstehen, sondern als Ausgangspunkt für Master‑ und Doktorarbeiten, die sich mit Architekturdesign, Sicherheitsanalyse, formaler Verifikation, normativen Anforderungen und der praktischen Umsetzung sicherer kryptographischer Module befassen. Gleichzeitig bleibt die Technologie für Studierende aller Niveaus zugänglich: Bachelor-, FH- und HF‑Arbeiten können sich auf Implementierung und Grundlagen konzentrieren, während Master‑ und PhD‑Arbeiten die wissenschaftliche Tiefe und normative Komplexität ausschöpfen.

Die Offenlegung des Projekts ermöglicht es der Allgemeinheit, der Forschungsgemeinschaft und der Industrie, die Idee weiterzuentwickeln, kritisch zu hinterfragen und für reale Anwendungen zu adaptieren. Innovation entsteht oft aus einfachen, aber klar strukturierten Konzepten – und dieses Projekt soll genau diese Art von Weiterentwicklung fördern.


## 2. Motivation

Die Motivation hinter der wissenschaftlichen Weiterentwicklung der C‑ABI‑Bridge‑AOT‑Technologie ergibt sich aus mehreren aktuellen Herausforderungen, die moderne Kryptographie‑Systeme betreffen. In sicherheitskritischen Bereichen – darunter staatliche Behörden, Banken, militärische Einrichtungen und die digitale Infrastruktur der Allgemeinheit – besteht ein wachsender Bedarf an kryptographischen Modulen, die nicht nur sicher, sondern auch reproduzierbar, interoperabel und langfristig auditierbar sind. Die zunehmende Komplexität moderner Software‑Ökosysteme führt dazu, dass kryptographische Funktionen oft über viele Schichten verteilt sind, was die Analyse, Zertifizierung und Überprüfung erschwert.

Die vorgestellte C‑ABI‑Bridge‑AOT‑Architektur adressiert diese Herausforderungen durch einen bewusst minimalistischen, klar definierten und reproduzierbaren Ansatz. Die Technologie trennt kryptographische Kernfunktionen von sprachspezifischen Laufzeitumgebungen und schafft eine stabile, universelle Schnittstelle, die unabhängig von Compiler, Plattform oder Programmiersprache funktioniert. Dadurch entsteht ein Modul, das leichter zu auditieren, zu testen und zu zertifizieren ist – ein entscheidender Vorteil für Organisationen, die strenge regulatorische oder sicherheitsrelevante Anforderungen erfüllen müssen.

Ein weiterer Motivationsfaktor liegt in der zunehmenden Bedeutung reproduzierbarer Builds und deterministischer Ausführung. Sicherheitsvorfälle der letzten Jahre haben gezeigt, dass komplexe Build‑Pipelines und dynamische Laufzeitverhalten erhebliche Risiken darstellen können. Eine klar definierte ABI‑Boundary reduziert diese Risiken, indem sie die kryptographische Logik in einen kontrollierten, stabilen und überprüfbaren Bereich verlagert. Dies ist insbesondere für FIPS‑140‑3‑konforme Module relevant, da normative Standards reproduzierbare, deterministische und auditierbare Implementierungen verlangen.

Die wissenschaftliche Motivation ergibt sich zudem aus der Interdisziplinarität des Themas. Die Technologie berührt Bereiche wie Kryptographie, Compiler‑Theorie, Software‑Architektur, Interoperabilität, Sicherheitsanalyse und normative Standardisierung. Diese Vielfalt macht das Projekt zu einem idealen Ausgangspunkt für Master‑ und Doktorarbeiten, die sowohl technische als auch theoretische Fragestellungen untersuchen können. Gleichzeitig bleibt die Technologie zugänglich: Studierende auf Bachelor‑, FH- oder HF‑Niveau können praktische Implementierungen und Grundlagen erforschen, während Master‑ und PhD‑Studierende die wissenschaftliche Tiefe und normative Komplexität ausschöpfen.

Schließlich besteht die Motivation darin, einen offenen Beitrag zur Forschungsgemeinschaft und zur Allgemeinheit zu leisten. Durch die Veröffentlichung der Technologie und der wissenschaftlichen Fragestellungen entsteht ein Raum für Zusammenarbeit, Kritik, Weiterentwicklung und Innovation. Die C‑ABI‑Bridge‑AOT‑Architektur soll nicht als fertiges Produkt verstanden werden, sondern als Ausgangspunkt für eine wissenschaftliche Diskussion, die langfristig zu sichereren, interoperableren und zertifizierbaren Kryptographie‑Modulen führen kann.


## 3. Wissenschaftliche Themenfelder

Die C‑ABI‑Bridge‑AOT‑Technologie berührt eine Vielzahl wissenschaftlicher Disziplinen, die sowohl theoretische als auch praktische Relevanz besitzen. Diese Themenfelder bilden die Grundlage für weiterführende Forschung und ermöglichen eine breite wissenschaftliche Auseinandersetzung, die von technischen Implementierungen bis hin zu formalen Sicherheitsbeweisen reicht. Die folgenden Bereiche zeigen, wie vielfältig und interdisziplinär die wissenschaftliche Weiterentwicklung dieser Technologie sein kann.

### 3.1 Kryptographie und Sicherheitsarchitektur

Die Technologie eröffnet neue Perspektiven für die Gestaltung sicherer kryptographischer Module. Relevante Fragestellungen umfassen:
- Design minimaler, auditierbarer Kryptographie‑APIs
- sichere Trennung zwischen kryptographischem Kern und Laufzeitumgebung
- deterministische Ausführung als Sicherheitsprinzip
- modulare Sicherheitsarchitektur für kritische Systeme

Diese Themen sind zentral für Behörden, Banken und militärische Anwendungen, die strenge Sicherheitsanforderungen erfüllen müssen.

### 3.2 ABI‑Design und Interoperabilität

Die C‑ABI‑Bridge bietet eine universelle Schnittstelle, die unabhängig von Programmiersprache und Plattform funktioniert. Wissenschaftlich relevant sind:
- formale Modelle für ABI‑Layouts
- reproduzierbare und stabile Schnittstellen
- Interoperabilität zwischen C#, Rust, Go, Python, Java und C++
- Analyse von Calling Conventions und Struct‑Packing
- Minimierung der Angriffsfläche durch deterministische ABI‑Boundaries

Diese Fragestellungen verbinden Software‑Engineering mit Compiler‑Theorie und Sicherheitsanalyse.

### 3.3 Compiler‑ und Runtime‑Theorie

Die Technologie ist ein ideales Forschungsobjekt für die Untersuchung von Compiler‑Verhalten und AOT‑Kryptographie:
- deterministische Build‑Pipelines
- Compiler‑Unabhängigkeit (Clang, GCC, MSVC)
- Analyse von Optimierungen und deren Einfluss auf Sicherheit
- AOT‑kompatible Kryptographie für sicherheitskritische Systeme
- reproduzierbare Builds als Grundlage für Zertifizierungen

Diese Themen sind besonders relevant für militärische und staatliche Systeme, die reproduzierbare Software verlangen.

### 3.4 Normative Standards und Zertifizierbarkeit

Die Architektur ermöglicht eine wissenschaftliche Auseinandersetzung mit normativen Anforderungen wie:
- FIPS 140‑3
- NIST SP 800‑90 (DRBG)
- NIST SP 800‑56 (Key Establishment)
- NIST SP 800‑131A (Crypto Transitioning)
- BSI‑Grundschutz und internationale Sicherheitsnormen

Wissenschaftlich interessant ist die Frage, wie eine minimalistische ABI‑Bridge als Grundlage für zertifizierbare Module dienen kann.

### 3.5 Formale Verifikation und mathematische Modelle

Für PhD‑Arbeiten besonders relevant sind formale Methoden:
- mathematische Modellierung der ABI‑Boundary
- formale Sicherheitsbeweise (Memory‑Safety, deterministische Ausführung)
- Modellierung von Fehlerzuständen und Ausnahmefällen
- formale Verifikation von Interop‑Boundaries
- Einsatz von Coq, Dafny, Lean, TLA+ oder Z3‑Solver

Diese Themen ermöglichen wissenschaftliche Publikationen und theoretische Beiträge zur Kryptographie‑Forschung.

### 3.6 Sicherheitsanalyse und Threat‑Modeling

Die Technologie erlaubt eine tiefgehende Analyse sicherheitskritischer Aspekte:
- Angriffsvektoren durch Interop‑Schnittstellen
- Memory‑Ownership‑Risiken
- sichere Modul‑Boundaries
- Minimierung dynamischer Code‑Pfadrisiken
- Threat‑Modeling für Cross‑Language‑Kryptographie

Diese Fragestellungen sind für Banken, Behörden und Militär besonders relevant.

### 3.7 Reproduzierbare Systeme und Supply‑Chain‑Security

Die Architektur unterstützt Forschung zu:
- reproduzierbaren Builds
- Supply‑Chain‑Sicherheit
- deterministischen Modulen
- auditierbaren Build‑Pipelines
- minimalistischem Design als Schutz vor Manipulation

Dies ist ein hochaktuelles Thema in der Sicherheitsforschung.

### 3.8 Open‑Source‑Forschung und gesellschaftliche Relevanz

Die Offenlegung der Technologie ermöglicht:
- kollaborative Weiterentwicklung
- wissenschaftliche Transparenz
- gesellschaftliche Diskussion über sichere Kryptographie
- Einsatz in öffentlichen digitalen Diensten

Damit wird die Technologie auch für die Allgemeinheit relevant.


## 4. Master‑Themenvorschlag

### Titel der Masterarbeit

**Evaluierung und wissenschaftliche Analyse einer reproduzierbaren, sprachübergreifenden C‑ABI‑Bridge als Grundlage sicherer Kryptographiemodule**

### Zielsetzung

Die Masterarbeit soll die vorgestellte C‑ABI‑Bridge‑AOT‑Technologie wissenschaftlich untersuchen, bewerten und dokumentieren. Der Fokus liegt auf Architekturdesign, Sicherheitsanalyse, Interoperabilität und normativen Anforderungen. Ziel ist es, die Eignung der Technologie als Grundlage für sichere, auditierbare und potenziell zertifizierbare Kryptographiemodule zu evaluieren.

### Konkrete Forschungsfragen

- Welche Designprinzipien ermöglichen eine reproduzierbare und stabile ABI‑Boundary?
- Wie beeinflusst ein minimalistisches API‑Design die Sicherheit und Auditierbarkeit?
- Welche Interoperabilitätsvorteile ergeben sich durch die Trennung von Kryptographie‑Modul und Laufzeitumgebung?
- Welche Anforderungen aus FIPS 140‑3 sind architekturbedingt bereits erfüllt?
- Welche Erweiterungen wären notwendig, um die Technologie FIPS‑kompatibel zu machen?
- Wie verhält sich die Bridge in verschiedenen Sprachen (C#, Rust, Go, Python)?
- Welche Risiken entstehen durch Interop‑Boundaries und wie können sie minimiert werden?

### Methodik

- Analyse der Architektur und ABI‑Layouts
- Sicherheitsanalyse (Threat‑Modeling, Memory‑Ownership, API‑Determinismus)
- Interop‑Tests in mehreren Sprachen
- Vergleich mit bestehenden Kryptographie‑Modulen (CNG, BoringCrypto, AWS‑LC)
- Normative Bewertung anhand FIPS‑140‑3 und NIST‑Standards
- Dokumentation und wissenschaftliche Argumentation

### Erwartete Ergebnisse

- wissenschaftlich fundierte Architektur‑Analyse
- normative Bewertung der Technologie
- Sicherheitsanalyse und Interop‑Evaluation
- Roadmap für FIPS‑Kompatibilität
- Empfehlungen für Behörden, Banken, Militär und öffentliche Infrastruktur


## 5. PhD‑Themenvorschlag

### Titel der Dissertation

**Formale Modellierung, Verifikation und normative Analyse einer reproduzierbaren C‑ABI‑Kryptographie‑Bridge für sicherheitskritische Systeme**

### Zielsetzung

Das Doktoratsprojekt soll die C‑ABI‑Bridge‑AOT‑Technologie formal modellieren, mathematisch analysieren und normativ bewerten. Ziel ist es, theoretische Grundlagen für sichere, reproduzierbare und zertifizierbare Kryptographiemodule zu schaffen, die über Sprachgrenzen hinweg funktionieren und in kritischen Bereichen eingesetzt werden können.

### 5.1 Konkrete wissenschaftliche Fragestellungen

### Formale Modelle

- Wie lässt sich eine ABI‑Boundary mathematisch modellieren?
- Welche formalen Eigenschaften müssen erfüllt sein, damit ein Modul deterministisch und reproduzierbar bleibt?
- Wie kann man Interop‑Boundaries formal verifizieren?

### Sicherheitsbeweise

- Wie lassen sich Memory‑Safety‑Eigenschaften formal beweisen?
- Welche Modelle beschreiben deterministische Kryptographie‑APIs?
- Wie können Fehlerzustände formalisiert werden?

### Normative Analyse

- Welche FIPS‑140‑3‑Anforderungen lassen sich formal beweisen?
- Wie kann die Module‑Boundary normativ und mathematisch beschrieben werden?
- Welche formalen Modelle eignen sich für Self‑Tests und DRBG‑Mechanismen?

### Compiler‑Theorie

- Wie beeinflussen Compiler‑Optimierungen die formale Sicherheit eines Moduls?
- Welche theoretischen Modelle beschreiben reproduzierbare Builds?

### Interoperabilität

- Wie lässt sich Cross‑Language‑Interop formal modellieren?
- Welche mathematischen Eigenschaften müssen erfüllt sein, damit Interop‑Schnittstellen sicher bleiben?

### Methodik

- Einsatz formaler Methoden (Coq, Dafny, Lean, TLA+, Z3‑Solver)
- mathematische Modellierung der ABI‑Boundary
- normative Analyse anhand FIPS‑140‑3 und NIST SP 800‑Serien
- theoretische Untersuchung von Compiler‑Verhalten
- wissenschaftliche Publikation der Ergebnisse

### Erwartete Ergebnisse

- vollständige formale Modelle der ABI‑Bridge
- mathematische Sicherheitsbeweise
- normative FIPS‑Analyse
- theoretische Erweiterungen der Kryptographie‑Architektur
- wissenschaftliche Publikationen
- Dissertation mit langfristiger Relevanz für kritische Infrastruktur


## 6. Abgrenzung Bachelor / FH / HF / Master / PhD

Die C‑ABI‑Bridge‑AOT‑Technologie eröffnet vielfältige Möglichkeiten für wissenschaftliche und technische Arbeiten auf unterschiedlichen Ausbildungsniveaus. Eine klare Abgrenzung der Niveaus ist notwendig, um die wissenschaftliche Tiefe korrekt einzuordnen, ohne Studierende oder Fachpersonen zu demotivieren. Die folgenden Definitionen beschreiben nicht den Wert einer Person, sondern die **Tiefe der Analyse**, die für die jeweiligen akademischen Stufen üblich ist.

### 6.1 Bachelor / FH / HF / Techniker (praxisorientiert)

Arbeiten auf diesem Niveau konzentrieren sich auf:
- praktische Implementierung
- grundlegende Architekturverständnis
- einfache Interoperabilitätsbeispiele
- funktionale Demonstrationen
- technische Dokumentation
- erste Sicherheitsüberlegungen

Diese Arbeiten sind wertvoll, da sie die Technologie zugänglich machen und praktische Grundlagen schaffen, auf denen weiterführende Forschung aufbauen kann.

### 6.2 Master (wissenschaftlich‑technisch)

Masterarbeiten vertiefen die wissenschaftliche Analyse und verbinden Theorie mit Praxis:
- detaillierte Architekturstudien
- normative Bewertung (z. B. FIPS‑140‑3)
- Interop‑Analyse über mehrere Sprachen
- Sicherheitsanalyse und Threat‑Modeling
- wissenschaftliche Argumentation
- Evaluierung der Reproduzierbarkeit
- Roadmaps für Zertifizierbarkeit

Dieses Niveau eignet sich ideal für die wissenschaftliche Weiterentwicklung der Technologie.

### 6.3 PhD / Doktorat (wissenschaftlich‑theoretisch)

Doktorarbeiten befassen sich mit formalen, mathematischen und theoretischen Fragestellungen:
- formale Modellierung der ABI‑Boundary
- mathematische Sicherheitsbeweise
- normative Proofs für FIPS‑140‑3
- Compiler‑Theorie und deterministische Build‑Modelle
- formale Verifikation (Coq, Dafny, Lean, TLA+, Z3)
- wissenschaftliche Publikationen
- theoretische Erweiterungen der Kryptographie‑Architektur

Dieses Niveau ist für tiefgehende Forschung geeignet, die neue wissenschaftliche Erkenntnisse schafft.

### 6.4 Motivation statt Ausschluss

Alle Niveaus tragen wertvolle Beiträge bei:
- **HF / FH / Bachelor** schaffen praktische Grundlagen
- **Master** liefern wissenschaftliche Analysen
- **PhD** erweitern die Theorie

Damit wird niemand ausgeschlossen — im Gegenteil: Die Technologie lädt alle ein, auf ihrem Niveau mitzuwirken.


## 7. Bedeutung für Behörden, Banken, Militär, Allgemeinheit

Die C‑ABI‑Bridge‑AOT‑Technologie besitzt besondere Relevanz für sicherheitskritische Bereiche, in denen kryptographische Module höchsten Anforderungen genügen müssen. Die Architektur adressiert zentrale Bedürfnisse dieser Akteure und bietet eine wissenschaftlich fundierte Grundlage für zukünftige Entwicklungen.

### 7.1 Behörden und staatliche Institutionen

Behörden benötigen:
- auditierbare Kryptographie
- reproduzierbare Builds
- klare Modul‑Boundaries
- deterministische Ausführung
- langfristige Zertifizierbarkeit

Die minimalistische ABI‑Bridge erleichtert Audits und reduziert die Komplexität staatlicher Sicherheitsprüfungen.

### 7.2 Banken und Finanzinfrastruktur

Finanzsysteme verlangen:
- regulatorische Konformität
- stabile Schnittstellen
- sichere Interoperabilität
- kontrollierbare Build‑Pipelines
- minimierte Angriffsflächen

Die Architektur unterstützt diese Anforderungen durch reproduzierbare Module und klare API‑Strukturen.

### 7.3 Militär und sicherheitskritische Einsatzbereiche

Militärische Systeme benötigen:
- AOT‑kompatible Kryptographie
- deterministische Module ohne dynamische Code‑Pfadrisiken
- reproduzierbare Builds für Einsatzszenarien
- minimalistische Angriffsflächen
- formale Sicherheitsmodelle

Die Technologie bietet eine Basis für Module, die unter extremen Bedingungen zuverlässig funktionieren müssen.

### 7.4 Öffentliche Infrastruktur und Allgemeinheit

Für die Allgemeinheit ist relevant:
- transparente Open‑Source‑Kryptographie
- interoperable Sicherheitsmodule
- sichere digitale Dienste
- nachvollziehbare Architektur
- gesellschaftliche Diskussion über Kryptographie

Die Offenlegung der Technologie ermöglicht es, Vertrauen in digitale Systeme zu stärken und Forschung zugänglich zu machen.

### 7.5 Zusammenführung der Interessen

Die Technologie verbindet die Anforderungen aller Akteure:
- Behörden → Auditierbarkeit
- Banken → Stabilität und Compliance
- Militär → Determinismus und Reproduzierbarkeit
- Allgemeinheit → Transparenz und Sicherheit

Damit entsteht ein wissenschaftlich und gesellschaftlich relevantes Projekt, das weit über die reine Implementierung hinausgeht.


## 8. Einladung zur Mitarbeit

Die C‑ABI‑Bridge‑AOT‑Technologie ist bewusst als offenes, transparentes und kollaboratives Projekt gestaltet. Sie soll nicht nur als technische Grundlage dienen, sondern als wissenschaftliche Plattform, die Studierende, Forschende, Entwicklerinnen und Entwickler sowie Institutionen dazu einlädt, eigene Ideen einzubringen, bestehende Konzepte zu hinterfragen und neue Ansätze zu entwickeln. Die Offenheit des Projekts ermöglicht es, unterschiedliche Perspektiven zusammenzuführen und die Technologie gemeinsam weiterzuentwickeln.

Diese Einladung richtet sich ausdrücklich an Personen aller Ausbildungsstufen.
Studierende auf Bachelor‑, FH- oder HF‑Niveau können praktische Implementierungen, Tests und Dokumentationen beitragen.
Master‑Studierende können wissenschaftliche Analysen, normative Bewertungen und Architekturstudien durchführen.
PhD‑Studierende und Forschende können formale Modelle, mathematische Verifikationen und theoretische Erweiterungen entwickeln.

Auch Organisationen wie Behörden, Banken, militärische Einrichtungen und Unternehmen der kritischen Infrastruktur sind eingeladen, ihre Anforderungen, Erfahrungen und Perspektiven einzubringen. Die Zusammenarbeit zwischen akademischer Forschung und praktischer Anwendung ist entscheidend, um robuste, auditierbare und langfristig vertrauenswürdige Kryptographie‑Module zu schaffen.

Das Projekt versteht sich als offene Forschungsplattform, die Innovation fördert, wissenschaftlichen Austausch ermöglicht und die Grundlage für zukünftige sicherheitskritische Systeme bildet. Jede Mitarbeit – unabhängig vom Niveau – ist willkommen und trägt dazu bei, die Technologie weiter zu stärken und ihre Einsatzmöglichkeiten zu erweitern.


## 9. Schlusswort / Vision

Die C‑ABI‑Bridge‑AOT‑Technologie ist mehr als eine technische Implementierung. Sie ist ein Konzept, eine Idee und eine Vision für die Zukunft sicherer, reproduzierbarer und interoperabler Kryptographie‑Module. In einer Zeit, in der digitale Sicherheit immer wichtiger wird und die Komplexität moderner Software‑Systeme stetig zunimmt, bietet diese Architektur einen klaren, strukturierten und wissenschaftlich fundierten Ansatz, um kryptographische Funktionen zuverlässig und langfristig auditierbar bereitzustellen.

Die Vision dieses Projekts besteht darin, eine Grundlage zu schaffen, die sowohl technisch als auch wissenschaftlich tragfähig ist. Eine Grundlage, die es ermöglicht, kryptographische Module über Sprachgrenzen hinweg sicher zu nutzen, reproduzierbar zu bauen und normativ zu bewerten. Eine Grundlage, die Behörden, Banken, militärische Einrichtungen und die Allgemeinheit gleichermaßen unterstützt. Und eine Grundlage, die offen genug ist, um zukünftige Entwicklungen, wissenschaftliche Erkenntnisse und technologische Innovationen aufzunehmen.

Dieses Dokument soll dazu beitragen, die Idee in einen akademischen Kontext zu stellen und den Weg für weiterführende Forschung zu ebnen. Die Technologie ist bewusst nicht als fertiges Produkt konzipiert, sondern als Ausgangspunkt für wissenschaftliche Arbeiten, Diskussionen und Weiterentwicklungen. Die Vision ist, dass aus dieser Basis langfristig robuste, zertifizierbare und vertrauenswürdige Kryptographie‑Module entstehen, die einen Beitrag zur digitalen Sicherheit unserer Gesellschaft leisten.

Die Zukunft sicherer Kryptographie beginnt oft mit einfachen, klaren und gut strukturierten Ideen. Dieses Projekt ist eine solche Idee – und es lädt alle ein, sie weiterzuführen.
