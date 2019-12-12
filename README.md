# PSZT

1. wybór listowego (skorzystać przy tym z już obecnego SortSchedulera) (przy generacji listowego zapamiętaj dla każdego 
zadania jaką ma przerwę wcześniej - obecna już zmienna waiting time)
2. dla każdego zadania (od najmniejszego P) sprawdź wszystkie większe (P) zadania i sprawdź czy :
- sprawdzane zadanie trwa tyle co moje P + mój waiting time
- sprawdzane zadanie ma R mniejsze równe moje R - mój waiting time
- czy zamiana polepszy sumaryczną karę?
3. Jeśli tak oznacz oba zadania nawzajem swoimi indeksami i powtarzaj krok 2 dla reszty zadań pomijając te
oznaczone indeksami
