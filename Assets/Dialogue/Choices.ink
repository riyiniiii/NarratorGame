Want to get up little Witch? #speaker:Narrator #portrait:Narrator #layout:right
-> main

== main ==

 + [Yes]
 
   "Good. Let's Continue the story" #speaker:Narrator #portrait:Narrator #layout:right
   
   ->DONE
   
+ [No] 

   "Did I hear that right?" #speaker:Narrator #portrait:Narrator_confused #layout:right
   
   ** nod 
   
   ** "no.." 
   
     -- "Moving on then!" #speaker:Narrator #portrait:Narrator #layout:right
   ->DONE
   
*[Ignore] 

   "Hello..? Little Witch? #speaker:Narrator #portrait:Narrator #layout:right
   ** Ignore more
   
   -- "Little Witch, over here!" #speaker :Narrator #portrait:Narrator #layout:right
   ->DONE
   
== chosen (choice) ==
You chose {choice}
->END
