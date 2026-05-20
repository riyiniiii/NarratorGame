#speaker:Narrator #portrait:Narrator 
<i>“Once upon a time, there was a witch who lived in a forest with her beloved cat, Miss Mya."

<i>"She often fell asleep in the forest, but it was getting late, and she needed to hurry back home to her most lovely cat."</i> 

<b><i>"The witch awakens from her deep sleep in the forest."</i><b>
 +[Awaken] 
 -> Awake_Choice
 +[Ignore]
 ->Ignore_Choice
 
 === Awake_Choice ===

<i> "As soon as she woke up, she noticed it was nearly evening. She was aware that she needed to return home for Miss Mya." </i> 

->END

 === Ignore_Choice ===

"<b>I said wake up<b>"
 -> Awake_Choice