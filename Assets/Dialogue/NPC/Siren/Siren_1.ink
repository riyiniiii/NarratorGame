# speaker:Siren
# portrait:Siren
# layout:right

"Has the little Witch finally come to subdue the evil siren..?"

-> Siren_1


=== Siren_1 ===

+ [?]
    -> Sirenchoice1


=== Sirenchoice1 ===

"Aren't you the same witch who tried to slay me before?
Where is the insufferable storyteller who should be describing?
Isn't he following you?"


+ [??]
    -> nod_choice1


=== nod_choice1 ===

"My. Don't tell me you're another witch?"


+ [nod]
    -> confused_choice


=== confused_choice ===

"In that case, tell me, little witch, can you help me find my scales and in return, I'll help you get away from that man called the storyteller."


+ [nod]

->start_puzzle
=== start_puzzle ===
# activate_puzzle:SRN_PUZZLE
-> END


=== siren_after_puzzle ===
# speaker:Siren
# portrait:Siren
# layout:right
You found them...
The storyteller will not like this.
-> END

