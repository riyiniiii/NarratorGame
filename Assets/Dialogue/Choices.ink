-> main

== main ==
Want to get up little Witch? #speaker:Narrator #portrait:Narrator_neutral #layout:left

 * [Yes] 
   "Good. Let's Continue the story" #speaker:Narrator #portrait:Narrator_neutral #layout:left 
   ->DONE
*[No]
   "Did I hear that right?" #speaker:Narrator #portrait:Narrator_confused #layout:left
   ** nod
   ** "no.."
     -- "Moving on then!" #speaker:Narrator #portrait:Narrator_happy #layout:left
   ->DONE
   
*[Ignore]
   "Hello..? Little Witch? #speaker:Narrator #portrait:Narrator_sad #layout:left
   ** Ignore more
   -- "Little Witch, over here!" #speaker :Narrator #portrait:Narrator_neutral #layout:left
   ->DONE
   
== chosen (choice) ==
You chose {choice}
->END
