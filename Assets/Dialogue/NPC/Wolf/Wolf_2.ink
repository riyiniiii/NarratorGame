#speaker:Wolf #portrait:Wolf
“I knew I smelt something other than the <b>Lamb</b> but I didn't expect to see you again, Little Witch."
-> start
=== start === 
+ [...]
    -> next1

=== next1 ===
"Tell me <b>Witch</b>. Where is the <b>Lamb<b>." 
#play_sound:WolfSnarl

+[Help the wolf]
 ->next2
+[Refuse]
 ->next3  

=== next2 ===
<i>[You point to the left to indicate where the Lamb rushed off]<i>
->Truth

=== next3 ===
<i>[You shake your head, expressing disagreement.]<i>
->Disagree

=== Truth ===
"Since you helped me, I will help you as well. Don't trust the <b>Narrator</b>. He is a <b>liar</b> himself and the one who keeps your <b>precious cat</b>."
->DONE

=== Disagree ===
"Why because you believe the <b>Lying Lamb</b>? Don't trust it. It is another wolf in disguise who lies for fun.. So I'll ask again. Tell me where the <b>Lamb</b> went." 
#play_sound:Growl

+[Help Wolf]
 ->next2
 
+[Help Lamb]
 ->HelpLamb
 
 === HelpLamb ===
 <i>You shake your head again.</i> 
 "Fine.Have it your way Witch."
 #wolf_howl
# wolf_vanish

-> END