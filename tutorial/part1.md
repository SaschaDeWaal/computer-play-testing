## Prevent broken games with computer play testing

#### Part 1. What is computer play testing



**Tutorial topic** How you can let the computer play and test your strategy or card game to measure the balance of your game.

**Interesting for** Game Developers, Game Designers or system designers that are (going to) build a strategy or card game with a lot of in game objects. Especially if the objects influence each other. 

**What you need** Programing knowledge, Unity5, a Game https://github.com/SaschaDeWaal/computer-play-testing

**Time of tutorial** 4 parts, Total 30 - 45 min of reading

**Contact Details** Any questions or updates of your cool project info@developersascha.nl

**Example projects** https://github.com/SaschaDeWaal/computer-play-testing

### Introduction

A game can be ruined if the balance isn't done right. Especially with pvp strategy games. If a unintended dominant strategy is discovered by the mainstream players, people get bored or frustrated and will eventually stop playing. So it is really important to prevent dominant strategy and broken gameplay. But when you have a complex gameplay with a lot of in game objects , it is hard to balance everything. There are ways like the popular Cost curve that uses math to help with balancing, but this method is focused on balancing single items, instead of combination of items. And the game environment is not always taking into account. Play testing is also a good way, but this cost a lot of time and is expensive.

In this tutorial we will look at a different method, Computer play testing. We will look at how you can build computer play testing and use it as a measurement tool. This tool is going to show you stats like how many times a card or any other object has won, how many times it is used, what combination will win more often then others, Or anything else you need depending on your game. We will use brute force to see all possible combinations and endings. This way, no dominant strategy won't go undiscovered before you release your game. And it give you also a chance to look and improve your balance.

### Who am I?

As a game programmer, I have always been interested in abstract systems. I have build traffic systems, environment systems, and many other AI systems. These are easy generic systems that can accomplish big things. I am also interested in game design, especially complex strategy games with a lot of elements. So when I started my graduation project at the University of art Utrecht (HKU), I wanted to look if I could create a system that would improve gameplay. This is how I started to look at computer play testing. A simple system that can help the designer to make better games.



### What is Computer play testing? And why would you? 

![l5rExample](.\images\l5rExample.gif)

So what do I mean with 'Computer play testing'? it's like a play test with humans, where you let them play the game and learn on the fly about the balance and the game mechanics. But this time the computer is playing the game instead of a human. 

Computers are not the same as humans of course, so doing usability test is not possible. However the computer can measure things like how much effect a specific object will have in different situation and different combinations. And it can look what strategies will win more often then others. These statistics can be used for balancing the game and preventing some unwanted dominant strategies.

The biggest benefit of doing it this way instead of something like the cost curve or excel sheets is that the results you get are tested inside the game. It's not math you have to calculate yourself, the data you get are direct from the game.

This way can also give you information of all possible actions inside your game. No (dominant) strategy, useless objects, or too powerful actions won't go undiscovered. You are also able to iterate over the balancing, change stuff, run the test, compare the last iteration with the new one, and keep going until you think the game is good (or you ran out of time)

A downside is that you need to have the digital game, and playtesting cost a lot of time. There are ways to improve the time, like not testing all actions, but only the important once. This is faster, but you might not get all the data.

The framework that we are going to build must be programmed inside the game. Every game is different, so a ready made tool is not possible. So you still need a programmer who is going to build it. 

### Computer play testing as a measurement tool

![website](.\images\website.png)

Every game is different, so every game needs to have a different balancing method. I believe that the designer of that game know's best when it is balanced, and what information he needs to achieve this goal. So I focused on using the computer playtest as a measurement tool. It doesn't say if the game is correctly balanced, but it does show stats that the designer can help to see if it is balanced. So It's an additional tool to help designers.



### Next up

**Part 2: Deep look at the framework**

**Part 3: Example of implementing the framework**

**Part 4: Tips , Tricks,and problem solvers**



### Thanks for reading

Thanks for reading this tutorial. If you have any question, or if you made something cool with or based on this framework. Please contact me at info@developersascha.nl