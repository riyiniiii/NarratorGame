# speaker:Fairy
# portrait:Fairy
# layout:right

"Oh. Who's this..?"

-> Fairy_1

=== Fairy_1 ===

+ [?]
    -> Fairychoice1


=== Fairychoice1 ===
# play_sound:FairyLaugh
"Just kidding! I know who you are hehe"
"Tell me witch how did you stumble across this time?"
+ [??]
    -> nod_choice1


=== nod_choice1 ===

"Hm not funny huh..?"
+ [nod]
    -> confused_choice


=== confused_choice ===

"Hey hey, you seem like a different witch from before. Let's make a deal you play my game and I'll help you out from that <b>Narrator</b>"
+ [nod]

"I'll give you a little hint on how to win my game aswell, <b><i>not everything seems as it appears</i>."

->start_puzzle
=== start_puzzle ===
# activate_puzzle:FRY_PUZZLE
-> END


=== siren_after_puzzle ===
# speaker:Fairy
# portrait:Fairy
# layout:right
"Wow! you really did it!"

# play_sound:F_Knock

"Hm.. We don't have much time so I'll make this quick."
"As promised, I'll help you get away from the Narrator.
# play_sound:Fairy appearance 
# show_object:HiddenDoor

"It's important that you do not <b>LISTEN</b> to him."
"Go past whatever <b>'goal'</b> he has for you. It will break the story, and eventually, you will be free."

"Now run run run!"

-> END
