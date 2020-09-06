# PhysicsSim - a configurable gravity sandbox

PhysicsSim is a customisable simulation of gravitational interactions between 'planets' depicted on a 2D plane. The simulation is controlled via a series of shortcut keys, which are remappable with a config file, the cursor and a scrollwheel (or scrolling gestures). It uses object-oriented programming techniques to remain efficient, despite having O(n^2) complexity. It runs at a fixed time step (60 Hz) and doesn't slow down even with a large number of planets on a modern processor. The simulation itself was constructed with care to improve efficiency and readability of the code, but no optimisation is done on the gravitational calculation, although this is possible by using clustering techniques, for example.

##Customisation

`config.xml` is a configuration file containing two types of elements - bindings and globals. Bindings are used to change the simulation controls; globals are used to change the underlying properties of the simulation. The config file is pre-populated with the default value for each attribute, which can be changed by the user at will. If certain options are not present, the simulator will default their values to these defaults automatically. If an invalid configuration is read, the simulator will not launch, which is why it is advisable to change the configuration options one by one, making configuration errors easier to catch.

### Controls

| Action | Key (default) | Description                                  |
|--------|---------------|----------------------------------------------|
| Pause  | NumPad0       | Pause the simulation                         |
| New    | NumPad1       | Advance planet creation                      |
| Grid   | NumPad2       | Toggle between three grid modes              |
| Clear  | NumPad3       | Clear existing planets/cancel planet creation|
| Debug  | NumPad4       | Toggle debug view                            |
| Trail  | NumPad5       | Toggle visibility of planet trails           |

The following is a the list of customisable keyboard shortcuts used to control the simulation. `config.xml` contains these default key bindings in a format understood by the simulator.  Any shortcut key can be remapped by replacing the `key` attribute with a valid keycode: a single capital letter denotes the corresponding letter on the keyboard, D* denotes a digit in the top row (where * is replaced by the intended digit) and NumPad* denotes a digit on the number pad (where * is replaced by the intended digit); an exhaustive list can be found [here](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=netframework-4.7.2#fields).

One must tap the 'New' shortcut three times to introduce a new planet into the simulation. The first tap introduces it to the canvas. Now, the position (controllable by moving he mouse to the desired location) and size (controllable with the mousewheel or a scrolling gesture on a laptop trackpad) can be set, before locking these properties with another tap of the same shortcut. The initial velocity of the planet can now be set by moving the mouse to the desired location relative to the location of the planet (the indicator is an exact representation of the distance covered in one second) before locking this final property with another tap of the shortcut.

## Globals

Aside from containing the key bindings, the `config.xml` file also permits the user to change a number of 'global' values used primarily to change the properties of the gravitational forces in the simulation. Each `value` attribute of each of the globals _must_ be an integer.

`Gglobal` stores the value of the gravitational constant (G) used in the simulation, `Tglobal` represents the number of seconds that are recorded as part of each planet's trail (trails are hidden by default and are toggleable by the 'Trail' shortcut) and the mass of each planet is calculated according to the following, making a crucial (yet configurable) link between the apparent size of a planet and its mass:

`mass = (radius ^ index) * coefficient + constant`

### Intended use

The simulation is intended to be used as a demonstration of gravitational interactions between bodies and not as a general-purpose physics simulation. For this reason there is only basic collision detection, which aims to maintain the course of the colliding planets by ignoring the gravitational force between any two planets which intersect one another. this feature only exists so that the simulation is not ruined by the centres of two planets approaching each other in such a way as to produce a huge force on both planets, sending them out of view. This workaround allows two planets to briefly touch without interrupting the either of their orbits.

The simulator allows easy visualisation of systems such as binary stars and also more conventional systems where one or more small planets orbit a star. It can also serve to demonstrate the chaotic nature of such systems by simulating three bodies (all of similar mass) being launched into orbit, where even a small change in starting conditions can result in a large difference in the state of the simulation at a later stage.

There is little UI to introduce the user to the simulator and its operation: developing a full interface would take a disproportionate amount of programming time to the amount of functionality it provides to the user, replacing the brief user guide above.

### Role of object-oriented programming (OOP)

A major component of the design of the simulator is OOP. The fact that the simulation concerns an arbitrary number of discrete objects ('planets') makes it easy to conceptualise each planet as an instance of the planet class, with its own properties such as mass, radius, position and velocity. Furthermore, each object must be able to update its location and velocity on every rendered frame, so it was intuitive to introduce a method of that class to handle updating these fields for a given planet. The principles of OOP allow each instance of this class to access data of all other instances, so that any given planet can calculate the force from all the other individual planets.

OOP is also used elsewhere in the simulation, for example to differentiate between the two available modes: the idle mode and a mode for planet creation. Each one is a class, implementing the `IMode` interface, which allows the swapping out of these classes inside a "current mode" variable. Both have an `Update()` method (as per the interface they inherit), allowing only certain actions to be performed in each mode, without a lengthy if else statement to determine the current mode and perform the appropriate function - this would have to be run every frame.

A similar technique is used to swap between the three 'modes' of possible mouse input. Because the co-ordinates of the mouse (as visible to the Planet class) have to be rounded depending on the current grid setting, there are also 3 separate classes for each of the grid levels, all implementing a common `IMouseInput` interface. In this way, the correct methods can be run to both get the current mouse co-ordinates and to draw the grid, without wasting an if statement for checking the current grid level.

## Development

It was clear that a project like this would benefit greatly from an OOP approach, ranging from the implementation of retrieving keyboard and mouse input to the concept of a Planet object itself. Having previously never used principles such as OOP (or even classes), this approach improved readability and decreased the time it took to find the location where a certain feature is implemented, so that the simulator's features could be added faster - just a few benefits of having a well-defined structure.

### OOP implementation

However, one must recognise that not all concepts relating to object-oriented design were strictly followed. For example, few fields are encapsulated in each class, meaning that quite a lot of them are necessary to be made accessible to other classes for various purposes. UI doesn't play a huge role here, so the on-screen elements are drawn directly in the `Simulator` class, requiring rather low-level access to the `Planet` class for details such as a planet's radius and position in case the debug switch is turned on by the user. One could move the drawing of these two particular elements to the `Planet` class, but this would separate these lines from the rest of the code responsible for drawing more general debug information such as the number of planets and whether the simulator is reaching its framerate goal. A whole separate class could be created solely for the purpose of drawing UI elements, but that wouldn't solve the issue of encapsulation as this class would still have to have the same amount of low-level access to the `Planet` class for displaying this debug information. Despite this, a few fields are in fact properly encapsulated, such as those responsible for storing the trail of the planet, and its current velocity - the methods which require those fields are already in the `Planet` class.

On the other hand, the `Planet` class requires access to the main spriteBatch (`Simulator.spriteBatch` - this is used in every function call where a shape is to be drawn to the canvas) for drawing itself on every frame. Here, both classes must have public/static elements to be able to perform their duties in the correct method, according to their purpose, as opposed to what the scope/hierarchy allows. At the very least this makes the simulator more maintainable, by allowing similar functionality to be achieved in the same method, regardless of scope.

### Use of version control
Git was used in the form of a GitHub plugin to maintain a record of changes and the additions of various features. Although only a small fraction of git's functionality was used in this one-man project, it allowed synchronisation between multiple devices without losing progress, and most importantly the ability to rollback changes as needed. On two occasions the functionality of the simulator was crippled by a lack of testing after the development of multiple features in quick succession, and reverting to a previous commit and inspecting the changes one by one (this time building the application to test it throughout this process) allowed easy debugging of the error. The power of git would have made much more of an impact on a group project or one where contributions are taken from the community, but its usefulness was not missed here.

### Future improvements

A number of features could still be thought of as necessary in a simulation like this but aren't included here. For example, a number of crucial graphics calls could be configured to allow the "camera" to be moved and the "zoom level" changed so that orbits going out of frame could be still be easily tracked. As a final point one must acknowledge the difficulty of simulating celestial bodies and the gravitational forces between them to scale: the gravitational constant is orders of magnitude smaller than in the default configuration and real celestial bodies are separated by distances vastly greater than their radii, completely unlike the values set in the default configuration. The simulator can be adjusted to near those values somewhat, but the effect of gravity is less noticeable at this scale. In the end, a compromise must be made between realistic parameters and a visually appealing demonstration of the gravitational forces at play.