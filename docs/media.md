# Media Management

Alle Medien werden zentral auf dem Controller gespeichert und auf die Nodes synchronisiert.

## Synchronisation
Nach der Registrierung einer Node am System ruft diese eigenständig eine Liste aller Medien über eine Schnittstelle vom Controller ab und lädt diese daraufhin herunter und speichert sie lokal.
Zusätzlich wird über einen gRPC Kanal auf Änderungen hinsichtlich der Medien gewartet.
Falls eine neue Datei am Controller hochgeladen wird informiert dieser alle Nodes, sodass diese die neue Datei herunterladen können. (Edit/Delete analog)
