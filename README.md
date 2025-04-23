# Unity Solitaire

## ✅ What’s in the build:

I implemented the basic Solitaire mechanics. It’s not fully working yet — I just need a bit more time.  
Specifically, **dragging card stacks doesn't work yet**, but **Undo is functional** as expected.  
Cards can be dragged individually, and a simple layout is generated at startup.  

To simplify testing, I set the rules to use **red/black suit alternation**.  
The Undo system took me about **5 minutes** to set up.  
The rest of the time was spent implementing the basic Solitaire logic.

### Summary:
- ✅ Dragging individual cards works
- ✅ There are objects for the deck and tableau piles
- ❌ You can't drag multiple cards at once
- ❌ Closed cards can't be flipped

---

## 🛠 What I’d improve with more time:

From an **architectural perspective**, I wouldn’t build the game so directly (as it is now).  
Currently, drag & drop operates mainly on visuals, not on an underlying game model.  
In a better design, **Deck** and **Card** would be pure data models, separated from their visual components.

That said, it wouldn’t be hard to refactor at this point — it just requires some effort and debugging (in case of bugs).

As for the **missing features** (like clicking on the deck or dragging stacks of cards) — I would simply implement them next.

---

## 🤖 AI Involvement: ChatGPT

I used ChatGPT to help with the project.

The first thing I did was ask it to generate a basic sketch of the Solitaire game (initial scripts).  
The scripts didn’t work as-is, but they gave me a good structure and a starting point.

I liked the result — but I got a little too carried away and started “fighting” the AI instead of taking control.  
In hindsight, I should have created my own models for cards and deck right away,  
built the logic manually, and then asked the AI for feedback or help optimizing it.

---

Thanks for reading!
