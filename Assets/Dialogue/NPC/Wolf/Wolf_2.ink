#speaker:Wolf #portrait:Wolf
“I knew I smelt something other than the <b>Lamb</b> but I didn't expect to see you again, Little Witch"
-> start
=== start === 
+ [nod]
    -> next1

=== next1 ===
"Tell me <b>Witch</b>. Where is the <b>Lamb<b>."

+[Help the wolf]
 ->next2
+[Shake Head]
 ->next3  

=== next2 ===
<i>You point to the left to indicate where the Lamb rushed off.<i>
->Truth

=== next3 ===
<i>You shake your head, expressing disagreement.<i>
->Disagree

=== Truth ===
"Since you helped me, I will help you as well. Don't trust the <b>Narrator</b>. He's the one who keeps your <b>precious Mya</b>."
->DONE

=== Disagree ===
"Why because you believe the <b>Lying Lamb</b>? You do realise thats another wolf in disguise who lies for fun.. So I'll ask again. Tell me where the <b>Lamb</b> went."

+[Help Wolf]
 ->next2
 
+[Help Lamb]
 ->HelpLamb
 
 === HelpLamb ===
 <i>You shake your head again.</i> 
 "Fine have it your way Witch."
 #wolf_howl


-> END