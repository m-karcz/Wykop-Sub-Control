# Wykop-Sub-Control

Wykop.pl has quite poor followers system - you don't get any notification if somebody unfollow you.
So i wrote system to control who and when unfollows me.

Whole system is presented below:

![System](http://i.imgur.com/UBo5k8K.png)

SubControl.exe is ran every 5 minutes to check if is somebody gave or revoke follow. Changes are saved in MySQL database. Then C# backend serve this data if necessary for frontend written in single html with Angular.JS framework.

It was written to try something new - in this case MySQL and using HTTP in .NET.

TODO:
* change backend to be more async
* add avatars refreshing every 24 hours
* change frontend to ASP.NET
