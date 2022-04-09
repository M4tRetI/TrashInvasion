# Nuova idea

## Gioco (interazione con l'utente)
### Dinamiche
- Gioco a due
  - Due player e nemici in comune
- Generazione endless dei nemici
- Griglia sullo sfondo per simboleggiare lo stato del vincitore. Più quadratini colorati della propria parte significa un punteggio maggiore.
  Ogni quadratino vale tipo 800 punti. Vince chi li conquista tutti.
- Se i nemici arrivano troppo vicino al player, egli perde.

### Meccaniche
- Power-up per intralciare l'avversario o favorire se stessi      | Arrivano dall'alto come i cubi di Mario Kart
  - Accecamento termporaneo
  - Moltiplicare x2
  - Rallentamento del rateo di fuoco del nemico
  - Scudo anti nemici/player
- Se un player invade la parte altrui, prende un tot di danno in un tempo t;
- Un player può stunnare l'altro avvicinandosi molto e premendo un tasto . Se però lo colpisce, perde una marea di punti (per bilanciare).
  Con stunnare si intende impossibilità di sparare. Per difendersi l'altro player, in un lasso molto limitato di tempo deve premere un bottone, e così facendo sfugge
  dalle grinfie del bastardo [perry]
  
**Tempo stimato di lavoro**: 16 gg
<br><br>
## Calcolo punteggio per Leaderboard
Il punteggio viene calcolato solo per il vincitore della partita
![Equazione](https://github.com/M4tRetI/TrashInvasion/blob/coop/Formula_calcolo_score.png)
Il bouns (b) vale 250

Legenda:
 - dk : Delta tra rapporto kill/sparo dei due giocatori (perdente - vincitore)
 - t  : Durata della partita in secondi
 - p  : Numero di perry eseguiti
 - rt : Rapporto tra tempo e numero di colpi ricevuti dai nemici (solo del vincitore)
 - b  : Bonus in caso di come back, tipo per meno del 30% della partità è stato perdente

---------------

## Quesiti / Problemi da risolvere
 *Nessuno*
