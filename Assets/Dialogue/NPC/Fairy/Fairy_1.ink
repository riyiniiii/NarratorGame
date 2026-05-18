# speaker:Fairy
# portrait:Fairy
# layout:right

"Oh. Who's this..?"

-> Fairy_1

=== Fairy_1 ===

+ [?]
    -> Fairychoice1


=== Fairychoice1 ===

"Just kidding! I know who you are hehe. Tell me witch how did you stumble across this time?"
+ [??]
    -> nod_choice1


=== nod_choice1 ===

"Hm not funny huh..?"
+ [nod]
    -> confused_choice


=== confused_choice ===

"Hey hey, let's make a deal you play my game and I'll help you out from that <b>storyteller</b>"
+ [nod]

"I'll give you a hint on how to win my game aswell, <b><i>not everything seems like how it may be</i>."

->start_puzzle
=== start_puzzle ===
# activate_puzzle:FRY_PUZZLE
-> END


=== siren_after_puzzle ===
# speaker:Fairy
# portrait:Fairy
# layout:right
"Wow! you really are quick on your feet aren't ya!"


"Hm.. We don't have much time so I'll make this quick."

# play_sound:Fairy appearance 
# show_object:HiddenDoor
"As promised, I'll help you get away from the Storyteller. It's important that you do not LISTEN to him. Go past whatever 'goal' he has for you. It will break the story and eventually you'll be free."

"Now run run run!"

-> END
