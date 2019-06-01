## Prevent broken games with computer play testing

#### Part 4. Tips, Tricks and problem solvers

This is part 4 of 4 about preventing broken games with computer play testing. If you didn't read part 1, please read that first. 



###Introduction

In the last part of this series, I would like to give some tips and tricks that might help you. If there is anything you learned, discovered or something you would like to share, please contact me so I can add it and we all can learn from it.



### Problem solver: Testing takes too much time

The biggest downside to computer play testing with brute force is that it takes a lot of time. But there are a few ways how you could solve this.

If you are planning to use this for iteration, you could set it up as a nightly test. At the end of the day, you can start the test. The next day you should have usable data, even if not everything is tested. It still give you a pointer to a direction.

You could run the test on multiple computers at the same time. in fact, I did do this with my case. You could use the office computers when they are not used after working hours, or hire a server farm from amazon.



### Tip: Keep the data variated and play multiple time

It is important to have mixed data instead of having detailed data about a single situation or game. So instead of testing all options in order you might want to shuffle the action order. You should also test multiple games. So you could for example give a single game 10000 ticks, when the test has reach this amount of ticks, restart the game with different starting stats. This way you test different situations and the data will better represent the overall balance of the game.



### Trick: Simple and smart action filter

Instead of randomly or testing all actions, you could add a weight to each action. The weight presents how important that action is for the test. Order the list on the weight and test only the 5 best of it. The selection and filtering of the action should take place in the `PlayTester`.



### Trick: Test only what you need

We build this system to learn more about the game. What you want to learn is different, but try to focus only on getting this data. When you are testing the battles of a game, only test the actions that are needed for this purpose.



###Tip: Hide untrusty data

When an object is not tested enough times (like two times), the data is 
not really treatable. Best is to hide this result so the designer won't 
get the wrong idea. It's better to know nothing, that knowing the wrong 
thing. It can lead you to the wrong path. You could also show how trustworthy the data is, by showing the test times.



### Tip: Take time into account

When I tested with designers, they all wanted to see how long a game would take. Play time is an important thing to the Designer and they like to keep this in mind when balancing. It is hard to do this in the framework because we work with ticks. But you could show the ticks and maybe say that 1 player action is around 10 seconds. So 10 ticks = 10 actions * 10 seconds = 100 seconds.



### Tip: Visualization

The test is worth nothing without having a good visualization of the results. Results should be the main focus. There is a lot of information online about how to visualize data. My advice is to work with a website. Yes it's might not be your thing, but think about it. Web is made to show a lot of data. There are so many good, free graphs you could use. 

![chrome_2019-05-15_12-20-06](images/chrome_2019-05-15_12-20-06.png?raw=true)

In my project I generated a cost heatmap and a win ratio. I used that to compare the objects. You could of course also generate a cost curve, or a really big list. But the UI is really depending on what the designer needs.



### Tip: Visualization will only be used by a few users

When designing the Visualization part you should remember that it only will be used by a few people. These people will use it so much, that learning to read should not be a problem. So don't focus on making the Visualization low entrance, but focus on making it fast readable for those who learned the tool.



### Tip: Visualization Focus on the relationship

Relationship between the data is very important. The designer should look at those relationships and use that to determine if it is balanced. Think about the cost curve for example. There it a relationship between the cost and the benefits. Try to make Visualization that can help to make relationship between data.



### Any more tips?

Please, contact me so I can add them. Info@developersascha.nl 



### Thanks for reading

Thanks for reading this tutorial. If you have any question, or if you made something cool with or based on this framework. Please contact me at info@developersascha.nl