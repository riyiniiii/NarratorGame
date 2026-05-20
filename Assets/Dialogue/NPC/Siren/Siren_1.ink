# speaker:Siren
# portrait:Siren
# layout:right

"Has the little Witch finally come to subdue the evil siren..?"

-> Siren_1

=== Siren_1 ===

+ [?]
    -> Sirenchoice1


=== Sirenchoice1 ===
# play_sound:SirenLaugh
"Aren't you the same little witch who tried to 'slay' me before?
Where is that insufferable Narrator? Isn't he following you?"

+ [??]
    -> nod_choice1


=== nod_choice1 ===

"My. Don't tell me you're another witch?"
+ [nod]
    -> confused_choice


=== confused_choice ===

"In that case, little witch, can you help me gather my precious objects and put them in the right place, and in return, <b> I'll help you get away from that Narrator</b>."
+ [nod]

->start_puzzle
=== start_puzzle ===
# activate_puzzle:SRN_PUZZLE
-> END


=== siren_after_puzzle ===
# speaker:Siren
# portrait:Siren
# layout:right
"You did it..."

"As promised, I'll help you get away from the Narrator."
# play_sound:SirenSong
[The siren sings her melody, and a doorway begins to emerge] 
# play_sound:HiddenDoorNoise
# show_object:HiddenDoor

"Little witch listen closely, it's important that you do not <b>LISTEN</b> to him."
"Go past whatever <b>'goal'</b> he has for you. It will break the story, and eventually, you will be free."

"Now go quickly."

-> END

