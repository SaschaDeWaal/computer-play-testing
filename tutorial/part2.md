## Prevent broken games with computer play testing

#### Part 2. Deep look at the framework



This is part 2 of 4 about preventing broken games with computer play testing. If you didn't read part 1, please read that first. 



###Introduction

In this tutorial we will take a deep technical look at the testing framework. We will look at the idea behind it, and we will have a detailed look at all elements of the testing framework. You could use this as a reference for when you are going to use this framework, but you could also use this as inspiration if you are going to build something like this.



### Role of the player

This framework will play the game. It will take over the role of the human player. So before we can start looking at the framework, we first need to take a look at the role of the player.

There are multiple moments in the game where the player is able to do an action. The actions that the player can do is depended on the current status of the game. For example, at the start the player can play cards, but he can't start a battle because there are no cards yet. Some actions might be stupid with the current status of the game, others might be smart. But there are only a few actions the player is allowed to do at this status.

Every time the player execute an action, and this action is applied to the game, a new status will appear with different actions that the player can do. 

![options](.\images\options.png)

The blue dot on the left side of the diagram represents the starting point of the game. This is the point where the game is set up. Cards and starting values are set here. After the start, we see the first game status. At this status, a few actions can be executed by the player. In the diagram you see that 3 actions are possible. If a action is chosen and executed, a new status will appear with new possible actions.

So to simulate the player, we need to know what the status of the game is, and what actions we can choose at this status. And we need to know how to execute this action if we chose to test it.

Because we want to test different actions with different scenario's, we are going to jump around to different game status and run the test from there. It takes too much time to test everything, so we need to be smart about which game status and actions are worth it to test, and which are not.



### Framework overview

Below you see the uml diagram of the framework.![diagram](.\images\diagram.png)





### Game status

![GameStatus](.\images\GameStatus.png)

It all starts with the status of the game. We need to know its  current status in order to know what the player can do, and we need to  jump from status to status so we test different scenarios in the game.  So the status of the game should be stored in `GameStatus`.

There are at least three strategies possible for this

1. If you work with data driven gameplay, you could store the data or models inside this object. Every time we jump to this `Gamestatus`, you could replace the main models with the modal of the status
2. If you have a save and loading system, you could load the game when we jump to this status, and you could save it when we jump away.
3. If you don't have one of the above, you need to find another way. You might want to copy some variables over, or include the game below this object. 



### Game Action

![playerAction](.\images\playerAction.png)

This might be the most important part of the system, The action. You could compare them to controllers in a m-v-c system. They can execute a single action like play a card, or start a battle. A `PlayerAction` should also know if this can be executed with the current `GameStatus`, so we can simply loop thought all actions in the game and look which ones are possible, and which one are not.



### PlayTester

We could run all possible actions, but that would be really heavy and some actions might be really stupid or unnecessary for our test. Instead of that, the `PlayTester` will decide what `PlayerAction` will be tested.

Every tick, all `PlayTesters` will be asked what action it wants to test in the current `GameStatus`. The play tester should filter all not possible actions away and select the actions that are relevant for the test.

Selecting important actions can be done inside the `PlayTester`, but it is also possible to give every `PlayerAction` a (dynamic) weight. You could order them on this weight and only select  the best 4. Limiting actions can improve the performance of the playtest and pushing the test more forward.

![weighSelection](.\images\weighSelection.png)

Random selecting a few actions is also a option. This way you test a mix of actions, and you will play more rounds and games.

Whatever method you like to choose, Keep in mind that you need to test variations of actions. Don't make the mistake I did by testing a similar actions. If you only testing the same kind of actions, it could show you the wrong data. Try to test as much different actions as possible. Also, multiple small games are worth more than a single in depth game. More variants = more trustworthy data

### TickComponent

![TickComponent](.\images\TickComponent.png)

In my first version everything was turn based. But the problem is that although the game I am testing with (Legend of the five rings: the card game) is turned based, there are still actions you can do when it's not your turn. If you think about it, for many games this is the case. With monopoly you can trade with the person in turn, or in my case, you can prevent an attack with a card when it's not your turn.

So I changed the system a bit, and I started working with Ticks. in Unity I created a component. Every 0.1 seconds it ticked the framework. In this tick all the players are asked what action they can and want to test. If it's not their turn, there might be nothing they can do. The game status decide what is possible or not. 



### PlayTestManager

![manager](.\images\manager.png)

The last part to be able to play it, is the manager. The manager, like the name is saying, managed the playtest. It will receive ticks from the `TickComponent`. Every tick it will jump to a Status, all `Playtesters` are asked what kind of action they like to test with this status. For every selected action, the status will be copied with serialization and the action will be executed to that new copied status. The status will then be added to the queue for a later tick.

| Step   | Action                                                       |
| ------ | ------------------------------------------------------------ |
| Step 1 | Receive tick                                                 |
| Step 2 | Jump to a queued status                                      |
| Step 3 | ask every playtesters what actions must be tested with this status |
| Step 4 | For every testable action, copy current status and apply that action |
| Step 5 | These new statuses will be added to the queue for a later tick |
| Step 6 | Current status will be removed                               |
| Step 7 | Waiting for next tick. Go to step 1                          |



Of course, how you select the `GameStatus` of the list does really matter. I chose to keep this random because I want to have a mixed test result. You could also do this as a queue, but then most actions might look the same.

It is important to notice that the actions will be executed to a copied status of the one we used to check if this action is executable. `PlayTester` receives the current status it should use to validate if the action can be executed, If so, it will return this in a list with more actions. Then the status will be copied, and there the action will be applied. So don't store any status related stuff inside the action like reference to cards, or reference to other objects. These objects might be deleted when we jump to another status. Instead store indexes of where the card should be. So when we jump to another status, you will edit the copied card, not the old still in memory card.

So, now we have all elements in place to play the game, we can start focusing on the thing why we are doing this, Testing the game. 

### Observer and Trackers

![trackers](.\images\trackers.png)

Now the computer can play the game, however we are building this to test the game. To do this, we need a Observer. This object will observe the play, and generate results based on what it sees and what it test. Every start of the Tick, and when an action is executed the observer get a chance to observe, and store data if it's think it is useful.

Sometimes you also want to be able to look at an object over time and then make a judgment over it. For this purpose, we have `ObjectTrackers`. These are objects created by the observer. They are stored with the game status so they will follow an object, even if we jump to another status. When there is a new branch, the object tracker will be copied. So the object will be tracked until the end.

![trackerLife](.\images\trackerLife.png)



The observer and the tracker can create `PlayTestResults`. These objects will be stored and at the end of the tick these objects will be send to a database. The `PlayTestResults` should contain the data the designer needs and are the only thing that will leave the playtest.

So why not store the `GameStatus`? I started with doing this, but the problem is that then there will be too much unnecessary data. So instead of saving all points in the test, we only will store the data we need. This will improve performance.

### Store and showing the data

![inkscape_2019-04-18_09-38-55](.\images\inkscape_2019-04-18_09-38-55.png)

So the framework is ready. But you still need a place to store and view the data. You could save it to the hard disk, but a better solution would be to store it to a database. By using a database, you could run the test on multiple computers at the same time and store all data together. I used CoucheDB as database because the `ResultObjects` will be serialized to JSON.

Viewing the data can be done with a website. I build a React website with **create-react-app**. The biggest advantage about using react is that building a website is fast, and there are a lot of free graphs available that you could use to show the data. Web is designed to show a lot of data, so make use of it. Even if you are not a web developer, every technology has it's own benefits.



### Next up

**Part 3: Example of implementing the framework**

**Part 4: Tips, Tricks, and problem solvers** 



### Thanks for reading

Thanks for reading this tutorial. If you have any question, or if you made something cool with or based on this framework. Please contact me at info@developersascha.nl