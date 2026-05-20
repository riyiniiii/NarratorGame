#speaker:Lamb #portrait:Sheep_Default #layout:right
-> start
=== start ===
<b> A cowardly Lamb runs up to you..<b>

+ [Turn around]
    -> next1

+ [Ignore]
    "Excuse me..?" Asks the uneasy Lamb. 
    -> next2

=== next1 ===
"Could you help me escape from the <color=red>Wicked Wolf</color>? "He is going to eat me, you see!" Says the terrified lamb. 
->next3  

=== next2 ===
"I'm sorry to bother you... But please help me!" The lamb cries out. 
->next1 

=== next3 ===
You nod as the fearful lamb escapes deeper into the forest. 
# wolf_howl
# runaway
-> END