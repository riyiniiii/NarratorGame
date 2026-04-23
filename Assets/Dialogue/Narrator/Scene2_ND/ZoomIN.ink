#topPanel
<i>“Once upon a time, there was a witch who lived in a forest with her beloved cat, Miss Mya. She often fell asleep in the forest, but it was getting late, and she needed to hurry back home to her most lovely cat."</i> 
#topPanel
 <i> "The witch awakens from her deep sleep in the forest."</i> 
 +[Awaken] 
 -> Awake_Choice
 +[Ignore]
 ->Ignore_Choice
 
 === Awake_Choice ===
 #topPanel
<i> "As soon as she woke up, she noticed it was nearly evening. She was aware that she needed to return home for Miss Mya." </i> 

->END

 === Ignore_Choice ===
 #topPanel
"I said <b>wake up<b>"
 -> Awake_Choice