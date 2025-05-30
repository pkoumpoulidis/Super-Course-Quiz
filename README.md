# Super Course Quiz Demo
Unity Version: Unity 6 (6000.0.32f1)
Target Platform: Windows
## Σύνδεσμοι
- [google.drive](https://drive.google.com/file/d/1eLXaT-V3Z1O4Rl06iWZ56iJlLEyEr2EB/view?usp=sharing)
- [itch.io](https://tandalos.itch.io/super-course-quiz)

## Πληροφορίες για το παιχνίδι
Ένα παιχνίδι quiz, όπου ο παίκτης μπορεί να επιλέξει ένα, από τα 6 ζώα-χαρακτήρες, και να απαντήσει ερωτήσεις σχετικές με αυτό.
- Ο παίκτης έχει μόνο ένα ξεκλειδωμένο χαρακτήρα.
- Για να ξεκλειδώσει τον επόμενο χαρακτήρα, πρέπει πρώτα να απαντήσει σωστά 5 ερωτήσεις στο σύνολο με το προηγούμενο χαρακτήρα.
- Αφού διαλέξει χαρακτήρα και πατήσει εκκίνηση, ο παίκτης καλείται να βρει 10 κουτιά με ερωτήσεις που είναι σκορπισμένα στο χάρτη.
- Μόλις βρει και τα 10 κουτιά και απαντήσει στις ερωτήσεις τους, το παιχνίδι τελειώνει.
- Ο παίκτης άμα θέλει μπορεί να ξανα παίξει με τον ίδιο χαρακτήρα, αφού αποθηκεύεται διαφορετικό συνολικο και τωρινό σκορ.

## Συστήματα που έχουν υλοποιηθεί
Το παιχνίδι υλοποιεί τους παρακάτω μηχανισμούς
- Load & Save System, όπου η πρόοδος του παίκτη αποθηκεύεται τοπικά.
- Character Selection & Unlock System, όπου ο παίκτης μπορεί να επιλέξει ένα χαρακτήρα όπως και να ξεκλειδώσει κάποιον άλλο παίζοντας.
- Quiz System, όπου οι ερωτήσεις φορτώνονται δυναμικά. Μπορεί να εισαχθεί οποιοδήποτε json αρχείο, αρκεί προφανώς να έχει την ίδια δομή.
- Character Controller, θεώρησα πως είναι σωστό να φτιάξω το δικό μου character controller, ώστε να μπορώ να το προσαρμόσω για μη humanoid models.

 ## Βελτιώσεις που θα μπορούσαν να γίνουν
 - Καλύτερη δομή κώδικα. Μερικά τμήματα κώδικα θα μπορούσαν να είναι καλύτερα δομημένα και να μην έχουν τόσες πολλές εξαρτήσεις σε άλλα τμήματα.
 - Κρυπτογράφηση αρχείου τοπικής αποθήκευσης, ώστε ο παίκτης να μην μπορεί εύκολα να το τροποποιήσει.
 - Sound Effects και dust particles για κάθε χαρακτήρα. Δεν υλοποιήθηκε λόγο του πλήθους των χαρακτήρων.

## Assets που χρησιμοποίησα
- [User Interface](https://penzilla.itch.io/basic-gui-bundle)
- [Characters](https://assetstore.unity.com/packages/3d/characters/animals/animals-free-animated-low-poly-3d-models-260727)
- [Enviroment](https://k0rveen.itch.io/lowpoly-environment-pack)
- [Chest](https://assetstore.unity.com/packages/3d/props/low-poly-chest-animated-247127)
- [Confeti PS](https://assetstore.unity.com/packages/vfx/particles/hyper-casual-fx-200333)
