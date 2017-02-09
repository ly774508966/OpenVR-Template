// The Arena Generator

// Thank you for using our Arena Generator. We hope it is to your liking!

//	Getting started:
//		-The easiest way to generate an arena is to open the Scene 'ArenaGenerator' that comes with this Package. In this Scene you also have a first person controller to walk around in the arena and check everything up close.
//		-If you want to create a new Scene or use one you already have, follow these steps:
//			-Create an empty GameObject in your Scene, and rename it to 'ArenaManager'
//			-Drag the Script 'ArenaSettings' onto it (or use Add Component)
//			-Drag the Prefab 'Arena' into your Scene.

//	Customizing the arena to your liking:
//		-Select the 'ArenaManager' in the Hierarchy and you will see the options for customizing your arena in the Inspector window.
//		-If you are happy with the settings click on the 'GENERATE ARENA' button. A new arena will be generated.
//		-If you click on the 'Keep This Arena' the arena will be cleaned up from all scripts and inactive objects.
//				----WANRING----
//		You will not be able to edit it with the 'ArenaManager' after you pressed this button!

//	Extra information:
//		-Scaling
//			The arena is pretty big because it is based on a correct size for a default Unity first person controller.
//			It is however possible to scale the arena. You should do this after pressing the 'Keep this arena' button however, because the ArenaManager expects a certain size to generate correctly. The particles of the torches will however not work properly if there is too much scaling involved.

//		-Multiple arena's
//			If you want to generate multiple arena's, you can do so by dragging the 'Arena' prefab into the scene multiple times. The ArenaManager will only change one of them, depending on which one it finds first in the Hierarchy. After you customized the first one and pressed the 'Keep this Arena' button it should automatically find the next arena in the scene. (you can drag the 'Arena' prefab into the scene at any time.

//	Contact:
//	For comments or questions you can contact us by mailing to:
//	info@digitalforest.nl