# Vector-Clock
A quick implementation in Unity for Vector Clocks. Due to the complications involved in the task itself given, the code is rather improperly made and should not be used as a basis. There are also 2 bugs that are avoidable if the user is aware of them.

Usage:

In the bottom left corner, choose the amount of processes you want to have, up to a maximum of 20. After that, unless no messages have been created, do not alter this.

Then, In Event mode, an event can be added on any process. Changing to Message mode allows The user to pick 2 events to turn them into a message. You can't create a message with 2 events on the same process. In here, make sure to pick the leftmost event you wish to add first and the rightmost second, as the calculations and ordering will get messed up otherwise.

After that is done, pushing Start will calculate all the vector timestamps, that can now be viewed in the bottom right corner when hovering over an event/message end/process name.

Pushing reset will remove all processes, events and messages, allowing to create another set of events/messages.

At any point, pressing escape will close the application.
